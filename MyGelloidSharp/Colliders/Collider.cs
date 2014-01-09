using System;
using System.IO;
using System.Xml.Serialization;

using Microsoft.DirectX;

namespace MyGelloidSharp.Colliders
{
    public abstract class CCollider : CRenderableObject
    {
        public enum ECollideState
        {
            NotColliding,
            Penetrating,
            Colliding
        };

        [XmlIgnore] public const float DepthEpsilon = 0.1f;
        [XmlIgnore] public const float ContactEpsilon = 0.5f; // Threshold for particle in contact. Must be > depthEpsilon
        protected CCollider()
        {
        }
        protected CCollider(string name)
            : base(name)
        {
        }

        public abstract ECollideState Collide(Vector3 point, Vector3 velocity, out Vector3 normal, out bool inContact);
        public abstract ECollideState Collide(Vector3 point, Vector3 velocity, out Vector3 normal, out Vector3 intersection); // TODO

        #region Xml Load/Save

        // Save to Xml
        public static void XmlExport(CCollider collider, string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(collider.GetType());
            TextWriter w = new StreamWriter(filePath);
            serializer.Serialize(w, collider);
            w.Close();
        }

        // Load from Xml
        public static object XmlImport(string filePath, Type colliderKind)
        {
            XmlSerializer serializer = new XmlSerializer(colliderKind);
            TextReader r = new StreamReader(filePath);
            object collider = serializer.Deserialize(r);
            r.Close();

            return collider;
        }

        #endregion
    }
}