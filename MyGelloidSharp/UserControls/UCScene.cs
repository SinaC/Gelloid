using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MyGelloidSharp.UserControls {
	public class UCScene : UCRoot {
		#region Fields
		private System.Windows.Forms.GroupBox groupScene;
		private MyGelloidSharp.UserControls.UCVector3 ucGravity;
		private System.Windows.Forms.Label lblGravity;
		private System.Windows.Forms.Label lblWindDirection;
		private MyGelloidSharp.UserControls.UCVector3 ucWindDirection;
		private System.Windows.Forms.Label lblWindSpeed;
		private System.Windows.Forms.TextBox txtWindSpeed;
		private System.Windows.Forms.Button btnReset;
		private System.Windows.Forms.Button btnUpdate;
		private System.Windows.Forms.TextBox txtName;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.Label lblMouseKs;
		private System.Windows.Forms.TextBox txtMouseKs;
		private System.ComponentModel.IContainer components = null;
		#endregion

		#region Constructors
		public UCScene() {
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call
		}
		#endregion

		#region Exposed Properties
		public override void Get(object o) {
			CScene scene = (CScene)o;
			scene.Gravity = ucGravity.Value;
			scene.WindDirection = ucWindDirection.Value;
			scene.WindSpeed = SceneWindSpeed;
			scene.MouseKs = SceneMouseKs;
			scene.Name = SceneName;
		}

		public override void Set(object o) {
			CScene scene = (CScene)o;
			ucGravity.Value = scene.Gravity;
			ucWindDirection.Value = scene.WindDirection;
			SceneWindSpeed = scene.WindSpeed;
			SceneMouseKs = scene.MouseKs;
			SceneName = scene.Name;
		}

		public float SceneWindSpeed {
			get { return (float)Convert.ToDouble(txtWindSpeed.Text); }
			set { txtWindSpeed.Text = value.ToString(); }
		}

		public string SceneName {
			get { return txtName.Text; }
			set { txtName.Text = value; }
		}

		public float SceneMouseKs {
			get { return (float)Convert.ToDouble(txtMouseKs.Text); }
			set { txtMouseKs.Text = value.ToString(); }
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
			this.groupScene = new System.Windows.Forms.GroupBox();
			this.txtMouseKs = new System.Windows.Forms.TextBox();
			this.lblMouseKs = new System.Windows.Forms.Label();
			this.txtName = new System.Windows.Forms.TextBox();
			this.lblName = new System.Windows.Forms.Label();
			this.btnReset = new System.Windows.Forms.Button();
			this.btnUpdate = new System.Windows.Forms.Button();
			this.txtWindSpeed = new System.Windows.Forms.TextBox();
			this.lblWindSpeed = new System.Windows.Forms.Label();
			this.lblWindDirection = new System.Windows.Forms.Label();
			this.ucWindDirection = new MyGelloidSharp.UserControls.UCVector3();
			this.lblGravity = new System.Windows.Forms.Label();
			this.ucGravity = new MyGelloidSharp.UserControls.UCVector3();
			this.groupScene.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupScene
			// 
			this.groupScene.Controls.Add(this.txtMouseKs);
			this.groupScene.Controls.Add(this.lblMouseKs);
			this.groupScene.Controls.Add(this.txtName);
			this.groupScene.Controls.Add(this.lblName);
			this.groupScene.Controls.Add(this.btnReset);
			this.groupScene.Controls.Add(this.btnUpdate);
			this.groupScene.Controls.Add(this.txtWindSpeed);
			this.groupScene.Controls.Add(this.lblWindSpeed);
			this.groupScene.Controls.Add(this.lblWindDirection);
			this.groupScene.Controls.Add(this.ucWindDirection);
			this.groupScene.Controls.Add(this.lblGravity);
			this.groupScene.Controls.Add(this.ucGravity);
			this.groupScene.Location = new System.Drawing.Point(0, 0);
			this.groupScene.Name = "groupScene";
			this.groupScene.Size = new System.Drawing.Size(272, 288);
			this.groupScene.TabIndex = 0;
			this.groupScene.TabStop = false;
			this.groupScene.Text = "Scene";
			// 
			// txtMouseKs
			// 
			this.txtMouseKs.Location = new System.Drawing.Point(96, 224);
			this.txtMouseKs.Name = "txtMouseKs";
			this.txtMouseKs.TabIndex = 19;
			this.txtMouseKs.Text = "";
			// 
			// lblMouseKs
			// 
			this.lblMouseKs.Location = new System.Drawing.Point(16, 224);
			this.lblMouseKs.Name = "lblMouseKs";
			this.lblMouseKs.Size = new System.Drawing.Size(72, 23);
			this.lblMouseKs.TabIndex = 18;
			this.lblMouseKs.Text = "Mouse Ks";
			// 
			// txtName
			// 
			this.txtName.Location = new System.Drawing.Point(56, 24);
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(192, 20);
			this.txtName.TabIndex = 17;
			this.txtName.Text = "";
			// 
			// lblName
			// 
			this.lblName.Location = new System.Drawing.Point(16, 24);
			this.lblName.Name = "lblName";
			this.lblName.Size = new System.Drawing.Size(40, 23);
			this.lblName.TabIndex = 16;
			this.lblName.Text = "Name";
			// 
			// btnReset
			// 
			this.btnReset.Location = new System.Drawing.Point(160, 256);
			this.btnReset.Name = "btnReset";
			this.btnReset.TabIndex = 11;
			this.btnReset.Text = "Reset";
			this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
			// 
			// btnUpdate
			// 
			this.btnUpdate.Location = new System.Drawing.Point(40, 256);
			this.btnUpdate.Name = "btnUpdate";
			this.btnUpdate.TabIndex = 10;
			this.btnUpdate.Text = "Update";
			this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
			// 
			// txtWindSpeed
			// 
			this.txtWindSpeed.Location = new System.Drawing.Point(96, 192);
			this.txtWindSpeed.Name = "txtWindSpeed";
			this.txtWindSpeed.TabIndex = 6;
			this.txtWindSpeed.Text = "";
			// 
			// lblWindSpeed
			// 
			this.lblWindSpeed.Location = new System.Drawing.Point(16, 192);
			this.lblWindSpeed.Name = "lblWindSpeed";
			this.lblWindSpeed.TabIndex = 5;
			this.lblWindSpeed.Text = "Wind Speed";
			// 
			// lblWindDirection
			// 
			this.lblWindDirection.Location = new System.Drawing.Point(16, 120);
			this.lblWindDirection.Name = "lblWindDirection";
			this.lblWindDirection.TabIndex = 4;
			this.lblWindDirection.Text = "Wind Direction";
			// 
			// ucWindDirection
			// 
			this.ucWindDirection.Location = new System.Drawing.Point(16, 144);
			this.ucWindDirection.Name = "ucWindDirection";
			this.ucWindDirection.Size = new System.Drawing.Size(240, 40);
			this.ucWindDirection.TabIndex = 3;
			// 
			// lblGravity
			// 
			this.lblGravity.Location = new System.Drawing.Point(16, 56);
			this.lblGravity.Name = "lblGravity";
			this.lblGravity.TabIndex = 2;
			this.lblGravity.Text = "Gravity";
			// 
			// ucGravity
			// 
			this.ucGravity.Location = new System.Drawing.Point(16, 80);
			this.ucGravity.Name = "ucGravity";
			this.ucGravity.Size = new System.Drawing.Size(240, 40);
			this.ucGravity.TabIndex = 0;
			// 
			// UCScene
			// 
			this.Controls.Add(this.groupScene);
			this.Name = "UCScene";
			this.Size = new System.Drawing.Size(272, 288);
			this.groupScene.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion
	}
}

