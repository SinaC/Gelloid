using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using MyGelloidSharp.PhysEnv;

namespace MyGelloidSharp.UserControls {
	/// <summary>
	/// Summary description for UCVertex.
	/// </summary>
	public class UCVertex : UCRoot {
		private System.Windows.Forms.Label lblParticle;
		private System.Windows.Forms.TextBox txtParticleId;
		private System.Windows.Forms.TextBox txtTv;
		private System.Windows.Forms.Label lblTv;
		private System.Windows.Forms.TextBox txtTu;
		private System.Windows.Forms.Label lblTu;
		#region Fields
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		#endregion

		#region Constructors
		public UCVertex() {
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call

		}
		#endregion

		#region Exposed Properties
		public override void Get( object o ) {
			CTriangle.CVertex vertex = (CTriangle.CVertex)o;
			vertex.P = ParticleId;
			vertex.Tu = Tu;
			vertex.Tv = Tv;
		}
		
		public override void Set( object o ) {
			CTriangle.CVertex vertex = (CTriangle.CVertex)o;
			ParticleId = vertex.P;
			Tu = vertex.Tu;
			Tv = vertex.Tv;
		}

		public int ParticleId {
			get { return Convert.ToInt32( txtParticleId.Text ); }
			set { txtParticleId.Text = value.ToString(); }
		}

		public float Tu {
			get { return (float)Convert.ToDouble( txtTu.Text ); }
			set { txtTu.Text = value.ToString(); }
		}

		public float Tv {
			get { return (float)Convert.ToDouble( txtTv.Text ); }
			set { txtTv.Text = value.ToString(); }
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
			this.lblParticle = new System.Windows.Forms.Label();
			this.txtParticleId = new System.Windows.Forms.TextBox();
			this.txtTv = new System.Windows.Forms.TextBox();
			this.lblTv = new System.Windows.Forms.Label();
			this.txtTu = new System.Windows.Forms.TextBox();
			this.lblTu = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// lblParticle
			// 
			this.lblParticle.Location = new System.Drawing.Point(0, 0);
			this.lblParticle.Name = "lblParticle";
			this.lblParticle.TabIndex = 0;
			this.lblParticle.Text = "Particle Id";
			// 
			// txtParticleId
			// 
			this.txtParticleId.Location = new System.Drawing.Point(0, 16);
			this.txtParticleId.Name = "txtParticleId";
			this.txtParticleId.TabIndex = 1;
			this.txtParticleId.Text = "";
			// 
			// txtTv
			// 
			this.txtTv.Location = new System.Drawing.Point(104, 56);
			this.txtTv.Name = "txtTv";
			this.txtTv.TabIndex = 3;
			this.txtTv.Text = "";
			// 
			// lblTv
			// 
			this.lblTv.Location = new System.Drawing.Point(104, 40);
			this.lblTv.Name = "lblTv";
			this.lblTv.TabIndex = 2;
			this.lblTv.Text = "Tv";
			// 
			// txtTu
			// 
			this.txtTu.Location = new System.Drawing.Point(0, 56);
			this.txtTu.Name = "txtTu";
			this.txtTu.TabIndex = 5;
			this.txtTu.Text = "";
			// 
			// lblTu
			// 
			this.lblTu.Location = new System.Drawing.Point(0, 40);
			this.lblTu.Name = "lblTu";
			this.lblTu.TabIndex = 4;
			this.lblTu.Text = "Tu";
			// 
			// UCVertex
			// 
			this.Controls.Add(this.txtTu);
			this.Controls.Add(this.lblTu);
			this.Controls.Add(this.txtTv);
			this.Controls.Add(this.lblTv);
			this.Controls.Add(this.txtParticleId);
			this.Controls.Add(this.lblParticle);
			this.Name = "UCVertex";
			this.Size = new System.Drawing.Size(208, 80);
			this.ResumeLayout(false);

		}
		#endregion
	}
}
