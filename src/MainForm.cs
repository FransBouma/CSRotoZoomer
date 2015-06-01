///////////////////////////////////////////////////////////////////
// C# implementation of old-skool demoscene roto-zoomer effect
// Programmed by Frans 'Otis' Bouma
// http://weblogs.asp.net/fbouma
// 
// Effect invented by Chaos / Sanity in 1989 on the Amiga 500
///////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;

namespace CSRotoZoomer
{
	public partial class MainForm : Form
	{
		#region Members
		private int		_canvasWidth, _canvasHeight, _zoomCounter, _zoomInMax, _zoomOutMax, _imageWidth, _imageHeight;
		private Bitmap	_renderCanvas;
		private bool	_zoomIn, _resetCanvas;
		private double	_gamma, _deltaGamma, _xZoomDelta, _yZoomDelta;
		private string	_fpsString, _canvasSizeString, _imageSizeString, _imageInfoString;
		private DateTime _timeCurrentFrame, _timePreviousFrame;

		// coordinate source.
		double[]		_xSourceCoords ={ -128, 128, -128 };
		double[]		_ySourceCoords ={ -64, -64, 64 };

		// coordinate destination. (for drawing).
		double[]		_xDestinationCoords ={ -128, 128, -128 };
		double[]		_yDestinationCoords ={ -64, -64, 64 };

		uint[]			_sourcePixels;
		#endregion


		/// <summary>
		/// CTor
		/// </summary>
		public MainForm()
		{
			InitializeComponent();

			// be sure this dialog uses double buffered rendering
			this.SetStyle( ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true );

			_resetCanvas = false;
			_fpsString = string.Empty;
			_canvasSizeString = string.Empty;
			_imageSizeString = string.Empty;
			_imageInfoString = string.Empty;
			_timeCurrentFrame = DateTime.Now;
			_timePreviousFrame = _timeCurrentFrame;
		}

		
		/// <summary>
		/// Handles the Click event of the openImageToolStripMenuItem control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void openImageToolStripMenuItem_Click( object sender, EventArgs e )
		{
			DialogResult result = _openImageDialog.ShowDialog( this );
			if( result != DialogResult.OK )
			{
				return;
			}

			// user has specified a filename, load the image and start the rotozoomer
			InitRotoZoomer( _openImageDialog.FileName );
		}


		/// <summary>
		/// Initializes the roto zoomer.
		/// </summary>
		private void InitRotoZoomer(string filename)
		{
			_fpsTimer.Enabled = false;
			_animTimer.Enabled = false;
			CreateCanvas();

			// load bitmap specified by the user.
			Bitmap srcImage = new Bitmap(filename);
			_imageWidth = srcImage.Width;
			_imageHeight = srcImage.Height;
			_imageInfoString = filename;
			_imageSizeString = string.Format( "Image: {0} x {1}", _imageWidth, _imageHeight );

			// initialize our parameters.
			_zoomCounter = 0;
			_zoomIn = false;

			// deltaGamma value, controls rotation.
			_deltaGamma = 2.0;

			// deltas for x and y zooming. If they're not the same the image gets stretched.
			_xZoomDelta = 2.0;
			_yZoomDelta = 1.0;

			// zoom counter values when the zoom action (in/out) will swap. Give this different values for wicked results. 
			_zoomInMax = 200;
			_zoomOutMax = 300;

			if( (_imageWidth == 0) || (_imageHeight == 0) )
			{
				throw new ApplicationException( "The image you selected has a width and/or height of 0 and isn't usable" );
			}

			double imageWidthD = (double)_imageWidth;
			double imageHeightD = (double)_imageHeight;
			_sourcePixels = new uint[_imageWidth * _imageHeight];
			if( _xZoomDelta != 0 )
			{
				_zoomInMax = (int)(((imageWidthD / 2.0) / _xZoomDelta) - 10.0);
			}
			_zoomOutMax = _zoomInMax * 5;
			_yZoomDelta = (imageHeightD / imageWidthD) * _xZoomDelta;

			// read the initial pixels into the srcpixel array. This makes it possible to perform an in-place rendering to avoid memory trashing
			BitmapData srcData = srcImage.LockBits( new Rectangle( 0, 0, _imageWidth, _imageHeight ), ImageLockMode.ReadWrite, srcImage.PixelFormat);
			unsafe
			{
				switch( srcImage.PixelFormat )
				{
					case PixelFormat.Format32bppArgb:
					case PixelFormat.Format32bppPArgb:
					case PixelFormat.Format32bppRgb:
						uint* pSrc32bpp = (uint*)srcData.Scan0;
						for( int i = 0; i < _imageHeight; i++ )
						{
							for( int j = 0; j < _imageWidth; j++ )
							{
								_sourcePixels[(i * _imageWidth) + j] = pSrc32bpp[(i * _imageWidth) + j];
							}
						}
						srcImage.UnlockBits( srcData );
						break;
					case PixelFormat.Format8bppIndexed:
						// first convert the palet to uint's (ARGB). This is an operation which needs to be done once, so we don't convert
						// the same color over and over again. 
						byte* pSrc8bindexed = (byte*)srcData.Scan0;
						uint[] paletteColors = new uint[srcImage.Palette.Entries.Length];
						for(int i=0;i<srcImage.Palette.Entries.Length;i++)
						{
							paletteColors[i] = (uint)srcImage.Palette.Entries[i].ToArgb();
						}
						// now convert the pixels to uints
						for( int i = 0; i < _imageHeight; i++ )
						{
							for( int j = 0; j < _imageWidth; j++ )
							{
								_sourcePixels[(i * _imageWidth) + j] = paletteColors[pSrc8bindexed[(i * _imageWidth) + j]];
							}
						}
						srcImage.UnlockBits( srcData );
						break;
					default:
						// first unlock, as GetPixel will lock/unlock the source for every pixel read... 
						srcImage.UnlockBits( srcData );

						// use slow getpixel method. This is slow because GetPixel is very slow and also the on-the-fly ARGB conversion. 
						this.Cursor = Cursors.WaitCursor;
						Application.DoEvents();

						for( int i = 0; i < _imageHeight; i++ )
						{
							for( int j = 0; j < _imageWidth; j++ )
							{
								Color pixel = srcImage.GetPixel( j, i );
								_sourcePixels[(i * _imageWidth) + j] = (uint)pixel.ToArgb();
							}
						}

						this.Cursor = Cursors.Default;
						Application.DoEvents();
						break;
				}
			}

			// init coord arrays for the three points we're using to read the source pixels
			// A ---------- B
			// | read direction ->
			// | 
			// |
			// |
			// C
			double halfWidthSrc = imageWidthD / 2.0;
			double halfHeightSrc = imageHeightD / 2.0;
			_xSourceCoords[0] = -halfWidthSrc;
			_xSourceCoords[1] = halfWidthSrc;
			_xSourceCoords[2] = -halfWidthSrc;
			_ySourceCoords[0] = -halfHeightSrc;
			_ySourceCoords[1] = -halfHeightSrc;
			_ySourceCoords[2] = halfHeightSrc;


			_animTimer.Enabled = true;
			_fpsTimer.Enabled = true;
			_animTimer.Start();
			_fpsTimer.Start();
		}

		
		/// <summary>
		/// Creates a new render canvas, which will be used to render the picture in for every frame.
		/// </summary>
		private void CreateCanvas()
		{
			if( this.WindowState == FormWindowState.Minimized )
			{
				return;
			}

			if( _renderCanvas != null )
			{
				_renderCanvas.Dispose();
				_renderCanvas = null;
			}

			// _renderDestination is a panel which isn't visible, and which we use to determine the position of the image we will be creating every frame.
			_canvasWidth = _renderDestination.Width;
			_canvasHeight = _renderDestination.Height;
			_renderCanvas = new Bitmap( _canvasWidth, _canvasHeight );

			double halfWidthDst = ((double)_canvasWidth) / 2.0;
			double halfHeightDst = ((double)_canvasHeight) / 2.0;
			_xDestinationCoords[0] = -halfWidthDst;
			_xDestinationCoords[1] = halfWidthDst;
			_xDestinationCoords[2] = -halfWidthDst;

			_yDestinationCoords[0] = -halfHeightDst;
			_yDestinationCoords[1] = -halfHeightDst;
			_yDestinationCoords[2] = halfHeightDst;

			_canvasSizeString = string.Format( "Canvas: {0} x {1}", _canvasWidth, _canvasHeight );
		}


		/// <summary>
		/// Handles the Tick event of the _animTimer control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void _animTimer_Tick( object sender, System.EventArgs e )
		{
			if( _resetCanvas )
			{
				CreateCanvas();
				_resetCanvas = false;
			}
			Zoom();
			Rotate();
			Animate();
		}


		/// <summary>
		/// Controls the zoom parameters, used by Animate()
		/// </summary>
		private void Zoom()
		{
			if( _zoomIn )
			{
				for( int i = 0; i < 3; i++ )
				{
					if( _xSourceCoords[i] < 0 )
					{
						_xSourceCoords[i] += _xZoomDelta;
					}
					else
					{
						_xSourceCoords[i] -= _xZoomDelta;
					}
					if( _ySourceCoords[i] < 0 )
					{
						_ySourceCoords[i] += _yZoomDelta;
					}
					else
					{
						_ySourceCoords[i] -= _yZoomDelta;
					}
				}
				_zoomCounter++;
				if( _zoomCounter > _zoomInMax )
				{
					_zoomIn = false;
				}
			}
			else
			{
				for( int i = 0; i < 3; i++ )
				{
					if( _xSourceCoords[i] < 0 )
					{
						_xSourceCoords[i] -= _xZoomDelta;
					}
					else
					{
						_xSourceCoords[i] += _xZoomDelta;
					}
					if( _ySourceCoords[i] < 0 )
					{
						_ySourceCoords[i] -= _yZoomDelta;
					}
					else
					{
						_ySourceCoords[i] += _yZoomDelta;
					}
				}
				_zoomCounter--;
				if( _zoomCounter < -_zoomOutMax )
				{
					_zoomIn = true;
				}
			}
		}

		/// <summary>
		/// Animates the actual image. It renders a new destination image into the rendercanvas pixelbuffer and invalidates the form so the image gets drawn
		/// in the paint event handler.
		/// </summary>
		private void Animate()
		{
			_timePreviousFrame = _timeCurrentFrame;
			_timeCurrentFrame = DateTime.Now;

			double xa = _xDestinationCoords[0];
			double xb = _xDestinationCoords[1];
			double xc = _xDestinationCoords[2];
			double ya = _yDestinationCoords[0];
			double yb = _yDestinationCoords[1];
			double yc = _yDestinationCoords[2];
			double canvasWidthD = (double)_canvasWidth;
			double canvasHeightD = (double)_canvasHeight;
			double xab_delta = (xb - xa) / canvasWidthD;
			double yab_delta = (yb - ya) / canvasWidthD;
			double xac_delta = (xc - xa) / canvasHeightD;
			double yac_delta = (yc - ya) / canvasHeightD;

			// transpose the rotating centre to the middle of the picture.
			double x_off = xa + (canvasWidthD * 0.5);
			double y_off = ya + (canvasHeightD * 0.5);

			// lock the bits so we can grab a 32bit pointer to the buffer which we can use to write to the buffer directly.
			BitmapData imageDst = _renderCanvas.LockBits( new Rectangle( 0, 0, _canvasWidth, _canvasHeight ), ImageLockMode.ReadWrite, PixelFormat.Format32bppRgb );
			System.IntPtr scan0 = imageDst.Scan0;

			unsafe
			{
				// grab the destination pointer into the pixelbuffer.
				uint* pDest = (uint*)scan0;

				for( int i = 0; i < _canvasHeight; i++ )
				{
					for( int j = 0; j < _canvasWidth; j++ )
					{
						// calculate for the current pixel on (j,i) in the rendercanvas the x, y for the pixel in the source image. 
						int readX = (int)x_off;
						int readY = (int)y_off;
						
						if( readX < 0 )
						{
							// clamp
							readX = (_imageWidth - 1) - (-readX % (_imageWidth - 1));
						}
						if( readY < 0 )
						{
							// clamp
							readY = (_imageHeight - 1) - (-readY % (_imageHeight - 1));
						}
						if( readX >= _imageWidth )
						{
							// clamp
							readX = 0 + (readX % (_imageWidth - 1));
						}
						if( readY >= _imageHeight )
						{
							// clamp
							readY = 0 + (readY % (_imageHeight - 1));
						}
						// write the pixel
						pDest[0] = _sourcePixels[(readY * _imageWidth) + readX];
						// set the offsets to the new sourcepixel coordinates for the next pixel on this rasterline. Remember that these pixels
						// are read using a rotated rectangle on the sourcepixels and the deltas for x and y are used to determine the next sourcepixel
						// on a rotated line on the sourcepixels.
						x_off += xab_delta;
						y_off += yab_delta;
						pDest++;
					}
					double iD = (double)i;
					// calculates the new start of the next line of the rendercanvas in the sourcepixel. We apply a little offset to the xac/yac deltas to get
					// a funny rubber effect. 
					x_off = xa + (iD * (xac_delta - 0.002 * iD)) + (canvasWidthD * 0.5);
					y_off = ya + (iD * (yac_delta + 0.001 * iD)) + (canvasHeightD * 0.5);
				}
			}

			// done, unlock the buffer so we can render the canvas.
			_renderCanvas.UnlockBits( imageDst );
			this.Invalidate();
		}


		/// <summary>
		/// Rotate the rectangle which is used to read the sourcepixels from the source image.
		/// </summary>
		public void Rotate()
		{
			// first update the angle. only gamma is interesting...
			_gamma = (_gamma + _deltaGamma) % 360;
			double gammaRad = (_gamma / 360) * 2 * Math.PI;

			double sinG = Math.Sin( gammaRad );
			double cosG = Math.Cos( gammaRad );

			// now rotate the coords...
			for( int i = 0; i < 3; i++ )
			{
				double yn = _ySourceCoords[i];
				double xn = _xSourceCoords[i];

				// easy-does-it rotation using simple euler angle math. 
				_xDestinationCoords[i] = (xn * cosG) - (yn * sinG);
				_yDestinationCoords[i] = (yn * cosG) + (xn * sinG);
			}
		}

		/// <summary>
		/// Handles the Click event of the _closeButton control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void _closeButton_Click( object sender, EventArgs e )
		{
			this.Close();
		}

		/// <summary>
		/// Handles the Resize event of the MainForm control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void MainForm_Resize( object sender, EventArgs e )
		{
			// set a new canvas so the destination render canvas is resized as well.
			_resetCanvas = true;
		}

		/// <summary>
		/// Handles the Paint event of the MainForm control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Windows.Forms.PaintEventArgs"/> instance containing the event data.</param>
		private void MainForm_Paint( object sender, PaintEventArgs e )
		{
			if( _renderCanvas == null )
			{
				// no canvas created, no drawing to be done
				return;
			}
			// draw the new frame onto the form.
			e.Graphics.DrawImageUnscaled( _renderCanvas, _renderDestination.Left, _renderDestination.Top );
			UpdateStatusBar();
		}

		/// <summary>
		/// Updates the status bar elements
		/// </summary>
		private void UpdateStatusBar()
		{
			_fpsLabel.Text = _fpsString;
			_canvasSizeLabel.Text = _canvasSizeString;
			_imageSizeLabel.Text = _imageSizeString;
			_imageInfoLabel.Text = _imageInfoString;
		}

		
		/// <summary>
		/// Handles the Tick event of the _fpsTimer control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void _fpsTimer_Tick( object sender, EventArgs e )
		{
			int millisecondsBetweenFrames = _timeCurrentFrame.Subtract( _timePreviousFrame ).Milliseconds;
			if( millisecondsBetweenFrames == 0 )
			{
				_fpsString = "0 frames/sec";
			}
			else
			{
				_fpsString = string.Format( "{0} frames / sec", ((float)(1000.0f / (float)millisecondsBetweenFrames)).ToString( ".00" ) );
			}
		}

		/// <summary>
		/// Handles the Click event of the exitToolStripMenuItem control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void exitToolStripMenuItem_Click( object sender, EventArgs e )
		{
			this.Close();
		}

	}
}