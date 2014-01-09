using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

using MyGelloidSharp.Colliders;
using MyGelloidSharp.Tesselation;

namespace MyGelloidSharp.PhysEnv
{

    [XmlRoot("Gelloid")]
    public class CGelloid : CRenderableObject
    {
        #region Fields

        [XmlIgnore] public const int MaxResolveColllisions = 100;
        [XmlIgnore] public const float MinDeltaTime = 0.00001f;

        [XmlIgnore] public List<CParticle>[] Particles; // Array of particles
        [XmlElement("particle")] public List<CParticle> ParticlesOriginal; // Represents particles before simulation has been started
        [XmlIgnore] public List<CParticle> ParticlesCurrent; // Represents current particles
        [XmlIgnore] public List<CParticle> ParticlesTarget; // Used to double-buffered computations

        [XmlElement("spring")] public List<CSpring> Springs; // Springs in the Gelloid

        [XmlElement("triangle")] public List<CTriangle> Triangles; // Triangles in the Gelloid

        [XmlIgnore] private List<CCollision> _collisions; // Collisions

        [XmlElement("Kd")] public float Kd = 0.04f; // Damping factor
        [XmlElement("Kr")] public float Kr = 0.3f; // 1.0 = Superball bounce  0.0 = Dead Weight
        [XmlElement("CSf")] public float CSf = 0.9f; // Static friction
        [XmlElement("CKf")] public float CKf = 0.7f; // Kinetic friction

        [XmlElement("Mass")] public float Mass; // Mass
        [XmlIgnore] public Vector3 GravityCenter; // Gravity center
        [XmlIgnore] public Vector3 GravitationalForce; // Gravitational force

        [XmlElement("useFriction")] public bool UseFriction = false;
        [XmlElement("useWind")] public bool UseWind = false;
        [XmlElement("useGravity")] public bool UseGravity = true;
        [XmlElement("useDamping")] public bool UseDamping = true;
        [XmlElement("useMouse")] public bool UseMouse = false;
        //[XmlElement("useUniversalGravitation")] public bool UseUniversalGravitation = false;

        [XmlElement("textureName")] public string TextureName = "belarge.bmp";

        // Picked
        [XmlIgnore] public int PickedParticle;
        [XmlIgnore] public int PickedSpring;

        // Used by Render methods
        [XmlIgnore] private VertexBuffer _vBuffer;
        [XmlIgnore] private IndexBuffer _iBuffer;
        [XmlIgnore] private CustomVertex.PositionColored[] _vertices;
        [XmlIgnore] private Texture _triangleTexture;
        [XmlIgnore] private VertexBuffer _vBufferTextured;
        [XmlIgnore] private CustomVertex.PositionTextured[] _verticesTextured;
        [XmlIgnore] private short[] _indices;
        [XmlIgnore] private Material _particleMaterial;
        [XmlIgnore] private Material _particle0Material;
        [XmlIgnore] private Material _pickedParticleMaterial;
        [XmlIgnore] private Material _springStructuralMaterial;
        [XmlIgnore] private Material _springShearMaterial;
        [XmlIgnore] private Material _springBendMaterial;
        [XmlIgnore] private Material _springManualMaterial;
        [XmlIgnore] private Material _springBrokenMaterial;
        [XmlIgnore] private Material _forceMaterial;
        [XmlIgnore] private Material _velocityMaterial;

        [XmlIgnore] public bool DrawParticles = false;
        [XmlIgnore] public bool DrawSphereParticles = false;

        [XmlIgnore] public bool DrawSprings = false;
        [XmlIgnore] public bool DrawSpringsNonStructural = false;
        [XmlIgnore] public bool DrawSpringsBroken = false;

        [XmlIgnore] public bool DrawVectors = false;
        [XmlIgnore] public bool DrawVelocity = false;
        [XmlIgnore] public bool DrawForce = false;

        [XmlIgnore] public bool DrawTriangles = true; // Non-textured triangles by default
        [XmlIgnore] public bool DrawTextured = false;

        [XmlIgnore] public bool DrawTriangleNormal = false;
        [XmlIgnore] public bool DrawVertexNormal = false;

        [XmlIgnore] public bool DisplayInfos = false;
        [XmlIgnore] public bool DisplayStats = true;

        [XmlIgnore] private int _loopCount; // Internal use for Statistics
        [XmlIgnore] private int _maxResolveCount;
        [XmlIgnore] private int _maxCollisionCount;

        #endregion

        #region Constructors

        public CGelloid()
        {
            Particles = new List<CParticle>[3];
            Particles[0] = new List<CParticle>();
            Particles[1] = new List<CParticle>();
            Particles[2] = new List<CParticle>();
            ParticlesOriginal = Particles[0]; // We'll set original to 0
            ParticlesCurrent = Particles[0]; //            current to 1
            ParticlesTarget = Particles[0]; //             target to 2  in Initialize()

            Springs = new List<CSpring>();
            _collisions = new List<CCollision>();
            Triangles = new List<CTriangle>();
        }
        public CGelloid(string name)
            : base(name)
        {
            Particles = new List<CParticle>[3];
            Particles[0] = new List<CParticle>();
            Particles[1] = new List<CParticle>();
            Particles[2] = new List<CParticle>();
            ParticlesOriginal = Particles[0]; // We'll set original to 0
            ParticlesCurrent = Particles[0]; //            current to 1
            ParticlesTarget = Particles[0]; //             target to 2  in Initialize()

            Springs = new List<CSpring>();
            _collisions = new List<CCollision>();
            Triangles = new List<CTriangle>();
        }

        #endregion

        #region Xml Load/Save

        // Save to Xml
        public static void XmlExport(CGelloid gelloid, string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof (CGelloid));
            TextWriter w = new StreamWriter(filePath);
            serializer.Serialize(w, gelloid);
            w.Close();
        }

        // Load from Xml
        public static CGelloid XmlImport(string filePath)
        {
            //CGelloid gelloid = new CGelloid();
            XmlSerializer serializer = new XmlSerializer(typeof (CGelloid));
            TextReader r = new StreamReader(filePath);
            CGelloid gelloid = (CGelloid) serializer.Deserialize(r);
            r.Close();

            return gelloid;
        }

        #endregion

        #region Predefined gelloid creation methods

        // Create a Spiral
        public static CGelloid CreateSpiral(string name, int nSides, int height, float radius, float gapY, bool structural, bool shear, bool bend)
        {
            CGelloid gelloid = new CGelloid(name);

            // Particles
            float yoff = (height - 1.0f)*gapY/2.0f;
            float yIncr = gapY/(float) nSides;
            for (int yi = 0; yi < height; yi++)
            {
                float baseY = yoff - (yi)*gapY;
                for (int i = 0; i < nSides; i++)
                {
                    float angle = (float) i*(float) (Math.PI*2.0)/(float) nSides;
                    float x = (float) (Math.Sin(angle)*radius);
                    float z = (float) (Math.Cos(angle)*radius);
                    float y = baseY - yIncr*(float) i;
                    gelloid.AddParticle(new Vector3(x, y, z), Vector3.Empty);
                }
            }

            // Springs
            for (int i = 0; i < gelloid.ParticlesOriginal.Count - 1; i++)
            {
                if (structural)
                    // Link neighbour
                    gelloid.AddSpring(i, i + 1, CSpring.ESpringKind.Structural);
                if (bend)
                    // Link odd and even
                    if (i < gelloid.ParticlesOriginal.Count - 2)
                        gelloid.AddSpring(i, i + 2, CSpring.ESpringKind.Bend);
            }
            if (shear)
            {
                int idx = 0;
                for (int yi = 0; yi < height; yi++)
                    for (int i = 0; i < nSides; i++)
                    {
                        if (yi < height - 1)
                            // Link vertical
                            gelloid.AddSpring(idx, idx + nSides, CSpring.ESpringKind.Shear);
                        if (yi < height - 1)
                            // Link diagonal
                            gelloid.AddSpring(idx, idx + nSides - 1, CSpring.ESpringKind.Shear);
                        idx++;
                    }
            }

            return gelloid;
        }

        // Create a Sphere
        public static CGelloid CreateSphere(string name, float radius, int stacks, int slices)
        {
            CTesselatedSphere sphere = CTesselatedSphere.CreateFromTetrahedron(radius, 2, CTesselatedSphere.ESubdivisionKind.VertexToEdge);

            CGelloid gelloid = new CGelloid(name);

            // Particles
            foreach (Vector3 p in sphere.Points)
                gelloid.AddParticle(p, Vector3.Empty);

            // Springs
            foreach (CTesselatedSphere.CTriangle triangle in sphere.Triangles)
            {
                gelloid.AddSpring(triangle.P1, triangle.P2, CSpring.ESpringKind.Structural);
                gelloid.AddSpring(triangle.P1, triangle.P3, CSpring.ESpringKind.Structural);
                gelloid.AddSpring(triangle.P2, triangle.P3, CSpring.ESpringKind.Structural);
            }

            // Triangles
            foreach (CTesselatedSphere.CTriangle t in sphere.Triangles)
            {
                CTriangle triangle = new CTriangle();
                // TODO: Tu, Tv
                triangle.Vertices[0].P = t.P1;
                triangle.Vertices[1].P = t.P2;
                triangle.Vertices[2].P = t.P3;
                gelloid.Triangles.Add(triangle);
            }

            return gelloid;
        }

        // Create a Tube
        public static CGelloid CreateTube(string name, int nSides, int height, float radiusIn, float radiusOut, float gapY, bool structural, bool shear, bool bend, bool tiled)
        {
            CGelloid gelloid = new CGelloid(name);

            // Particles
            // Outer cylinder
            float yoff = (height - 1.0f)*gapY/2.0f;
            for (int i = 0; i < nSides; i++)
            {
                float angle = (float) i*(float) (Math.PI*2.0)/(float) nSides;
                float x = (float) (Math.Sin(angle)*radiusOut);
                float z = (float) (Math.Cos(angle)*radiusOut);
                for (int yi = 0; yi < height; yi++)
                {
                    float y = yoff - (yi)*gapY;
                    gelloid.AddParticle(new Vector3(x, y, z), Vector3.Empty);
                }
            }
            // Inner Cylinder
            for (int i = 0; i < nSides; i++)
            {
                float angle = (float) i*(float) (Math.PI*2.0)/(float) nSides;
                float x = (float) (Math.Sin(angle)*radiusIn);
                float z = (float) (Math.Cos(angle)*radiusIn);
                for (int yi = 0; yi < height; yi++)
                {
                    float y = yoff - (yi)*gapY;
                    gelloid.AddParticle(new Vector3(x, y, z), Vector3.Empty);
                }
            }

            // Springs
            int outToIn = nSides*height;
            int idx = 0;
            for (int i = 0; i < nSides; i++)
                for (int y = 0; y < height; y++)
                {
                    if (y < height - 1)
                    {
                        if (structural)
                        {
                            // Vertical bonds
                            gelloid.AddSpring(outToIn + idx, outToIn + idx + 1, CSpring.ESpringKind.Structural);
                            gelloid.AddSpring(idx, idx + 1, CSpring.ESpringKind.Structural);
                        }
                        if (shear)
                        {
                            // Diagonal bonds
                            gelloid.AddSpring(outToIn + idx, outToIn + (idx + height + 1)%outToIn, CSpring.ESpringKind.Shear);
                            gelloid.AddSpring(idx, (idx + height + 1)%outToIn, CSpring.ESpringKind.Shear);
                            // In-Out diagonal bonds
                            gelloid.AddSpring(idx, outToIn + (idx + 1)%outToIn, CSpring.ESpringKind.Shear);
                        }
                    }
                    if (y < height - 2)
                    {
                        // Vertical odd and even together
                        gelloid.AddSpring(outToIn + idx, outToIn + idx + 2, CSpring.ESpringKind.Bend);
                        gelloid.AddSpring(idx, idx + 2, CSpring.ESpringKind.Bend);
                    }
                    if (y > 0)
                    {
                        if (shear)
                        {
                            // Diagonal bonds
                            gelloid.AddSpring(outToIn + idx, outToIn + (idx + height - 1)%outToIn, CSpring.ESpringKind.Shear);
                            gelloid.AddSpring(idx, (idx + height - 1)%outToIn, CSpring.ESpringKind.Shear);
                            // In-Out diagonal bonds
                            gelloid.AddSpring(idx, outToIn + (idx - 1)%outToIn, CSpring.ESpringKind.Shear);
                        }
                    }
                    if (structural)
                    {
                        // Horizontal bonds
                        gelloid.AddSpring(outToIn + idx, outToIn + (idx + height)%outToIn, CSpring.ESpringKind.Structural);
                        gelloid.AddSpring(idx, (idx + height)%outToIn, CSpring.ESpringKind.Structural);
                        // In-Out horizontal bonds
                        gelloid.AddSpring(idx, idx + outToIn, CSpring.ESpringKind.Structural);
                    }
                    if (bend)
                    {
                        // Spiral In-Out horizontal bonds
                        gelloid.AddSpring(idx, outToIn + (idx + height)%outToIn, CSpring.ESpringKind.Bend);
                        gelloid.AddSpring(idx, outToIn + (outToIn + idx - height)%outToIn, CSpring.ESpringKind.Bend); // Add OutToIn to be sure to have a positive number
                    }
                    idx++;
                }

            // Triangles
            idx = 0;
            for (int i = 0; i < nSides; i++)
            {
                for (int y = 0; y < height; y++)
                {
                    CTriangle triangle;
                    // Top Cap
                    if (y == 0)
                    {
                        // Outer
                        triangle = new CTriangle();
                        triangle.Vertices[0].P = idx;
                        triangle.Vertices[0].Tu = (gelloid.ParticlesOriginal[triangle.Vertices[0].P].Position.X + radiusOut)/(2.0f*radiusOut);
                        triangle.Vertices[0].Tv = (gelloid.ParticlesOriginal[triangle.Vertices[0].P].Position.Z + radiusOut)/(2.0f*radiusOut);
                        triangle.Vertices[1].P = (idx + height)%outToIn;
                        triangle.Vertices[1].Tu = (gelloid.ParticlesOriginal[triangle.Vertices[1].P].Position.X + radiusOut)/(2.0f*radiusOut);
                        triangle.Vertices[1].Tv = (gelloid.ParticlesOriginal[triangle.Vertices[1].P].Position.Z + radiusOut)/(2.0f*radiusOut);
                        triangle.Vertices[2].P = outToIn + idx;
                        triangle.Vertices[2].Tu = (gelloid.ParticlesOriginal[triangle.Vertices[2].P].Position.X + radiusOut)/(2.0f*radiusOut);
                        triangle.Vertices[2].Tv = (gelloid.ParticlesOriginal[triangle.Vertices[2].P].Position.Z + radiusOut)/(2.0f*radiusOut);
                        gelloid.Triangles.Add(triangle);

                        // Inner
                        triangle = new CTriangle();
                        triangle.Vertices[0].P = outToIn + idx;
                        triangle.Vertices[0].Tu = (gelloid.ParticlesOriginal[triangle.Vertices[0].P].Position.X + radiusOut)/(2.0f*radiusOut);
                        triangle.Vertices[0].Tv = (gelloid.ParticlesOriginal[triangle.Vertices[0].P].Position.Z + radiusOut)/(2.0f*radiusOut);
                        triangle.Vertices[1].P = (idx + height)%outToIn;
                        triangle.Vertices[1].Tu = (gelloid.ParticlesOriginal[triangle.Vertices[1].P].Position.X + radiusOut)/(2.0f*radiusOut);
                        triangle.Vertices[1].Tv = (gelloid.ParticlesOriginal[triangle.Vertices[1].P].Position.Z + radiusOut)/(2.0f*radiusOut);
                        triangle.Vertices[2].P = outToIn + (idx + height)%outToIn;
                        triangle.Vertices[2].Tu = (gelloid.ParticlesOriginal[triangle.Vertices[2].P].Position.X + radiusOut)/(2.0f*radiusOut);
                        triangle.Vertices[2].Tv = (gelloid.ParticlesOriginal[triangle.Vertices[2].P].Position.Z + radiusOut)/(2.0f*radiusOut);
                        gelloid.Triangles.Add(triangle);
                    }
                        // Bottom Cap 
                    else if (y == height - 1)
                    {
                        // Outer
                        triangle = new CTriangle();
                        triangle.Vertices[0].P = idx;
                        triangle.Vertices[0].Tu = (gelloid.ParticlesOriginal[triangle.Vertices[0].P].Position.X + radiusOut)/(2.0f*radiusOut);
                        triangle.Vertices[0].Tv = (gelloid.ParticlesOriginal[triangle.Vertices[0].P].Position.Z + radiusOut)/(2.0f*radiusOut);
                        triangle.Vertices[1].P = outToIn + idx;
                        triangle.Vertices[1].Tu = (gelloid.ParticlesOriginal[triangle.Vertices[1].P].Position.X + radiusOut)/(2.0f*radiusOut);
                        triangle.Vertices[1].Tv = (gelloid.ParticlesOriginal[triangle.Vertices[1].P].Position.Z + radiusOut)/(2.0f*radiusOut);
                        triangle.Vertices[2].P = (idx + height)%outToIn;
                        triangle.Vertices[2].Tu = (gelloid.ParticlesOriginal[triangle.Vertices[2].P].Position.X + radiusOut)/(2.0f*radiusOut);
                        triangle.Vertices[2].Tv = (gelloid.ParticlesOriginal[triangle.Vertices[2].P].Position.Z + radiusOut)/(2.0f*radiusOut);
                        gelloid.Triangles.Add(triangle);

                        // Inner
                        triangle = new CTriangle();
                        triangle.Vertices[0].P = outToIn + idx;
                        triangle.Vertices[0].Tu = (gelloid.ParticlesOriginal[triangle.Vertices[0].P].Position.X + radiusOut)/(2.0f*radiusOut);
                        triangle.Vertices[0].Tv = (gelloid.ParticlesOriginal[triangle.Vertices[0].P].Position.Z + radiusOut)/(2.0f*radiusOut);
                        triangle.Vertices[1].P = outToIn + (idx + height)%outToIn;
                        triangle.Vertices[1].Tu = (gelloid.ParticlesOriginal[triangle.Vertices[1].P].Position.X + radiusOut)/(2.0f*radiusOut);
                        triangle.Vertices[1].Tv = (gelloid.ParticlesOriginal[triangle.Vertices[1].P].Position.Z + radiusOut)/(2.0f*radiusOut);
                        triangle.Vertices[2].P = (idx + height)%outToIn;
                        triangle.Vertices[2].Tu = (gelloid.ParticlesOriginal[triangle.Vertices[2].P].Position.X + radiusOut)/(2.0f*radiusOut);
                        triangle.Vertices[2].Tv = (gelloid.ParticlesOriginal[triangle.Vertices[2].P].Position.Z + radiusOut)/(2.0f*radiusOut);
                        gelloid.Triangles.Add(triangle);
                    }

                    if (y < height - 1)
                    {
                        // Outer
                        triangle = new CTriangle();
                        triangle.Vertices[0].P = idx;
                        triangle.Vertices[0].Tu = 0.0f;
                        if (tiled)
                            triangle.Vertices[0].Tv = 0.0f;
                        else
                            triangle.Vertices[0].Tv = (float) y/(float) height;
                        triangle.Vertices[1].P = (idx + 1)%outToIn;
                        triangle.Vertices[1].Tu = 0.0f;
                        if (tiled)
                            triangle.Vertices[1].Tv = 1.0f;
                        else
                            triangle.Vertices[1].Tv = (float) (y + 1)/(float) height;
                        triangle.Vertices[2].P = (idx + height)%outToIn;
                        triangle.Vertices[2].Tu = 1.0f;
                        if (tiled)
                            triangle.Vertices[2].Tv = 0.0f;
                        else
                            triangle.Vertices[2].Tv = (float) y/(float) height;
                        gelloid.Triangles.Add(triangle);

                        triangle = new CTriangle();
                        triangle.Vertices[0].P = (idx + 1)%outToIn;
                        triangle.Vertices[0].Tu = 0.0f;
                        if (tiled)
                            triangle.Vertices[0].Tv = 1.0f;
                        else
                            triangle.Vertices[0].Tv = (float) (y + 1)/(float) height;
                        triangle.Vertices[1].P = (idx + height + 1)%outToIn;
                        triangle.Vertices[1].Tu = 1.0f;
                        if (tiled)
                            triangle.Vertices[1].Tv = 1.0f; //(float)(y+1) / (float) height;
                        else
                            triangle.Vertices[1].Tv = (float) (y + 1)/(float) height;
                        triangle.Vertices[2].P = (idx + height)%outToIn;
                        triangle.Vertices[2].Tu = 1.0f;
                        if (tiled)
                            triangle.Vertices[2].Tv = 0.0f;
                        else
                            triangle.Vertices[2].Tv = (float) y/(float) height;
                        gelloid.Triangles.Add(triangle);

                        // Inner
                        triangle = new CTriangle();
                        triangle.Vertices[0].P = outToIn + idx;
                        triangle.Vertices[0].Tu = 0.0f;
                        if (tiled)
                            triangle.Vertices[0].Tv = 0.0f;
                        else
                            triangle.Vertices[0].Tv = (float) y/(float) height;
                        triangle.Vertices[1].P = outToIn + (idx + height)%outToIn;
                        triangle.Vertices[1].Tu = 1.0f;
                        if (tiled)
                            triangle.Vertices[1].Tv = 0.0f;
                        else
                            triangle.Vertices[1].Tv = (float) y/(float) height;
                        triangle.Vertices[2].P = outToIn + (idx + 1)%outToIn;
                        triangle.Vertices[2].Tu = 0.0f;
                        if (tiled)
                            triangle.Vertices[2].Tv = 1.0f;
                        else
                            triangle.Vertices[2].Tv = (float) (y + 1)/(float) height;
                        gelloid.Triangles.Add(triangle);

                        triangle = new CTriangle();
                        triangle.Vertices[0].P = outToIn + (idx + height)%outToIn;
                        triangle.Vertices[0].Tu = 1.0f;
                        if (tiled)
                            triangle.Vertices[0].Tv = 0.0f;
                        else
                            triangle.Vertices[0].Tv = (float) y/(float) height;
                        triangle.Vertices[1].P = outToIn + (idx + height + 1)%outToIn;
                        triangle.Vertices[1].Tu = 1.0f;
                        if (tiled)
                            triangle.Vertices[1].Tv = 1.0f;
                        else
                            triangle.Vertices[1].Tv = (float) (y + 1)/(float) height;
                        triangle.Vertices[2].P = outToIn + (idx + 1)%outToIn;
                        triangle.Vertices[2].Tu = 0.0f;
                        if (tiled)
                            triangle.Vertices[2].Tv = 1.0f;
                        else
                            triangle.Vertices[2].Tv = (float) (y + 1)/(float) height;
                        gelloid.Triangles.Add(triangle);
                    }
                    idx++;
                }
            }
            return gelloid;
        }

        public static CGelloid CreateCylinder(string name, int nSides, int height, float radius, float gapY, bool structural, bool shear, bool bend, bool tiled, bool axis)
        {
            CGelloid gelloid = new CGelloid(name);

            // Particles
            //  Cylinder
            float yoff = (height - 1.0f)*gapY/2.0f;
            for (int i = 0; i < nSides; i++)
            {
                float angle = (float) i*(float) (Math.PI*2.0)/(float) nSides;
                float x = (float) (Math.Sin(angle)*radius);
                float z = (float) (Math.Cos(angle)*radius);
                for (int yi = 0; yi < height; yi++)
                {
                    float y = yoff - (float) yi*gapY;
                    gelloid.AddParticle(new Vector3(x, y, z), Vector3.Empty);
                }
            }
            int bottomCenter = 0, topCenter = 0;
            if (axis)
            {
                //  Central axis
                for (int y = 0; y < height; y++)
                {
                    int pI = gelloid.AddParticle(new Vector3(0, yoff - (float) y*gapY, 0), Vector3.Empty);
                    //  Top and Bottom cap center
                    if (y == 0)
                        topCenter = pI;
                    else if (y == height - 1)
                        bottomCenter = pI;
                }
            }

            // Springs
            int outToIn = nSides*height;
            int idx = 0;
            for (int i = 0; i < nSides; i++)
                for (int y = 0; y < height; y++)
                {
                    if (y < height - 1)
                    {
                        if (structural)
                        {
                            // Vertical bonds
                            gelloid.AddSpring(idx, idx + 1, CSpring.ESpringKind.Structural);
                        }
                        if (shear)
                        {
                            // Diagonal bonds
                            gelloid.AddSpring(idx, (idx + height + 1)%outToIn, CSpring.ESpringKind.Shear);
                        }
                    }
                    if (y < height - 2)
                    {
                        if (bend)
                        {
                            // Vertical odd and even together
                            gelloid.AddSpring(idx, idx + 2, CSpring.ESpringKind.Bend);
                        }
                    }
                    if (y > 0)
                    {
                        if (shear)
                        {
                            // Diagonal bonds
                            gelloid.AddSpring(idx, (idx + height - 1)%outToIn, CSpring.ESpringKind.Shear);
                        }
                        if (axis)
                        {
                            if (bend)
                            {
                                // Vertical bonds on central axis
                                gelloid.AddSpring(topCenter + y - 1, topCenter + y, CSpring.ESpringKind.Bend);
                            }
                        }
                    }
                    if (axis)
                    {
                        if (bend)
                        {
                            // Radial bonds
                            gelloid.AddSpring(topCenter + y, idx, CSpring.ESpringKind.Bend);
                        }
                    }
                    if (structural)
                    {
                        // Horizontal bonds
                        gelloid.AddSpring(idx, (idx + height)%outToIn, CSpring.ESpringKind.Structural);
                    }
                    idx++;
                }

            // Triangles
            idx = 0;
            for (int i = 0; i < nSides; i++)
            {
                for (int y = 0; y < height; y++)
                {
                    CTriangle triangle;
                    if (axis)
                    {
                        if (y == 0)
                        {
                            // Top radial triangles
                            triangle = new CTriangle();
                            triangle.Vertices[0].P = topCenter; // top cap center
                            triangle.Vertices[0].Tu = 0.5f;
                            triangle.Vertices[0].Tv = 0.5f;
                            triangle.Vertices[1].P = i*height; // point on cap
                            triangle.Vertices[1].Tu = (gelloid.ParticlesOriginal[triangle.Vertices[1].P].Position.X + radius)/(2.0f*radius);
                            triangle.Vertices[1].Tv = (gelloid.ParticlesOriginal[triangle.Vertices[1].P].Position.Z + radius)/(2.0f*radius);
                            triangle.Vertices[2].P = ((i + 1)%nSides)*height; // next point on cap
                            triangle.Vertices[2].Tu = (gelloid.ParticlesOriginal[triangle.Vertices[2].P].Position.X + radius)/(2.0f*radius);
                            triangle.Vertices[2].Tv = (gelloid.ParticlesOriginal[triangle.Vertices[2].P].Position.Z + radius)/(2.0f*radius);
                            gelloid.Triangles.Add(triangle);
                        }
                        else if (y == height - 1)
                        {
                            // Bottom radial triangles
                            triangle = new CTriangle();
                            triangle.Vertices[0].P = bottomCenter; // bottom cap center
                            triangle.Vertices[0].Tu = 0.5f;
                            triangle.Vertices[0].Tv = 0.5f;
                            triangle.Vertices[1].P = height - 1 + ((i + 1)%nSides)*height; // next point on cap
                            triangle.Vertices[1].Tu = (gelloid.ParticlesOriginal[triangle.Vertices[1].P].Position.X + radius)/(2.0f*radius);
                            triangle.Vertices[1].Tv = (gelloid.ParticlesOriginal[triangle.Vertices[1].P].Position.Z + radius)/(2.0f*radius);
                            triangle.Vertices[2].P = height - 1 + i*height; // point on cap
                            triangle.Vertices[2].Tu = (gelloid.ParticlesOriginal[triangle.Vertices[2].P].Position.X + radius)/(2.0f*radius);
                            triangle.Vertices[2].Tv = (gelloid.ParticlesOriginal[triangle.Vertices[2].P].Position.Z + radius)/(2.0f*radius);
                            gelloid.Triangles.Add(triangle);
                        }
                    }
                    if (y < height - 1)
                    {
                        // Outer
                        triangle = new CTriangle();
                        triangle.Vertices[0].P = idx;
                        triangle.Vertices[0].Tu = 0.0f;
                        if (tiled)
                            triangle.Vertices[0].Tv = 0.0f;
                        else
                            triangle.Vertices[0].Tv = (float) y/(float) height;
                        triangle.Vertices[1].P = (idx + 1)%outToIn;
                        triangle.Vertices[1].Tu = 0.0f;
                        if (tiled)
                            triangle.Vertices[1].Tv = 1.0f;
                        else
                            triangle.Vertices[1].Tv = (float) (y + 1)/(float) height;
                        triangle.Vertices[2].P = (idx + height)%outToIn;
                        triangle.Vertices[2].Tu = 1.0f;
                        if (tiled)
                            triangle.Vertices[2].Tv = 0.0f;
                        else
                            triangle.Vertices[2].Tv = (float) y/(float) height;
                        gelloid.Triangles.Add(triangle);

                        triangle = new CTriangle();
                        triangle.Vertices[0].P = (idx + 1)%outToIn;
                        triangle.Vertices[0].Tu = 0.0f;
                        if (tiled)
                            triangle.Vertices[0].Tv = 1.0f;
                        else
                            triangle.Vertices[0].Tv = (float) (y + 1)/(float) height;
                        triangle.Vertices[1].P = (idx + height + 1)%outToIn;
                        triangle.Vertices[1].Tu = 1.0f;
                        if (tiled)
                            triangle.Vertices[1].Tv = 1.0f;
                        else
                            triangle.Vertices[1].Tv = (float) (y + 1)/(float) height;
                        triangle.Vertices[2].P = (idx + height)%outToIn;
                        triangle.Vertices[2].Tu = 1.0f;
                        if (tiled)
                            triangle.Vertices[2].Tv = 0.0f;
                        else
                            triangle.Vertices[2].Tv = (float) y/(float) height;
                        gelloid.Triangles.Add(triangle);

                    }
                    idx++;
                }
            }

            return gelloid;
        }

        // Create a cube linked to 4 anchors
        public static CGelloid CreateAnchoredCube()
        {
            CGelloid gelloid = CreateCube("Anchored Cube", 5, 2, 5, 10, 10, 10, true, true, true);
            CParticle p0 = gelloid.FindParticle(0);
            int anchor1i = gelloid.AddParticle(p0.Position - new Vector3(20, 0, 0), new Vector3(0, 0, 0));
            int anchor2i = gelloid.AddParticle(p0.Position + new Vector3(20, 0, 0), new Vector3(0, 0, 0));
            int anchor3i = gelloid.AddParticle(p0.Position + new Vector3(0, 0, 20), new Vector3(0, 0, 0));
            int anchor4i = gelloid.AddParticle(p0.Position - new Vector3(0, 0, 20), new Vector3(0, 0, 0));
            CParticle anchor1 = gelloid.FindParticle(anchor1i);
            CParticle anchor2 = gelloid.FindParticle(anchor2i);
            CParticle anchor3 = gelloid.FindParticle(anchor3i);
            CParticle anchor4 = gelloid.FindParticle(anchor4i);
            anchor1.NotMovable = true;
            anchor2.NotMovable = true;
            anchor3.NotMovable = true;
            anchor4.NotMovable = true;
            int spring1i = gelloid.AddSpring(anchor1i, 0, CSpring.ESpringKind.Manual);
            int spring2i = gelloid.AddSpring(anchor2i, 0, CSpring.ESpringKind.Manual);
            int spring3i = gelloid.AddSpring(anchor3i, 0, CSpring.ESpringKind.Manual);
            int spring4i = gelloid.AddSpring(anchor4i, 0, CSpring.ESpringKind.Manual);
            CSpring spring1 = gelloid.FindSpring(spring1i);
            CSpring spring2 = gelloid.FindSpring(spring2i);
            CSpring spring3 = gelloid.FindSpring(spring3i);
            CSpring spring4 = gelloid.FindSpring(spring4i);
            spring1.MaxExtension = 1000.0f;
            spring2.MaxExtension = 1000.0f;
            spring3.MaxExtension = 1000.0f;
            spring4.MaxExtension = 1000.0f;
            gelloid.Translate(new Vector3(0, 100, 0));
            return gelloid;
        }

        // Create a cube
        public static CGelloid CreateCube(string name, int sizeX, int sizeY, int sizeZ, float gapX, float gapY, float gapZ, bool structural, bool shear, bool bend)
        {
            CGelloid gelloid = new CGelloid(name);

            float xoff = (sizeX - 1.0f)*gapX/2.0f;
            float yoff = (sizeY - 1.0f)*gapY/2.0f;
            float zoff = (sizeZ - 1.0f)*gapZ/2.0f;

            // Create particles
            int p = 0;
            for (int z = 0; z < sizeZ; z++)
                for (int y = 0; y < sizeY; y++)
                    for (int x = 0; x < sizeX; x++)
                    {
                        gelloid.AddParticle(new Vector3(xoff - x*gapX, yoff - y*gapY, zoff - z*gapZ), Vector3.Empty);
                        p++;
                    }

            // Create springs
            int idx = 0;
            for (int z = 0; z < sizeZ; z++)
                for (int y = 0; y < sizeY; y++)
                    for (int x = 0; x < sizeX; x++)
                    {
                        if (structural)
                        {
                            // Structural    .-.
                            //               | |
                            //               .-.
                            if (x > 0)
                                gelloid.AddSpring(idx - 1, idx, CSpring.ESpringKind.Structural);
                            if (y > 0)
                                gelloid.AddSpring(idx - sizeX, idx, CSpring.ESpringKind.Structural);
                            if (z > 0)
                                gelloid.AddSpring(idx - sizeX*sizeY, idx, CSpring.ESpringKind.Structural);
                        }

                        if (shear)
                        {
                            // Shear .  .  link diagonally
                            //        \/
                            //        .
                            //        /\
                            //       .  .
                            if (x > 0 && y > 0)
                                gelloid.AddSpring(idx, idx - sizeX - 1, CSpring.ESpringKind.Shear);
                            if (x > 0 && y < sizeY - 1)
                                gelloid.AddSpring(idx, idx + sizeX - 1, CSpring.ESpringKind.Shear);
                            if (x > 0 && z > 0)
                                gelloid.AddSpring(idx, idx - sizeX*sizeY - 1, CSpring.ESpringKind.Shear);
                            if (x > 0 && z < sizeZ - 1)
                                gelloid.AddSpring(idx, idx + sizeX*sizeY - 1, CSpring.ESpringKind.Shear);
                            if (y > 0 && z > 0)
                                gelloid.AddSpring(idx, idx - sizeX*sizeY - sizeX, CSpring.ESpringKind.Shear);
                            if (y > 0 && z < sizeZ - 1)
                                gelloid.AddSpring(idx, idx + sizeX*sizeY - sizeX, CSpring.ESpringKind.Shear);
                        }

                        if (bend)
                        {
                            // Bend   ,--,  link odd together and even together
                            //       . . .
                            //      /
                            //      |.
                            //      \
                            //       .
                            if (x > 1)
                                gelloid.AddSpring(idx, idx - 2, CSpring.ESpringKind.Bend);
                            if (y > 1)
                                gelloid.AddSpring(idx, idx - sizeX*2, CSpring.ESpringKind.Bend);
                            if (z > 1)
                                gelloid.AddSpring(idx, idx - sizeX*sizeY*2, CSpring.ESpringKind.Bend);
                        }
                        idx++;
                    }

            // Create triangles
            idx = 0;
            for (int z = 0; z < sizeZ; z++)
                for (int y = 0; y < sizeY; y++)
                    for (int x = 0; x < sizeX; x++)
                    {
                        if (x == 0 && y < sizeY - 1 && z < sizeZ - 1)
                        {
                            // yz plane left
                            CTriangle t = new CTriangle();
                            t.Vertices[0].P = idx;
                            t.Vertices[0].Tu = CUtils.Range((float) z/(float) (sizeZ - 1), 0, 1);
                            t.Vertices[0].Tv = CUtils.Range((float) y/(float) (sizeY - 1), 0, 1);
                            t.Vertices[1].P = idx + sizeX;
                            t.Vertices[1].Tu = CUtils.Range((float) z/(float) (sizeZ - 1), 0, 1);
                            t.Vertices[1].Tv = CUtils.Range((float) (y + 1)/(float) (sizeY - 1), 0, 1);
                            t.Vertices[2].P = idx + sizeX + sizeX*sizeY;
                            t.Vertices[2].Tu = CUtils.Range((float) (z + 1)/(float) (sizeZ - 1), 0, 1);
                            t.Vertices[2].Tv = CUtils.Range((float) (y + 1)/(float) (sizeY - 1), 0, 1);
                            gelloid.Triangles.Add(t);
                            t = new CTriangle();
                            t.Vertices[0].P = idx;
                            t.Vertices[0].Tu = CUtils.Range((float) z/(float) (sizeZ - 1), 0, 1);
                            t.Vertices[0].Tv = CUtils.Range((float) y/(float) (sizeY - 1), 0, 1);
                            t.Vertices[1].P = idx + sizeX + sizeX*sizeY;
                            t.Vertices[1].Tu = CUtils.Range((float) (z + 1)/(float) (sizeZ - 1), 0, 1);
                            t.Vertices[1].Tv = CUtils.Range((float) (y + 1)/(float) (sizeY - 1), 0, 1);
                            t.Vertices[2].P = idx + sizeX*sizeY;
                            t.Vertices[2].Tu = CUtils.Range((float) (z + 1)/(float) (sizeZ - 1), 0, 1);
                            t.Vertices[2].Tv = CUtils.Range((float) y/(float) (sizeY - 1), 0, 1);
                            gelloid.Triangles.Add(t);
                        }
                        if (x == sizeX - 1 && x != 0 && y < sizeY - 1 && z < sizeZ - 1)
                        {
                            // yz plane right
                            CTriangle t = new CTriangle();
                            t.Vertices[0].P = idx;
                            t.Vertices[0].Tu = CUtils.Range((float) z/(float) (sizeZ - 1), 0, 1);
                            t.Vertices[0].Tv = CUtils.Range((float) y/(float) (sizeY - 1), 0, 1);
                            t.Vertices[1].P = idx + sizeX + sizeX*sizeY;
                            t.Vertices[1].Tu = CUtils.Range((float) (z + 1)/(float) (sizeZ - 1), 0, 1);
                            t.Vertices[1].Tv = CUtils.Range((float) (y + 1)/(float) (sizeY - 1), 0, 1);
                            t.Vertices[2].P = idx + sizeX;
                            t.Vertices[2].Tu = CUtils.Range((float) z/(float) (sizeZ - 1), 0, 1);
                            t.Vertices[2].Tv = CUtils.Range((float) (y + 1)/(float) (sizeY - 1), 0, 1);
                            gelloid.Triangles.Add(t);
                            t = new CTriangle();
                            t.Vertices[0].P = idx;
                            t.Vertices[0].Tu = CUtils.Range((float) z/(float) (sizeZ - 1), 0, 1);
                            t.Vertices[0].Tv = CUtils.Range((float) y/(float) (sizeY - 1), 0, 1);
                            t.Vertices[1].P = idx + sizeX*sizeY;
                            t.Vertices[1].Tu = CUtils.Range((float) (z + 1)/(float) (sizeZ - 1), 0, 1);
                            t.Vertices[1].Tv = CUtils.Range((float) y/(float) (sizeY - 1), 0, 1);
                            t.Vertices[2].P = idx + sizeX + sizeX*sizeY;
                            t.Vertices[2].Tu = CUtils.Range((float) (z + 1)/(float) (sizeZ - 1), 0, 1);
                            t.Vertices[2].Tv = CUtils.Range((float) (y + 1)/(float) (sizeY - 1), 0, 1);
                            gelloid.Triangles.Add(t);
                        }
                        if (y == 0 && x < sizeX - 1 && z < sizeZ - 1)
                        {
                            // xz plane top
                            CTriangle t = new CTriangle();
                            t.Vertices[0].P = idx;
                            t.Vertices[0].Tu = CUtils.Range((float) x/(float) (sizeX - 1), 0, 1);
                            t.Vertices[0].Tv = CUtils.Range((float) z/(float) (sizeZ - 1), 0, 1);
                            t.Vertices[1].P = idx + sizeX*sizeY + 1;
                            t.Vertices[1].Tu = CUtils.Range((float) (x + 1)/(float) (sizeX - 1), 0, 1);
                            t.Vertices[1].Tv = CUtils.Range((float) (z + 1)/(float) (sizeZ - 1), 0, 1);
                            t.Vertices[2].P = idx + 1;
                            t.Vertices[2].Tu = CUtils.Range((float) (x + 1)/(float) (sizeX - 1), 0, 1);
                            t.Vertices[2].Tv = CUtils.Range((float) z/(float) (sizeZ - 1), 0, 1);
                            gelloid.Triangles.Add(t);
                            t = new CTriangle();
                            t.Vertices[0].P = idx;
                            t.Vertices[0].Tu = CUtils.Range((float) x/(float) (sizeX - 1), 0, 1);
                            t.Vertices[0].Tv = CUtils.Range((float) z/(float) (sizeZ - 1), 0, 1);
                            t.Vertices[1].P = idx + sizeX*sizeY;
                            t.Vertices[1].Tu = CUtils.Range((float) x/(float) (sizeX - 1), 0, 1);
                            t.Vertices[1].Tv = CUtils.Range((float) (z + 1)/(float) (sizeZ - 1), 0, 1);
                            t.Vertices[2].P = idx + sizeX*sizeY + 1;
                            t.Vertices[2].Tu = CUtils.Range((float) (x + 1)/(float) (sizeX - 1), 0, 1);
                            t.Vertices[2].Tv = CUtils.Range((float) (z + 1)/(float) (sizeZ - 1), 0, 1);
                            gelloid.Triangles.Add(t);
                        }
                        if (y == sizeY - 1 && y != 0 && x < sizeX - 1 && z < sizeZ - 1)
                        {
                            // xz plane bottom
                            CTriangle t = new CTriangle();
                            t.Vertices[0].P = idx;
                            t.Vertices[0].Tu = CUtils.Range((float) x/(float) (sizeX - 1), 0, 1);
                            t.Vertices[0].Tv = CUtils.Range((float) z/(float) (sizeZ - 1), 0, 1);
                            t.Vertices[1].P = idx + 1;
                            t.Vertices[1].Tu = CUtils.Range((float) (x + 1)/(float) (sizeX - 1), 0, 1);
                            t.Vertices[1].Tv = CUtils.Range((float) z/(float) (sizeZ - 1), 0, 1);
                            t.Vertices[2].P = idx + sizeX*sizeY + 1;
                            t.Vertices[2].Tu = CUtils.Range((float) (x + 1)/(float) (sizeX - 1), 0, 1);
                            t.Vertices[2].Tv = CUtils.Range((float) (z + 1)/(float) (sizeZ - 1), 0, 1);
                            gelloid.Triangles.Add(t);
                            t = new CTriangle();
                            t.Vertices[0].P = idx;
                            t.Vertices[0].Tu = CUtils.Range((float) x/(float) (sizeX - 1), 0, 1);
                            t.Vertices[0].Tv = CUtils.Range((float) z/(float) (sizeZ - 1), 0, 1);
                            t.Vertices[1].P = idx + sizeX*sizeY + 1;
                            t.Vertices[1].Tu = CUtils.Range((float) (x + 1)/(float) (sizeX - 1), 0, 1);
                            t.Vertices[1].Tv = CUtils.Range((float) (z + 1)/(float) (sizeZ - 1), 0, 1);
                            t.Vertices[2].P = idx + sizeX*sizeY;
                            t.Vertices[2].Tu = CUtils.Range((float) x/(float) (sizeX - 1), 0, 1);
                            t.Vertices[2].Tv = CUtils.Range((float) (z + 1)/(float) (sizeZ - 1), 0, 1);
                            gelloid.Triangles.Add(t);
                        }
                        if (z == 0 && x < sizeX - 1 && y < sizeY - 1)
                        {
                            // xy front
                            CTriangle t = new CTriangle();
                            t.Vertices[0].P = idx;
                            t.Vertices[0].Tu = CUtils.Range((float) x/(float) (sizeX - 1), 0, 1);
                            t.Vertices[0].Tv = CUtils.Range((float) y/(float) (sizeY - 1), 0, 1);
                            t.Vertices[1].P = idx + 1;
                            t.Vertices[1].Tu = CUtils.Range((float) (x + 1)/(float) (sizeX - 1), 0, 1);
                            t.Vertices[1].Tv = CUtils.Range((float) y/(float) (sizeY - 1), 0, 1);
                            t.Vertices[2].P = idx + sizeX + 1;
                            t.Vertices[2].Tu = CUtils.Range((float) (x + 1)/(float) (sizeX - 1), 0, 1);
                            t.Vertices[2].Tv = CUtils.Range((float) (y + 1)/(float) (sizeY - 1), 0, 1);
                            gelloid.Triangles.Add(t);
                            t = new CTriangle();
                            t.Vertices[0].P = idx;
                            t.Vertices[0].Tu = CUtils.Range((float) x/(float) (sizeX - 1), 0, 1);
                            t.Vertices[0].Tv = CUtils.Range((float) y/(float) (sizeY - 1), 0, 1);
                            t.Vertices[1].P = idx + sizeX + 1;
                            t.Vertices[1].Tu = CUtils.Range((float) (x + 1)/(float) (sizeX - 1), 0, 1);
                            t.Vertices[1].Tv = CUtils.Range((float) (y + 1)/(float) (sizeY - 1), 0, 1);
                            t.Vertices[2].P = idx + sizeX;
                            t.Vertices[2].Tu = CUtils.Range((float) x/(float) (sizeX - 1), 0, 1);
                            t.Vertices[2].Tv = CUtils.Range((float) (y + 1)/(float) (sizeY - 1), 0, 1);
                            gelloid.Triangles.Add(t);
                        }
                        if (z == sizeZ - 1 && z != 0 && x < sizeX - 1 && y < sizeY - 1)
                        {
                            // xy back
                            CTriangle t = new CTriangle();
                            t.Vertices[0].P = idx;
                            t.Vertices[0].Tu = CUtils.Range((float) x/(float) (sizeX - 1), 0, 1);
                            t.Vertices[0].Tv = CUtils.Range((float) y/(float) (sizeY - 1), 0, 1);
                            t.Vertices[1].P = idx + sizeX + 1;
                            t.Vertices[1].Tu = CUtils.Range((float) (x + 1)/(float) (sizeX - 1), 0, 1);
                            t.Vertices[1].Tv = CUtils.Range((float) (y + 1)/(float) (sizeY - 1), 0, 1);
                            t.Vertices[2].P = idx + 1;
                            t.Vertices[2].Tu = CUtils.Range((float) (x + 1)/(float) (sizeX - 1), 0, 1);
                            t.Vertices[2].Tv = CUtils.Range((float) y/(float) (sizeY - 1), 0, 1);
                            gelloid.Triangles.Add(t);
                            t = new CTriangle();
                            t.Vertices[0].P = idx;
                            t.Vertices[0].Tu = CUtils.Range((float) x/(float) (sizeX - 1), 0, 1);
                            t.Vertices[0].Tv = CUtils.Range((float) y/(float) (sizeY - 1), 0, 1);
                            t.Vertices[1].P = idx + sizeX;
                            t.Vertices[1].Tu = CUtils.Range((float) x/(float) (sizeX - 1), 0, 1);
                            t.Vertices[1].Tv = CUtils.Range((float) (y + 1)/(float) (sizeY - 1), 0, 1);
                            t.Vertices[2].P = idx + sizeX + 1;
                            t.Vertices[2].Tu = CUtils.Range((float) (x + 1)/(float) (sizeX - 1), 0, 1);
                            t.Vertices[2].Tv = CUtils.Range((float) (y + 1)/(float) (sizeY - 1), 0, 1);
                            gelloid.Triangles.Add(t);
                        }
                        idx++;
                    }
            return gelloid;
        }

        #endregion

        #region Methods not related to simulation

        // Check is a spring/particle is picked
        public override bool Pick(Vector3 rayOrigin, Vector3 rayDirection, out float distance)
        {
            distance = float.MaxValue;
            // Build bounding sphere and bounding box
            //Vector3 center = Vector3.Empty;
            //float radius2 = float.MinValue;
            float minX = float.MaxValue;
            float minY = float.MaxValue;
            float minZ = float.MaxValue;
            float maxX = float.MinValue;
            float maxY = float.MinValue;
            float maxZ = float.MinValue;
            foreach (CParticle particle in ParticlesCurrent)
            {
                if (particle.Position.X < minX) minX = particle.Position.X;
                else if (particle.Position.X > maxX) maxX = particle.Position.X;
                if (particle.Position.Y < minY) minY = particle.Position.Y;
                else if (particle.Position.Y > maxY) maxY = particle.Position.Y;
                if (particle.Position.Z < minZ) minZ = particle.Position.Z;
                else if (particle.Position.Z > maxZ) maxZ = particle.Position.Z;
            }
            Vector3 center = new Vector3(minX + maxX, minY + maxY, minZ + maxZ)*0.5f;
            float radius2 = (new Vector3(maxX - minX, maxY - minY, maxZ - minZ)*0.5f).LengthSq();
            // Test bounding sphere
            if (!CIntersection.IntersectsSphere(rayOrigin, rayDirection, center, radius2))
                return false;
            // Test AABB, enlarged AABB (+-5,+-5,+-5)
            if (!CIntersection.IntersectsAABB(rayOrigin, rayDirection, minX - 5.0f, minY - 5.0f, minZ - 5.0f, maxX + 5.0f, maxY + 5.0f, maxZ + 5.0f))
                return false;
            // Create and test particles bounding sphere
            bool pickFound = false;
            PickedParticle = -1;
            for (int i = 0; i < ParticlesCurrent.Count; i++)
            {
                CParticle particle = ParticlesCurrent[i];
                float dist = float.MaxValue;
                if (CIntersection.IntersectionSphere(rayOrigin, rayDirection, particle.Position, 3*3, out dist))
                    if (dist < distance)
                    {
                        distance = dist;
                        pickFound = true;
                        PickedParticle = i;
                    }
            }
            // Not yet implemented		
            //			// Create springs bounding cylinder/OBB
            //			PickedSpring = -1;
            //			for ( int i = 0; i < springs.Count; i++ ) {
            //				CSpring spring = springs[i];
            //				CParticle p1 = particlesCurrent[spring.p1i];
            //				CParticle p2 = particlesCurrent[spring.p2i];
            //
            //				Vector3 axis = p2.position - p1.position;
            //				Vector3 centerCyl = ( p2.position + p1.position ) * 0.5f;
            //				float height = axis.Length();
            //				axis = axis * ( 1.0f / height );
            //				height = height * 0.5f;
            //				float dist = float.MaxValue;
            //				if ( CIntersection.intersectionCappedCylinder( rayOrigin, rayDirection, centerCyl, axis, height, 2*2, out dist ) )
            //					if ( dist < distance ) {
            //						distance = dist;
            //						pickFound = true;
            //						PickedSpring = i;
            //					}
            //			}
            return pickFound;
        }

        // Get the center of Gelloid bounding box
        public Vector3 BBCenter()
        {
            float minX = 10000, minY = 10000, minZ = 10000;
            float maxX = -10000, maxY = -10000, maxZ = -10000;
            foreach (CParticle particle in ParticlesCurrent)
            {
                Vector3 pos = particle.Position;
                if (pos.X < minX) minX = pos.X;
                if (pos.X > maxX) maxX = pos.X;
                if (pos.Y < minY) minY = pos.Y;
                if (pos.Y > maxY) maxY = pos.Y;
                if (pos.Z < minZ) minZ = pos.Z;
                if (pos.Z > maxZ) maxZ = pos.Z;
            }
            return new Vector3(minX + maxX, minY + maxY, minZ + maxZ)*0.5f;
        }

        // Return a particle
        public CParticle FindParticle(int index)
        {
            return ParticlesCurrent[index];
        }

        // Return a spring
        public CSpring FindSpring(int index)
        {
            return Springs[index];
        }

        // Add a spring, specifying 2 indices
        public int AddSpring(int p1i, int p2i, CSpring.ESpringKind kind)
        {
            CSpring spring = new CSpring
                {
                    P1Index = p1i,
                    P2Index = p2i
                };
            CParticle p1 = FindParticle(p1i);
            CParticle p2 = FindParticle(p2i);
            spring.RestLength = (p1.Position - p2.Position).Length();
            spring.Kind = kind;
            Springs.Add(spring);
            return Springs.Count-1;
        }

        // Add a spring, specifying 2 indices and spring additional informations
        public int AddSpring(int p1i, int p2i, CSpring.ESpringKind kind, float Ks, float Kd, float maxExtension)
        {
            CSpring spring = new CSpring
                {
                    P1Index = p1i,
                    P2Index = p2i
                };
            CParticle p1 = FindParticle(p1i);
            CParticle p2 = FindParticle(p2i);
            spring.RestLength = (p1.Position - p2.Position).Length();
            spring.Kind = kind;
            spring.Ks = Ks;
            spring.Kd = Kd;
            spring.MaxExtension = maxExtension;
            Springs.Add(spring);
            return Springs.Count-1;
        }

        // Add a particle
        public int AddParticle(Vector3 position, Vector3 velocity)
        {
            CParticle particle = new CParticle
                {
                    Position = position,
                    Velocity = velocity
                };
            ParticlesCurrent.Add(particle);
            return ParticlesCurrent.Count-1;
        }

        // Add a particle
        public int AddParticle(Vector3 position, Vector3 velocity, float oneOverMass)
        {
            CParticle particle = new CParticle
                {
                    Position = position,
                    Velocity = velocity,
                    OneOverMass = oneOverMass
                };
            ParticlesCurrent.Add(particle);
            return ParticlesCurrent.Count-1;
        }

        // Rotate entire gelly
        public void Rotate(Vector3 axis, float angle)
        {
            foreach (CParticle particle in ParticlesCurrent)
                particle.Position.TransformNormal(Matrix.RotationAxis(axis, angle));
        }

        // Translate entire gelly
        public void Translate(Vector3 trans)
        {
            foreach (CParticle particle in ParticlesCurrent)
                particle.Position = particle.Position + trans;
        }

        // Give the gelly some spin
        public void AddAngularMom(Vector3 axis, float spin)
        {
            foreach (CParticle particle in ParticlesCurrent)
            {
                Vector3 p2 = new Vector3(particle.Position.X, particle.Position.Y, particle.Position.Z);
                p2.TransformNormal(Matrix.RotationAxis(axis, spin));
                particle.Velocity = particle.Velocity + (p2 - particle.Position);
            }
        }

        // Give the gelly some velocity
        public void AddVelocity(Vector3 vel)
        {
            foreach (CParticle particle in ParticlesCurrent)
                particle.Velocity = particle.Velocity + vel;
        }

        #endregion

        #region Methods related with simulation
        // Compute gravity center
        public void ComputeGravityCenter()
        {
            GravityCenter = ParticlesCurrent.Aggregate(Vector3.Empty, (current, particle) => current + particle.Position*particle.OneOverMass) * (1.0f / (float)ParticlesCurrent.Count);
        }
        
        // Compute triangles normal and vertices normal
        private void ComputeNormals(List<CParticle> system)
        {
            if (!UseWind) // No need to compute normal if wind is null
                return;
            // Compute triangle normals
            foreach (CTriangle triangle in Triangles)
                triangle.ComputeNormal(system);
            // Reset vertices normals
            foreach (CParticle particle in system)
                particle.Normal = Vector3.Empty;
            // Compute triangle vertices normals
            foreach (CTriangle triangle in Triangles)
            {
                CParticle p1 = system[triangle.Vertices[0].P];
                CParticle p2 = system[triangle.Vertices[1].P];
                CParticle p3 = system[triangle.Vertices[2].P];
                p1.Normal = p1.Normal + triangle.Normal;
                p2.Normal = p2.Normal + triangle.Normal;
                p3.Normal = p3.Normal + triangle.Normal;
            }
            // Normalize each vertices normals
            foreach (CParticle particle in system)
                particle.Normal.Normalize();
        }

        // Compute force applied on each particles
        private void ComputeForces(List<CParticle> system, Vector3 gravity, Vector3 windDirection, float windSpeed, Vector3 mouseForce, float mouseKs)
        {
            //// 1 / Mass
            //float oneOverMass = 1.0f/Mass;
            // Compute forces
            foreach (CParticle particle in system)
            {
                particle.ResetForce();
                if (particle.NotMovable)
                    continue;
                //if (UseUniversalGravitation) particle.ApplyForce(GravitationalForce * oneOverMass);
                if (UseGravity) particle.ApplyGravity(gravity);
                if (UseDamping) particle.ApplyDamping(Kd);
                if (UseWind) particle.ApplyWind(windDirection, windSpeed);
                if (UseFriction) particle.ApplyFriction(CSf, CKf);
                if (UseMouse && PickedParticle >= 0 && particle == system[PickedParticle]) particle.ApplyMouse(mouseForce, mouseKs);
            }

            // Do all springs
            foreach (CSpring spring in Springs)
                spring.Apply(system);
        }

        // Integrate computed forced in velocity and position for each particles
        private void Integrate(List<CParticle> initial, List<CParticle> source, List<CParticle> target, float deltaTime)
        {
            for (int i = 0; i < target.Count; i++)
                target[i].Integrate(initial[i], source[i], deltaTime);
        }

        // Build a list of collision. A collision for each particles colliding a collider
        // If a particle penetrates a collider, we leave the loop and integrate on a smaller step, until no particles is penetrating
        private CCollider.ECollideState CheckForCollisions(List<CParticle> system, List<CCollider> colliders)
        {
            if (_collisions.Count > _maxCollisionCount) _maxCollisionCount = _collisions.Count; // STATS

            _collisions.Clear();
            CCollider.ECollideState collisionState = CCollider.ECollideState.NotColliding;
            for (int i = 0; i < system.Count; i++)
            {
                CParticle particle = system[i];
                foreach (CCollider collider in colliders)
                {
                    Vector3 normal;
                    bool inContact;
                    CCollider.ECollideState localState = collider.Collide(particle.Position, particle.Velocity, out normal, out inContact);
                    if (localState == CCollider.ECollideState.Penetrating) // Once any particles penetrates, quit the loop
                        return localState;
                    else if (localState == CCollider.ECollideState.Colliding)
                    {
                        // If collision, add collision to collision list
                        CCollision collision = new CCollision
                            {
                                Particle = i,
                                Normal = normal,
                                Collider = collider
                            };
                        _collisions.Add(collision);
                        collisionState = localState;
                    }
                    if (inContact && UseFriction)
                    {
                        // If contact, save the contact normal for later (friction)
                        particle.InContact = true;
                        particle.NormatAtContact = normal;
                    }
                }
            }
            return collisionState;
        }

        // Resolve collisions list built in CheckForCollisions
        private void ResolveCollisions(List<CParticle> system)
        {
            foreach (CCollision collision in _collisions)
            {
                CParticle particle = system[collision.Particle];
                //TODO: il faudrait être sur qu'au prochain update l'object se trouve au-dessus du collider -> calculer la distance
                // Calculate Vn
                float VdotN = Vector3.Dot(collision.Normal, particle.Velocity);
                Vector3 Vn = collision.Normal*VdotN;
                // Calculate Vt
                Vector3 Vt = particle.Velocity - Vn;
                // Scale Vn by coefficient of restitution
                Vn = Vn*Kr;
                // Set the velocity to be the new impulse
                particle.Velocity = Vt - Vn;
            }
        }

        // One simulation step
        // WHILE ELAPSED TIME < DELTA TIME
        // 1: ComputeForces
        //    Integrate
        //    CheckForCollisions
        //    IF penetration THEN
        //       GOTO 1 with a smaller delta time
        //    ELSE
        //       ResolveCollisions
        //       Advance time
        //       Switch particles buffers
        //    ENDIF
        // ENDWHILE
        public void Simulate(float deltaTime, List<CCollider> colliders, Vector3 gravity, Vector3 windDirection, float windSpeed, Vector3 mouseForce, float mouseKs)
        {
            float currentTime = 0;
            float targetTime = deltaTime;

            _loopCount = 0;
            _maxResolveCount = 0;
            _maxCollisionCount = 0;
            while (currentTime < deltaTime)
            {
                _loopCount++;

                ComputeForces(ParticlesCurrent, gravity, windDirection, windSpeed, mouseForce, mouseKs);
                Integrate(ParticlesCurrent, ParticlesCurrent, ParticlesTarget, targetTime - currentTime);

                CCollider.ECollideState collisionState = CheckForCollisions(ParticlesTarget, colliders);
                if (collisionState == CCollider.ECollideState.Penetrating)
                {
                    // we simulated too far, so subdivide time and try again
                    targetTime = (currentTime + targetTime)/2.0f;
                    if (System.Math.Abs(targetTime - currentTime) < MinDeltaTime)
                        throw new Exception("DeltaTime too small[" + targetTime + " - " + currentTime + " = " + System.Math.Abs(targetTime - currentTime) + "]!");
                    // if |targetTime-currentTime| < EPS => EXIT SIMULATION
                }
                else
                {
                    if (collisionState == CCollider.ECollideState.Colliding)
                    {
                        bool noMoreCollisions = false;
                        ;
                        for (int i = 0; i < MaxResolveColllisions; i++)
                        {
                            if (i + 1 > _maxResolveCount) _maxResolveCount = i + 1; // STATS

                            ResolveCollisions(ParticlesTarget);
                            if (CheckForCollisions(ParticlesTarget, colliders) != CCollider.ECollideState.Colliding)
                            {
                                noMoreCollisions = true;
                                break;
                            }
                        }
                        if (!noMoreCollisions)
                            throw new Exception("Too many ResolveCollisions!");
                        // if i == 100 => EXIT SIMULATION
                    }

                    currentTime = targetTime;
                    targetTime = deltaTime;

                    // Swap the 2 particles buffers so we can do it again
                    List<CParticle> temp = ParticlesCurrent;
                    ParticlesCurrent = ParticlesTarget;
                    ParticlesTarget = temp;
                }
            }
            // Compute normals
            ComputeNormals(ParticlesCurrent);
        }

        #endregion

        #region Initialize and Reset

        // Copy particles informations from particles original to particles current and particles target
        public void Initialize()
        {
            ParticlesOriginal = Particles[0];
            ParticlesCurrent = Particles[1];
            ParticlesTarget = Particles[2];
            ParticlesCurrent.Clear();
            ParticlesTarget.Clear();
            foreach (CParticle pO in ParticlesOriginal)
            {
                CParticle p = new CParticle();
                p.Copy(pO);
                ParticlesCurrent.Add(p);
                p = new CParticle();
                p.Copy(pO);
                ParticlesTarget.Add(p);
            }
        }

        // Reset particles and springs
        public void Reset()
        {
            Initialize();
            foreach (CSpring spring in Springs)
                spring.Broken = false;
        }

        #endregion

        #region Rendering

        // Initialize graphics, create vertex buffer, index buffer and materials
        public override void InitializeGraphics(Device device, CFont font)
        {
            base.InitializeGraphics(device, font);

            //			_vBuffer = new VertexBuffer( typeof(CustomVertex.PositionColored), particlesOriginal.Count *3, device, Usage.Dynamic | Usage.WriteOnly, CustomVertex.PositionColored.Format, Pool.Default );
            //			vertices = new CustomVertex.PositionColored[ particlesOriginal.Count *3];
            //			_iBuffer = new IndexBuffer( typeof(short), springs.Count *2 + particlesOriginal.Count*4, device, Usage.Dynamic | Usage.WriteOnly, Pool.Default );
            //			indices = new short[springs.Count*2 + particlesOriginal.Count*4];

            // Particles with mass
            _particleMaterial = new Material
                {
                    Ambient = Color.White,
                    Diffuse = Color.White
                };
            // Particles without mass
            _particle0Material = new Material
                {
                    Ambient = _particleMaterial.Diffuse = Color.Red
                };
            // Picked particle
            _pickedParticleMaterial = new Material
                {
                    Ambient = Color.Green,
                    Diffuse = Color.Green
                };
            // Structural Springs
            _springStructuralMaterial = new Material
                {
                    Ambient = Color.White,
                    Diffuse = Color.White
                };
            // Shear Springs
            _springShearMaterial = new Material
                {
                    Ambient = Color.Blue,
                    Diffuse = Color.Blue
                };
            // Bend Springs
            _springBendMaterial = new Material
                {
                    Ambient = Color.Green,
                    Diffuse = Color.Green
                };
            // Manual Springs
            _springManualMaterial = new Material
                {
                    Ambient = Color.Yellow,
                    Diffuse = Color.Yellow
                };
            // Broken Springs
            _springBrokenMaterial = new Material
                {
                    Ambient = Color.Red,
                    Diffuse = Color.Red
                };
            // Velocity Vectors
            _velocityMaterial = new Material
                {
                    Ambient = Color.Magenta,
                    Diffuse = Color.Magenta
                };
            // Force Vectors
            _forceMaterial = new Material
                {
                    Ambient = Color.Cyan,
                    Diffuse = Color.Cyan
                };
            // new index and vertex buffer
            _vBuffer = new VertexBuffer(typeof (CustomVertex.PositionColored), Triangles.Count*2 + ParticlesCurrent.Count*3, device, Usage.Dynamic | Usage.WriteOnly, CustomVertex.PositionColored.Format, Pool.Default);
            _vertices = new CustomVertex.PositionColored[Triangles.Count*2 + ParticlesCurrent.Count*3];
            _iBuffer = new IndexBuffer(typeof (short), Triangles.Count*3 + Springs.Count*2 + ParticlesOriginal.Count*4, device, Usage.Dynamic | Usage.WriteOnly, Pool.Default);
            _indices = new short[Triangles.Count*3 + Springs.Count*2 + ParticlesOriginal.Count*4];

            if (Triangles.Count > 0)
            {
                // TEXTURE TEST
                _vBufferTextured = new VertexBuffer(typeof (CustomVertex.PositionTextured), Triangles.Count*3, device, Usage.Dynamic | Usage.WriteOnly, CustomVertex.PositionTextured.Format, Pool.Default);
                _verticesTextured = new CustomVertex.PositionTextured[Triangles.Count*3];
                try
                {
                    _triangleTexture = TextureLoader.FromFile(device, @"D:\TEMP\TEXTURES\" + TextureName);
                }
                catch (Microsoft.DirectX.Direct3D.InvalidDataException e)
                {
                    _triangleTexture = TextureLoader.FromFile(device, TextureName);
                }
            }
        }

        public override void Render()
        {
            Device.Transform.World = Matrix.Identity;
            Device.RenderState.FillMode = FillMode.WireFrame;
            Device.RenderState.Lighting = true;
            Device.RenderState.Ambient = Color.White;

            for (int i = 0; i < ParticlesCurrent.Count; i++)
            {
                CParticle particle = FindParticle(i);
                _vertices[i].Position = new Vector3(particle.Position.X, particle.Position.Y, particle.Position.Z);
                _vertices[i].Color = Color.White.ToArgb();
            }

            if (DrawParticles)
            {
                // Draw particules
                if (!DrawSphereParticles)
                {
                    _vBuffer.SetData(_vertices, 0, LockFlags.None);
                    Device.SetStreamSource(0, _vBuffer, 0);
                    Device.VertexFormat = CustomVertex.PositionColored.Format;

                    Device.Material = _particleMaterial;
                    Device.RenderState.FillMode = FillMode.WireFrame;
                    Device.DrawPrimitives(PrimitiveType.PointList, 0, ParticlesCurrent.Count);
                }
                else
                {
                    Device.RenderState.FillMode = FillMode.Solid;
                    Mesh mesh = Mesh.Sphere(Device, 2, 5, 5);
                    int i = 0;
                    foreach (CParticle particle in ParticlesCurrent)
                    {
                        if (particle.NotMovable)
                            Device.Material = _particle0Material;
                        else
                            Device.Material = _particleMaterial;
                        if (i == PickedParticle) // PickedParticle
                            Device.Material = _pickedParticleMaterial;
                        Device.Transform.World = Matrix.Translation(particle.Position.X, particle.Position.Y, particle.Position.Z);
                        mesh.DrawSubset(0);
                        i++;
                    }
                    Device.Transform.World = Matrix.Identity;
                }
            }
            if (DrawSprings)
            {
                // Draw springs
                // Broken
                if (DrawSpringsBroken)
                {
                    int s = 0;
                    foreach (CSpring spring in Springs)
                        if (spring.Broken)
                        {
                            _indices[s*2 + 0] = (short) spring.P1Index;
                            _indices[s*2 + 1] = (short) spring.P2Index;
                            s++;
                        }
                    if (s > 0)
                    {
                        _vBuffer.SetData(_vertices, 0, LockFlags.None);
                        _iBuffer.SetData(_indices, 0, LockFlags.None);
                        Device.SetStreamSource(0, _vBuffer, 0);
                        Device.Indices = _iBuffer;
                        Device.Material = _springBrokenMaterial;
                        Device.VertexFormat = CustomVertex.PositionColored.Format;
                        Device.RenderState.FillMode = FillMode.WireFrame;
                        Device.DrawIndexedPrimitives(PrimitiveType.LineList, 0, 0, ParticlesCurrent.Count, 0, s);
                    }
                }
                // Non-broken
                foreach (CSpring.ESpringKind springKind in Enum.GetValues(typeof (CSpring.ESpringKind)))
                {
                    int s = 0;
                    if (springKind != CSpring.ESpringKind.Structural && !DrawSpringsNonStructural) // Draw non structural only if asked
                        continue;
                    foreach (CSpring spring in Springs)
                    {
                        if (spring.Kind == springKind && !spring.Broken)
                        {
                            _indices[s*2 + 0] = (short) spring.P1Index;
                            _indices[s*2 + 1] = (short) spring.P2Index;
                            s++;
                        }
                    }
                    _vBuffer.SetData(_vertices, 0, LockFlags.None);
                    _iBuffer.SetData(_indices, 0, LockFlags.None);
                    Device.SetStreamSource(0, _vBuffer, 0);
                    Device.Indices = _iBuffer;
                    Device.VertexFormat = CustomVertex.PositionColored.Format;

                    switch (springKind)
                    {
                        case CSpring.ESpringKind.Structural:
                            Device.Material = _springStructuralMaterial;
                            break;
                        case CSpring.ESpringKind.Shear:
                            Device.Material = _springShearMaterial;
                            break;
                        case CSpring.ESpringKind.Bend:
                            Device.Material = _springBendMaterial;
                            break;
                        case CSpring.ESpringKind.Manual:
                            Device.Material = _springManualMaterial;
                            break;
                        default:
                            Device.Material = _springStructuralMaterial;
                            break;
                    }
                    Device.RenderState.FillMode = FillMode.WireFrame;
                    Device.DrawIndexedPrimitives(PrimitiveType.LineList, 0, 0, ParticlesCurrent.Count, 0, s);
                }
            }
            if (DrawVectors)
            {
                // Draw velocity and forces
                int particlesCount = ParticlesCurrent.Count;
                int i = 0;
                foreach (CParticle particle in ParticlesCurrent)
                {
                    Vector3 to = particle.Position + (particle.Velocity*5);
                    _vertices[i + particlesCount].Position = new Vector3(to.X, to.Y, to.Z);
                    _vertices[i + particlesCount].Color = Color.Green.ToArgb();

                    to = particle.Position + (particle.Force*5);
                    _vertices[i + particlesCount*2].Position = new Vector3(to.X, to.Y, to.Z);
                    _vertices[i + particlesCount*2].Color = Color.Red.ToArgb();

                    i++;
                }
                int springsCount = Springs.Count;
                for (i = 0; i < particlesCount; i++)
                {
                    _indices[2*springsCount + i*2 + 0] = (short) i;
                    _indices[2*springsCount + i*2 + 1] = (short) (particlesCount + i);

                    _indices[2*springsCount + 2*particlesCount + i*2 + 0] = (short) i;
                    _indices[2*springsCount + 2*particlesCount + i*2 + 1] = (short) (particlesCount*2 + i);
                }
                _vBuffer.SetData(_vertices, 0, LockFlags.None);
                _iBuffer.SetData(_indices, 0, LockFlags.None);
                Device.SetStreamSource(0, _vBuffer, 0);
                Device.Indices = _iBuffer;
                Device.VertexFormat = CustomVertex.PositionColored.Format;
                if (DrawVelocity)
                {
                    Device.Material = _velocityMaterial;
                    Device.RenderState.FillMode = FillMode.WireFrame;
                    Device.DrawIndexedPrimitives(PrimitiveType.LineList, 0, 0, particlesCount*2, springsCount*2, particlesCount);
                }
                if (DrawForce)
                {
                    Device.Material = _forceMaterial;
                    Device.RenderState.FillMode = FillMode.WireFrame;
                    Device.DrawIndexedPrimitives(PrimitiveType.LineList, 0, 0, particlesCount*3, springsCount*2 + particlesCount*2, particlesCount);
                }
                // BOTH: device.DrawIndexedPrimitives( PrimitiveType.LineList, 0, 0, particlesCount*3, springsCount*2, particlesCount*2 );
            }
            if (DrawTriangles)
            {
                Device.Transform.World = Matrix.Identity;
                // Draw triangles
                if (!DrawTextured)
                {
                    for (int i = 0; i < Triangles.Count; i++)
                    {
                        CTriangle triangle = Triangles[i];
                        _indices[i*3 + 0] = (short) triangle.Vertices[0].P;
                        _indices[i*3 + 1] = (short) triangle.Vertices[1].P;
                        _indices[i*3 + 2] = (short) triangle.Vertices[2].P;
                    }
                    _vBuffer.SetData(_vertices, 0, LockFlags.None);
                    _iBuffer.SetData(_indices, 0, LockFlags.None);
                    Device.SetStreamSource(0, _vBuffer, 0);
                    Device.Indices = _iBuffer;
                    Device.VertexFormat = CustomVertex.PositionColored.Format;
                    Device.Material = _particleMaterial;
                    Device.RenderState.FillMode = FillMode.WireFrame;
                    Device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, ParticlesCurrent.Count, 0, Triangles.Count);
                }
                else
                {
                    for (int i = 0; i < Triangles.Count; i++)
                    {
                        CParticle particle;
                        CTriangle triangle = Triangles[i];

                        particle = ParticlesCurrent[triangle.Vertices[0].P];
                        _verticesTextured[i*3 + 0].Position = particle.Position;
                        _verticesTextured[i*3 + 0].Tu = triangle.Vertices[0].Tu;
                        _verticesTextured[i*3 + 0].Tv = triangle.Vertices[0].Tv;

                        particle = ParticlesCurrent[triangle.Vertices[1].P];
                        _verticesTextured[i*3 + 1].Position = particle.Position;
                        _verticesTextured[i*3 + 1].Tu = triangle.Vertices[1].Tu;
                        _verticesTextured[i*3 + 1].Tv = triangle.Vertices[1].Tv;

                        particle = ParticlesCurrent[triangle.Vertices[2].P];
                        _verticesTextured[i*3 + 2].Position = particle.Position;
                        _verticesTextured[i*3 + 2].Tu = triangle.Vertices[2].Tu;
                        _verticesTextured[i*3 + 2].Tv = triangle.Vertices[2].Tv;

                        _indices[i*3 + 0] = (short) (i*3 + 0);
                        _indices[i*3 + 1] = (short) (i*3 + 1);
                        _indices[i*3 + 2] = (short) (i*3 + 2);
                    }
                    Device.RenderState.Lighting = true;
                    _vBufferTextured.SetData(_verticesTextured, 0, LockFlags.None);
                    _iBuffer.SetData(_indices, 0, LockFlags.None);
                    Device.SetStreamSource(0, _vBufferTextured, 0);
                    Device.Indices = _iBuffer;
                    Device.Material = _particleMaterial;
                    Device.VertexFormat = CustomVertex.PositionTextured.Format;
                    Device.SetTexture(0, _triangleTexture);
                    Device.RenderState.FillMode = FillMode.Solid;
                    Device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, Triangles.Count*3, 0, Triangles.Count);
                    Device.SetTexture(0, null);
                }
            }
            if (DrawTriangleNormal)
            {
                // Draw triangle normals
                for (int i = 0; i < Triangles.Count; i++)
                {
                    CTriangle triangle = Triangles[i];
                    CParticle p1 = ParticlesCurrent[triangle.Vertices[0].P];
                    CParticle p2 = ParticlesCurrent[triangle.Vertices[1].P];
                    CParticle p3 = ParticlesCurrent[triangle.Vertices[2].P];
                    _vertices[i*2 + 0].Position = (p1.Position + p2.Position + p3.Position)*(1.0f/3.0f);
                    _vertices[i*2 + 0].Color = Color.White.ToArgb();
                    _vertices[i*2 + 1].Position = _vertices[i*2 + 0].Position + (triangle.Normal*5.0f);
                    _vertices[i*2 + 1].Color = Color.White.ToArgb();
                }
                for (int i = 0; i < Triangles.Count; i++)
                {
                    _indices[i*2 + 0] = (short) (i*2 + 0);
                    _indices[i*2 + 1] = (short) (i*2 + 1);
                }
                _vBuffer.SetData(_vertices, 0, LockFlags.None);
                _iBuffer.SetData(_indices, 0, LockFlags.None);
                Device.SetStreamSource(0, _vBuffer, 0);
                Device.Indices = _iBuffer;
                Device.VertexFormat = CustomVertex.PositionColored.Format;
                Device.Material = _particle0Material;
                Device.RenderState.FillMode = FillMode.WireFrame;
                Device.DrawIndexedPrimitives(PrimitiveType.LineList, 0, 0, Triangles.Count*2, 0, Triangles.Count);
            }
            if (DrawVertexNormal)
            {
                // Draw vertices normals
                for (int i = 0; i < ParticlesCurrent.Count; i++)
                {
                    CParticle particle = FindParticle(i);
                    _vertices[i*2 + 0].Position = particle.Position;
                    _vertices[i*2 + 0].Color = Color.White.ToArgb();
                    _vertices[i*2 + 1].Position = particle.Position + (particle.Normal*5.0f);
                    _vertices[i*2 + 1].Color = Color.White.ToArgb();
                }
                for (int i = 0; i < ParticlesCurrent.Count; i++)
                {
                    _indices[i*2 + 0] = (short) (i*2 + 0);
                    _indices[i*2 + 1] = (short) (i*2 + 1);
                }
                _vBuffer.SetData(_vertices, 0, LockFlags.None);
                _iBuffer.SetData(_indices, 0, LockFlags.None);
                Device.SetStreamSource(0, _vBuffer, 0);
                Device.Indices = _iBuffer;
                Device.VertexFormat = CustomVertex.PositionColored.Format;
                Device.Material = _velocityMaterial;
                Device.RenderState.FillMode = FillMode.WireFrame;
                Device.DrawIndexedPrimitives(PrimitiveType.LineList, 0, 0, ParticlesCurrent.Count, 0, ParticlesCurrent.Count);
            }

            if (DisplayInfos)
            {
                int y = 0;
                int x = 0;
                int i = 0;
                foreach (CParticle particle in ParticlesCurrent)
                {
                    Font.DrawText("Particle: " + i, x, y, Color.White, false);
                    y += 10;
                    Font.DrawText("Pos: " + CUtils.Vector3ToStr(particle.Position), x, y, Color.White, false);
                    y += 10;
                    Font.DrawText("Vel: " + CUtils.Vector3ToStr(particle.Velocity), x, y, Color.White, false);
                    y += 10;
                    Font.DrawText("Force: " + CUtils.Vector3ToStr(particle.Force), x, y, Color.White, false);
                    y += 10;
                    Font.DrawText("Mass: " + particle.OneOverMass, x, y, Color.White, false);
                    y += 10;
                    Font.DrawText("Movable: " + (!particle.NotMovable), x, y, Color.White, false);
                    y += 10;
                    i++;
                }
                i = 0;
                foreach (CSpring spring in Springs)
                {
                    Font.DrawText("Spring: " + i, x, y, Color.White, false);
                    y += 10;
                    Font.DrawText("p1: " + spring.P1Index, x, y, Color.White, false);
                    y += 10;
                    Font.DrawText("p2: " + spring.P2Index, x, y, Color.White, false);
                    y += 10;
                    Font.DrawText("Kd: " + spring.Kd, x, y, Color.White, false);
                    y += 10;
                    Font.DrawText("Ks: " + spring.Ks, x, y, Color.White, false);
                    y += 10;
                    Font.DrawText("Length: " + spring.RestLength, x, y, Color.White, false);
                    y += 10;
                    i++;
                }
            }

            if (DisplayStats)
            {
                int y = 0;
                Font.DrawText("#Loop:" + _loopCount, 0, y, Color.White, false);
                y += 10;
                Font.DrawText("Max resolve count:" + _maxResolveCount, 0, y, Color.White, false);
                y += 10;
                Font.DrawText("Max collision count:" + _maxCollisionCount, 0, y, Color.White, false);
                y += 10;
                Font.DrawText("#Particles:" + ParticlesCurrent.Count, 0, y, Color.White, false);
                y += 10;
                Font.DrawText("#Springs:" + Springs.Count, 0, y, Color.White, false);
                y += 10;
                Font.DrawText("#Triangles:" + Triangles.Count, 0, y, Color.White, false);
                y += 10;
                float length = 0.0f;
                int structural = 0;
                int shear = 0;
                int bend = 0;
                int manual = 0;
                int broken = 0;
                foreach (CSpring spring in Springs)
                {
                    CParticle p1 = ParticlesCurrent[spring.P1Index];
                    CParticle p2 = ParticlesCurrent[spring.P2Index];
                    length += (p1.Position - p2.Position).Length();
                    switch (spring.Kind)
                    {
                        case CSpring.ESpringKind.Structural:
                            structural++;
                            break;
                        case CSpring.ESpringKind.Shear:
                            shear++;
                            break;
                        case CSpring.ESpringKind.Bend:
                            bend++;
                            break;
                        case CSpring.ESpringKind.Manual:
                            manual++;
                            break;
                    }
                    if (spring.Broken) 
                        broken++;
                }
                Font.DrawText("#Structurals:" + structural, 0, y, Color.White, false);
                y += 10;
                Font.DrawText("#Shears:" + shear, 0, y, Color.White, false);
                y += 10;
                Font.DrawText("#Bends:" + bend, 0, y, Color.White, false);
                y += 10;
                Font.DrawText("#Manuals:" + manual, 0, y, Color.White, false);
                y += 10;
                Font.DrawText("#Brokens:" + broken, 0, y, Color.White, false);
                y += 10;
                Font.DrawText("Length:" + length, 0, y, Color.White, false);
                y += 10;
                Font.DrawText("Average Length:" + (length/(float) Springs.Count), 0, y, Color.White, false);
                y += 10;
                Vector3 gCenter = Vector3.Empty;
                foreach (CParticle particle in ParticlesCurrent)
                    gCenter = gCenter + particle.Position*particle.OneOverMass;
                gCenter = gCenter*(1.0f/(float) ParticlesCurrent.Count);
                Font.DrawText("Gravity center:" + CUtils.Vector3ToStr(gCenter), 0, y, Color.White, false);
                y += 10;
                Font.DrawText("Friction:" + UseFriction, 0, y, Color.White, false);
                y += 10;
                if (UseFriction)
                {
                    Font.DrawText("CSf:" + CSf, 0, y, Color.White, false);
                    y += 10;
                    Font.DrawText("CKf:" + CKf, 0, y, Color.White, false);
                    y += 10;
                }
                Font.DrawText("Gravity:" + UseGravity, 0, y, Color.White, false);
                y += 10;
                Font.DrawText("Damping:" + UseDamping, 0, y, Color.White, false);
                y += 10;
                Font.DrawText("Wind:" + UseWind, 0, y, Color.White, false);
                y += 10;
                Font.DrawText("Mouse:" + UseMouse, 0, y, Color.White, false);
                y += 10;
            }
        }

        #endregion
    }
}