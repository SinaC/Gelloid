using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using MyGelloidSharp.PhysEnv;

namespace MyGelloidSharp.UserControls {
	public class UCTriangle : MyGelloidSharp.UserControls.UCRoot {
		#region Fields
		private System.Windows.Forms.GroupBox groupTriangle;
		private System.Windows.Forms.Label lblVertex1;
		private MyGelloidSharp.UserControls.UCVertex ucVertex1;
		private MyGelloidSharp.UserControls.UCVertex ucVertex2;
		private System.Windows.Forms.Label lblVertex2;
		private MyGelloidSharp.UserControls.UCVertex ucVertex3;
		private System.Windows.Forms.Label lblVertex3;
		private System.Windows.Forms.Button btnReset;
		private System.Windows.Forms.Button btnUpdate;
		private System.ComponentModel.IContainer components = null;
		#endregion

		#region Constructors
		public UCTriangle() {
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call
		}
		#endregion

		#region Exposed Properties
		public override void Get( object o ) {
			CTriangle triangle = (CTriangle)o;
			ucVertex1.Get( triangle.Vertices[0] );
			ucVertex2.Get( triangle.Vertices[1] );
			ucVertex3.Get( triangle.Vertices[2] );
		}

		public override void Set( object o ) {
			CTriangle triangle = (CTriangle)o;
			ucVertex1.Set( triangle.Vertices[0] );
			ucVertex2.Set( triangle.Vertices[1] );
			ucVertex3.Set( triangle.Vertices[2] );
		}
		#endregion

		#region Dispose
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing ) {
			if( disposing ) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}
		#endregion

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.groupTriangle = new System.Windows.Forms.GroupBox();
			this.ucVertex3 = new MyGelloidSharp.UserControls.UCVertex();
			this.lblVertex3 = new System.Windows.Forms.Label();
			this.ucVertex2 = new MyGelloidSharp.UserControls.UCVertex();
			this.lblVertex2 = new System.Windows.Forms.Label();
			this.ucVertex1 = new MyGelloidSharp.UserControls.UCVertex();
			this.lblVertex1 = new System.Windows.Forms.Label();
			this.btnReset = new System.Windows.Forms.Button();
			this.btnUpdate = new System.Windows.Forms.Button();
			this.groupTriangle.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupTriangle
			// 
			this.groupTriangle.Controls.Add(this.btnReset);
			this.groupTriangle.Controls.Add(this.btnUpdate);
			this.groupTriangle.Controls.Add(this.ucVertex3);
			this.groupTriangle.Controls.Add(this.lblVertex3);
			this.groupTriangle.Controls.Add(this.ucVertex2);
			this.groupTriangle.Controls.Add(this.lblVertex2);
			this.groupTriangle.Controls.Add(this.ucVertex1);
			this.groupTriangle.Controls.Add(this.lblVertex1);
			this.groupTriangle.Location = new System.Drawing.Point(0, 0);
			this.groupTriangle.Name = "groupTriangle";
			this.groupTriangle.Size = new System.Drawing.Size(448, 248);
			this.groupTriangle.TabIndex = 0;
			this.groupTriangle.TabStop = false;
			this.groupTriangle.Text = "Triangle";
			// 
			// ucVertex3
			// 
			this.ucVertex3.Location = new System.Drawing.Point(16, 160);
			this.ucVertex3.Name = "ucVertex3";
			this.ucVertex3.Size = new System.Drawing.Size(208, 80);
			this.ucVertex3.TabIndex = 5;
			// 
			// lblVertex3
			// 
			this.lblVertex3.Location = new System.Drawing.Point(16, 136);
			this.lblVertex3.Name = "lblVertex3";
			this.lblVertex3.TabIndex = 4;
			this.lblVertex3.Text = "Vertex 3";
			// 
			// ucVertex2
			// 
			this.ucVertex2.Location = new System.Drawing.Point(232, 48);
			this.ucVertex2.Name = "ucVertex2";
			this.ucVertex2.Size = new System.Drawing.Size(208, 80);
			this.ucVertex2.TabIndex = 3;
			// 
			// lblVertex2
			// 
			this.lblVertex2.Location = new System.Drawing.Point(232, 24);
			this.lblVertex2.Name = "lblVertex2";
			this.lblVertex2.TabIndex = 2;
			this.lblVertex2.Text = "Vertex 2";
			// 
			// ucVertex1
			// 
			this.ucVertex1.Location = new System.Drawing.Point(16, 48);
			this.ucVertex1.Name = "ucVertex1";
			this.ucVertex1.Size = new System.Drawing.Size(208, 80);
			this.ucVertex1.TabIndex = 1;
			// 
			// lblVertex1
			// 
			this.lblVertex1.Location = new System.Drawing.Point(16, 24);
			this.lblVertex1.Name = "lblVertex1";
			this.lblVertex1.TabIndex = 0;
			this.lblVertex1.Text = "Vertex 1";
			// 
			// btnReset
			// 
			this.btnReset.Location = new System.Drawing.Point(360, 208);
			this.btnReset.Name = "btnReset";
			this.btnReset.TabIndex = 11;
			this.btnReset.Text = "Reset";
			this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
			// 
			// btnUpdate
			// 
			this.btnUpdate.Location = new System.Drawing.Point(240, 208);
			this.btnUpdate.Name = "btnUpdate";
			this.btnUpdate.TabIndex = 10;
			this.btnUpdate.Text = "Update";
			this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
			// 
			// UCTriangle
			// 
			this.Controls.Add(this.groupTriangle);
			this.Name = "UCTriangle";
			this.Size = new System.Drawing.Size(448, 248);
			this.groupTriangle.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion
	}
}

