using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Linq;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

using MyGelloidSharp.PhysEnv;
using MyGelloidSharp.Colliders;
using MyGelloidSharp.Cameras;

namespace MyGelloidSharp
{
    /// <summary>
    /// Summary description for Scene.
    /// </summary>
    [XmlRoot("Scene")]
    public class CScene
    {
        #region Fields

        // Gelloids in scene
        [XmlElement("Gelloid")] public List<CGelloid> Gelloids;

        // Colliders in scene
        // We have to list every kind of collider. TODO: http://www.codeproject.com/csharp/XmlSerializerForUnknown.asp
        [XmlElement(typeof (CSimplePlane))] [XmlElement(typeof (CComplexPlane))] [XmlElement(typeof (CSimpleSphere))] [XmlElement(typeof (CComplexSphere))] [XmlElement(typeof (CCircle))] [XmlElement(typeof (CSquare))] [XmlElement("Collider")] public List<CCollider> Colliders;

        // Cameras
        [XmlElement("Camera")] public List<CCamera> Cameras;
        [XmlElement("CurrentCamera")] private int _currentCameraId;

        // Variables
        [XmlElement("Gravity")] public Vector3 Gravity = new Vector3(0, -0.2f, 0);
        [XmlElement("WindDirection")] public Vector3 WindDirection = new Vector3(0, 0, 1);
        [XmlElement("WindSpeed")] public float WindSpeed = 0.0f;
        [XmlIgnore] public Vector3 MouseForce = new Vector3(0, 0, 0);
        [XmlElement("MouseKs")] public float MouseKs = 2.0f;

        //// Constants
        ////[XmlIgnore] public const float GravitationalConstant = 6.67384E-11f; // N.m²/kg²
        //[XmlIgnore]
        //public const float GravitationalConstant = 10f; // N.m²/kg²

        // Scene name
        [XmlElement("Name")] public string Name = "Scene";

        [XmlElement("drawColliders")] public bool DrawColliders = true;

        [XmlIgnore] private Device _device;
        [XmlIgnore] private CFont _font;

        #endregion

        #region Constructors

        public CScene()
        {
            Gelloids = new List<CGelloid>();
            Colliders = new List<CCollider>();
            Cameras = new List<CCamera>();
        }

        #endregion

        #region Properties and Indexers

        [XmlIgnore]
        public CCamera CurrentCamera
        {
            get { return Cameras[_currentCameraId]; }
        }

        #endregion

        #region Xml Load/Save

        // Save to Xml
        public static void XmlExport(CScene scene, string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof (CScene));
            TextWriter w = new StreamWriter(filePath);
            serializer.Serialize(w, scene);
            w.Close();
        }

        // Load from Xml
        public static CScene XmlImport(string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof (CScene));
            TextReader r = new StreamReader(filePath);
            CScene scene = (CScene) serializer.Deserialize(r);
            r.Close();

            return scene;
        }

        #endregion

        #region Predefined scene creation methods

        public static CScene CreateTest()
        {
            //CScene scene = new CScene();
            //scene.Clear();
            //scene.Name = "Test Scene";

            //// Sphere 1
            //CGelloid sphere1 = CGelloid.CreateSphere("Sphere1", 20, 10, 10);
            //sphere1.Mass = 100;
            //sphere1.UseGravity = false;
            //sphere1.UseUniversalGravitation = true;
            //sphere1.UseDamping = false;
            //sphere1.UseWind = false;
            //sphere1.UseFriction = false;
            //sphere1.DrawTriangles = true;
            //sphere1.DrawTextured = false;
            //sphere1.DrawParticles = true;
            //sphere1.DrawSphereParticles = true;
            //sphere1.DrawSprings = true;
            //sphere1.DrawSpringsNonStructural = true;
            //sphere1.DrawVectors = true;
            //sphere1.DrawVelocity = true;
            //sphere1.DrawForce = true;
            //sphere1.DrawTriangleNormal = false;
            //sphere1.DrawVertexNormal = false;
            //sphere1.Translate(new Vector3(100, 0, 0));
            //sphere1.AddVelocity(new Vector3(0, 0, -3));

            //scene.AddGelloid(sphere1);

            //// Sphere 2
            //CGelloid sphere2 = CGelloid.CreateSphere("Sphere2", 20, 10, 10);
            //sphere2.Mass = 100;
            //sphere2.UseGravity = false;
            //sphere2.UseUniversalGravitation = true;
            //sphere2.UseDamping = false;
            //sphere2.UseWind = false;
            //sphere2.UseFriction = false;
            //sphere2.DrawTriangles = true;
            //sphere2.DrawTextured = false;
            //sphere2.DrawParticles = true;
            //sphere2.DrawSphereParticles = true;
            //sphere2.DrawSprings = true;
            //sphere2.DrawSpringsNonStructural = true;
            //sphere2.DrawVectors = true;
            //sphere2.DrawVelocity = true;
            //sphere2.DrawForce = true;
            //sphere2.DrawTriangleNormal = false;
            //sphere2.DrawVertexNormal = false;
            //sphere2.Translate(new Vector3(-100, 0, 0));
            //sphere2.AddVelocity(new Vector3(0, 0, 3));

            //scene.AddGelloid(sphere2);

            //// Sphere 1
            //CGelloid sphere1 = CGelloid.CreateSphere("Sphere1", 20, 10, 10);
            //sphere1.Mass = 100;
            //sphere1.UseGravity = false;
            //sphere1.UseUniversalGravitation = true;
            //sphere1.UseDamping = false;
            //sphere1.UseWind = false;
            //sphere1.UseFriction = false;
            //sphere1.DrawTriangles = true;
            //sphere1.DrawTextured = false;
            //sphere1.DrawParticles = true;
            //sphere1.DrawSphereParticles = true;
            //sphere1.DrawSprings = true;
            //sphere1.DrawSpringsNonStructural = true;
            //sphere1.DrawVectors = true;
            //sphere1.DrawVelocity = true;
            //sphere1.DrawForce = true;
            //sphere1.DrawTriangleNormal = false;
            //sphere1.DrawVertexNormal = false;
            //sphere1.Translate(new Vector3(500, 0, 0));
            //sphere1.AddVelocity(new Vector3(0, 0, -30));

            //scene.AddGelloid(sphere1);

            //// Sphere 2
            //CGelloid sphere2 = CGelloid.CreateSphere("Sphere2", 20, 10, 10);
            //sphere2.Mass = 50000;
            //sphere2.UseGravity = false;
            //sphere2.UseUniversalGravitation = true;
            //sphere2.UseDamping = false;
            //sphere2.UseWind = false;
            //sphere2.UseFriction = false;
            //sphere2.DrawTriangles = true;
            //sphere2.DrawTextured = false;
            //sphere2.DrawParticles = true;
            //sphere2.DrawSphereParticles = true;
            //sphere2.DrawSprings = true;
            //sphere2.DrawSpringsNonStructural = true;
            //sphere2.DrawVectors = true;
            //sphere2.DrawVelocity = true;
            //sphere2.DrawForce = true;
            //sphere2.DrawTriangleNormal = false;
            //sphere2.DrawVertexNormal = false;
            //sphere2.Translate(new Vector3(0, 0, 0));
            //sphere2.AddVelocity(new Vector3(0, 0, 0));

            //scene.AddGelloid(sphere2);

            //// Sphere 3
            //CGelloid sphere3 = CGelloid.CreateSphere("Sphere3", 20, 10, 10);
            //sphere3.Mass = 100;
            //sphere3.UseGravity = false;
            //sphere3.UseUniversalGravitation = true;
            //sphere3.UseDamping = false;
            //sphere3.UseWind = false;
            //sphere3.UseFriction = false;
            //sphere3.DrawTriangles = true;
            //sphere3.DrawTextured = false;
            //sphere3.DrawParticles = true;
            //sphere3.DrawSphereParticles = true;
            //sphere3.DrawSprings = true;
            //sphere3.DrawSpringsNonStructural = true;
            //sphere3.DrawVectors = true;
            //sphere3.DrawVelocity = true;
            //sphere3.DrawForce = true;
            //sphere3.DrawTriangleNormal = false;
            //sphere3.DrawVertexNormal = false;
            //sphere3.Translate(new Vector3(0, 0, 100));
            //sphere3.AddVelocity(new Vector3(3, 0, 0));

            //scene.AddGelloid(sphere3);

            //// Sphere 4
            //CGelloid sphere4 = CGelloid.CreateSphere("Sphere4", 20, 10, 10);
            //sphere4.Mass = 100;
            //sphere4.UseGravity = false;
            //sphere4.UseUniversalGravitation = true;
            //sphere4.UseDamping = false;
            //sphere4.UseWind = false;
            //sphere4.UseFriction = false;
            //sphere4.DrawTriangles = true;
            //sphere4.DrawTextured = false;
            //sphere4.DrawParticles = true;
            //sphere4.DrawSphereParticles = true;
            //sphere4.DrawSprings = true;
            //sphere4.DrawSpringsNonStructural = true;
            //sphere4.DrawVectors = true;
            //sphere4.DrawVelocity = true;
            //sphere4.DrawForce = true;
            //sphere4.DrawTriangleNormal = false;
            //sphere4.DrawVertexNormal = false;
            //sphere4.Translate(new Vector3(0, 0, -100));
            //sphere4.AddVelocity(new Vector3(-3, 0, 0));

            //scene.AddGelloid(sphere4);

            //// Collider
            //scene.AddCollider(new CComplexPlane("Ground", new Vector3(0, 1, 0), 60)); // Ground

            //// Create camera
            //CCamera cam = new CCamera("Camera1");
            //cam.Initialize(new Vector3(-100, 100, -100), new Vector3(0, 0, 0));

            //scene.AddCamera(cam);

            //// Initialize scene
            //scene.Initialize();

            //return scene;

            CScene scene = new CScene();
            scene.Clear();
            scene.Name = "Test Scene";

            // Create drape
            CGelloid gelly = new CGelloid("Test");
            gelly.AddParticle(new Vector3(0, 30, 0), new Vector3(0, 0, 0));
            gelly.AddParticle(new Vector3(0, 50, 0), new Vector3(0, 0, 0));
            gelly.AddSpring(0, 1, CSpring.ESpringKind.Structural);

            gelly.UseGravity = true;
            gelly.UseDamping = true;
            gelly.UseWind = false;
            gelly.UseFriction = false;
            gelly.DrawTriangles = false;
            gelly.DrawTextured = false;
            gelly.DrawParticles = true;
            gelly.DrawSphereParticles = true;
            gelly.DrawSprings = true;
            gelly.DrawSpringsNonStructural = false;
            gelly.DrawVectors = false;
            gelly.DrawTriangleNormal = false;
            gelly.DrawVertexNormal = false;

            scene.AddGelloid(gelly);

            scene.AddCollider(new CComplexPlane("Ground", new Vector3(0, 1, 0), 60)); // Ground

            //scene.AddCollider( new CCylinder( "Cylinder1", new Vector3( 0, -30, 0 ), new Vector3( 0, 1, 0 ), 20, 50 ) );
            scene.AddCollider(new CCylinder("Cylinder1", new Vector3(5, -30, 0), new Vector3(0, 1, 1), 20, 50));

            // Set scene variables
            scene.Gravity = new Vector3(0, -2.0f, 0);
            scene.WindSpeed = 2.0f;

            // Create camera
            CCamera cam = new CCamera("Camera1");
            cam.Initialize(new Vector3(-100, 100, -100), new Vector3(0, 0, 0));

            scene.AddCamera(cam);

            // Initialize scene
            scene.Initialize();

            return scene;
        }

        public static CScene CreateMancheAir()
        {
            CScene scene = new CScene();
            scene.Clear();
            scene.Name = "Manche A Air";

            // Create cylinder
            const int nSides = 10;
            const int height = 10;
            CGelloid gelly = CGelloid.CreateCylinder("Cylinder", nSides, height, 12.0f, 10.0f, true, true, true, true, false);

            //gelly.Translate( new Vector3( 0, 60, 0 ) );
            //gelly.Translate( new Vector3( 0, 150, 0 ) );

            gelly.UseGravity = true;
            gelly.UseDamping = true;
            gelly.UseWind = false;
            gelly.UseFriction = false;
            gelly.DrawTriangles = false;
            gelly.DrawTextured = false;
            gelly.DrawParticles = true;
            gelly.DrawSphereParticles = true;
            gelly.DrawSprings = true;
            gelly.DrawSpringsNonStructural = true;
            gelly.DrawVectors = false;
            gelly.DrawTriangleNormal = false;
            gelly.DrawVertexNormal = false;

            gelly.TextureName = "tritile.bmp";

            // Fix some particles
            for (int i = 0; i < nSides; i++)
            {
                gelly.ParticlesOriginal[gelly.ParticlesOriginal.Count - i*height - 1].NotMovable = true;
            }

            scene.AddGelloid(gelly);
            // Add colliders
            //scene.AddCollider( new CComplexPlane( "Ground", new Vector3( 0, 1, 0 ), 40 ) ); // Bottom
            //scene.AddCollider( new CComplexSphere( "Sphere1", new Vector3( 0, 0, 0 ), 20 ) );

            // Set scene variables
            scene.Gravity = new Vector3(-0.05f, -0.2f, 0);
            scene.WindSpeed = 10.0f;
            scene.WindDirection = new Vector3(0, 1, 0);

            // Create camera
            CCamera cam = new CCamera("Camera1");
            cam.Initialize(new Vector3(-100, 100, -100), new Vector3(0, 0, 0));
            scene.AddCamera(cam);

            cam = new CCamera("Camera2");
            cam.Initialize(new Vector3(-9, -59, -43), new Vector3(0.115f, 0.550f, 0.821f));
            scene.AddCamera(cam);

            // Initialize scene
            scene.Initialize();

            return scene;
        }

        public static CScene CreateFallingDrape()
        {
            CScene scene = new CScene();
            scene.Clear();
            scene.Name = "Falling Drape Scene";

            // Create drape
            CGelloid gelly = CGelloid.CreateCube("Drape", 15, 1, 15, 10, 0, 10, true, true, true);

            //gelly.Rotate( new Vector3( 0, 0, 1 ), 0.3f );
            gelly.AddAngularMom(new Vector3(0, 1, 0), 0.3f);
            gelly.Translate(new Vector3(0, 50, 0));

            gelly.UseGravity = true;
            gelly.UseDamping = true;
            gelly.UseWind = false;
            gelly.UseFriction = false;
            gelly.DrawTriangles = true;
            gelly.DrawTextured = false;
            gelly.DrawParticles = false;
            gelly.DrawSphereParticles = false;
            gelly.DrawSprings = false;
            gelly.DrawSpringsNonStructural = false;
            gelly.DrawVectors = false;
            gelly.DrawTriangleNormal = false;
            gelly.DrawVertexNormal = false;

            gelly.TextureName = "leaf5.jpg";

            scene.AddGelloid(gelly);

            // Add colliders
            scene.AddCollider(new CComplexSphere("Sphere1", new Vector3(-30, 0, 30), 20));
            scene.AddCollider(new CComplexSphere("Sphere2", new Vector3(30, 0, 30), 20));
            scene.AddCollider(new CComplexSphere("Sphere3", new Vector3(-30, 0, -30), 20));
            scene.AddCollider(new CComplexSphere("Sphere4", new Vector3(30, 0, -30), 20));

            scene.AddCollider(new CComplexPlane("Ground", new Vector3(0, 1, 0), 60)); // Ground

            // Set scene variables
            scene.Gravity = new Vector3(0, -1.0f, 0);
            scene.WindSpeed = 2.0f;

            // Create camera
            CCamera cam = new CCamera("Camera1");
            cam.Initialize(new Vector3(-100, 100, -100), new Vector3(0, 0, 0));

            scene.AddCamera(cam);

            // Initialize scene
            scene.Initialize();

            return scene;
        }

        public static CScene CreateSpiral()
        {
            CScene scene = new CScene();
            scene.Clear();
            scene.Name = "Sphere Scene";

            // Create gelloid
            CGelloid gelly = CGelloid.CreateSpiral("Spiral", 20, 5, 20.0f, 5.0f, true, true, true);
            gelly.UseGravity = true;
            gelly.UseDamping = true;
            gelly.UseWind = false;
            gelly.UseFriction = false;
            gelly.DrawTriangles = false;
            gelly.DrawParticles = true;
            gelly.DrawSphereParticles = false;
            gelly.DrawSprings = true;
            gelly.DrawSpringsNonStructural = true;
            gelly.DrawVectors = false;
            gelly.DrawTriangleNormal = false;
            gelly.DrawVertexNormal = false;

            scene.AddGelloid(gelly);

            // Add colliders
            scene.AddCollider(new CComplexPlane("Ground", new Vector3(0, 1, 0), 60)); // Bottom

            // Set scene variables
            scene.Gravity = new Vector3(0, -2.0f, 0);
            scene.WindSpeed = 2.0f;

            // Create camera
            CCamera cam = new CCamera("Camera1");
            cam.Initialize(new Vector3(-100, 100, -100), new Vector3(0, 0, 0));

            scene.AddCamera(cam);

            // Initialize scene
            scene.Initialize();

            return scene;
        }

        public static CScene CreateSphere()
        {
            CScene scene = new CScene();
            scene.Clear();
            scene.Name = "Sphere Scene";

            // Create gelloid
            CGelloid gelly = CGelloid.CreateSphere("Sphere", 10.0f, 10, 10);
            gelly.UseGravity = true;
            gelly.UseDamping = true;
            gelly.UseWind = false;
            gelly.UseFriction = false;
            gelly.DrawTriangles = false;
            gelly.DrawParticles = true;
            gelly.DrawSphereParticles = false;
            gelly.DrawSprings = true;
            gelly.DrawSpringsNonStructural = false;
            gelly.DrawVectors = false;
            gelly.DrawTriangleNormal = false;
            gelly.DrawVertexNormal = false;

            scene.AddGelloid(gelly);

            // Add colliders
            scene.AddCollider(new CComplexPlane("Ground", new Vector3(0, 1, 0), 60)); // Bottom

            // Set scene variables
            scene.Gravity = new Vector3(0, -2.0f, 0);
            scene.WindSpeed = 2.0f;

            // Create camera
            CCamera cam = new CCamera("Camera1");
            cam.Initialize(new Vector3(-100, 100, -100), new Vector3(0, 0, 0));

            scene.AddCamera(cam);

            // Initialize scene
            scene.Initialize();

            return scene;
        }

        public static CScene CreateTube()
        {
            CScene scene = new CScene();
            scene.Clear();
            scene.Name = "Tube Scene";

            // Create gelloid
            CGelloid gelly = CGelloid.CreateTube("Tube", 15, 20, 12.0f, 25.0f, 10.0f, true, true, true, true);

            //gelly.Translate( new Vector3( 0, 60, 0 ) );
            gelly.Translate(new Vector3(0, 150, 0));

            gelly.UseGravity = true;
            gelly.UseDamping = true;
            gelly.UseWind = false;
            gelly.UseFriction = false;
            gelly.DrawTriangles = true;
            gelly.DrawTextured = true;
            gelly.DrawParticles = false;
            gelly.DrawSphereParticles = false;
            gelly.DrawSprings = false;
            gelly.DrawSpringsNonStructural = false;
            gelly.DrawVectors = false;
            gelly.DrawTriangleNormal = false;
            gelly.DrawVertexNormal = false;

            //gelly.textureName = "bowtiles.bmp";
            gelly.TextureName = "curveterracotta.bmp";

            scene.AddGelloid(gelly);
            // Add colliders
            //scene.AddCollider( new CComplexPlane( "Ground", new Vector3( 0, 1, 0 ), 40 ) ); // Bottom
            scene.AddCollider(new CComplexSphere("Sphere1", new Vector3(0, 0, 0), 20));

            // Set scene variables
            scene.Gravity = new Vector3(0, -2.0f, 0);
            scene.WindSpeed = 2.0f;

            // Create camera
            CCamera cam = new CCamera("Camera1");
            cam.Initialize(new Vector3(-100, 100, -100), new Vector3(0, 0, 0));
            scene.AddCamera(cam);

            cam = new CCamera("Camera2");
            cam.Initialize(new Vector3(-9, -59, -43), new Vector3(0.115f, 0.550f, 0.821f));
            scene.AddCamera(cam);

            // Initialize scene
            scene.Initialize();

            return scene;
        }

        public static CScene CreateCylinder()
        {
            CScene scene = new CScene();
            scene.Clear();
            scene.Name = "Cylinder Scene";

            // Create gelloid
            CGelloid gelly = CGelloid.CreateCylinder("Cylinder", 3, 4, 12.0f, 10.0f, true, true, true, true, true);

            //gelly.Translate( new Vector3( 0, 60, 0 ) );
            gelly.Translate(new Vector3(0, 150, 0));

            gelly.UseGravity = true;
            gelly.UseDamping = true;
            gelly.UseWind = false;
            gelly.UseFriction = false;
            gelly.DrawTriangles = false;
            gelly.DrawTextured = false;
            gelly.DrawParticles = true;
            gelly.DrawSphereParticles = true;
            gelly.DrawSprings = true;
            gelly.DrawSpringsNonStructural = true;
            gelly.DrawVectors = false;
            gelly.DrawTriangleNormal = false;
            gelly.DrawVertexNormal = false;

            gelly.TextureName = "pebbles4.bmp";

            scene.AddGelloid(gelly);
            // Add colliders
            scene.AddCollider(new CComplexPlane("Ground", new Vector3(0, 1, 0), 40)); // Bottom
            scene.AddCollider(new CComplexSphere("Sphere1", new Vector3(0, 0, 0), 20));

            // Set scene variables
            scene.Gravity = new Vector3(0, -2.0f, 0);
            scene.WindSpeed = 2.0f;

            // Create camera
            CCamera cam = new CCamera("Camera1");
            cam.Initialize(new Vector3(-100, 100, -100), new Vector3(0, 0, 0));
            scene.AddCamera(cam);

            cam = new CCamera("Camera2");
            cam.Initialize(new Vector3(-70, -5, -150), new Vector3(0, 0, 0));
            scene.AddCamera(cam);

            // Initialize scene
            scene.Initialize();

            return scene;
        }

        public static CScene CreateCuboid()
        {
            CScene scene = new CScene();
            scene.Clear();
            scene.Name = "Cuboid Scene";

            // Create gelloid
            const int sizeX = 5;
            const int sizeY = 5;
            const int sizeZ = 6;
            CGelloid newGelly = CGelloid.CreateCube("Cuboid", sizeX, sizeY, sizeZ, 10.0f, 10.0f, 10.0f, true, true, true);
            newGelly.Translate(new Vector3(0, 60, 0));

            newGelly.UseGravity = true;
            newGelly.UseDamping = true;
            newGelly.UseWind = false;
            newGelly.UseFriction = false;

            newGelly.TextureName = "water20.bmp";

            scene.AddGelloid(newGelly);

            // Add colliders
            scene.AddCollider(new CComplexPlane("Ground", new Vector3(0, 1, 0), 60)); // Bottom
            scene.AddCollider(new CComplexSphere("Sphere1", new Vector3(0, 0, -1), 20));
            scene.AddCollider(new CComplexSphere("Sphere2", new Vector3(15, -40, 40), 20));

            // Set scene variables
            scene.Gravity = new Vector3(0, -2.0f, 0);
            scene.WindSpeed = 2.0f;

            // Create camera
            CCamera camera = new CCamera("Camera1");
            camera.Initialize(new Vector3(-100, 100, -100), new Vector3(0, 0, 0));

            scene.AddCamera(camera);

            // Initialize scene
            scene.Initialize();

            return scene;
        }

        public static CScene CreateFlag()
        {
            CScene scene = new CScene();
            scene.Clear();
            scene.Name = "Flag Scene";

            // Create flag
            const int sizeX = 10;
            const int sizeY = 10;
            const int sizeZ = 1;
            CGelloid newGelly = CGelloid.CreateCube("Flag", sizeX, sizeY, sizeZ, 100.0f/(float) sizeX, 100.0f/(float) sizeY, 5.0f, true, true, true);

            newGelly.Translate(new Vector3(-20, 50, -30));

            newGelly.UseGravity = true;
            newGelly.UseDamping = true;
            newGelly.UseWind = true;
            newGelly.UseFriction = false;
            newGelly.DrawTriangles = true;
            newGelly.DrawTextured = true;
            newGelly.DrawParticles = false;
            newGelly.DrawSphereParticles = false;
            newGelly.DrawSprings = false;
            newGelly.DrawSpringsNonStructural = false;
            newGelly.DrawVectors = false;
            newGelly.DrawTriangleNormal = false;
            newGelly.DrawVertexNormal = false;

            // Fix particles on extremity of the pole
            for (int i = 0; i < sizeY; i++)
                if (i <= 1 || i >= sizeY - 2)
                    newGelly.FindParticle(i*sizeX).NotMovable = true;

            scene.AddGelloid(newGelly);

            // Create camera
            CCamera camera = new CCamera("Camera1");
            camera.Initialize(new Vector3(0, 100, -100), new Vector3(0, 0, 0));

            scene.AddCamera(camera);

            // Set scene variables
            scene.Gravity = new Vector3(0, -1.0f, 0);
            scene.WindDirection = new Vector3(0, 0, 1);
            scene.WindSpeed = 5.0f;

            // Add colliders
            scene.AddCollider(new CComplexPlane("Ground", new Vector3(0, 1, 0), 50)); // Bottom

            scene.Initialize();

            return scene;
        }

        #endregion

        #region Methods

        public CCamera NextCamera()
        {
            _currentCameraId++;
            if (_currentCameraId >= Cameras.Count)
                _currentCameraId = 0;
            return CurrentCamera;
        }
        public CCamera PreviousCamera()
        {
            _currentCameraId--;
            if (_currentCameraId < 0)
                _currentCameraId = Cameras.Count - 1;
            return CurrentCamera;
        }

        public void Clear()
        {
            Gelloids.Clear();
            Colliders.Clear();
            Cameras.Clear();
        }

        public void Reset()
        {
            foreach (CGelloid gelloid in Gelloids)
                gelloid.Reset();
        }

        public void AddGelloid(CGelloid gelloid)
        {
            Gelloids.Add(gelloid);
        }

        public void AddCollider(CCollider collider)
        {
            Colliders.Add(collider);
        }

        public void AddCamera(CCamera camera)
        {
            Cameras.Add(camera);
        }

        public void Simulate(float deltaTime)
        {
            //// Compute gravity center
            //foreach (CGelloid gelloid in Gelloids)
            //    gelloid.ComputeGravityCenter();
            //// Reset gravitation force
            //foreach (CGelloid gelloid in Gelloids)
            //    gelloid.GravitationalForce = Vector3.Empty; // Reset
            //// Compute gravitation force for each gelloid
            //foreach (CGelloid gelloid in Gelloids)
            //{
            //    CGelloid gelloid1 = gelloid;
            //    foreach (CGelloid otherGelloid in Gelloids.Where(x => x != gelloid1))
            //    {
            //        // Compute distance from gravity center
            //        Vector3 distanceVector = gelloid1.GravityCenter - otherGelloid.GravityCenter;
            //        float distance = distanceVector.LengthSq();
            //        // Compute gravitional force
            //        Vector3 direction = Vector3.Normalize(distanceVector);
            //        float gravitationalForceLength = GravitationalConstant*gelloid1.Mass*otherGelloid.Mass/distance;
            //        otherGelloid.GravitationalForce += direction * gravitationalForceLength;
            //        gelloid1.GravitationalForce += direction * -gravitationalForceLength;
            //    }
            //}
            // Simulate
            foreach (CGelloid gelloid in Gelloids)
                gelloid.Simulate(deltaTime, Colliders, Gravity, WindDirection, WindSpeed, MouseForce, MouseKs);
        }

        public void Initialize()
        {
            foreach (CGelloid gelloid in Gelloids)
                gelloid.Initialize();
        }

        public bool Pick(float x, float y)
        {
            Vector3 rayOrigin, rayDirection;
            Matrix proj = CurrentCamera.Projection;
            // Convert the mouse point into a 3D point
            Vector3 v = new Vector3(
                (((2.0f*x)/_device.PresentationParameters.BackBufferWidth) - 1)/proj.M11,
                -(((2.0f*y)/_device.PresentationParameters.BackBufferHeight) - 1)/proj.M22,
                1.0f);

            // Get the inverse of the composite view and world matrix
            Matrix m = CurrentCamera.View*CurrentCamera.World;
            m.Invert();
            // Transform the screen space pick ray into 3D space
            rayDirection.X = v.X*m.M11 + v.Y*m.M21 + v.Z*m.M31;
            rayDirection.Y = v.X*m.M12 + v.Y*m.M22 + v.Z*m.M32;
            rayDirection.Z = v.X*m.M13 + v.Y*m.M23 + v.Z*m.M33;
            rayOrigin.X = m.M41;
            rayOrigin.Y = m.M42;
            rayOrigin.Z = m.M43;
            rayDirection.Normalize();

            bool pickFound = false;
            float minDistance = float.MaxValue;
            foreach (CGelloid gelloid in Gelloids)
            {
                float distance = float.MaxValue;
                if (gelloid.Pick(rayOrigin, rayDirection, out distance))
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        pickFound = true;
                        // TODO: store gelloid and particle
                    }
            }
            foreach (CCollider collider in Colliders)
            {
                float distance = float.MaxValue;
                if (collider.Pick(rayOrigin, rayDirection, out distance))
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        pickFound = true;
                        // TODO: store collider
                    }
            }
            return pickFound;
        }

        public void SetMouseForce(float deltaX, float deltaY)
        {
            if (Gelloids[0].PickedParticle >= 0)
            {
                Matrix m = CurrentCamera.View;
                Vector3 localX = new Vector3(m.M11, m.M21, m.M31)*(deltaX*0.3f);
                Vector3 localY = new Vector3(m.M12, m.M22, m.M32)*(deltaY*-0.3f);
                MouseForce = Gelloids[0].ParticlesCurrent[Gelloids[0].PickedParticle].Position + localX + localY;
            }
        }

        #endregion

        #region Rendering

        public void InitializeGraphics(Device device, CFont font)
        {
            _device = device;
            _font = font;

            foreach (CGelloid gelloid in Gelloids)
                gelloid.InitializeGraphics(device, font);

            foreach (CCollider collider in Colliders)
                collider.InitializeGraphics(device, font);
        }

        public void Render()
        {
            foreach (CGelloid gelloid in Gelloids)
                gelloid.Render();

            if (DrawColliders)
            {
                foreach (CCollider collider in Colliders)
                    collider.Render();
            }
        }

        #endregion
    }
}