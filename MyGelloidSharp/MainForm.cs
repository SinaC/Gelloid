using System;
using System.Windows.Forms;
using MyGelloidSharp.PhysEnv;
using MyGelloidSharp.Colliders;
using MyGelloidSharp.Cameras;
using MyGelloidSharp.UserControls;

namespace MyGelloidSharp {
	/// <summary>
	/// Summary description for MainForm.
	/// </summary>
	public class MainForm : System.Windows.Forms.Form {
		private System.Windows.Forms.MenuItem menuItem8;
		private System.Windows.Forms.MenuItem menuItem11;
		private System.Windows.Forms.MenuItem menuItem19;
		private System.Windows.Forms.MenuItem menuItem23;
		private System.Windows.Forms.MenuItem menuItem25;
		private System.Windows.Forms.MenuItem menuItem28;
		private System.Windows.Forms.MenuItem menuItem32;
		private System.Windows.Forms.MenuItem menuFile;
		private System.Windows.Forms.MenuItem menuOpenScene;
		private System.Windows.Forms.MenuItem menuView;
		private System.Windows.Forms.MenuItem menuSimulation;
		private System.Windows.Forms.MenuItem menuHelp;
		private System.Windows.Forms.MenuItem menuAbout;
		private System.Windows.Forms.MenuItem menuSaveScene;
		private System.Windows.Forms.MenuItem menuExit;
		private System.Windows.Forms.MenuItem menuStructSprings;
		private System.Windows.Forms.MenuItem menuShearSprings;
		private System.Windows.Forms.MenuItem menuBendSprings;
		private System.Windows.Forms.MenuItem menuManualSprings;
		private System.Windows.Forms.MenuItem menuTriangles;
		private System.Windows.Forms.MenuItem menuTexTriangles;
		private System.Windows.Forms.MenuItem menuVelocity;
		private System.Windows.Forms.MenuItem menuForce;
		private System.Windows.Forms.MenuItem menuSphereParticles;
		private System.Windows.Forms.MenuItem menuColliders;
		private System.Windows.Forms.MenuItem menuRunning;
		private System.Windows.Forms.MenuItem menuReset;
		private System.Windows.Forms.MenuItem menuSimProp;
		private System.Windows.Forms.MenuItem menuTimProp;
		private System.Windows.Forms.MenuItem menuMass;
		private System.Windows.Forms.MenuItem menuGravity;
		private System.Windows.Forms.MenuItem menuFriction;
		private System.Windows.Forms.MenuItem menuWind;
		private System.Windows.Forms.MenuItem menuDotParticles;
		private System.Windows.Forms.MainMenu mainMenu;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem menuItem5;
		private System.Windows.Forms.MenuItem menuItem6;
		private System.Windows.Forms.MenuItem menuItem7;
		private System.Windows.Forms.MenuItem menuItem9;
		private System.Windows.Forms.MenuItem menuItem10;
		
		private System.Windows.Forms.MenuItem menuItem12;
		private System.Windows.Forms.MenuItem menuItem13;
		private System.Windows.Forms.MenuItem menuItem14;
		private System.Windows.Forms.MenuItem menuItem15;
		private System.Windows.Forms.MenuItem menuItem16;
		private System.Windows.Forms.TreeView sceneTreeView;
		private System.Windows.Forms.MenuItem menuPredefinedScenes;
		private System.Windows.Forms.MenuItem menuCreateFlag;
		private System.Windows.Forms.MenuItem menuCreateCuboid;
		private System.Windows.Forms.MenuItem menuShowHelp;
		private System.Windows.Forms.MenuItem menuRender;



		private static RenderForm renderForm = null;
		private static CScene sceneToRender = null;
		private static bool rendering = false;
		private System.Windows.Forms.Panel panelNode;
		private UCRoot createdUC;
		private System.Windows.Forms.MenuItem menuCreateTube;
		private System.Windows.Forms.MenuItem menuCreateSphere;
		private System.Windows.Forms.MenuItem menuCreateCylinder;
		private System.Windows.Forms.MenuItem menuCreateSpiral;
		private System.Windows.Forms.MenuItem menuCreateFallingDrape;
		private System.Windows.Forms.MenuItem menuItem17;
		private System.Windows.Forms.MenuItem menuCreateTest;
		private System.Windows.Forms.MenuItem menuCreateMancheAir;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public MainForm() {
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.mainMenu = new System.Windows.Forms.MainMenu();
			this.menuFile = new System.Windows.Forms.MenuItem();
			this.menuOpenScene = new System.Windows.Forms.MenuItem();
			this.menuSaveScene = new System.Windows.Forms.MenuItem();
			this.menuItem32 = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.menuItem5 = new System.Windows.Forms.MenuItem();
			this.menuItem6 = new System.Windows.Forms.MenuItem();
			this.menuItem7 = new System.Windows.Forms.MenuItem();
			this.menuItem9 = new System.Windows.Forms.MenuItem();
			this.menuItem10 = new System.Windows.Forms.MenuItem();
			this.menuExit = new System.Windows.Forms.MenuItem();
			this.menuView = new System.Windows.Forms.MenuItem();
			this.menuStructSprings = new System.Windows.Forms.MenuItem();
			this.menuShearSprings = new System.Windows.Forms.MenuItem();
			this.menuBendSprings = new System.Windows.Forms.MenuItem();
			this.menuManualSprings = new System.Windows.Forms.MenuItem();
			this.menuItem19 = new System.Windows.Forms.MenuItem();
			this.menuTriangles = new System.Windows.Forms.MenuItem();
			this.menuTexTriangles = new System.Windows.Forms.MenuItem();
			this.menuItem23 = new System.Windows.Forms.MenuItem();
			this.menuVelocity = new System.Windows.Forms.MenuItem();
			this.menuForce = new System.Windows.Forms.MenuItem();
			this.menuItem25 = new System.Windows.Forms.MenuItem();
			this.menuDotParticles = new System.Windows.Forms.MenuItem();
			this.menuSphereParticles = new System.Windows.Forms.MenuItem();
			this.menuItem28 = new System.Windows.Forms.MenuItem();
			this.menuColliders = new System.Windows.Forms.MenuItem();
			this.menuItem12 = new System.Windows.Forms.MenuItem();
			this.menuItem13 = new System.Windows.Forms.MenuItem();
			this.menuItem14 = new System.Windows.Forms.MenuItem();
			this.menuItem15 = new System.Windows.Forms.MenuItem();
			this.menuItem16 = new System.Windows.Forms.MenuItem();
			this.menuSimulation = new System.Windows.Forms.MenuItem();
			this.menuRunning = new System.Windows.Forms.MenuItem();
			this.menuReset = new System.Windows.Forms.MenuItem();
			this.menuItem8 = new System.Windows.Forms.MenuItem();
			this.menuSimProp = new System.Windows.Forms.MenuItem();
			this.menuTimProp = new System.Windows.Forms.MenuItem();
			this.menuMass = new System.Windows.Forms.MenuItem();
			this.menuItem11 = new System.Windows.Forms.MenuItem();
			this.menuGravity = new System.Windows.Forms.MenuItem();
			this.menuFriction = new System.Windows.Forms.MenuItem();
			this.menuWind = new System.Windows.Forms.MenuItem();
			this.menuPredefinedScenes = new System.Windows.Forms.MenuItem();
			this.menuCreateFlag = new System.Windows.Forms.MenuItem();
			this.menuCreateCuboid = new System.Windows.Forms.MenuItem();
			this.menuCreateTube = new System.Windows.Forms.MenuItem();
			this.menuCreateSphere = new System.Windows.Forms.MenuItem();
			this.menuCreateCylinder = new System.Windows.Forms.MenuItem();
			this.menuCreateSpiral = new System.Windows.Forms.MenuItem();
			this.menuCreateFallingDrape = new System.Windows.Forms.MenuItem();
			this.menuCreateMancheAir = new System.Windows.Forms.MenuItem();
			this.menuItem17 = new System.Windows.Forms.MenuItem();
			this.menuCreateTest = new System.Windows.Forms.MenuItem();
			this.menuHelp = new System.Windows.Forms.MenuItem();
			this.menuAbout = new System.Windows.Forms.MenuItem();
			this.menuShowHelp = new System.Windows.Forms.MenuItem();
			this.menuRender = new System.Windows.Forms.MenuItem();
			this.sceneTreeView = new System.Windows.Forms.TreeView();
			this.panelNode = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			// 
			// mainMenu
			// 
			this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.menuFile,
																					 this.menuView,
																					 this.menuSimulation,
																					 this.menuPredefinedScenes,
																					 this.menuHelp,
																					 this.menuRender});
			// 
			// menuFile
			// 
			this.menuFile.Index = 0;
			this.menuFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.menuOpenScene,
																					 this.menuSaveScene,
																					 this.menuItem32,
																					 this.menuItem2,
																					 this.menuItem1,
																					 this.menuItem3,
																					 this.menuItem4,
																					 this.menuItem5,
																					 this.menuItem6,
																					 this.menuItem7,
																					 this.menuItem9,
																					 this.menuItem10,
																					 this.menuExit});
			this.menuFile.Text = "&File";
			// 
			// menuOpenScene
			// 
			this.menuOpenScene.Index = 0;
			this.menuOpenScene.Shortcut = System.Windows.Forms.Shortcut.F3;
			this.menuOpenScene.Text = "&Open Scene";
			this.menuOpenScene.Click += new System.EventHandler(this.menuOpenScene_Click);
			// 
			// menuSaveScene
			// 
			this.menuSaveScene.Index = 1;
			this.menuSaveScene.Shortcut = System.Windows.Forms.Shortcut.F2;
			this.menuSaveScene.Text = "&Save Scene";
			this.menuSaveScene.Click += new System.EventHandler(this.menuSaveScene_Click);
			// 
			// menuItem32
			// 
			this.menuItem32.Index = 2;
			this.menuItem32.Text = "-";
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 3;
			this.menuItem2.Text = "Import &Gelloid";
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 4;
			this.menuItem1.Text = "Export Gelloid";
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 5;
			this.menuItem3.Text = "-";
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 6;
			this.menuItem4.Text = "Import Collider";
			// 
			// menuItem5
			// 
			this.menuItem5.Index = 7;
			this.menuItem5.Text = "Export Collider";
			// 
			// menuItem6
			// 
			this.menuItem6.Index = 8;
			this.menuItem6.Text = "-";
			// 
			// menuItem7
			// 
			this.menuItem7.Index = 9;
			this.menuItem7.Text = "Import Camera";
			// 
			// menuItem9
			// 
			this.menuItem9.Index = 10;
			this.menuItem9.Text = "Export Camera";
			// 
			// menuItem10
			// 
			this.menuItem10.Index = 11;
			this.menuItem10.Text = "-";
			// 
			// menuExit
			// 
			this.menuExit.Index = 12;
			this.menuExit.Shortcut = System.Windows.Forms.Shortcut.CtrlX;
			this.menuExit.Text = "&Exit";
			this.menuExit.Click += new System.EventHandler(this.menuExit_Click);
			// 
			// menuView
			// 
			this.menuView.Index = 1;
			this.menuView.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.menuStructSprings,
																					 this.menuShearSprings,
																					 this.menuBendSprings,
																					 this.menuManualSprings,
																					 this.menuItem19,
																					 this.menuTriangles,
																					 this.menuTexTriangles,
																					 this.menuItem23,
																					 this.menuVelocity,
																					 this.menuForce,
																					 this.menuItem25,
																					 this.menuDotParticles,
																					 this.menuSphereParticles,
																					 this.menuItem28,
																					 this.menuColliders,
																					 this.menuItem12,
																					 this.menuItem13,
																					 this.menuItem14,
																					 this.menuItem15,
																					 this.menuItem16});
			this.menuView.Text = "&View";
			// 
			// menuStructSprings
			// 
			this.menuStructSprings.Index = 0;
			this.menuStructSprings.RadioCheck = true;
			this.menuStructSprings.Text = "Show &Structural Springs\t2";
			// 
			// menuShearSprings
			// 
			this.menuShearSprings.Index = 1;
			this.menuShearSprings.RadioCheck = true;
			this.menuShearSprings.Text = "Show S&hear Springs\t2";
			// 
			// menuBendSprings
			// 
			this.menuBendSprings.Index = 2;
			this.menuBendSprings.RadioCheck = true;
			this.menuBendSprings.Text = "Show &Bend Springs\t2";
			// 
			// menuManualSprings
			// 
			this.menuManualSprings.Index = 3;
			this.menuManualSprings.Text = "Show &Manual Springs\t2";
			// 
			// menuItem19
			// 
			this.menuItem19.Index = 4;
			this.menuItem19.Text = "-";
			// 
			// menuTriangles
			// 
			this.menuTriangles.Index = 5;
			this.menuTriangles.Text = "Show F&aces\t4";
			// 
			// menuTexTriangles
			// 
			this.menuTexTriangles.Index = 6;
			this.menuTexTriangles.Text = "Show Te&xtured Faces\t4";
			// 
			// menuItem23
			// 
			this.menuItem23.Index = 7;
			this.menuItem23.Text = "-";
			// 
			// menuVelocity
			// 
			this.menuVelocity.Index = 8;
			this.menuVelocity.Text = "Show &Velocity\t3";
			// 
			// menuForce
			// 
			this.menuForce.Index = 9;
			this.menuForce.Text = "Show &Force\t3";
			// 
			// menuItem25
			// 
			this.menuItem25.Index = 10;
			this.menuItem25.Text = "-";
			// 
			// menuDotParticles
			// 
			this.menuDotParticles.Index = 11;
			this.menuDotParticles.Text = "Show Particles as &Dot\t1";
			// 
			// menuSphereParticles
			// 
			this.menuSphereParticles.Index = 12;
			this.menuSphereParticles.Text = "Show Particles as S&phere\t1";
			// 
			// menuItem28
			// 
			this.menuItem28.Index = 13;
			this.menuItem28.Text = "-";
			// 
			// menuColliders
			// 
			this.menuColliders.Index = 14;
			this.menuColliders.Text = "Show &Colliders\t6";
			// 
			// menuItem12
			// 
			this.menuItem12.Index = 15;
			this.menuItem12.Text = "-";
			// 
			// menuItem13
			// 
			this.menuItem13.Index = 16;
			this.menuItem13.Text = "Show Faces &normals\t5";
			// 
			// menuItem14
			// 
			this.menuItem14.Index = 17;
			this.menuItem14.Text = "Show Vertex n&ormals\t5";
			// 
			// menuItem15
			// 
			this.menuItem15.Index = 18;
			this.menuItem15.Text = "-";
			// 
			// menuItem16
			// 
			this.menuItem16.Index = 19;
			this.menuItem16.Text = "Show statistics\t7";
			// 
			// menuSimulation
			// 
			this.menuSimulation.Index = 2;
			this.menuSimulation.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						   this.menuRunning,
																						   this.menuReset,
																						   this.menuItem8,
																						   this.menuSimProp,
																						   this.menuTimProp,
																						   this.menuMass,
																						   this.menuItem11,
																						   this.menuGravity,
																						   this.menuFriction,
																						   this.menuWind});
			this.menuSimulation.Text = "&Simulation";
			// 
			// menuRunning
			// 
			this.menuRunning.Index = 0;
			this.menuRunning.Text = "&Running\tR";
			this.menuRunning.Click += new System.EventHandler(this.menuRunning_Click);
			// 
			// menuReset
			// 
			this.menuReset.Index = 1;
			this.menuReset.Text = "Rese&t\tT";
			this.menuReset.Click += new System.EventHandler(this.menuReset_Click);
			// 
			// menuItem8
			// 
			this.menuItem8.Index = 2;
			this.menuItem8.Text = "-";
			// 
			// menuSimProp
			// 
			this.menuSimProp.Index = 3;
			this.menuSimProp.Text = "&Simulation Properties";
			this.menuSimProp.Click += new System.EventHandler(this.menuSimProp_Click);
			// 
			// menuTimProp
			// 
			this.menuTimProp.Index = 4;
			this.menuTimProp.Text = "T&iming Properties";
			// 
			// menuMass
			// 
			this.menuMass.Index = 5;
			this.menuMass.Text = "Vertex &Mass";
			// 
			// menuItem11
			// 
			this.menuItem11.Index = 6;
			this.menuItem11.Text = "-";
			// 
			// menuGravity
			// 
			this.menuGravity.Index = 7;
			this.menuGravity.Text = "Use &Gravity\tG";
			// 
			// menuFriction
			// 
			this.menuFriction.Index = 8;
			this.menuFriction.Text = "Use &Friction\tF";
			// 
			// menuWind
			// 
			this.menuWind.Index = 9;
			this.menuWind.Text = "Use &Wind\tW";
			// 
			// menuPredefinedScenes
			// 
			this.menuPredefinedScenes.Index = 3;
			this.menuPredefinedScenes.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																								 this.menuCreateFlag,
																								 this.menuCreateCuboid,
																								 this.menuCreateTube,
																								 this.menuCreateSphere,
																								 this.menuCreateCylinder,
																								 this.menuCreateSpiral,
																								 this.menuCreateFallingDrape,
																								 this.menuItem17,
																								 this.menuCreateTest,
																								 this.menuCreateMancheAir});
			this.menuPredefinedScenes.Text = "&Predefined Scenes";
			// 
			// menuCreateFlag
			// 
			this.menuCreateFlag.Index = 0;
			this.menuCreateFlag.Text = "&Flag";
			this.menuCreateFlag.Click += new System.EventHandler(this.menuCreateFlag_Click);
			// 
			// menuCreateCuboid
			// 
			this.menuCreateCuboid.Index = 1;
			this.menuCreateCuboid.Text = "&Cuboid";
			this.menuCreateCuboid.Click += new System.EventHandler(this.menuCreateCuboid_Click);
			// 
			// menuCreateTube
			// 
			this.menuCreateTube.Index = 2;
			this.menuCreateTube.Text = "&Tube";
			this.menuCreateTube.Click += new System.EventHandler(this.menuCreateTube_Click);
			// 
			// menuCreateSphere
			// 
			this.menuCreateSphere.Index = 3;
			this.menuCreateSphere.Text = "&Sphere";
			this.menuCreateSphere.Click += new System.EventHandler(this.menuCreateSphere_Click);
			// 
			// menuCreateCylinder
			// 
			this.menuCreateCylinder.Index = 4;
			this.menuCreateCylinder.Text = "C&ylinder";
			this.menuCreateCylinder.Click += new System.EventHandler(this.menuCreateCylinder_Click);
			// 
			// menuCreateSpiral
			// 
			this.menuCreateSpiral.Index = 5;
			this.menuCreateSpiral.Text = "Sp&iral";
			this.menuCreateSpiral.Click += new System.EventHandler(this.menuCreateSpiral_Click);
			// 
			// menuCreateFallingDrape
			// 
			this.menuCreateFallingDrape.Index = 6;
			this.menuCreateFallingDrape.Text = "F&alling Drape";
			this.menuCreateFallingDrape.Click += new System.EventHandler(this.menuCreateFallingDrape_Click);
			// 
			// menuCreateMancheAir
			// 
			this.menuCreateMancheAir.Index = 9;
			this.menuCreateMancheAir.Text = "[Manche A Air]";
			this.menuCreateMancheAir.Click += new System.EventHandler(this.menuCreateMancheAir_Click);
			// 
			// menuItem17
			// 
			this.menuItem17.Index = 7;
			this.menuItem17.Text = "-";
			// 
			// menuCreateTest
			// 
			this.menuCreateTest.Index = 8;
			this.menuCreateTest.Text = "T&est";
			this.menuCreateTest.Click += new System.EventHandler(this.menuCreateTest_Click);
			// 
			// menuHelp
			// 
			this.menuHelp.Index = 4;
			this.menuHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.menuAbout,
																					 this.menuShowHelp});
			this.menuHelp.Text = "&Help";
			// 
			// menuAbout
			// 
			this.menuAbout.Index = 0;
			this.menuAbout.Shortcut = System.Windows.Forms.Shortcut.F1;
			this.menuAbout.Text = "&About";
			// 
			// menuShowHelp
			// 
			this.menuShowHelp.Index = 1;
			this.menuShowHelp.Text = "&Help\tH";
			// 
			// menuRender
			// 
			this.menuRender.Index = 5;
			this.menuRender.Text = "<Start Render>";
			this.menuRender.Click += new System.EventHandler(this.menuRender_Click);
			// 
			// sceneTreeView
			// 
			this.sceneTreeView.ImageIndex = -1;
			this.sceneTreeView.Location = new System.Drawing.Point(0, 8);
			this.sceneTreeView.Name = "sceneTreeView";
			this.sceneTreeView.PathSeparator = ".";
			this.sceneTreeView.SelectedImageIndex = -1;
			this.sceneTreeView.Size = new System.Drawing.Size(192, 360);
			this.sceneTreeView.TabIndex = 0;
			this.sceneTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.sceneTreeView_AfterSelect);
			// 
			// panelNode
			// 
			this.panelNode.Location = new System.Drawing.Point(200, 8);
			this.panelNode.Name = "panelNode";
			this.panelNode.Size = new System.Drawing.Size(520, 360);
			this.panelNode.TabIndex = 2;
			// 
			// MainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(728, 377);
			this.Controls.Add(this.panelNode);
			this.Controls.Add(this.sceneTreeView);
			this.Menu = this.mainMenu;
			this.Name = "MainForm";
			this.Text = "Physics Mainform";
			this.ResumeLayout(false);

		}
		#endregion

        [STAThreadAttribute]
		static void Main() {
			Application.Run(new MainForm());
		}

		private void StartRender() {
			if ( sceneToRender == null )
				return;
			if ( renderForm != null && renderForm.FormRunning )
				renderForm.Dispose();

			rendering = true;
			menuRender.Text = "<Stop Rendering>";

			renderForm = new RenderForm( this );
			renderForm.Scene = sceneToRender;
			renderForm.Run();
		}

		public void renderClosed() {
			rendering = false;
			menuRender.Text = "<Start Rendering>";
		}

		private void StopRender() {
			if ( renderForm != null && renderForm.FormRunning )
				renderForm.Dispose();
			renderForm = null;
			
			rendering = false;
			menuRender.Text = "<Start Rendering>";
		}

		private void BuildSceneTree() {
			if ( sceneToRender == null )
				return;
			panelNode.Controls.Clear();

			TreeNode rootNode = new TreeNode("Scene");
			rootNode.Tag = sceneToRender;
			
			TreeNode gelloidsNode = new TreeNode("Gelloids");
			foreach ( CGelloid gelloid in sceneToRender.Gelloids ) {
				TreeNode gelloidNode = new TreeNode( gelloid.Name );
				gelloidNode.Tag = gelloid;

				// Particles
				TreeNode particlesNode = new TreeNode("Particles");
				for ( int i = 0; i < gelloid.ParticlesOriginal.Count; i++ ) {
					TreeNode particleNode = new TreeNode( i.ToString() );
					UCParticle.CParticlePointer pointer = new UCParticle.CParticlePointer();
					pointer.gelloid = gelloid;
					pointer.index = i;
					particleNode.Tag = pointer;
					particlesNode.Nodes.Add( particleNode );
				}
				gelloidNode.Nodes.Add( particlesNode );

				// Springs
				TreeNode springsNode = new TreeNode("Springs");
				for ( int i = 0; i < gelloid.Springs.Count; i++ ) {
					TreeNode springNode = new TreeNode( i.ToString() );
					springNode.Tag = gelloid.Springs[i];
					springsNode.Nodes.Add( springNode );
				}
				gelloidNode.Nodes.Add( springsNode );

				// Triangles
				TreeNode trianglesNode = new TreeNode( "Triangles" );
				for ( int i = 0; i < gelloid.Triangles.Count; i++ ) {
					TreeNode triangleNode = new TreeNode( i.ToString() );
					triangleNode.Tag = gelloid.Triangles[i];
					trianglesNode.Nodes.Add( triangleNode );
				}
				gelloidNode.Nodes.Add( trianglesNode );

				gelloidsNode.Nodes.Add( gelloidNode );
			}
			rootNode.Nodes.Add( gelloidsNode );

			TreeNode camerasNode = new TreeNode("Cameras");
			foreach ( CCamera camera in sceneToRender.Cameras ) {
				TreeNode cameraNode = new TreeNode( camera.Name );
				cameraNode.Tag = camera;
				camerasNode.Nodes.Add( cameraNode );
			}
			rootNode.Nodes.Add( camerasNode );
			
			TreeNode collidersNode = new TreeNode("Colliders");
			foreach ( CCollider collider in sceneToRender.Colliders ) {
				TreeNode colliderNode = new TreeNode( collider.Name );
				colliderNode.Tag = collider;
				collidersNode.Nodes.Add(colliderNode);
			}
			rootNode.Nodes.Add( collidersNode );
			
			sceneTreeView.Nodes.Clear();
			sceneTreeView.Nodes.Add( rootNode );
		}

		#region Events
		protected override void OnKeyPress( KeyPressEventArgs e ) {
			if ( renderForm != null && renderForm.FormRunning ) // Never used because we are always in a control
				renderForm.KeyPressed( e.KeyChar );
		}

		private void menuOpenScene_Click(object sender, System.EventArgs e) {
			OpenFileDialog openScene = new OpenFileDialog();
			openScene.DefaultExt = "scn";
			openScene.Filter = "Scene files (*.scn)|*.scn|Xml files (*.xml)|*.xml";
			if ( openScene.ShowDialog() == DialogResult.OK ) {
				try {
					sceneToRender = CScene.XmlImport( openScene.FileName );
					BuildSceneTree();
					StopRender();
				}
				catch ( Exception ex ) {
					MessageBox.Show( "Exception: "+ex.Message, "Open error" );
				}
			}
		}

		private void menuSaveScene_Click(object sender, System.EventArgs e) {
			SaveFileDialog saveScene = new SaveFileDialog();
			saveScene.DefaultExt = "scn";
			saveScene.Filter = "Scene files (*.scn)|*.scn|Xml files (*.xml)|*.xml";
			if ( saveScene.ShowDialog() == DialogResult.OK ) {
				try {
					CScene.XmlExport( sceneToRender, saveScene.FileName );
				}
				catch ( Exception ex ) {
					MessageBox.Show( "Exception: "+ex.Message, "Save error" );
				}
			}		
		}

		private void menuExit_Click(object sender, System.EventArgs e) {
			this.Close();
		}

		private void menuRunning_Click(object sender, System.EventArgs e) {
			if ( renderForm != null && renderForm.FormRunning )
				renderForm.SimulationRunning = !renderForm.SimulationRunning;
		}

		private void menuReset_Click(object sender, System.EventArgs e) {
			if ( renderForm != null && renderForm.FormRunning )
				renderForm.Scene.Reset();
		}

		private void sceneTreeView_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e) {
			TreeNode node = e.Node;
			Control controlToAdd ;
			if ( node.Tag == null )
				return; // Nothing to do
			else if ( node.Tag.GetType() == typeof(CCamera) ) { // Camera
				CCamera camera = (CCamera)node.Tag;
				UCCamera UC = new UCCamera();
				UC.resetClick += new EventHandler( this.bubblingReset );
				UC.updateClick += new EventHandler( this.bubblingUpdate );
				UC.Set( camera );
				createdUC = UC;
				controlToAdd = createdUC;
			}
			else if ( node.Tag.GetType() == typeof(CGelloid) ) { // Gelloid
				CGelloid gelloid = (CGelloid)node.Tag;
				UCGelloid UC = new UCGelloid();
				UC.resetClick += new EventHandler( this.bubblingReset );
				UC.updateClick += new EventHandler( this.bubblingUpdate );
				UC.Set( gelloid );
				createdUC = UC;
				controlToAdd = createdUC;
			}
			else if ( node.Tag.GetType() == typeof(CSpring) ) { // Spring
				CSpring spring = (CSpring)node.Tag;
				UCSpring UC = new UCSpring();
				UC.resetClick += new EventHandler( this.bubblingReset );
				UC.updateClick += new EventHandler( this.bubblingUpdate );
				UC.Set( spring );
				createdUC = UC;
				controlToAdd = createdUC;
			}
			else if ( node.Tag.GetType() == typeof(CTriangle) ) { // Triangle
				CTriangle triangle = (CTriangle)node.Tag;
				UCTriangle UC = new UCTriangle();
				UC.resetClick += new EventHandler( this.bubblingReset );
				UC.updateClick += new EventHandler( this.bubblingUpdate );
				UC.Set( triangle );
				createdUC = UC;
				controlToAdd = createdUC;
			}
			else if ( node.Tag.GetType().IsSubclassOf(typeof(CPlane)) ) { // Plane
				CPlane plane = (CPlane)node.Tag;
				UCPlane UC = new UCPlane();
				UC.resetClick += new EventHandler( this.bubblingReset );
				UC.updateClick += new EventHandler( this.bubblingUpdate );
				UC.Set( plane );
				createdUC = UC;
				controlToAdd = createdUC;
			}
			else if ( node.Tag.GetType().IsSubclassOf(typeof(CSphere)) ) { // Sphere
				CSphere sphere = (CSphere)node.Tag;
				UCSphere UC = new UCSphere();
				UC.resetClick += new EventHandler( this.bubblingReset );
				UC.updateClick += new EventHandler( this.bubblingUpdate );
				UC.Set( sphere );
				createdUC = UC;
				controlToAdd = createdUC;
			}
			else if ( node.Tag.GetType() == typeof(CScene) ) { // Scene
				CScene scene = (CScene)node.Tag;
				UCScene UC = new UCScene();
				UC.resetClick += new EventHandler( this.bubblingReset );
				UC.updateClick += new EventHandler( this.bubblingUpdate );
				UC.Set( scene );
				createdUC = UC;
				controlToAdd = createdUC;
			}
			else if ( node.Tag.GetType() == typeof(UCParticle.CParticlePointer) ) { // Particle
				UCParticle.CParticlePointer pointer = (UCParticle.CParticlePointer)node.Tag;
				UCParticle UC = new UCParticle();
				UC.resetClick += new EventHandler( this.bubblingReset );
				UC.updateClick += new EventHandler( this.bubblingUpdate );
				UC.Set( pointer );
				createdUC = UC;
				controlToAdd = createdUC;
			}
			else { // TODO: collider: square, circle
				Label label1 = new Label();
				label1.Text = "<NOT YET IMPLEMENTED>";
				label1.Width = 300;
				label1.Height = 100;
				controlToAdd = label1;
			}
			panelNode.Controls.Clear();
			panelNode.Controls.Add( controlToAdd );
		}

		#region Bubbling Events
		private void bubblingReset( object sender, System.EventArgs e ) {
			TreeNode node = sceneTreeView.SelectedNode;
			if ( node == null )
				return;
			if ( createdUC == null )
				return;
			if ( node.Tag == null )
				return;
			createdUC.Set( node.Tag ); // Reset node
		}

		private void bubblingUpdate( object sender, System.EventArgs e ) {
			TreeNode node = sceneTreeView.SelectedNode;
			if ( node == null )
				return;
			if ( createdUC == null )
				return;
			if ( node.Tag == null )
				return;
			createdUC.Get( node.Tag ); // Update node
		}
		#endregion

		private void menuRender_Click(object sender, System.EventArgs e) {
			if ( rendering )
				StopRender();
			else
				StartRender();
		}

		private void menuCreateFlag_Click(object sender, System.EventArgs e) {
			StopRender();
			sceneToRender = CScene.CreateFlag();
			BuildSceneTree();
			//StartRender();
		}

		private void menuCreateCuboid_Click(object sender, System.EventArgs e) {
			StopRender();
			sceneToRender = CScene.CreateCuboid();
			BuildSceneTree();
			//StartRender();
		}

		private void menuCreateTube_Click(object sender, System.EventArgs e) {
			StopRender();
			sceneToRender = CScene.CreateTube();
			BuildSceneTree();
		}

		private void menuCreateSphere_Click(object sender, System.EventArgs e) {
			StopRender();
			sceneToRender = CScene.CreateSphere();
			BuildSceneTree();
		}
	
		private void menuCreateCylinder_Click(object sender, System.EventArgs e) {
			StopRender();
			sceneToRender = CScene.CreateCylinder();
			BuildSceneTree();
		}

		private void menuSimProp_Click(object sender, System.EventArgs e) {
			foreach ( TreeNode node in sceneTreeView.Nodes )
				if ( node.Tag.GetType() == typeof(CScene) ) {
					sceneTreeView.SelectedNode = node;
					break;
				}
		}

		private void menuCreateSpiral_Click(object sender, System.EventArgs e) {
			StopRender();
			sceneToRender = CScene.CreateSpiral();
			BuildSceneTree();
		}

		private void menuCreateFallingDrape_Click(object sender, System.EventArgs e) {
			StopRender();
			sceneToRender = CScene.CreateFallingDrape();
			BuildSceneTree();		
		}

		private void menuCreateTest_Click(object sender, System.EventArgs e) {
			StopRender();
			sceneToRender = CScene.CreateTest();
			BuildSceneTree();		
		}

		private void menuCreateMancheAir_Click(object sender, System.EventArgs e) {
			StopRender();
			sceneToRender = CScene.CreateMancheAir();
			BuildSceneTree();
		}

		#endregion
	}
}
