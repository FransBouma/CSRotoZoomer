namespace CSRotoZoomer
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose( bool disposing )
		{
			if( disposing && (components != null) )
			{
				components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this._mainMenu = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this._openImageDialog = new System.Windows.Forms.OpenFileDialog();
			this._animTimer = new System.Windows.Forms.Timer( this.components );
			this._renderDestination = new System.Windows.Forms.Panel();
			this._statusBar = new System.Windows.Forms.StatusStrip();
			this._fpsLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this._canvasSizeLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this._imageSizeLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this._imageInfoLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this._fpsTimer = new System.Windows.Forms.Timer( this.components );
			this._mainMenu.SuspendLayout();
			this._statusBar.SuspendLayout();
			this.SuspendLayout();
			// 
			// _mainMenu
			// 
			this._mainMenu.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem} );
			this._mainMenu.Location = new System.Drawing.Point( 0, 0 );
			this._mainMenu.Name = "_mainMenu";
			this._mainMenu.Size = new System.Drawing.Size( 468, 24 );
			this._mainMenu.TabIndex = 1;
			this._mainMenu.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.openImageToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem} );
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size( 35, 20 );
			this.fileToolStripMenuItem.Text = "&File";
			// 
			// openImageToolStripMenuItem
			// 
			this.openImageToolStripMenuItem.Name = "openImageToolStripMenuItem";
			this.openImageToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.openImageToolStripMenuItem.Size = new System.Drawing.Size( 196, 22 );
			this.openImageToolStripMenuItem.Text = "&Open Image...";
			this.openImageToolStripMenuItem.Click += new System.EventHandler( this.openImageToolStripMenuItem_Click );
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size( 193, 6 );
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
			this.exitToolStripMenuItem.Size = new System.Drawing.Size( 196, 22 );
			this.exitToolStripMenuItem.Text = "E&xit";
			this.exitToolStripMenuItem.Click += new System.EventHandler( this.exitToolStripMenuItem_Click );
			// 
			// _openImageDialog
			// 
			this._openImageDialog.Filter = "Image files (jpg/bmp/gif/png)|*.jpg;*.bmp;*.gif;*.png|All files|*.*";
			this._openImageDialog.InitialDirectory = "c:\\";
			this._openImageDialog.Title = "Open an image file to use in the routine";
			// 
			// _animTimer
			// 
			this._animTimer.Interval = 10;
			this._animTimer.Tick += new System.EventHandler( this._animTimer_Tick );
			// 
			// _renderDestination
			// 
			this._renderDestination.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this._renderDestination.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this._renderDestination.Location = new System.Drawing.Point( 3, 28 );
			this._renderDestination.Name = "_renderDestination";
			this._renderDestination.Size = new System.Drawing.Size( 462, 409 );
			this._renderDestination.TabIndex = 3;
			this._renderDestination.Visible = false;
			// 
			// _statusBar
			// 
			this._statusBar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
			this._statusBar.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this._fpsLabel,
            this._canvasSizeLabel,
            this._imageSizeLabel,
            this._imageInfoLabel} );
			this._statusBar.Location = new System.Drawing.Point( 0, 440 );
			this._statusBar.Name = "_statusBar";
			this._statusBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
			this._statusBar.Size = new System.Drawing.Size( 468, 22 );
			this._statusBar.TabIndex = 4;
			// 
			// _fpsLabel
			// 
			this._fpsLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
						| System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
						| System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
			this._fpsLabel.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
			this._fpsLabel.Name = "_fpsLabel";
			this._fpsLabel.Size = new System.Drawing.Size( 4, 17 );
			// 
			// _canvasSizeLabel
			// 
			this._canvasSizeLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
						| System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
						| System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
			this._canvasSizeLabel.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
			this._canvasSizeLabel.Name = "_canvasSizeLabel";
			this._canvasSizeLabel.Size = new System.Drawing.Size( 4, 17 );
			// 
			// _imageSizeLabel
			// 
			this._imageSizeLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
						| System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
						| System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
			this._imageSizeLabel.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
			this._imageSizeLabel.Name = "_imageSizeLabel";
			this._imageSizeLabel.Size = new System.Drawing.Size( 4, 17 );
			// 
			// _imageInfoLabel
			// 
			this._imageInfoLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top)
						| System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)
						| System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
			this._imageInfoLabel.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
			this._imageInfoLabel.Name = "_imageInfoLabel";
			this._imageInfoLabel.Size = new System.Drawing.Size( 410, 17 );
			this._imageInfoLabel.Spring = true;
			this._imageInfoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// _fpsTimer
			// 
			this._fpsTimer.Interval = 500;
			this._fpsTimer.Tick += new System.EventHandler( this._fpsTimer_Tick );
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size( 468, 462 );
			this.Controls.Add( this._statusBar );
			this.Controls.Add( this._renderDestination );
			this.Controls.Add( this._mainMenu );
			this.MainMenuStrip = this._mainMenu;
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "C# RotoZoomer";
			this.Paint += new System.Windows.Forms.PaintEventHandler( this.MainForm_Paint );
			this.Resize += new System.EventHandler( this.MainForm_Resize );
			this._mainMenu.ResumeLayout( false );
			this._mainMenu.PerformLayout();
			this._statusBar.ResumeLayout( false );
			this._statusBar.PerformLayout();
			this.ResumeLayout( false );
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip _mainMenu;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openImageToolStripMenuItem;
		private System.Windows.Forms.OpenFileDialog _openImageDialog;
		private System.Windows.Forms.Timer _animTimer;
		private System.Windows.Forms.Panel _renderDestination;
		private System.Windows.Forms.StatusStrip _statusBar;
		private System.Windows.Forms.ToolStripStatusLabel _fpsLabel;
		private System.Windows.Forms.ToolStripStatusLabel _canvasSizeLabel;
		private System.Windows.Forms.ToolStripStatusLabel _imageSizeLabel;
		private System.Windows.Forms.ToolStripStatusLabel _imageInfoLabel;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.Timer _fpsTimer;
	}
}

