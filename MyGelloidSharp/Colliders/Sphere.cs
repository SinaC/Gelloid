using System.Drawing;
using System.Xml.Serialization;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace MyGelloidSharp.Colliders
{
    /// <summary>
    /// Summary description for Sphere Collider
    /// </summary>
    public abstract class CSphere : CCollider
    {
        [XmlElement("center")] public Vector3 Center;
        [XmlElement("radius")] public float Radius;

        [XmlIgnore] protected const float RenderTolerance = 0.5f; // Depends on gelloid granularity/size

        [XmlIgnore] protected Mesh Mesh;
        [XmlIgnore] protected Matrix MeshMatrix;

        protected CSphere()
        {
        }

        protected CSphere(string name, Vector3 center, float radius)
            : base(name)
        {
            Center = center;
            Radius = radius;
        }

        public void InitializeGraphics()
        {
            if (Device == null)
                return;
            Mesh = Mesh.Sphere(Device, Radius - RenderTolerance, 10, 10);
            MeshMatrix = Matrix.Translation(Center);
            Material = new Material
                {
                    Ambient = Color.FromArgb(255, 0, 255),
                    Diffuse = Color.FromArgb(255, 255, 255)
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
            Device.RenderState.FillMode = FillMode.Solid;
            Device.RenderState.Lighting = true;
            Device.RenderState.Ambient = Color.FromArgb(255, 255, 255);
            Device.Lights[0].Enabled = false; // no directional light
            Device.Material = Material;
            Device.Transform.World = MeshMatrix;
            Mesh.DrawSubset(0);
        }
    }

    public class CSimpleSphere : CSphere
    {
        public CSimpleSphere()
        {
        }

        public CSimpleSphere(string name, Vector3 center, float radius)
            : base(name, center, radius)
        {
        }

        public override ECollideState Collide(Vector3 point, Vector3 velocity, out Vector3 normal, out bool inContact)
        {
            inContact = false; // Simple collision without penetration
            normal = Vector3.Empty;
            Vector3 distVect = point - Center;
            float dist = distVect.LengthSq() - (Radius*Radius); // since it is testing the squared distance, square the radius also
            if (dist < DepthEpsilon)
            {
                distVect.Normalize();
                float relativeVelocity = Vector3.Dot(distVect, velocity);
                if (relativeVelocity < 0.0f)
                {
                    normal = distVect;
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

    public class CComplexSphere : CSphere
    {
        public CComplexSphere()
        {
        }

        public CComplexSphere(string name, Vector3 center, float radius)
            : base(name, center, radius)
        {
        }

        public override ECollideState Collide(Vector3 point, Vector3 velocity, out Vector3 normal, out bool inContact)
        {
            // Complex collision with penetration
            inContact = false;
            normal = Vector3.Empty;
            Vector3 distVect = point - Center;
            ECollideState returnValue = ECollideState.NotColliding;
            float dist = distVect.LengthSq() - (Radius*Radius); // since it is testing the squared distance, square the radius also
            if (dist < -DepthEpsilon) // Penetrates
                // Once any particle penetrates, quit
                return ECollideState.Penetrating;
            else if (dist < DepthEpsilon)
            {
                // Collides
                distVect.Normalize();
                float relativeVelocity = Vector3.Dot(distVect, velocity);
                if (relativeVelocity < 0.0f)
                {
                    normal = distVect;
                    returnValue = ECollideState.Colliding;
                }
                // Check if the Particle is in contact and need friction applied
                if (dist < ContactEpsilon)
                {
                    // Contacts
                    normal = distVect;
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