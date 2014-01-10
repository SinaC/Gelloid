using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using MyGelloidSharp.PhysEnv;
using MyGelloidSharp.Cameras;

namespace MyGelloidSharp {
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class RenderForm : System.Windows.Forms.Form {
		// Our rendering device
		private Device _device; 
		// Font
		private CFont _font;
		// Last mouse location
		private Point _lastMouseLoc;

		private readonly Random _random;

		// MainForm
	    readonly MainForm _mainForm;

		// Scene
		public CScene Scene;

		//
		public bool FormRunning = false;
		public bool FrameByFrame = false;
		private bool _nextFrame = false;
		public bool CameraRotating = false;
		private float _cameraAngle = 0.0f;
		public bool OnPlane = false;
		public bool CameraCentered = false;
		public bool SimulationRunning = true;
		public bool DisplayHelp = false;
		public float LastTime = 0, MaxTimeStep = 0.01f, TimeIterations = 10; // step * iter = 0.1

		public RenderForm( MainForm mainForm ) {
			// Set the initial size of our form
			ClientSize = new Size(800,600);

			_random = new Random();
			Scene = new CScene();

			_mainForm = mainForm;
		}

		protected override void OnClosed(EventArgs e) {
			_mainForm.renderClosed();
			base.OnClosed (e);
		}

		protected override void OnMouseMove( MouseEventArgs e ) {
			Point delta = new Point( e.X - _lastMouseLoc.X, e.Y - _lastMouseLoc.Y );
			_lastMouseLoc = new Point( e.X, e.Y );
			if ( e.Button == MouseButtons.Right ) {
				Scene.CurrentCamera.Yaw( (float)delta.X * 0.01f ); 
				Scene.CurrentCamera.Pitch( (float)delta.Y * 0.01f );
			}
			else if ( e.Button == MouseButtons.Left ) {
				Scene.SetMouseForce( delta.X, delta.Y );
			}
		}

		protected override void OnMouseDown( MouseEventArgs e ) {
			if ( e.Button == MouseButtons.Left ) {
				bool pickFound = Scene.Pick( e.X, e.Y );
				if ( pickFound ) {
					// TODO: select correct node in mainForm.sceneTreeView
				}
			}
		}

		protected override void OnMouseWheel( MouseEventArgs e ) {
			Scene.CurrentCamera.Zoom( -e.Delta * 0.001f );
		}

		public void KeyPressed(char key) {
			CGelloid newGelly = Scene.Gelloids.Count > 0 ? Scene.Gelloids[0] : null;
			CCamera camera = Scene.CurrentCamera;
			CParticle particle;
			switch ( (int)Char.ToUpper(key) ) {
					// Esc was pressed -> QUIT
				case (int)Keys.Escape: this.Close(); break;
					// Enter: move a random particle to a random position
				case (int)Keys.Enter:
					particle = newGelly.FindParticle(_random.Next(newGelly.ParticlesCurrent.Count));
					particle.Position = new Vector3( (float)_random.NextDouble()*100.0f-50.0f, (float)_random.NextDouble()*100.0f+50.0f, (float)_random.NextDouble()*100.0f-50.0f );
					break;
					// R: Pause simulation
				case 'R': SimulationRunning = !SimulationRunning; break;
					// O: Fix/unfix particle 0
				case (int)Keys.O:
					particle = newGelly.FindParticle(0);
					particle.NotMovable = !particle.NotMovable;
					break;
					// 8: Move particle up
				case '8':
					particle = newGelly.FindParticle(0);
					particle.Position = particle.Position + new Vector3( 0, 1, 0 );
					break;
					// C: Center camera on gelloid
				case 'C': CameraCentered = !CameraCentered; break;
					// Y: Rotate camera position around the gelloid
				case 'Y': CameraRotating = !CameraRotating; break;
					// ²: Frame by frame
				case '²': FrameByFrame = !FrameByFrame; break;
					// Z: Camera forward
				case 'Z': camera.Walk( 1.0f ); break;
					// S: Camera backward
				case 'S': camera.Walk( -1.0f ); break;
					// Q: Camera left slide
				case 'Q': camera.Strafe( -1.0f ); break;
					// D: Camera right slide
				case 'D': camera.Strafe( 1.0f ); break;
					// A: Camera -Roll
				case 'A': camera.Roll( -0.1f ); break;
					// 'E': Camera +Roll
				case 'E': camera.Roll( 0.1f ); break;
					// T: Reset gelloid
				case 'T': Scene.Reset(); break;
					// N: Next camera
				case 'N': Scene.NextCamera(); break;
					// B: Camera on plane
				case 'B': OnPlane = !OnPlane; break;
					// F: Friction
				case 'F': newGelly.UseFriction = !newGelly.UseFriction; break;
					// W: Wind
				case 'W': newGelly.UseWind = !newGelly.UseWind; break;
					// K: Damping
				case 'K': newGelly.UseDamping = !newGelly.UseDamping; break;
					// G: Gravity
				case 'G': newGelly.UseGravity = !newGelly.UseGravity; break;
					// M: Mouse
				case 'M': newGelly.UseMouse = !newGelly.UseMouse; break;
					// X: add x velocity
				case 'X': newGelly.AddVelocity( new Vector3( 10.0f, 0, 0 ) ); break;
					// H: display help
				case 'H': DisplayHelp = !DisplayHelp; break;
/*
					// /: load scene from file
				case '/': {
					scene = new CScene();
					XmlSerializer serializer = new XmlSerializer( typeof( CScene ) );
					TextReader r = new StreamReader( @"c:\scene.xml" );
					scene = (CScene)serializer.Deserialize( r );
					r.Close();
					scene.Initialize();
					scene.InitializeGraphics( device, font );
					break;
				}
					// *: dump scene to file
				case '*': {
					XmlSerializer serializer = new XmlSerializer( typeof( CScene ) );
					TextWriter w = new StreamWriter( @"c:\scene.xml" );
					serializer.Serialize( w, scene );
					w.Close();
					break;
				}
					// =: load a collider (must know the type before loading it)
				case '=': {
					CComplexSphere collider = new CComplexSphere();
					XmlSerializer serializer = new XmlSerializer( typeof( CComplexSphere ) );
					TextReader r = new StreamReader( @"c:\collider.xml" );
					collider = (CComplexSphere)serializer.Deserialize( r );
					r.Close();
					collider.InitializeGraphics( device, font );
					scene.AddCollider( collider );
					break;
				}
					// +: save a collider
				case '+': {
					CCollider collider = scene.colliders[1];
					XmlSerializer serializer = new XmlSerializer( collider.GetType() );
					TextWriter w = new StreamWriter( @"c:\collider.xml" );
					serializer.Serialize( w, collider );
					w.Close();
					break;
				}
				*/
					// 1: Draw particles, normal or sphere
				case '1':
					if ( newGelly.DrawParticles )
						if ( newGelly.DrawSphereParticles ) {
							newGelly.DrawParticles = false;
							newGelly.DrawSphereParticles = false;
						}
						else
							newGelly.DrawSphereParticles = true;
					else
						newGelly.DrawParticles = true;
					break;
					// 2: Draw springs, non structural, broken
				case '2':
					if ( newGelly.DrawSprings )
						if ( newGelly.DrawSpringsNonStructural ) {
							newGelly.DrawSprings = false;
							newGelly.DrawSpringsNonStructural = false;
							newGelly.DrawSpringsBroken = false;
						}
						else {
							newGelly.DrawSpringsNonStructural = true;
							newGelly.DrawSpringsBroken = true;
						}
					else
						newGelly.DrawSprings = true;
					break;
					// 3: Draw vectors, force, velocity
				case '3':
					if ( newGelly.DrawVectors ) {
						newGelly.DrawVectors = false;
						newGelly.DrawForce = false;
						newGelly.DrawVelocity = false;
					}
					else {
						newGelly.DrawVectors = true;
						newGelly.DrawForce = true;
						newGelly.DrawVelocity = true;
					}
					break;
					// 4: Draw triangles, textured
				case '4':
					if ( newGelly.DrawTriangles )
						if ( newGelly.DrawTextured ) {
							newGelly.DrawTriangles = false;
							newGelly.DrawTextured = false;
						}
						else
							newGelly.DrawTextured = true;
					else
						newGelly.DrawTriangles = true;
					break;
					// 5: Draw triangle normal, vertex normal
				case '5':
					if ( newGelly.DrawTriangleNormal )
						if ( newGelly.DrawVertexNormal ) {
							newGelly.DrawTriangleNormal = false;
							newGelly.DrawVertexNormal = false;
						}
						else
							newGelly.DrawVertexNormal = true;
					else
						newGelly.DrawTriangleNormal = true;
					break;
					// 6: Draw colliders
				case '6': Scene.DrawColliders = !Scene.DrawColliders; break;
					// 7: Display stats
				case '7': newGelly.DisplayStats = !newGelly.DisplayStats; break;
					// Default: Next frame only if frame by frame activated
				default: _nextFrame = true; break;
			}

		}

		protected override void OnKeyPress( KeyPressEventArgs e ) {
			KeyPressed(e.KeyChar);
		}

		//static void Main() {
		public void Run() {
//			using (RenderForm frm = new RenderForm()) {
//				frm.scene = new CScene();
//
//				frm.InitializeGelloid();
//
//				
//
//				if ( !frm.InitializeGraphics() ) { // Initialize Direct3D
//					MessageBox.Show( "Could not initialize Direct3D.  This program will exit." );
//					return;
//				}
//
//				//Show the window
//				frm.Show();
//				// While the form is still valid, render and process messages
//				while ( frm.Created ) {
//					frm.Render(); //------------------------------------------------------------------------------------------------
//					Application.DoEvents();
//				}
//			}
			if ( !InitializeGraphics() ) {
				MessageBox.Show( "Could not initialize Direct3D.  This program will exit." );
				return;
			}

			FormRunning = true;
			Show();
			while ( Created ) {
				Render();
				Application.DoEvents();
			}
			FormRunning = false;
		}

		public bool InitializeGraphics() {
			try {
				// Create a storage for presentation settings
				PresentParameters presentParams = new PresentParameters();
				// Select windowed mode
				presentParams.Windowed = true;
				// Let the system select the most efficient presentation technique
				presentParams.SwapEffect = SwapEffect.Discard;
				// Z-Buffer
				presentParams.EnableAutoDepthStencil = true;
				presentParams.AutoDepthStencilFormat = DepthFormat.D16;

				// Create the Direct3D device
				_device = new Device(
					// Select Adapter #0
					0, 
					// Choose hardware to use graphic adapter acceleration
					DeviceType.Hardware, 
					// The rendering target Control
					this, 
					// Select the vertex processing method
					CreateFlags.HardwareVertexProcessing,
					// Pass the presentation settings
					presentParams);
				// Handle device creation event
				//--device.DeviceCreated += new System.EventHandler( this.OnCreateDevice );
				OnCreateDevice(_device, null);
				// Handle device reset event
				_device.DeviceReset += OnResetDevice;
				OnResetDevice(_device, null);

				_font = new CFont( _device, new System.Drawing.Font("Verdana", 10 ) );

				Scene.InitializeGraphics( _device, _font );

				//That's all
				return true;
			}
			catch ( DirectXException ) { 
				return false; 
			}
		}

		public void OnCreateDevice(object sender, EventArgs e) {
		}

		public void OnResetDevice(object sender, EventArgs e) {
			Device dev = (Device)sender;
			// Turn off culling, so we see the front and back of the triangle
			dev.RenderState.CullMode = Cull.None;
			// Select wireframe mode
			dev.RenderState.FillMode = FillMode.WireFrame;
			dev.RenderState.ZBufferEnable = true;
		}

		private void InitializeComponent() {
			// 
			// RenderForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(800, 600);
			this.Name = "RenderForm";
			this.Text = "Physics Render";

		}

		private void Render() {
			if ( _device == null )
				return;
			// Clear the backbuffer to a black color 
			_device.Clear( ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1.0f, 0);
			// Begin the scene
			_device.BeginScene();
			_device.RenderState.Lighting = false;

			// Gelly is not transformed
			_device.Transform.World = Matrix.Identity;

			if ( ( _nextFrame || !FrameByFrame ) && SimulationRunning ) {
			    float time = LastTime + (MaxTimeStep * TimeIterations);
				while ( LastTime < time ) {
					float deltaTime = time - LastTime;
					if ( deltaTime > MaxTimeStep )
						deltaTime = MaxTimeStep;

					Scene.Simulate( deltaTime );
					LastTime += deltaTime;
				}
				LastTime = time;

				_nextFrame = false;
			}

			Scene.Render();

			// Transform View
			// Store the rotation period
			const int period = 5000;
			if ( CameraRotating ) //Compute new angle from a time reference
				_cameraAngle = (Environment.TickCount % period) * 2.0f * (float)Math.PI / (float)period;
			if ( CameraCentered ) {
				//scene.camera.Center( scene.gelloids[0].BoundingBoxCenter() );
				//scene.camera.look = scene.gelloids[0].BoundingBoxCenter();
				Scene.CurrentCamera.SetTarget( Scene.Gelloids[0].BoundingBoxCenter() );
				//Vector3 diff = scene.camera.look - scene.camera.pos;
				//scene.camera.radius = diff.Length();

				if ( OnPlane )
					Scene.CurrentCamera.Pos = new Vector3( Scene.CurrentCamera.Pos.X, -50.0f, Scene.CurrentCamera.Pos.Z );
				if ( CameraRotating )
					Scene.CurrentCamera.Pos.TransformNormal(Matrix.RotationY(_cameraAngle));
			}
			else if ( CameraRotating ) {
				//scene.camera.position = scene.camera.target + new Vector3( (float)System.Math.Cos( cameraAngle ), 0, (float)System.Math.Sin( cameraAngle ) ) * 100.0f;
				Scene.CurrentCamera.SetPosition( new Vector3( (float)Math.Cos( _cameraAngle ), 0, (float)Math.Sin( _cameraAngle ) ) * 100.0f );
			}

			_font.DrawText("Gravity: "+CUtils.Vector3ToStr(Scene.Gravity), 0, 450, Color.White, false );
			_font.DrawText("Wind Direction: "+CUtils.Vector3ToStr(Scene.WindDirection), 0, 460, Color.White, false );
			_font.DrawText("Wind Speed: "+Scene.WindSpeed, 0, 470, Color.White, false );
			_font.DrawText("Mouse Force: "+CUtils.Vector3ToStr(Scene.MouseForce), 0, 480, Color.White, false );
			_font.DrawText("Mouse Ks: "+Scene.MouseKs, 0, 490, Color.White, false );
			_font.DrawText("Position: "+CUtils.Vector3ToStr(Scene.CurrentCamera.Pos), 0, 500, Color.White, false);
			_font.DrawText("LookAt: "+CUtils.Vector3ToStr(Scene.CurrentCamera.Look), 0, 510, Color.White, false);
			_font.DrawText("Up: "+CUtils.Vector3ToStr(Scene.CurrentCamera.Up), 0, 520, Color.White, false);
			_font.DrawText("Right: "+CUtils.Vector3ToStr(Scene.CurrentCamera.Right), 0, 530, Color.White, false);

			if ( DisplayHelp ) {
				int y = 0; int x = 500;
				_font.DrawText("H:display this help",x,y,Color.White,false); y+= 10;
				_font.DrawText("I:toggle between predefined scene",x,y,Color.White,false); y+= 10;
				_font.DrawText("R:pause simulation",x,y,Color.White,false); y+= 10;
				_font.DrawText("C:center camera on gelloid",x,y,Color.White,false); y+= 10;
				_font.DrawText("N:next camera",x,y,Color.White,false); y+= 10;
				_font.DrawText("T:reset gelloid",x,y,Color.White,false); y+= 10;
				_font.DrawText("W:enable/disable wind",x,y,Color.White,false); y+= 10;
				_font.DrawText("F:enable/disable friction",x,y,Color.White,false); y+= 10;
				_font.DrawText("G:enable/disable gravity",x,y,Color.White,false); y+= 10;
				_font.DrawText("K:enable/disable damping",x,y,Color.White,false); y+= 10;
				_font.DrawText("M:enable/disable mouse move",x,y,Color.White,false); y+= 10;
				y+= 10;
				_font.DrawText("Z:walk forward",x,y,Color.White,false); y+= 10;
				_font.DrawText("S:walk backward",x,y,Color.White,false); y+= 10;
				_font.DrawText("Q:strafe left",x,y,Color.White,false); y+= 10;
				_font.DrawText("D:strafe right",x,y,Color.White,false); y+= 10;
				_font.DrawText("A:roll left",x,y,Color.White,false); y+= 10;
				_font.DrawText("E:roll right",x,y,Color.White,false); y+= 10;
				y+= 10;
				_font.DrawText("1:display particles",x,y,Color.White,false); y+= 10;
				_font.DrawText("2:display springs",x,y,Color.White,false); y+= 10;
				_font.DrawText("3:display force/velocity",x,y,Color.White,false); y+= 10;
				_font.DrawText("4:display faces/textures",x,y,Color.White,false); y+= 10;
				_font.DrawText("5:display normals",x,y,Color.White,false); y+= 10;
				_font.DrawText("6:display colliders",x,y,Color.White,false); y+= 10;
				_font.DrawText("7:display stats",x,y,Color.White,false); y+= 10;
				y+= 10;
				_font.DrawText("Mouse move+right-click:move camera",x,y,Color.White,false); y+= 10;
				_font.DrawText("Mouse wheel: zoom",x,y,Color.White,false); y+= 10;
			}
			else {
				int y = 0; int x = 500;
				_font.DrawText("H:display help",x,y,Color.White,false); y+= 10;
			}
			_device.Transform.World = Scene.CurrentCamera.World;
			_device.Transform.View = Scene.CurrentCamera.View;
			_device.Transform.Projection = Scene.CurrentCamera.Projection;
			//End the scene and present the result
			_device.EndScene();
			_device.Present();
		}
	}
}
