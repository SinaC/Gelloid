using System;
using System.IO;
using System.Xml.Serialization;

using Microsoft.DirectX;

namespace MyGelloidSharp.Cameras
{

    [XmlRoot("Camera")]
    public class CCamera : CNamedObject
    {
        public enum ECameraType
        {
            LandObject,
            Aircraft,
            Mixed
        }

        #region Fields

        [XmlElement("type")] public ECameraType CameraType;
        [XmlElement("right")] public Vector3 Right;
        [XmlElement("up")] public Vector3 Up;
        [XmlElement("look")] public Vector3 Look;
        [XmlElement("pos")] public Vector3 Pos;

        [XmlElement("fov")] public float Fov = (float) Math.PI/4.0f;
        [XmlElement("ratio")] public float Ratio = 1.0f;
        [XmlElement("nearClip")] public float NearClip = 1.0f;
        [XmlElement("farClip")] public float FarClip = 1000.0f;

        #endregion

        #region Constructors

        public CCamera()
        {
            CameraType = ECameraType.Mixed;
            Pos = new Vector3(0.0f, 0.0f, -5.0f);
            Look = new Vector3(0.0f, 0.0f, 0.0f);
            Up = new Vector3(0.0f, 1.0f, 0.0f);
            Right = new Vector3(1.0f, 0.0f, 0.0f);
        }

        public CCamera(string name)
            : base(name)
        {
            CameraType = ECameraType.Mixed;
            Pos = new Vector3(0.0f, 0.0f, -5.0f);
            Look = new Vector3(0.0f, 0.0f, 0.0f);
            Up = new Vector3(0.0f, 1.0f, 0.0f);
            Right = new Vector3(1.0f, 0.0f, 0.0f);
        }

        public CCamera(string name, ECameraType cameraType)
            : base(name)
        {
            Name = name;
            CameraType = cameraType;
            Pos = new Vector3(0.0f, 0.0f, -5.0f);
            Look = new Vector3(0.0f, 0.0f, 0.0f);
            Up = new Vector3(0.0f, 1.0f, 0.0f);
            Right = new Vector3(1.0f, 0.0f, 0.0f);
        }

        #endregion

        #region Properties and Indexers

        // Identity
        [XmlIgnore]
        public Matrix World
        {
            get { return Matrix.Identity; }
        }
        // Position, target, direction
        [XmlIgnore]
        public Matrix View
        {
            get { return BuildViewMatrix(); }
        }
        // Fov, aspect ratio, near plane, far plane
        [XmlIgnore]
        public Matrix Projection
        {
            get { return Matrix.PerspectiveFovLH(Fov, Ratio, NearClip, FarClip); }
        }

        #endregion

        #region Xml Load/Save

        // Save to Xml
        public static void XmlExport(CCamera camera, string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(camera.GetType());
            TextWriter w = new StreamWriter(filePath);
            serializer.Serialize(w, camera);
            w.Close();
        }

        // Load from Xml
        public static CCamera XmlImport(string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof (CCamera));
            TextReader r = new StreamReader(filePath);
            CCamera camera = (CCamera) serializer.Deserialize(r);
            r.Close();

            return camera;
        }

        #endregion

        #region Methods

        public void Initialize(Vector3 pos, Vector3 target)
        {
            Pos = pos;
            SetTarget(target);
        }

        public void SetTarget(Vector3 target)
        {
            Look = target - Pos;
            Look.Normalize();
            //right = Vector3.Cross( up, look );
        }

        public void SetPosition(Vector3 position)
        {
            Vector3 target = Pos + Look;
            Pos = position;
            SetTarget(target);
        }

        public void Strafe(float units)
        {
            if (CameraType == ECameraType.LandObject)
                Pos += new Vector3(Right.X, 0.0f, Right.Z)*units;
            else if (CameraType == ECameraType.Aircraft
                     || CameraType == ECameraType.Mixed)
                Pos += Right*units;
        }

        public void Fly(float units)
        {
            if (CameraType == ECameraType.Aircraft
                || CameraType == ECameraType.Mixed)
                Pos += Up*units;
        }

        public void Walk(float units)
        {
            if (CameraType == ECameraType.LandObject)
                Pos += new Vector3(Look.X, 0.0f, Look.Z)*units;
            else if (CameraType == ECameraType.Aircraft
                     || CameraType == ECameraType.Mixed)
                Pos += Look*units;
        }

        public void Pitch(float angle)
        {
            Matrix T = Matrix.RotationAxis(Right, angle);

            Up = Vector3.TransformCoordinate(Up, T);
            Look = Vector3.TransformCoordinate(Look, T);
        }

        public void Yaw(float angle)
        {
            Matrix T;

            if (CameraType == ECameraType.LandObject
                || CameraType == ECameraType.Mixed)
                T = Matrix.RotationY(angle);
            else if (CameraType == ECameraType.Aircraft)
                T = Matrix.RotationAxis(Up, angle);
            else
                return;

            Right = Vector3.TransformCoordinate(Right, T);
            Look = Vector3.TransformCoordinate(Look, T);
        }

        public void Roll(float angle)
        {
            if (CameraType == ECameraType.Aircraft
                || CameraType == ECameraType.Mixed)
            {
                Matrix T = Matrix.RotationAxis(Look, angle);

                Right = Vector3.TransformCoordinate(Right, T);
                Up = Vector3.TransformCoordinate(Up, T);
            }

        }

        // Zoom using fov
        public void Zoom(float z)
        {
            Fov += z;
            if (Fov < 0.01f)
                Fov = 0.01f;
            if (Fov > (float) Math.PI - 0.1f)
                Fov = (float) Math.PI - 0.1f;
        }

        #endregion

        #region Private methods

        private Matrix BuildViewMatrix()
        {
            Matrix v;

            Look = Vector3.Normalize(Look);

            Up = Vector3.Cross(Look, Right);
            Up = Vector3.Normalize(Up);

            Right = Vector3.Cross(Up, Look);

            float x = -Vector3.Dot(Right, Pos);
            float y = -Vector3.Dot(Up, Pos);
            float z = -Vector3.Dot(Look, Pos);

            v.M11 = Right.X;
            v.M12 = Up.X;
            v.M13 = Look.X;
            v.M14 = 0.0f;

            v.M21 = Right.Y;
            v.M22 = Up.Y;
            v.M23 = Look.Y;
            v.M24 = 0.0f;

            v.M31 = Right.Z;
            v.M32 = Up.Z;
            v.M33 = Look.Z;
            v.M34 = 0.0f;

            v.M41 = x;
            v.M42 = y;
            v.M43 = z;
            v.M44 = 1.0f;

            return v;
        }

        #endregion
    }
}