using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using MyGelloidSharp.PhysEnv;

namespace MyGelloidSharp.UserControls {
	/// <summary>
	/// Summary description for UCParticleSub.
	/// </summary>
	public class UCParticleSub : System.Windows.Forms.UserControl {
		#region Fields
		private System.Windows.Forms.ComboBox comboMovable;
		private System.Windows.Forms.Label lblMovable;
		private System.Windows.Forms.Label lblMass;
		private System.Windows.Forms.TextBox txtMass;
		private System.Windows.Forms.Label lblVelocity;
		private System.Windows.Forms.Label lblPosition;
		private MyGelloidSharp.UserControls.UCVector3 ucVelocity;
		private MyGelloidSharp.UserControls.UCVector3 ucPosition;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		#endregion

		#region Constructors
		public UCParticleSub() {
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call

		}
		#endregion

		#region Exposed Properties
		public void Get( CParticle particle ) {
			particle.Position = ucPosition.Value;
			particle.Velocity = ucVelocity.Value;
			particle.OneOverMass = (float)Convert.ToDouble( txtMass.Text );
			particle.NotMovable = !Convert.ToBoolean( comboMovable.Text );
		}

		public void Set( CParticle particle ) {
			ucPosition.Value = particle.Position;
			ucVelocity.Value = particle.Velocity;
			txtMass.Text = particle.OneOverMass.ToString();
			comboMovable.Text = (!particle.NotMovable).ToString();
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
			this.comboMovable = new System.Windows.Forms.ComboBox();
			this.lblMovable = new System.Windows.Forms.Label();
			this.lblMass = new System.Windows.Forms.Label();
			this.txtMass = new System.Windows.Forms.TextBox();
			this.lblVelocity = new System.Windows.Forms.Label();
			this.lblPosition = new System.Windows.Forms.Label();
			this.ucVelocity = new MyGelloidSharp.UserControls.UCVector3();
			this.ucPosition = new MyGelloidSharp.UserControls.UCVector3();
			this.SuspendLayout();
			// 
			// comboMovable
			// 
			this.comboMovable.Items.AddRange(new object[] {
															  "True",
															  "False"});
			this.comboMovable.Location = new System.Drawing.Point(80, 96);
			this.comboMovable.Name = "comboMovable";
			this.comboMovable.Size = new System.Drawing.Size(121, 21);
			this.comboMovable.TabIndex = 19;
			// 
			// lblMovable
			// 
			this.lblMovable.Location = new System.Drawing.Point(0, 96);
			this.lblMovable.Name = "lblMovable";
			this.lblMovable.Size = new System.Drawing.Size(72, 23);
			this.lblMovable.TabIndex = 18;
			this.lblMovable.Text = "Movable";
			// 
			// lblMass
			// 
			this.lblMass.Location = new System.Drawing.Point(0, 72);
			this.lblMass.Name = "lblMass";
			this.lblMass.Size = new System.Drawing.Size(80, 23);
			this.lblMass.TabIndex = 17;
			this.lblMass.Text = "OneOverMass";
			// 
			// txtMass
			// 
			this.txtMass.Location = new System.Drawing.Point(80, 72);
			this.txtMass.Name = "txtMass";
			this.txtMass.TabIndex = 16;
			this.txtMass.Text = "";
			// 
			// lblVelocity
			// 
			this.lblVelocity.Location = new System.Drawing.Point(0, 48);
			this.lblVelocity.Name = "lblVelocity";
			this.lblVelocity.Size = new System.Drawing.Size(48, 23);
			this.lblVelocity.TabIndex = 15;
			this.lblVelocity.Text = "Velocity";
			// 
			// lblPosition
			// 
			this.lblPosition.Location = new System.Drawing.Point(0, 16);
			this.lblPosition.Name = "lblPosition";
			this.lblPosition.Size = new System.Drawing.Size(48, 23);
			this.lblPosition.TabIndex = 14;
			this.lblPosition.Text = "Position";
			// 
			// ucVelocity
			// 
			this.ucVelocity.Location = new System.Drawing.Point(48, 32);
			this.ucVelocity.Name = "ucVelocity";
			this.ucVelocity.Size = new System.Drawing.Size(240, 40);
			this.ucVelocity.TabIndex = 13;
			// 
			// ucPosition
			// 
			this.ucPosition.Location = new System.Drawing.Point(48, 0);
			this.ucPosition.Name = "ucPosition";
			this.ucPosition.Size = new System.Drawing.Size(240, 40);
			this.ucPosition.TabIndex = 12;
			// 
			// UCParticleSub
			// 
			this.Controls.Add(this.comboMovable);
			this.Controls.Add(this.lblMovable);
			this.Controls.Add(this.lblMass);
			this.Controls.Add(this.txtMass);
			this.Controls.Add(this.lblVelocity);
			this.Controls.Add(this.lblPosition);
			this.Controls.Add(this.ucVelocity);
			this.Controls.Add(this.ucPosition);
			this.Name = "UCParticleSub";
			this.Size = new System.Drawing.Size(288, 120);
			this.ResumeLayout(false);

		}
		#endregion	
	}
}