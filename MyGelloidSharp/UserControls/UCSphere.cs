using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using MyGelloidSharp.Colliders;

namespace MyGelloidSharp.UserControls {
	public class UCSphere : UCRoot {
		#region Fields
		private System.Windows.Forms.GroupBox groupSphere;
		private System.Windows.Forms.Button btnReset;
		private System.Windows.Forms.Button btnUpdate;
		private System.Windows.Forms.TextBox txtRadius;
		private System.Windows.Forms.Label lblRadius;
		private MyGelloidSharp.UserControls.UCVector3 ucCenter;
		private System.Windows.Forms.Label lblCenter;
		private System.Windows.Forms.TextBox txtName;
		private System.Windows.Forms.Label lblName;
		private System.ComponentModel.IContainer components = null;
		#endregion

		#region Constructors
		public UCSphere() {
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call
		}
		#endregion

		#region Exposed Properties
		public override void Get( object o ) {
			CSphere sphere = (CSphere)o;
			sphere.Center = ucCenter.Value;
			sphere.Radius = Radius;
			sphere.Name = SphereName;
			sphere.InitializeGraphics();
		}

		public override void Set( object o ) {
			CSphere sphere = (CSphere)o;
			ucCenter.Value = sphere.Center;
			SphereName = sphere.Name;
			Radius = sphere.Radius;
		}

		public float Radius {
			get { return (float)Convert.ToDouble(txtRadius.Text); }
			set { txtRadius.Text = value.ToString(); }
		}

		public string SphereName {
			get { return txtName.Text; }
			set { txtName.Text = value; }
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
			this.groupSphere = new System.Windows.Forms.GroupBox();
			this.txtName = new System.Windows.Forms.TextBox();
			this.lblName = new System.Windows.Forms.Label();
			this.btnReset = new System.Windows.Forms.Button();
			this.btnUpdate = new System.Windows.Forms.Button();
			this.txtRadius = new System.Windows.Forms.TextBox();
			this.lblRadius = new System.Windows.Forms.Label();
			this.ucCenter = new MyGelloidSharp.UserControls.UCVector3();
			this.lblCenter = new System.Windows.Forms.Label();
			this.groupSphere.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupSphere
			// 
			this.groupSphere.Controls.Add(this.txtName);
			this.groupSphere.Controls.Add(this.lblName);
			this.groupSphere.Controls.Add(this.btnReset);
			this.groupSphere.Controls.Add(this.btnUpdate);
			this.groupSphere.Controls.Add(this.txtRadius);
			this.groupSphere.Controls.Add(this.lblRadius);
			this.groupSphere.Controls.Add(this.ucCenter);
			this.groupSphere.Controls.Add(this.lblCenter);
			this.groupSphere.Location = new System.Drawing.Point(0, 0);
			this.groupSphere.Name = "groupSphere";
			this.groupSphere.Size = new System.Drawing.Size(368, 144);
			this.groupSphere.TabIndex = 0;
			this.groupSphere.TabStop = false;
			this.groupSphere.Text = "Sphere";
			// 
			// txtName
			// 
			this.txtName.Location = new System.Drawing.Point(48, 16);
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(200, 20);
			this.txtName.TabIndex = 21;
			this.txtName.Text = "";
			// 
			// lblName
			// 
			this.lblName.Location = new System.Drawing.Point(8, 16);
			this.lblName.Name = "lblName";
			this.lblName.Size = new System.Drawing.Size(40, 23);
			this.lblName.TabIndex = 20;
			this.lblName.Text = "Name";
			// 
			// btnReset
			// 
			this.btnReset.Location = new System.Drawing.Point(264, 112);
			this.btnReset.Name = "btnReset";
			this.btnReset.TabIndex = 19;
			this.btnReset.Text = "Reset";
			this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
			// 
			// btnUpdate
			// 
			this.btnUpdate.Location = new System.Drawing.Point(144, 112);
			this.btnUpdate.Name = "btnUpdate";
			this.btnUpdate.TabIndex = 18;
			this.btnUpdate.Text = "Update";
			this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
			// 
			// txtRadius
			// 
			this.txtRadius.Location = new System.Drawing.Point(8, 112);
			this.txtRadius.Name = "txtRadius";
			this.txtRadius.TabIndex = 17;
			this.txtRadius.Text = "";
			// 
			// lblRadius
			// 
			this.lblRadius.Location = new System.Drawing.Point(8, 96);
			this.lblRadius.Name = "lblRadius";
			this.lblRadius.TabIndex = 16;
			this.lblRadius.Text = "Radius";
			// 
			// ucCenter
			// 
			this.ucCenter.Location = new System.Drawing.Point(8, 56);
			this.ucCenter.Name = "ucCenter";
			this.ucCenter.Size = new System.Drawing.Size(240, 40);
			this.ucCenter.TabIndex = 15;
			// 
			// lblCenter
			// 
			this.lblCenter.Location = new System.Drawing.Point(8, 40);
			this.lblCenter.Name = "lblCenter";
			this.lblCenter.TabIndex = 14;
			this.lblCenter.Text = "Center";
			// 
			// UCSphere
			// 
			this.Controls.Add(this.groupSphere);
			this.Name = "UCSphere";
			this.Size = new System.Drawing.Size(368, 144);
			this.groupSphere.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion
	}
}