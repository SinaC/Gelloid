using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using Microsoft.DirectX;

namespace MyGelloidSharp.UserControls {
	public class UCVector3 : System.Windows.Forms.UserControl {
		#region Fields
		private System.Windows.Forms.TextBox txtX;
		private System.Windows.Forms.TextBox txtY;
		private System.Windows.Forms.TextBox txtZ;
		private System.Windows.Forms.Label lblX;
		private System.Windows.Forms.Label lblY;
		private System.Windows.Forms.Label lblZ;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		#endregion

		#region Constructors
		public UCVector3() {
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call

		}
		#endregion

		#region Exposed Properties
		public Vector3 Value {
			get { return new Vector3( (float)Convert.ToDouble(txtX.Text), (float)Convert.ToDouble(txtY.Text), (float)Convert.ToDouble(txtZ.Text) ); }
			set { txtX.Text = value.X.ToString(); txtY.Text = value.Y.ToString(); txtZ.Text = value.Z.ToString(); }
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
			this.txtX = new System.Windows.Forms.TextBox();
			this.txtY = new System.Windows.Forms.TextBox();
			this.txtZ = new System.Windows.Forms.TextBox();
			this.lblX = new System.Windows.Forms.Label();
			this.lblY = new System.Windows.Forms.Label();
			this.lblZ = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// txtX
			// 
			this.txtX.Location = new System.Drawing.Point(0, 16);
			this.txtX.Name = "txtX";
			this.txtX.Size = new System.Drawing.Size(80, 20);
			this.txtX.TabIndex = 0;
			this.txtX.Text = "";
			// 
			// txtY
			// 
			this.txtY.Location = new System.Drawing.Point(80, 16);
			this.txtY.Name = "txtY";
			this.txtY.Size = new System.Drawing.Size(80, 20);
			this.txtY.TabIndex = 1;
			this.txtY.Text = "";
			// 
			// txtZ
			// 
			this.txtZ.Location = new System.Drawing.Point(160, 16);
			this.txtZ.Name = "txtZ";
			this.txtZ.Size = new System.Drawing.Size(80, 20);
			this.txtZ.TabIndex = 2;
			this.txtZ.Text = "";
			// 
			// lblX
			// 
			this.lblX.Location = new System.Drawing.Point(0, 0);
			this.lblX.Name = "lblX";
			this.lblX.Size = new System.Drawing.Size(80, 16);
			this.lblX.TabIndex = 3;
			this.lblX.Text = "X";
			// 
			// lblY
			// 
			this.lblY.Location = new System.Drawing.Point(80, 0);
			this.lblY.Name = "lblY";
			this.lblY.Size = new System.Drawing.Size(80, 16);
			this.lblY.TabIndex = 4;
			this.lblY.Text = "Y";
			// 
			// lblZ
			// 
			this.lblZ.Location = new System.Drawing.Point(160, 0);
			this.lblZ.Name = "lblZ";
			this.lblZ.Size = new System.Drawing.Size(80, 16);
			this.lblZ.TabIndex = 5;
			this.lblZ.Text = "Z";
			// 
			// UCVector3
			// 
			this.Controls.Add(this.lblZ);
			this.Controls.Add(this.lblY);
			this.Controls.Add(this.lblX);
			this.Controls.Add(this.txtZ);
			this.Controls.Add(this.txtY);
			this.Controls.Add(this.txtX);
			this.Name = "UCVector3";
			this.Size = new System.Drawing.Size(240, 40);
			this.ResumeLayout(false);

		}
		#endregion
	}
}