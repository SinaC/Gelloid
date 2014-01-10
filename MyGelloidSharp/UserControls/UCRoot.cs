using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MyGelloidSharp.UserControls {
	/// <summary>
	/// Summary description for UCRoot.
	/// </summary>
	public class UCRoot : System.Windows.Forms.UserControl {
		#region Fields
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		#endregion

		#region Constructors
		public UCRoot() {
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call

		}
		#endregion

		#region Virtual Methods
		public virtual void Get( object o ) {} // pure virtual methods (cannot create abstract user control)
		public virtual void Set( object o ) {} // pure virtual methods (cannot create abstract user control)
		#endregion

		#region Bubbling Events
		public EventHandler updateClick;
		public EventHandler resetClick;

		protected void btnUpdate_Click(object sender, System.EventArgs e) {
			if ( updateClick != null )
				updateClick( sender, e );
		}
		protected void btnReset_Click(object sender, System.EventArgs e) {
			if ( resetClick != null )
				resetClick( sender, e );
		}
		#endregion

		#region Dispose
		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing ) {
			if( disposing ) {
				if(components != null) {
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}
		#endregion

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			components = new System.ComponentModel.Container();
		}
		#endregion
	}
}