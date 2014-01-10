using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using MyGelloidSharp.PhysEnv;

namespace MyGelloidSharp.UserControls {
	public class UCSpring : UCRoot {
		#region Fields
		private System.Windows.Forms.GroupBox groupSpring;
		private System.Windows.Forms.Label lblParticle1;
		private System.Windows.Forms.TextBox txtParticle1;
		private System.Windows.Forms.TextBox txtParticle2;
		private System.Windows.Forms.Label lblParticle2;
		private System.Windows.Forms.ComboBox comboKind;
		private System.Windows.Forms.Label lblKind;
		private System.Windows.Forms.TextBox txtKs;
		private System.Windows.Forms.Label lblKs;
		private System.Windows.Forms.TextBox txtKd;
		private System.Windows.Forms.Label lblKd;
		private System.Windows.Forms.TextBox txtRestLength;
		private System.Windows.Forms.Label lblRestLength;
		private System.Windows.Forms.TextBox txtMaxExtension;
		private System.Windows.Forms.Label lblMaxExtension;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox comboBroken;
		private System.Windows.Forms.Button btnReset;
		private System.Windows.Forms.Button btnUpdate;

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		#endregion

		#region Constructors
		public UCSpring() {
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call
			comboKind.Items.Clear();
			foreach ( string s in Enum.GetNames(typeof(CSpring.ESpringKind)) )
				comboKind.Items.Add( s );
		}
		#endregion

		#region Exposed Properties
		public override void Get( object o ) {	
			CSpring spring = (CSpring)o;
			spring.Index1 = SpringParticle1;
			spring.Index2 = SpringParticle2;
			spring.Kind = SpringKind;
			spring.Ks = SpringKs;
			spring.Kd = SpringKd;
			spring.RestLength = SpringRestLength;
			spring.MaxExtension = SpringMaxExtension;
			spring.Broken = SpringBroken;
		}

		public override void Set( object o ) {
			CSpring spring = (CSpring)o;
			SpringParticle1 = spring.Index1;
			SpringParticle2 = spring.Index2;
			SpringKind = spring.Kind;
			SpringKs = spring.Ks;
			SpringKd = spring.Kd;
			SpringRestLength = spring.RestLength;
			SpringMaxExtension = spring.MaxExtension;
			SpringBroken = spring.Broken;
		}

		public int SpringParticle1 {
			get { return Convert.ToInt32( txtParticle1.Text ); }
			set { txtParticle1.Text = value.ToString(); }
		}

		public int SpringParticle2 {
			get { return Convert.ToInt32( txtParticle2.Text ); }
			set { txtParticle2.Text = value.ToString(); }
		}

		public CSpring.ESpringKind SpringKind {
			get {
				foreach ( CSpring.ESpringKind kind in Enum.GetValues( typeof(CSpring.ESpringKind) ) )
					if ( Enum.GetName( typeof(CSpring.ESpringKind), kind ) == comboKind.Text )
						return kind;
				return CSpring.ESpringKind.Structural;
			}
			set {
				foreach( string item in comboKind.Items )
					if ( item == Enum.GetName( typeof(CSpring.ESpringKind), value ) ) {
						comboKind.Text = item;
						break;
					}
			}
		}

		public float SpringKs {
			get { return (float)Convert.ToDouble( txtKs.Text ); }
			set { txtKs.Text = value.ToString(); }
		}

		public float SpringKd {
			get { return (float)Convert.ToDouble( txtKd.Text ); }
			set { txtKd.Text = value.ToString(); }
		}

		public float SpringRestLength {
			get { return (float)Convert.ToDouble( txtRestLength.Text ); }
			set { txtRestLength.Text = value.ToString(); }
		}

		public float SpringMaxExtension {
			get { return (float)Convert.ToDouble( txtMaxExtension.Text ); }
			set { txtMaxExtension.Text = value.ToString(); }
		}

		public bool SpringBroken {
			get { return Convert.ToBoolean( comboBroken.Text ); }
			set { comboBroken.Text = Convert.ToString( value ); }
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
			this.groupSpring = new System.Windows.Forms.GroupBox();
			this.btnReset = new System.Windows.Forms.Button();
			this.btnUpdate = new System.Windows.Forms.Button();
			this.comboBroken = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.txtMaxExtension = new System.Windows.Forms.TextBox();
			this.lblMaxExtension = new System.Windows.Forms.Label();
			this.txtRestLength = new System.Windows.Forms.TextBox();
			this.lblRestLength = new System.Windows.Forms.Label();
			this.txtKd = new System.Windows.Forms.TextBox();
			this.lblKd = new System.Windows.Forms.Label();
			this.txtKs = new System.Windows.Forms.TextBox();
			this.lblKs = new System.Windows.Forms.Label();
			this.lblKind = new System.Windows.Forms.Label();
			this.comboKind = new System.Windows.Forms.ComboBox();
			this.txtParticle2 = new System.Windows.Forms.TextBox();
			this.lblParticle2 = new System.Windows.Forms.Label();
			this.txtParticle1 = new System.Windows.Forms.TextBox();
			this.lblParticle1 = new System.Windows.Forms.Label();
			this.groupSpring.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupSpring
			// 
			this.groupSpring.Controls.Add(this.btnReset);
			this.groupSpring.Controls.Add(this.btnUpdate);
			this.groupSpring.Controls.Add(this.comboBroken);
			this.groupSpring.Controls.Add(this.label1);
			this.groupSpring.Controls.Add(this.txtMaxExtension);
			this.groupSpring.Controls.Add(this.lblMaxExtension);
			this.groupSpring.Controls.Add(this.txtRestLength);
			this.groupSpring.Controls.Add(this.lblRestLength);
			this.groupSpring.Controls.Add(this.txtKd);
			this.groupSpring.Controls.Add(this.lblKd);
			this.groupSpring.Controls.Add(this.txtKs);
			this.groupSpring.Controls.Add(this.lblKs);
			this.groupSpring.Controls.Add(this.lblKind);
			this.groupSpring.Controls.Add(this.comboKind);
			this.groupSpring.Controls.Add(this.txtParticle2);
			this.groupSpring.Controls.Add(this.lblParticle2);
			this.groupSpring.Controls.Add(this.txtParticle1);
			this.groupSpring.Controls.Add(this.lblParticle1);
			this.groupSpring.Location = new System.Drawing.Point(0, 0);
			this.groupSpring.Name = "groupSpring";
			this.groupSpring.Size = new System.Drawing.Size(256, 264);
			this.groupSpring.TabIndex = 0;
			this.groupSpring.TabStop = false;
			this.groupSpring.Text = "Spring";
			// 
			// btnReset
			// 
			this.btnReset.Location = new System.Drawing.Point(136, 232);
			this.btnReset.Name = "btnReset";
			this.btnReset.TabIndex = 21;
			this.btnReset.Text = "Reset";
			this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
			// 
			// btnUpdate
			// 
			this.btnUpdate.Location = new System.Drawing.Point(40, 232);
			this.btnUpdate.Name = "btnUpdate";
			this.btnUpdate.TabIndex = 20;
			this.btnUpdate.Text = "Update";
			this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
			// 
			// comboBroken
			// 
			this.comboBroken.Items.AddRange(new object[] {
															 "true",
															 "false"});
			this.comboBroken.Location = new System.Drawing.Point(112, 192);
			this.comboBroken.Name = "comboBroken";
			this.comboBroken.Size = new System.Drawing.Size(121, 21);
			this.comboBroken.TabIndex = 15;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 192);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(72, 23);
			this.label1.TabIndex = 14;
			this.label1.Text = "Broken";
			// 
			// txtMaxExtension
			// 
			this.txtMaxExtension.Location = new System.Drawing.Point(136, 168);
			this.txtMaxExtension.Name = "txtMaxExtension";
			this.txtMaxExtension.TabIndex = 13;
			this.txtMaxExtension.Text = "";
			// 
			// lblMaxExtension
			// 
			this.lblMaxExtension.Location = new System.Drawing.Point(136, 144);
			this.lblMaxExtension.Name = "lblMaxExtension";
			this.lblMaxExtension.Size = new System.Drawing.Size(112, 23);
			this.lblMaxExtension.TabIndex = 12;
			this.lblMaxExtension.Text = "Max Extension";
			// 
			// txtRestLength
			// 
			this.txtRestLength.Location = new System.Drawing.Point(16, 168);
			this.txtRestLength.Name = "txtRestLength";
			this.txtRestLength.TabIndex = 11;
			this.txtRestLength.Text = "";
			// 
			// lblRestLength
			// 
			this.lblRestLength.Location = new System.Drawing.Point(16, 144);
			this.lblRestLength.Name = "lblRestLength";
			this.lblRestLength.Size = new System.Drawing.Size(112, 23);
			this.lblRestLength.TabIndex = 10;
			this.lblRestLength.Text = "Rest Length";
			// 
			// txtKd
			// 
			this.txtKd.Location = new System.Drawing.Point(136, 120);
			this.txtKd.Name = "txtKd";
			this.txtKd.TabIndex = 9;
			this.txtKd.Text = "";
			// 
			// lblKd
			// 
			this.lblKd.Location = new System.Drawing.Point(136, 96);
			this.lblKd.Name = "lblKd";
			this.lblKd.Size = new System.Drawing.Size(112, 23);
			this.lblKd.TabIndex = 8;
			this.lblKd.Text = "Damping Factor (Kd)";
			// 
			// txtKs
			// 
			this.txtKs.Location = new System.Drawing.Point(16, 120);
			this.txtKs.Name = "txtKs";
			this.txtKs.TabIndex = 7;
			this.txtKs.Text = "";
			// 
			// lblKs
			// 
			this.lblKs.Location = new System.Drawing.Point(16, 96);
			this.lblKs.Name = "lblKs";
			this.lblKs.Size = new System.Drawing.Size(104, 23);
			this.lblKs.TabIndex = 6;
			this.lblKs.Text = "Hook Constant (Ks)";
			// 
			// lblKind
			// 
			this.lblKind.Location = new System.Drawing.Point(16, 72);
			this.lblKind.Name = "lblKind";
			this.lblKind.Size = new System.Drawing.Size(80, 23);
			this.lblKind.TabIndex = 5;
			this.lblKind.Text = "Kind";
			// 
			// comboKind
			// 
			this.comboKind.Location = new System.Drawing.Point(112, 72);
			this.comboKind.Name = "comboKind";
			this.comboKind.Size = new System.Drawing.Size(121, 21);
			this.comboKind.TabIndex = 4;
			// 
			// txtParticle2
			// 
			this.txtParticle2.Location = new System.Drawing.Point(136, 48);
			this.txtParticle2.Name = "txtParticle2";
			this.txtParticle2.TabIndex = 3;
			this.txtParticle2.Text = "";
			// 
			// lblParticle2
			// 
			this.lblParticle2.Location = new System.Drawing.Point(136, 24);
			this.lblParticle2.Name = "lblParticle2";
			this.lblParticle2.TabIndex = 2;
			this.lblParticle2.Text = "Particle 2";
			// 
			// txtParticle1
			// 
			this.txtParticle1.Location = new System.Drawing.Point(16, 48);
			this.txtParticle1.Name = "txtParticle1";
			this.txtParticle1.TabIndex = 1;
			this.txtParticle1.Text = "";
			// 
			// lblParticle1
			// 
			this.lblParticle1.Location = new System.Drawing.Point(16, 24);
			this.lblParticle1.Name = "lblParticle1";
			this.lblParticle1.TabIndex = 0;
			this.lblParticle1.Text = "Particle 1";
			// 
			// UCSpring
			// 
			this.Controls.Add(this.groupSpring);
			this.Name = "UCSpring";
			this.Size = new System.Drawing.Size(256, 264);
			this.groupSpring.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion
	}
}

