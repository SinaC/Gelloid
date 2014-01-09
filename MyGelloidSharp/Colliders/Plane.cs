using System.Drawing;
using System.Xml.Serialization;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace MyGelloidSharp.Colliders
{
    /// <summary>
    /// Summary description for Plane Collider
    /// </summary>
    public abstract class CPlane : CCollider
    {
        [XmlElement("normal")] public Vector3 Normal;
        [XmlElement("distance")] public float Distance;

        [XmlIgnore] protected const float PatchSize = 50.0f;
        [XmlIgnore] protected const int PatchHalfSlice = 10;

        [XmlIgnore] protected Mesh Mesh;
        [XmlIgnore] protected Matrix MeshMatrix;
        protected CPlane()
        {
        }
        protected CPlane(string name, Vector3 normal, float distance)
            : base(name)
        {
            Normal = Vector3.Normalize(normal);
            Distance = distance;
        }

        public void InitializeGraphics()
        {
            if (Device == null)
                return;
            Mesh = Mesh.Box(Device, PatchSize, 0, PatchSize);
            //			meshMatrix = Matrix.RotationAxis( normal, (float)System.Math.Acos( Vector3.Dot( normal, new Vector3( 0, 1, 0 ) ) ) )
            //				* Matrix.Translation( normal * (-distance) );
            MeshMatrix = Matrix.Translation(Normal*(-Distance)); // FIXME

            Material = new Material
                {
                    Ambient = Color.YellowGreen,
                    Diffuse = Color.YellowGreen
                };
        }

        public override bool Pick(Vector3 rayOrigin, Vector3 rayDirection, out float distance)
        {
            distance = float.MaxValue;
            return false; // TODO
        }

        public override void InitializeGraphics(Device device, CFont font)
        {
            base.InitializeGraphics(device, font);
            InitializeGraphics();
        }

        public override void Render()
        {
            // Put Collider in world
            Device.SetTexture(0, null);
            Device.RenderState.Lighting = true;
            Device.RenderState.FillMode = FillMode.WireFrame;
            Device.Material = Material;
            Device.RenderState.Ambient = Color.White;

            for (int x = -PatchHalfSlice; x <= PatchHalfSlice; x++)
            {
                for (int z = -PatchHalfSlice; z <= PatchHalfSlice; z++)
                {
                    Device.Transform.World = Matrix.Translation(x*PatchSize, 0, z*PatchSize)*MeshMatrix;
                    Mesh.DrawSubset(0);
                }
            }

            Device.Transform.World = Matrix.Identity;
        }
    }

    public class CSimplePlane : CPlane
    {
        public CSimplePlane()
        {
        }

        public CSimplePlane(string name, Vector3 normal, float distance)
            : base(name, normal, distance)
        {
        }

        public override ECollideState Collide(Vector3 point, Vector3 velocity, out Vector3 outNormal, out bool inContact)
        {
            // Simple without penetration
            inContact = false;
            outNormal = Vector3.Empty;
            float axbyczd = Vector3.Dot(point, Normal) + Distance;
            if (axbyczd < DepthEpsilon)
            {
                float relativeVelocity = Vector3.Dot(Normal, velocity);
                if (relativeVelocity < 0.0f)
                {
                    outNormal = Normal;
                    return ECollideState.Colliding;
                }
            }
            return ECollideState.NotColliding;
        }

        public override ECollideState Collide(Vector3 point, Vector3 velocity, out Vector3 normal, out Vector3 intersection)
        {
            normal = Vector3.Empty; // TODO
            intersection = Vector3.Empty;
            return ECollideState.NotColliding;
        }
    }

    public class CComplexPlane : CPlane
    {
        public CComplexPlane()
        {
        }

        public CComplexPlane(string name, Vector3 normal, float distance)
            : base(name, normal, distance)
        {
        }

        public override ECollideState Collide(Vector3 point, Vector3 velocity, out Vector3 outNormal, out bool inContact)
        {
            // Complex collision with penetration
            inContact = false;
            outNormal = Vector3.Empty;
            ECollideState returnValue = ECollideState.NotColliding;
            double axbyczd = Vector3.Dot(point, Normal) + Distance;
            if (axbyczd < -DepthEpsilon) // Penetrates
                return ECollideState.Penetrating;
            else if (axbyczd < DepthEpsilon)
            {
                // Collides
                float relativeVelocity = Vector3.Dot(Normal, velocity);
                if (relativeVelocity < 0.0f)
                {
                    outNormal = Normal;
                    returnValue = ECollideState.Colliding;
                }
                // Check if the Particle is in contact and need friction applied
                if (axbyczd < ContactEpsilon)
                {
                    // Contacts
                    outNormal = Normal;
                    inContact = true;
                }
            }
            return returnValue;
        }

        public override ECollideState Collide(Vector3 point, Vector3 velocity, out Vector3 normal, out Vector3 intersection)
        {
            normal = Vector3.Empty; // TODO
            intersection = Vector3.Empty;
            return ECollideState.NotColliding;
        }

    }
}