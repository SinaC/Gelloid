using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using MyGelloidSharp.PhysEnv;

namespace MyGelloidSharp.UserControls {
	/// <summary>
	/// Summary description for UCGelloid.
	/// </summary>
	public class UCGelloid : UCRoot {
		#region Fields
		private System.Windows.Forms.Label lblKr;
		private System.Windows.Forms.TextBox txtKr;
		private System.Windows.Forms.Label lblKd;
		private System.Windows.Forms.TextBox txtKd;
		private System.Windows.Forms.Label lblCsf;
		private System.Windows.Forms.TextBox txtCsf;
		private System.Windows.Forms.Label lblCkf;
		private System.Windows.Forms.TextBox txtCkf;
		private System.Windows.Forms.GroupBox groupGelloid;
		private System.Windows.Forms.Button btnUpdate;
		private System.Windows.Forms.Button btnReset;
		private System.Windows.Forms.CheckBox chkUseGravity;
		private System.Windows.Forms.CheckBox chkUseDamping;
		private System.Windows.Forms.CheckBox chkUseWind;
		private System.Windows.Forms.CheckBox chkUseFriction;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.TextBox txtName;
		private System.Windows.Forms.Label lblParticleCount;
		private System.Windows.Forms.Label lblSpringCount;
		private System.Windows.Forms.Label lblTriangleCount;
		private System.Windows.Forms.Label lblStructuralCount;
		private System.Windows.Forms.Label lblShearCount;
		private System.Windows.Forms.Label lblBendCount;
		private System.Windows.Forms.Label lblManualCount;
		private System.Windows.Forms.Label lblBrokenCount;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		#endregion

		#region Constructors
		public UCGelloid() {
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call

		}
		#endregion

		#region Exposed Properties
		public override void Get( object o ) {	
			CGelloid gelloid = (CGelloid)o;
			gelloid.Kr = GelloidKr;
			gelloid.Kd = GelloidKd;
			gelloid.CKf = GelloidCKf;
			gelloid.CSf = GelloidCSf;
			gelloid.UseGravity = GelloidUseGravity;
			gelloid.UseDamping = GelloidUseDamping;
			gelloid.UseWind = GelloidUseWind;
			gelloid.UseFriction = GelloidUseFriction;
			gelloid.Name = GelloidName;
		}

		public override void Set( object o ) {
			CGelloid gelloid = (CGelloid)o;
			GelloidKr = gelloid.Kr;
			GelloidKd = gelloid.Kd;
			GelloidCKf = gelloid.CKf;
			GelloidCSf = gelloid.CSf;
			GelloidUseGravity = gelloid.UseGravity;
			GelloidUseDamping = gelloid.UseDamping;
			GelloidUseWind = gelloid.UseWind;
			GelloidUseFriction = gelloid.UseFriction;
			GelloidName = gelloid.Name;
			int structural = 0;
			int shear = 0;
			int bend = 0;
			int manual = 0;
			int broken = 0;
			foreach ( CSpring spring in gelloid.Springs ) {
				if ( spring.Kind == CSpring.ESpringKind.Structural ) structural++;
				else if ( spring.Kind == CSpring.ESpringKind.Shear ) shear++;
				else if ( spring.Kind == CSpring.ESpringKind.Bend ) bend++;
				else if ( spring.Kind == CSpring.ESpringKind.Manual ) manual++;
				if ( spring.Broken ) broken++;
			}
			lblBendCount.Text = "#Bends="+bend;
			lblBrokenCount.Text = "#Brokens="+broken;
			lblManualCount.Text = "#Manuals="+manual;
			lblParticleCount.Text = "#Particles="+gelloid.ParticlesCurrent.Count;
			lblShearCount.Text = "#Shears="+shear;
			lblSpringCount.Text = "#Springs="+gelloid.Springs.Count;
			lblStructuralCount.Text = "#Structurals="+structural;
			lblTriangleCount.Text = "#Triangles="+gelloid.Triangles.Count;
		}
		
		public float GelloidKr {
			get { return (float)Convert.ToDouble( txtKr.Text ); }
			set { txtKr.Text = value.ToString(); }
		}

		public float GelloidKd {
			get { return (float)Convert.ToDouble( txtKd.Text ); }
			set { txtKd.Text = value.ToString(); }
		}

		public float GelloidCKf {
			get { return (float)Convert.ToDouble( txtCkf.Text ); }
			set { txtCkf.Text = value.ToString(); }
		}

		public float GelloidCSf {
			get { return (float)Convert.ToDouble( txtCsf.Text ); }
			set { txtCsf.Text = value.ToString(); }
		}

		public bool GelloidUseGravity {
			get { return chkUseGravity.Checked; }
			set { chkUseGravity.Checked = value; }
		}

		public bool GelloidUseDamping {
			get { return chkUseDamping.Checked; }
			set { chkUseDamping.Checked = value; }
		}
		
		public bool GelloidUseWind {
			get { return chkUseWind.Checked; }
			set { chkUseWind.Checked = value; }
		}
		
		public bool GelloidUseFriction {
			get { return chkUseFriction.Checked; }
			set { chkUseFriction.Checked = value; }
		}

		public string GelloidName {
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
			this.lblKr = new System.Windows.Forms.Label();
			this.txtKr = new System.Windows.Forms.TextBox();
			this.groupGelloid = new System.Windows.Forms.GroupBox();
			this.lblBrokenCount = new System.Windows.Forms.Label();
			this.lblManualCount = new System.Windows.Forms.Label();
			this.lblBendCount = new System.Windows.Forms.Label();
			this.lblShearCount = new System.Windows.Forms.Label();
			this.lblStructuralCount = new System.Windows.Forms.Label();
			this.lblTriangleCount = new System.Windows.Forms.Label();
			this.lblSpringCount = new System.Windows.Forms.Label();
			this.lblParticleCount = new System.Windows.Forms.Label();
			this.txtName = new System.Windows.Forms.TextBox();
			this.lblName = new System.Windows.Forms.Label();
			this.chkUseFriction = new System.Windows.Forms.CheckBox();
			this.chkUseWind = new System.Windows.Forms.CheckBox();
			this.chkUseDamping = new System.Windows.Forms.CheckBox();
			this.chkUseGravity = new System.Windows.Forms.CheckBox();
			this.btnReset = new System.Windows.Forms.Button();
			this.btnUpdate = new System.Windows.Forms.Button();
			this.lblCkf = new System.Windows.Forms.Label();
			this.txtCkf = new System.Windows.Forms.TextBox();
			this.lblCsf = new System.Windows.Forms.Label();
			this.txtCsf = new System.Windows.Forms.TextBox();
			this.lblKd = new System.Windows.Forms.Label();
			this.txtKd = new System.Windows.Forms.TextBox();
			this.groupGelloid.SuspendLayout();
			this.SuspendLayout();
			// 
			// lblKr
			// 
			this.lblKr.Location = new System.Drawing.Point(16, 40);
			this.lblKr.Name = "lblKr";
			this.lblKr.TabIndex = 0;
			this.lblKr.Text = "Bounce Factor (Kr)";
			// 
			// txtKr
			// 
			this.txtKr.Location = new System.Drawing.Point(16, 64);
			this.txtKr.Name = "txtKr";
			this.txtKr.TabIndex = 1;
			this.txtKr.Text = "";
			// 
			// groupGelloid
			// 
			this.groupGelloid.Controls.Add(this.lblBrokenCount);
			this.groupGelloid.Controls.Add(this.lblManualCount);
			this.groupGelloid.Controls.Add(this.lblBendCount);
			this.groupGelloid.Controls.Add(this.lblShearCount);
			this.groupGelloid.Controls.Add(this.lblStructuralCount);
			this.groupGelloid.Controls.Add(this.lblTriangleCount);
			this.groupGelloid.Controls.Add(this.lblSpringCount);
			this.groupGelloid.Controls.Add(this.lblParticleCount);
			this.groupGelloid.Controls.Add(this.txtName);
			this.groupGelloid.Controls.Add(this.lblName);
			this.groupGelloid.Controls.Add(this.chkUseFriction);
			this.groupGelloid.Controls.Add(this.chkUseWind);
			this.groupGelloid.Controls.Add(this.chkUseDamping);
			this.groupGelloid.Controls.Add(this.chkUseGravity);
			this.groupGelloid.Controls.Add(this.btnReset);
			this.groupGelloid.Controls.Add(this.btnUpdate);
			this.groupGelloid.Controls.Add(this.lblCkf);
			this.groupGelloid.Controls.Add(this.txtCkf);
			this.groupGelloid.Controls.Add(this.lblCsf);
			this.groupGelloid.Controls.Add(this.txtCsf);
			this.groupGelloid.Controls.Add(this.lblKd);
			this.groupGelloid.Controls.Add(this.txtKd);
			this.groupGelloid.Controls.Add(this.lblKr);
			this.groupGelloid.Controls.Add(this.txtKr);
			this.groupGelloid.Location = new System.Drawing.Point(0, 0);
			this.groupGelloid.Name = "groupGelloid";
			this.groupGelloid.Size = new System.Drawing.Size(400, 240);
			this.groupGelloid.TabIndex = 2;
			this.groupGelloid.TabStop = false;
			this.groupGelloid.Text = "Gelloid";
			// 
			// lblBrokenCount
			// 
			this.lblBrokenCount.Location = new System.Drawing.Point(248, 192);
			this.lblBrokenCount.Name = "lblBrokenCount";
			this.lblBrokenCount.TabIndex = 23;
			// 
			// lblManualCount
			// 
			this.lblManualCount.Location = new System.Drawing.Point(248, 168);
			this.lblManualCount.Name = "lblManualCount";
			this.lblManualCount.TabIndex = 22;
			// 
			// lblBendCount
			// 
			this.lblBendCount.Location = new System.Drawing.Point(248, 144);
			this.lblBendCount.Name = "lblBendCount";
			this.lblBendCount.TabIndex = 21;
			// 
			// lblShearCount
			// 
			this.lblShearCount.Location = new System.Drawing.Point(248, 120);
			this.lblShearCount.Name = "lblShearCount";
			this.lblShearCount.TabIndex = 20;
			// 
			// lblStructuralCount
			// 
			this.lblStructuralCount.Location = new System.Drawing.Point(248, 96);
			this.lblStructuralCount.Name = "lblStructuralCount";
			this.lblStructuralCount.TabIndex = 19;
			// 
			// lblTriangleCount
			// 
			this.lblTriangleCount.Location = new System.Drawing.Point(248, 72);
			this.lblTriangleCount.Name = "lblTriangleCount";
			this.lblTriangleCount.TabIndex = 18;
			// 
			// lblSpringCount
			// 
			this.lblSpringCount.Location = new System.Drawing.Point(248, 48);
			this.lblSpringCount.Name = "lblSpringCount";
			this.lblSpringCount.TabIndex = 17;
			// 
			// lblParticleCount
			// 
			this.lblParticleCount.Location = new System.Drawing.Point(248, 24);
			this.lblParticleCount.Name = "lblParticleCount";
			this.lblParticleCount.TabIndex = 16;
			// 
			// txtName
			// 
			this.txtName.Location = new System.Drawing.Point(56, 16);
			this.txtName.Name = "txtName";
			this.txtName.Size = new System.Drawing.Size(176, 20);
			this.txtName.TabIndex = 15;
			this.txtName.Text = "";
			// 
			// lblName
			// 
			this.lblName.Location = new System.Drawing.Point(16, 16);
			this.lblName.Name = "lblName";
			this.lblName.Size = new System.Drawing.Size(40, 23);
			this.lblName.TabIndex = 14;
			this.lblName.Text = "Name";
			// 
			// chkUseFriction
			// 
			this.chkUseFriction.Location = new System.Drawing.Point(128, 176);
			this.chkUseFriction.Name = "chkUseFriction";
			this.chkUseFriction.TabIndex = 13;
			this.chkUseFriction.Text = "use friction";
			// 
			// chkUseWind
			// 
			this.chkUseWind.Location = new System.Drawing.Point(128, 152);
			this.chkUseWind.Name = "chkUseWind";
			this.chkUseWind.TabIndex = 12;
			this.chkUseWind.Text = "use wind";
			// 
			// chkUseDamping
			// 
			this.chkUseDamping.Location = new System.Drawing.Point(16, 176);
			this.chkUseDamping.Name = "chkUseDamping";
			this.chkUseDamping.TabIndex = 11;
			this.chkUseDamping.Text = "use damping";
			// 
			// chkUseGravity
			// 
			this.chkUseGravity.Location = new System.Drawing.Point(16, 152);
			this.chkUseGravity.Name = "chkUseGravity";
			this.chkUseGravity.TabIndex = 10;
			this.chkUseGravity.Text = "use gravity";
			// 
			// btnReset
			// 
			this.btnReset.Location = new System.Drawing.Point(136, 208);
			this.btnReset.Name = "btnReset";
			this.btnReset.TabIndex = 9;
			this.btnReset.Text = "Reset";
			this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
			// 
			// btnUpdate
			// 
			this.btnUpdate.Location = new System.Drawing.Point(16, 208);
			this.btnUpdate.Name = "btnUpdate";
			this.btnUpdate.TabIndex = 8;
			this.btnUpdate.Text = "Update";
			this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
			// 
			// lblCkf
			// 
			this.lblCkf.Location = new System.Drawing.Point(136, 88);
			this.lblCkf.Name = "lblCkf";
			this.lblCkf.Size = new System.Drawing.Size(112, 23);
			this.lblCkf.TabIndex = 6;
			this.lblCkf.Text = "Kinetic Friction (CKf)";
			// 
			// txtCkf
			// 
			this.txtCkf.Location = new System.Drawing.Point(136, 112);
			this.txtCkf.Name = "txtCkf";
			this.txtCkf.TabIndex = 7;
			this.txtCkf.Text = "";
			// 
			// lblCsf
			// 
			this.lblCsf.Location = new System.Drawing.Point(136, 40);
			this.lblCsf.Name = "lblCsf";
			this.lblCsf.Size = new System.Drawing.Size(104, 23);
			this.lblCsf.TabIndex = 4;
			this.lblCsf.Text = "Static Friction (CSf)";
			// 
			// txtCsf
			// 
			this.txtCsf.Location = new System.Drawing.Point(136, 64);
			this.txtCsf.Name = "txtCsf";
			this.txtCsf.TabIndex = 5;
			this.txtCsf.Text = "";
			// 
			// lblKd
			// 
			this.lblKd.Location = new System.Drawing.Point(16, 88);
			this.lblKd.Name = "lblKd";
			this.lblKd.Size = new System.Drawing.Size(112, 23);
			this.lblKd.TabIndex = 2;
			this.lblKd.Text = "Damping Factor (Kd)";
			// 
			// txtKd
			// 
			this.txtKd.Location = new System.Drawing.Point(16, 112);
			this.txtKd.Name = "txtKd";
			this.txtKd.TabIndex = 3;
			this.txtKd.Text = "";
			// 
			// UCGelloid
			// 
			this.Controls.Add(this.groupGelloid);
			this.Name = "UCGelloid";
			this.Size = new System.Drawing.Size(416, 240);
			this.groupGelloid.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion
	}
}