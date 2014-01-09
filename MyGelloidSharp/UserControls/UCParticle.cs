using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using MyGelloidSharp.PhysEnv;

namespace MyGelloidSharp.UserControls {
	public class UCParticle : UCRoot {
		#region Fields
		private System.Windows.Forms.GroupBox groupParticle;
		private System.Windows.Forms.GroupBox groupOriginal;
		private System.Windows.Forms.Button btnUpdate;
		private System.Windows.Forms.Button btnReset;
		private System.Windows.Forms.GroupBox groupCurrent;
		private MyGelloidSharp.UserControls.UCParticleSub ucOriginal;
		private MyGelloidSharp.UserControls.UCParticleSub ucCurrent;
		private System.ComponentModel.IContainer components = null;
		#endregion

		#region Constructors
		public UCParticle() {
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call
		}
		#endregion

		#region Exposed Properties
		public override void Get(object o) {
			CParticlePointer particle = (CParticlePointer)o;
			CParticle original = particle.gelloid.ParticlesOriginal[particle.index];
			CParticle current = particle.gelloid.ParticlesCurrent[particle.index];
			CParticle target = particle.gelloid.ParticlesTarget[particle.index];
			ucOriginal.Get( original );
			ucCurrent.Get( current );
			target.NotMovable = current.NotMovable;
		}

		public override void Set(object o) {
			CParticlePointer particle = (CParticlePointer)o;
			CParticle original = particle.gelloid.ParticlesOriginal[particle.index];
			CParticle current = particle.gelloid.ParticlesCurrent[particle.index];
			ucOriginal.Set( original );
			ucCurrent.Set( current );
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
			this.groupParticle = new System.Windows.Forms.GroupBox();
			this.btnReset = new System.Windows.Forms.Button();
			this.btnUpdate = new System.Windows.Forms.Button();
			this.groupOriginal = new System.Windows.Forms.GroupBox();
			this.groupCurrent = new System.Windows.Forms.GroupBox();
			this.ucOriginal = new MyGelloidSharp.UserControls.UCParticleSub();
			this.ucCurrent = new MyGelloidSharp.UserControls.UCParticleSub();
			this.groupParticle.SuspendLayout();
			this.groupOriginal.SuspendLayout();
			this.groupCurrent.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupParticle
			// 
			this.groupParticle.Controls.Add(this.btnReset);
			this.groupParticle.Controls.Add(this.btnUpdate);
			this.groupParticle.Controls.Add(this.groupOriginal);
			this.groupParticle.Controls.Add(this.groupCurrent);
			this.groupParticle.Location = new System.Drawing.Point(0, 0);
			this.groupParticle.Name = "groupParticle";
			this.groupParticle.Size = new System.Drawing.Size(336, 360);
			this.groupParticle.TabIndex = 0;
			this.groupParticle.TabStop = false;
			this.groupParticle.Text = "Particle";
			// 
			// btnReset
			// 
			this.btnReset.Location = new System.Drawing.Point(184, 328);
			this.btnReset.Name = "btnReset";
			this.btnReset.TabIndex = 10;
			this.btnReset.Text = "Reset";
			this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
			// 
			// btnUpdate
			// 
			this.btnUpdate.Location = new System.Drawing.Point(80, 328);
			this.btnUpdate.Name = "btnUpdate";
			this.btnUpdate.TabIndex = 9;
			this.btnUpdate.Text = "Update";
			this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
			// 
			// groupOriginal
			// 
			this.groupOriginal.Controls.Add(this.ucOriginal);
			this.groupOriginal.Location = new System.Drawing.Point(8, 16);
			this.groupOriginal.Name = "groupOriginal";
			this.groupOriginal.Size = new System.Drawing.Size(320, 144);
			this.groupOriginal.TabIndex = 8;
			this.groupOriginal.TabStop = false;
			this.groupOriginal.Text = "Original";
			// 
			// groupCurrent
			// 
			this.groupCurrent.Controls.Add(this.ucCurrent);
			this.groupCurrent.Location = new System.Drawing.Point(8, 176);
			this.groupCurrent.Name = "groupCurrent";
			this.groupCurrent.Size = new System.Drawing.Size(320, 144);
			this.groupCurrent.TabIndex = 12;
			this.groupCurrent.TabStop = false;
			this.groupCurrent.Text = "Current";
			// 
			// ucOriginal
			// 
			this.ucOriginal.Location = new System.Drawing.Point(8, 16);
			this.ucOriginal.Name = "ucOriginal";
			this.ucOriginal.Size = new System.Drawing.Size(288, 120);
			this.ucOriginal.TabIndex = 0;
			// 
			// ucCurrent
			// 
			this.ucCurrent.Location = new System.Drawing.Point(8, 16);
			this.ucCurrent.Name = "ucCurrent";
			this.ucCurrent.Size = new System.Drawing.Size(288, 120);
			this.ucCurrent.TabIndex = 0;
			// 
			// UCParticle
			// 
			this.Controls.Add(this.groupParticle);
			this.Name = "UCParticle";
			this.Size = new System.Drawing.Size(336, 360);
			this.groupParticle.ResumeLayout(false);
			this.groupOriginal.ResumeLayout(false);
			this.groupCurrent.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region Inner class used to store a pointer to gelloid and particle index
		public class CParticlePointer {
			public int index;
			public CGelloid gelloid;
		}
		#endregion
	}
}