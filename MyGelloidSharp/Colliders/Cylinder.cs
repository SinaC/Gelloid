using System;
using System.Drawing;
using System.Xml.Serialization;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace MyGelloidSharp.Colliders
{

    public class CCylinder : CCollider
    {
        [XmlIgnore] private const float Epsilon = 0.001f;
        [XmlIgnore] private static readonly Vector3 OriginalAxis = new Vector3(0, 0, 1); // Mesh.cylinder axis

        [XmlElement("Center")] public Vector3 Center;
        [XmlElement("Axis")] public Vector3 Axis;
        [XmlElement("Radius")] public float Radius;
        [XmlElement("Height")] public float Height;

        [XmlIgnore] protected Mesh Mesh;
        [XmlIgnore] protected Matrix MeshMatrix;
        [XmlIgnore] private Matrix _invMatrix;

        public CCylinder()
        {
        }

        public CCylinder(string name, Vector3 center, Vector3 axis, float radius, float height)
            : base(name)
        {
            this.Center = center;
            this.Axis = Vector3.Normalize(axis);
            this.Radius = radius;
            this.Height = height;
        }

        public override ECollideState Collide(Vector3 point, Vector3 velocity, out Vector3 normal, out bool inContact)
        {
            normal = Vector3.Empty;
            inContact = false;

            Vector3 W = Axis;
            Vector3 U, V;
            MyGelloidSharp.CIntersection.GenerateOrthonormalBasis(W, out U, out V);

            //			Vector3 origin = Vector3.TransformCoordinate( point, invMatrix );
            //			Vector3 direction = Vector3.TransformNormal( Vector3.Normalize( velocity ), invMatrix );
            //			// Origin and direction are now in mesh axis (0,0,1)
            Vector3 diffV = point - Center;
            Vector3 newV = Vector3.Normalize(velocity);
            Vector3 origin = new Vector3(Vector3.Dot(diffV, U), Vector3.Dot(diffV, V), Vector3.Dot(diffV, W));
            Vector3 direction = new Vector3(Vector3.Dot(newV, U), Vector3.Dot(newV, V), Vector3.Dot(newV, W));

            float t0, t1, tmp0, tmp1, a, b, c, discr, root, t;
            float halfHeight = Height/2.0f;

            // Check if outside infinite plane on cap
            if (origin.Z < -halfHeight - CCollider.DepthEpsilon || origin.Z > halfHeight + CCollider.DepthEpsilon)
                return ECollideState.NotColliding;

            // Check if outside infinite cylinder
            float dist = origin.X*origin.X + origin.Y*origin.Y - Radius*Radius;
            if (dist > CCollider.DepthEpsilon)
                return ECollideState.NotColliding;

            // We are inside a capped cylinder  center: 0,0,0  axis: 0,0,1  radius: radius+depthEpsilon  caps: +-(height/2+depthEpsilon)

            //TODO: inContact: same test as below with -CCollider.contactEpsilon and +CCollider.contactEpsilon

            // Check if inside a smaller cylinder -> penetrating
            if (origin.Z > -halfHeight + CCollider.DepthEpsilon && origin.Z < halfHeight - CCollider.DepthEpsilon
                && dist < -CCollider.DepthEpsilon)
                return ECollideState.Penetrating;

            // Use directx to compute intersections
            //			IntersectInformation[] closests; // Interesting but doesn't compute normal
            //			mesh.Intersect( origin, direction, out closests );
            //			int closestId = -1;
            //			float closestDist = float.MaxValue;
            //			for ( int i = 0; i < closests.Length; i++ )
            //				if ( closests[i].Dist > 0.0f && closests[i].Dist < closestDist ) {
            //					closestId = i;
            //					closestDist = closests[i].Dist;
            //				}
            //			if ( closestId != -1 ) {
            //				Vector3 v = ( origin + closestDist * direction ) - center;
            //				float p = Vector3.Dot( v, axis );
            //				normal = Vector3.Normalize( ( v - ( axis * p ) ) );
            //				return ECollideState.COLLIDING;
            //			}

            // Parallel to axis (0,0,1), we hit the caps
            if (direction.Z >= 1.0f - Epsilon)
            {
                tmp0 = (1.0f/direction.Z);
                float tTop = (+halfHeight - origin.Z)*tmp0;
                float tBottom = (-halfHeight - origin.Z)*tmp0;
                float tMin = float.MaxValue;
                Vector3 nMin = Vector3.Empty;
                bool found = false;
                if (tTop > 0.0f && tTop < tMin)
                {
                    normal = Axis;
                    tMin = tTop;
                    found = true;
                }
                if (tBottom > 0.0f && tBottom < tMin)
                {
                    normal = -Axis;
                    tMin = tBottom;
                    found = true;
                }
                if (!found)
                    return ECollideState.NotColliding;
                return ECollideState.Colliding;
            }

            // Perpendicular to axis, we hit the wall
            if (Math.Abs(direction.Z) <= Epsilon)
            {
                a = direction.X*direction.X + direction.Y*direction.Y;
                b = origin.X*direction.X + origin.Y*direction.Y;
                c = origin.X*origin.X + origin.Y*origin.Y - Radius*Radius + CCollider.DepthEpsilon;
                discr = b*b - a*c;
                if (discr < 0.0f) // no intersection with wall
                    return ECollideState.NotColliding;
                else if (discr > 0.0f)
                {
                    // 2 intersections
                    root = (float) Math.Sqrt(discr);
                    tmp0 = 1.0f/a;
                    t0 = (-b - root)*tmp0;
                    t1 = (-b + root)*tmp0;
                    float tMin = float.MaxValue;
                    bool found = false;
                    if (t0 > 0.0f && t0 < tMin)
                    {
                        tMin = t0;
                        found = true;
                    }
                    if (t1 > 0.0f && t1 < tMin)
                    {
                        tMin = t1;
                        found = true;
                    }
                    if (!found)
                        return ECollideState.NotColliding;
                    Vector3 v = (origin + tMin*direction) - Center;
                    float p = Vector3.Dot(v, Axis);
                    normal = Vector3.Normalize((v - (Axis*p)));
                    return ECollideState.Colliding;
                }
                else
                {
                    // 1 intersection
                    float tMin = -b/a;
                    if (tMin <= 0.0f)
                        return ECollideState.NotColliding;
                    Vector3 v = (origin + tMin*direction) - Center;
                    float p = Vector3.Dot(v, Axis);
                    normal = Vector3.Normalize((v - (Axis*p)));
                    return ECollideState.Colliding;
                }
            }

            // Test intersection with caps first
            CIntersection[] intersections = new CIntersection[2];
            int idx = 0;
            float inv = 1.0f/direction.Z;

            t0 = (+halfHeight - origin.Z)*inv;
            tmp0 = origin.X + t0*direction.X;
            tmp1 = origin.Y + t0*direction.Y;
            if (t0 > 0.0f && tmp0*tmp0 + tmp1*tmp1 <= Radius*Radius + CCollider.DepthEpsilon)
                intersections[idx++] = new CIntersection(t0, CIntersection.EIntersectionKind.TOP_CAP);
            t1 = (-halfHeight - origin.Z)*inv;
            tmp0 = origin.X + t1*direction.X;
            tmp1 = origin.Y + t1*direction.Y;
            if (t1 > 0.0f && tmp0*tmp0 + tmp1*tmp1 <= Radius*Radius + CCollider.DepthEpsilon)
                intersections[idx++] = new CIntersection(t1, CIntersection.EIntersectionKind.BOTTOM_CAP);

            // Line intersects both top and bottom
            if (idx == 2)
            {
                CIntersection intersection = intersections[CIntersection.GetSmallestPositive(intersections, idx)]; // we're sure Get... will not return -1
                if (intersection.kind == CIntersection.EIntersectionKind.TOP_CAP)
                    normal = Axis;
                else if (intersection.kind == CIntersection.EIntersectionKind.BOTTOM_CAP)
                    normal = -Axis;
                return ECollideState.Colliding;
            }

            // If only 1 intersection, then line must intersect cylinder wall
            // somewhere between caps in a single point.  This case is detected
            // in the following code that tests for intersection between line and
            // cylinder wall.

            a = direction.X*direction.X + direction.Y*direction.Y;
            b = origin.X*direction.X + origin.Y*direction.Y;
            c = origin.X*origin.X - origin.Y*origin.Y - Radius*Radius + CCollider.DepthEpsilon;
            discr = b*b - a*c;
            if (discr < 0.0f)
                // line does not intersect cylinder wall
                return ECollideState.NotColliding; // SHOULD NEVER HAPPEN
            else if (discr > 0.0f)
            {
                root = (float) Math.Sqrt(discr);
                inv = 1.0f/a;
                t = (-b - root)*inv;
                if (t0 <= t1)
                {
                    if (t0 <= t && t <= t1)
                        intersections[idx++] = new CIntersection(t, CIntersection.EIntersectionKind.WALL);
                }
                else if (t1 <= t && t <= t0)
                    intersections[idx++] = new CIntersection(t, CIntersection.EIntersectionKind.WALL);
                if (idx != 2)
                {
                    t = (-b + root)*inv;
                    if (t0 <= t1)
                    {
                        if (t0 <= t && t <= t1)
                            intersections[idx++] = new CIntersection(t, CIntersection.EIntersectionKind.WALL);
                    }
                    else if (t1 <= t && t <= t0)
                        intersections[idx++] = new CIntersection(t, CIntersection.EIntersectionKind.WALL);
                }
            }
            else
            {
                t = -b/a;
                if (t0 <= t1)
                {
                    if (t0 <= t && t <= t1)
                        intersections[idx++] = new CIntersection(t, CIntersection.EIntersectionKind.WALL);
                }
                else if (t1 <= t && t <= t0)
                    intersections[idx++] = new CIntersection(t, CIntersection.EIntersectionKind.WALL);
            }
            int interId = CIntersection.GetSmallestPositive(intersections, idx);
            if (interId == -1)
                return ECollideState.NotColliding;
            CIntersection inter = intersections[interId];
            if (inter.kind == CIntersection.EIntersectionKind.TOP_CAP)
                normal = Axis;
            else if (inter.kind == CIntersection.EIntersectionKind.BOTTOM_CAP)
                normal = -Axis;
            else if (inter.kind == CIntersection.EIntersectionKind.WALL)
            {
                Vector3 v = (origin + inter.distance*direction) - Center;
                float p = Vector3.Dot(v, Axis);
                normal = Vector3.Normalize((v - (Axis*p)));
            }
            return ECollideState.Colliding;
        }

        public override ECollideState Collide(Vector3 point, Vector3 velocity, out Vector3 normal, out Vector3 intersection)
        {
            normal = Vector3.Empty;
            intersection = Vector3.Empty;
            return ECollideState.NotColliding;
        }

        public override void InitializeGraphics(Device device, CFont font)
        {
            base.InitializeGraphics(device, font);
            if (device == null)
                return;
            Mesh = Mesh.Cylinder(device, Radius, Radius, Height, 10, 10);
            float angle = (float) Math.Acos(Vector3.Dot(Axis, OriginalAxis)); // Compute angle between our cylinder axis and Mesh.cylinder axis
            Vector3 rotAxis = Vector3.Cross(Axis, OriginalAxis);
            MeshMatrix = Matrix.RotationAxis(rotAxis, angle)*Matrix.Translation(Center);
            _invMatrix = Matrix.Invert(MeshMatrix);
            Material = new Material
                {
                    Ambient = Color.FromArgb(255, 255, 0),
                    Diffuse = Color.FromArgb(255, 255, 0)
                };
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

        public override bool Pick(Vector3 rayOrigin, Vector3 rayDirection, out float distance)
        {
            distance = 0;
            return false;
        }

        private void GenerateOrthonormalBasis(ref Vector3 U, ref Vector3 V, ref Vector3 W)
        {
            if (Math.Abs(W.X) >= Math.Abs(W.Y))
            {
                // W.x or W.z is the largest magnitude component, swap them
                float invLength = (float) (1.0/Math.Sqrt(W.X*W.X + W.Z*W.Z));
                U = new Vector3(-W.Z*invLength, 0.0f, +W.X*invLength);
            }
            else
            {
                // W.y or W.z is the largest magnitude component, swap them
                float invLength = (float) (1.0/Math.Sqrt(W.Y*W.Y + W.Z*W.Z));
                U = new Vector3(0.0f, +W.Z*invLength, -W.Y*invLength);
            }
            V = Vector3.Cross(W, U);
        }

        private class CIntersection
        {
            public enum EIntersectionKind
            {
                TOP_CAP,
                BOTTOM_CAP,
                WALL
            };

            public float distance;
            public EIntersectionKind kind;

            public CIntersection(float distance, EIntersectionKind kind)
            {
                this.distance = distance;
                this.kind = kind;
            }

            public static int GetSmallestPositive(CIntersection[] intersections, int count)
            {
                float tMin = float.MaxValue;
                int found = -1;
                for (int i = 0; i < count; i++)
                {
                    CIntersection intersection = intersections[i];
                    if (intersection.distance > 0.0f && intersection.distance < tMin)
                    {
                        found = i;
                        tMin = intersection.distance;
                    }
                }
                return found;
            }

            public static int GetSmallest(CIntersection[] intersections, int count)
            {
                float tMin = float.MaxValue;
                int found = -1;
                for (int i = 0; i < count; i++)
                {
                    CIntersection intersection = intersections[i];
                    if (intersection.distance < tMin)
                    {
                        found = i;
                        tMin = intersection.distance;
                    }
                }
                return found;
            }
        }
    }
}