using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using MyGelloidSharp.Colliders;

namespace MyGelloidSharp.UserControls {
	public class UCPlane : MyGelloidSharp.UserControls.UCRoot {
		#region Fields
		private System.Windows.Forms.GroupBox groupPlane;
		private System.Windows.Forms.Label lblNormal;
		private MyGelloidSharp.UserControls.UCVector3 ucNormal;
		private System.Windows.Forms.Label lblDistance;
		private System.Windows.Forms.TextBox txtDistance;
		private System.Windows.Forms.Button btnReset;
		private System.Windows.Forms.Button btnUpdate;
		private System.Windows.Forms.TextBox txtName;
		private System.Windows.Forms.Label lblName;
		private System.ComponentModel.IContainer components = null;
		#endregion

		#region Constructors
		public UCPlane() {
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call
		}
		#endregion

		#region Exposed Properties
		public override void Get(object o) {
			CPlane plane = (CPlane)o;
			plane.Normal = ucNormal.Value;
			plane.Distance = Distance;
			plane.Name = PlaneName;
			plane.InitializeGraphics();
		}

		public override void Set(object o) {
			CPlane plane = (CPlane)o;
			ucNormal.Value = plane.Normal;
			Distance = plane.Distance;
			PlaneName = plane.Name;
		}

		public float Distance {
			get { return (float)Convert.ToDouble(txtDistance.Text); }
			set { txtDistance.Text = value.ToString(); }
		}

		public string PlaneName {
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
			this.groupPlane = new System.Windows.Forms.GroupBox();
			this.txtName = new System.Windows.Forms.TextBox();
			this.lblName = new System.Windows.Forms.Label();
			this.btnReset = new System.Windows.Forms.Button();
			this.btnUpdate = new System.Windows.Forms.Button();
			this.txtDistance = new System.Windows.Forms.TextBox();
			this.lblDistance = new System.Windows.Forms.Label();
			this.ucNormal = new MyGelloidSharp.UserControls.UCVector3();
			this.lblNormal = new System.Windows.Forms.Label();
			this.groupPlane.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupPlane
			// 
			this.groupPlane.Controls.Add(this.txtName);
			this.groupPlane.Controls.Add(this.lblName);
			this.groupPlane.Controls.Add(this.btnReset);
			this.groupPlane.Controls.Add(this.btnUpdate);
			this.groupPlane.Controls.Add(this.txtDistance);
			this.groupPlane.Controls.Add(this.lblDistance);
			this.groupPlane.Controls.Add(this.ucNormal);
			this.groupPlane.Controls.Add(this.lblNormal);
			this.groupPlane.Location = new System.Drawing.Point(0, 0);
			this.groupPlane.Name = "groupPlane";
			this.groupPlane.Size = new System.Drawing.Size(368, 144);
			this.groupPlane.TabIndex = 0;
			this.groupPlane.TabStop = false;
			this.groupPlane.Text = "Plane";
			// 
			// txtName
			// 
			this.txtName.Location = new System.Drawing.Point(48, 16);
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(200, 20);
			this.txtName.TabIndex = 19;
			this.txtName.Text = "";
			// 
			// lblName
			// 
			this.lblName.Location = new System.Drawing.Point(8, 16);
			this.lblName.Name = "lblName";
			this.lblName.Size = new System.Drawing.Size(40, 23);
			this.lblName.TabIndex = 18;
			this.lblName.Text = "Name";
			// 
			// btnReset
			// 
			this.btnReset.Location = new System.Drawing.Point(264, 112);
			this.btnReset.Name = "btnReset";
			this.btnReset.TabIndex = 13;
			this.btnReset.Text = "Reset";
			this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
			// 
			// btnUpdate
			// 
			this.btnUpdate.Location = new System.Drawing.Point(144, 112);
			this.btnUpdate.Name = "btnUpdate";
			this.btnUpdate.TabIndex = 12;
			this.btnUpdate.Text = "Update";
			this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
			// 
			// txtDistance
			// 
			this.txtDistance.Location = new System.Drawing.Point(8, 112);
			this.txtDistance.Name = "txtDistance";
			this.txtDistance.TabIndex = 3;
			this.txtDistance.Text = "";
			// 
			// lblDistance
			// 
			this.lblDistance.Location = new System.Drawing.Point(8, 96);
			this.lblDistance.Name = "lblDistance";
			this.lblDistance.TabIndex = 2;
			this.lblDistance.Text = "Distance to Origin";
			// 
			// ucNormal
			// 
			this.ucNormal.Location = new System.Drawing.Point(8, 56);
			this.ucNormal.Name = "ucNormal";
			this.ucNormal.Size = new System.Drawing.Size(240, 40);
			this.ucNormal.TabIndex = 1;
			// 
			// lblNormal
			// 
			this.lblNormal.Location = new System.Drawing.Point(8, 40);
			this.lblNormal.Name = "lblNormal";
			this.lblNormal.TabIndex = 0;
			this.lblNormal.Text = "Normal";
			// 
			// UCPlane
			// 
			this.Controls.Add(this.groupPlane);
			this.Name = "UCPlane";
			this.Size = new System.Drawing.Size(368, 144);
			this.groupPlane.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

	}
}

