using System.Xml.Serialization;

using Microsoft.DirectX;

using MyGelloidSharp.Colliders;

namespace MyGelloidSharp.PhysEnv {
	public class CCollision {
		[XmlElement("collider")] public CCollider Collider; // Collider
		[XmlElement("particle")] public int Particle; // Particle in collision
		[XmlElement("normal")] public Vector3 Normal; // Normal at collision point

		public CCollision() {
			Collider = null;
			Particle = 0;
			Normal = Vector3.Empty;
		}
	}
}