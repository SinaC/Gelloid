using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using Microsoft.DirectX;

using MyGelloidSharp.Cameras;

namespace MyGelloidSharp.UserControls {
	/// <summary>
	/// Summary description for UCCamera.
	/// </summary>
	public class UCCamera : UCRoot {
		#region Fields
		private System.Windows.Forms.GroupBox groupCamera;
		private System.Windows.Forms.Label lblPos;
		private System.Windows.Forms.Label lblLook;
		private System.Windows.Forms.Label lblUp;
		private System.Windows.Forms.Label lblRight;
		private System.Windows.Forms.ComboBox comboKind;
		private System.Windows.Forms.Label lblFov;
		private System.Windows.Forms.TextBox txtFov;
		private System.Windows.Forms.Label lblRatio;
		private System.Windows.Forms.TextBox txtRatio;
		private System.Windows.Forms.TextBox txtNear;
		private System.Windows.Forms.Label lblNear;
		private System.Windows.Forms.TextBox txtFar;
		private System.Windows.Forms.Label lblFar;
		private System.Windows.Forms.Label lblKind;
		private System.Windows.Forms.Button btnUpdate;
		private System.Windows.Forms.Button btnReset;
		private MyGelloidSharp.UserControls.UCVector3 ucPosition;
		private MyGelloidSharp.UserControls.UCVector3 ucLook;
		private MyGelloidSharp.UserControls.UCVector3 ucUp;
		private MyGelloidSharp.UserControls.UCVector3 ucRight;
		private System.Windows.Forms.TextBox txtName;
		private System.Windows.Forms.Label lblName;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		#endregion

		#region Constructors
		public UCCamera() {
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			comboKind.Items.Clear();
			foreach ( string s in Enum.GetNames(typeof(CCamera.ECameraType)) )
				comboKind.Items.Add( s );
		}
		#endregion

		#region Exposed Properties
		public override void Get( object o ) {
			CCamera camera = (CCamera)o;
			camera.Pos = CameraPosition;
			camera.Look = CameraLook;
			camera.Up = CameraUp;
			camera.Right = CameraRight;
			camera.CameraType = CameraKind;
			camera.Fov = CameraFov;
			camera.Ratio = CameraRatio;
			camera.NearClip = CameraNearClip;
			camera.FarClip = CameraFarClip;
			camera.Name = CameraName;
		}

		public override void Set( object o ) {
			CCamera camera = (CCamera)o;
			CameraPosition = camera.Pos;
			CameraLook = camera.Look;
			CameraUp = camera.Up;
			CameraRight = camera.Right;
			CameraKind = camera.CameraType;
			CameraFov = camera.Fov;
			CameraRatio = camera.Ratio;
			CameraNearClip = camera.NearClip;
			CameraFarClip = camera.FarClip;
			CameraName = camera.Name;
		}

		public Vector3 CameraPosition {
			get { return ucPosition.Value; }
			set { ucPosition.Value = value; }
		}

		public Vector3 CameraLook {
			get { return ucLook.Value; }
			set { ucLook.Value = value; }
		}

		public Vector3 CameraUp {
			get { return ucUp.Value; }
			set { ucUp.Value = value; }
		}

		public Vector3 CameraRight {
			get { return ucRight.Value; }
			set { ucRight.Value = value; }
		}

		public CCamera.ECameraType CameraKind {
			get {
				foreach ( CCamera.ECameraType kind in Enum.GetValues( typeof(CCamera.ECameraType) ) )
					if ( Enum.GetName( typeof(CCamera.ECameraType), kind ) == comboKind.Text )
						return kind;
				return CCamera.ECameraType.Mixed;
			}
			set {
				foreach( string item in comboKind.Items )
					if ( item == Enum.GetName( typeof(CCamera.ECameraType), value ) ) {
						comboKind.Text = item;
						break;
					}
			}
		}

		public float CameraFov {
			get { return (float)Convert.ToDouble(txtFov.Text); }
			set { txtFov.Text = value.ToString(); }
		}

		public float CameraRatio {
			get { return (float)Convert.ToDouble(txtRatio.Text); }
			set { txtRatio.Text = value.ToString(); }
		}

		public float CameraNearClip {
			get { return (float)Convert.ToDouble(txtNear.Text); }
			set { txtNear.Text = value.ToString(); }
		}

		public float CameraFarClip {
			get { return (float)Convert.ToDouble(txtFar.Text); }
			set { txtFar.Text = value.ToString(); }
		}

		public string CameraName {
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
			this.groupCamera = new System.Windows.Forms.GroupBox();
			this.ucRight = new MyGelloidSharp.UserControls.UCVector3();
			this.ucUp = new MyGelloidSharp.UserControls.UCVector3();
			this.ucLook = new MyGelloidSharp.UserControls.UCVector3();
			this.ucPosition = new MyGelloidSharp.UserControls.UCVector3();
			this.btnReset = new System.Windows.Forms.Button();
			this.btnUpdate = new System.Windows.Forms.Button();
			this.lblKind = new System.Windows.Forms.Label();
			this.txtFar = new System.Windows.Forms.TextBox();
			this.lblFar = new System.Windows.Forms.Label();
			this.txtNear = new System.Windows.Forms.TextBox();
			this.lblNear = new System.Windows.Forms.Label();
			this.txtRatio = new System.Windows.Forms.TextBox();
			this.lblRatio = new System.Windows.Forms.Label();
			this.txtFov = new System.Windows.Forms.TextBox();
			this.lblFov = new System.Windows.Forms.Label();
			this.comboKind = new System.Windows.Forms.ComboBox();
			this.lblRight = new System.Windows.Forms.Label();
			this.lblUp = new System.Windows.Forms.Label();
			this.lblLook = new System.Windows.Forms.Label();
			this.lblPos = new System.Windows.Forms.Label();
			this.txtName = new System.Windows.Forms.TextBox();
			this.lblName = new System.Windows.Forms.Label();
			this.groupCamera.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupCamera
			// 
			this.groupCamera.Controls.Add(this.txtName);
			this.groupCamera.Controls.Add(this.lblName);
			this.groupCamera.Controls.Add(this.ucRight);
			this.groupCamera.Controls.Add(this.ucUp);
			this.groupCamera.Controls.Add(this.ucLook);
			this.groupCamera.Controls.Add(this.ucPosition);
			this.groupCamera.Controls.Add(this.btnReset);
			this.groupCamera.Controls.Add(this.btnUpdate);
			this.groupCamera.Controls.Add(this.lblKind);
			this.groupCamera.Controls.Add(this.txtFar);
			this.groupCamera.Controls.Add(this.lblFar);
			this.groupCamera.Controls.Add(this.txtNear);
			this.groupCamera.Controls.Add(this.lblNear);
			this.groupCamera.Controls.Add(this.txtRatio);
			this.groupCamera.Controls.Add(this.lblRatio);
			this.groupCamera.Controls.Add(this.txtFov);
			this.groupCamera.Controls.Add(this.lblFov);
			this.groupCamera.Controls.Add(this.comboKind);
			this.groupCamera.Controls.Add(this.lblRight);
			this.groupCamera.Controls.Add(this.lblUp);
			this.groupCamera.Controls.Add(this.lblLook);
			this.groupCamera.Controls.Add(this.lblPos);
			this.groupCamera.Location = new System.Drawing.Point(0, 0);
			this.groupCamera.Name = "groupCamera";
			this.groupCamera.Size = new System.Drawing.Size(448, 280);
			this.groupCamera.TabIndex = 0;
			this.groupCamera.TabStop = false;
			this.groupCamera.Text = "Camera";
			// 
			// ucRight
			// 
			this.ucRight.Location = new System.Drawing.Point(8, 232);
			this.ucRight.Name = "ucRight";
			this.ucRight.Size = new System.Drawing.Size(240, 40);
			this.ucRight.TabIndex = 23;
			// 
			// ucUp
			// 
			this.ucUp.Location = new System.Drawing.Point(8, 176);
			this.ucUp.Name = "ucUp";
			this.ucUp.Size = new System.Drawing.Size(240, 40);
			this.ucUp.TabIndex = 22;
			// 
			// ucLook
			// 
			this.ucLook.Location = new System.Drawing.Point(8, 120);
			this.ucLook.Name = "ucLook";
			this.ucLook.Size = new System.Drawing.Size(240, 40);
			this.ucLook.TabIndex = 21;
			// 
			// ucPosition
			// 
			this.ucPosition.Location = new System.Drawing.Point(8, 56);
			this.ucPosition.Name = "ucPosition";
			this.ucPosition.Size = new System.Drawing.Size(240, 40);
			this.ucPosition.TabIndex = 20;
			// 
			// btnReset
			// 
			this.btnReset.Location = new System.Drawing.Point(360, 240);
			this.btnReset.Name = "btnReset";
			this.btnReset.TabIndex = 19;
			this.btnReset.Text = "Reset";
			this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
			// 
			// btnUpdate
			// 
			this.btnUpdate.Location = new System.Drawing.Point(264, 240);
			this.btnUpdate.Name = "btnUpdate";
			this.btnUpdate.TabIndex = 18;
			this.btnUpdate.Text = "Update";
			this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
			// 
			// lblKind
			// 
			this.lblKind.Location = new System.Drawing.Point(200, 16);
			this.lblKind.Name = "lblKind";
			this.lblKind.Size = new System.Drawing.Size(40, 23);
			this.lblKind.TabIndex = 17;
			this.lblKind.Text = "Kind";
			// 
			// txtFar
			// 
			this.txtFar.Location = new System.Drawing.Point(336, 152);
			this.txtFar.Name = "txtFar";
			this.txtFar.TabIndex = 16;
			this.txtFar.Text = "";
			// 
			// lblFar
			// 
			this.lblFar.Location = new System.Drawing.Point(264, 152);
			this.lblFar.Name = "lblFar";
			this.lblFar.Size = new System.Drawing.Size(64, 23);
			this.lblFar.TabIndex = 15;
			this.lblFar.Text = "Far Clip";
			// 
			// txtNear
			// 
			this.txtNear.Location = new System.Drawing.Point(336, 128);
			this.txtNear.Name = "txtNear";
			this.txtNear.TabIndex = 14;
			this.txtNear.Text = "";
			// 
			// lblNear
			// 
			this.lblNear.Location = new System.Drawing.Point(264, 128);
			this.lblNear.Name = "lblNear";
			this.lblNear.Size = new System.Drawing.Size(64, 23);
			this.lblNear.TabIndex = 13;
			this.lblNear.Text = "Near Clip";
			// 
			// txtRatio
			// 
			this.txtRatio.Location = new System.Drawing.Point(336, 104);
			this.txtRatio.Name = "txtRatio";
			this.txtRatio.TabIndex = 12;
			this.txtRatio.Text = "";
			// 
			// lblRatio
			// 
			this.lblRatio.Location = new System.Drawing.Point(264, 104);
			this.lblRatio.Name = "lblRatio";
			this.lblRatio.Size = new System.Drawing.Size(64, 23);
			this.lblRatio.TabIndex = 11;
			this.lblRatio.Text = "Ratio";
			// 
			// txtFov
			// 
			this.txtFov.Location = new System.Drawing.Point(336, 80);
			this.txtFov.Name = "txtFov";
			this.txtFov.TabIndex = 10;
			this.txtFov.Text = "";
			// 
			// lblFov
			// 
			this.lblFov.Location = new System.Drawing.Point(264, 80);
			this.lblFov.Name = "lblFov";
			this.lblFov.Size = new System.Drawing.Size(64, 23);
			this.lblFov.TabIndex = 9;
			this.lblFov.Text = "FOV";
			// 
			// comboKind
			// 
			this.comboKind.Location = new System.Drawing.Point(240, 16);
			this.comboKind.Name = "comboKind";
			this.comboKind.Size = new System.Drawing.Size(121, 21);
			this.comboKind.TabIndex = 8;
			// 
			// lblRight
			// 
			this.lblRight.Location = new System.Drawing.Point(8, 216);
			this.lblRight.Name = "lblRight";
			this.lblRight.TabIndex = 3;
			this.lblRight.Text = "Right";
			// 
			// lblUp
			// 
			this.lblUp.Location = new System.Drawing.Point(8, 160);
			this.lblUp.Name = "lblUp";
			this.lblUp.TabIndex = 2;
			this.lblUp.Text = "Up";
			// 
			// lblLook
			// 
			this.lblLook.Location = new System.Drawing.Point(8, 104);
			this.lblLook.Name = "lblLook";
			this.lblLook.TabIndex = 1;
			this.lblLook.Text = "Look";
			// 
			// lblPos
			// 
			this.lblPos.Location = new System.Drawing.Point(8, 40);
			this.lblPos.Name = "lblPos";
			this.lblPos.TabIndex = 0;
			this.lblPos.Text = "Position";
			// 
			// txtName
			// 
			this.txtName.Location = new System.Drawing.Point(48, 16);
			this.txtName.Name = "txtName";
			this.txtName.TabIndex = 25;
			this.txtName.Text = "";
			// 
			// lblName
			// 
			this.lblName.Location = new System.Drawing.Point(8, 16);
			this.lblName.Name = "lblName";
			this.lblName.Size = new System.Drawing.Size(40, 23);
			this.lblName.TabIndex = 24;
			this.lblName.Text = "Name";
			// 
			// UCCamera
			// 
			this.Controls.Add(this.groupCamera);
			this.Name = "UCCamera";
			this.Size = new System.Drawing.Size(448, 280);
			this.groupCamera.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion
	}
}