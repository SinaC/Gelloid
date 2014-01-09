using System.Xml.Serialization;

using Microsoft.DirectX;

namespace MyGelloidSharp.Colliders
{

    public class CCircle : CCollider
    {
        [XmlElement("centerX")] public float CenterX;
        [XmlElement("centerZ")] public float CenterZ;
        [XmlElement("radius")] public float Radius;
        [XmlElement("height")] public float Height;
        [XmlElement("thickness")] public float Thickness;

        public CCircle()
        {
        }

        public CCircle(string name, float centerX, float centerZ, float radius, float height, float thickness)
            : base(name)
        {
            CenterX = centerX;
            CenterZ = centerZ;
            Radius = radius;
            Height = height;
            Thickness = thickness;
        }

        public override ECollideState Collide(Vector3 point, Vector3 velocity, out Vector3 normal, out bool inContact)
        {
            inContact = false;
            normal = new Vector3(0, 1, 0);
            float dist = (point.X - CenterX)*(point.X - CenterX) + (point.Z - CenterZ)*(point.Z - CenterZ);
            if (dist < Radius*Radius && point.Y < Height && point.Y > Height - Thickness)
            {
                float relativeVelocity = Vector3.Dot(velocity, normal);
                if (relativeVelocity < 0.0f)
                {
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

        public override bool Pick(Vector3 rayOrigin, Vector3 rayDirection, out float distance)
        {
            distance = float.MaxValue;
            return false; // TODO
        }
    }
}