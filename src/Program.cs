///////////////////////////////////////////////////////////////////
// C# implementation of old-skool demoscene roto-zoomer effect
// Programmed by Frans 'Otis' Bouma
// http://weblogs.asp.net/fbouma
// 
// Effect invented by Chaos / Sanity in 1989 on the Amiga 500
///////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CSRotoZoomer
{
	/// <summary>
	/// Class to start the app.
	/// </summary>
	static class Program
	{
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault( false );
			Application.ThreadException += new System.Threading.ThreadExceptionEventHandler( Application_ThreadException );
			Application.Run( new MainForm() );
		}

		/// <summary>
		/// Handles the ThreadException event of the Application control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Threading.ThreadExceptionEventArgs"/> instance containing the event data.</param>
		static void Application_ThreadException( object sender, System.Threading.ThreadExceptionEventArgs e )
		{
			DialogResult result = MessageBox.Show( string.Format("Exception caught: {0}{1}{1}StackTrace:{1}{2}{1}{1}Do you want to abort the application?", e.Exception.Message, Environment.NewLine, e.Exception.StackTrace), 
				"Exception caught", MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1 );

			if( result == DialogResult.Yes )
			{
				Application.Exit();
			}
		}
	}
}