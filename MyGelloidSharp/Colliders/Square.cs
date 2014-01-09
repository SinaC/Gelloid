using System.Xml.Serialization;

using Microsoft.DirectX;

namespace MyGelloidSharp.Colliders {
	/// <summary>
	/// Summary description for Square Collider
	/// </summary>
	public class CSquare: CCollider {
		[XmlElement("minX")] public float MinX;
		[XmlElement("minZ")] public float MinZ;
		[XmlElement("maxX")] public float MaxX;
		[XmlElement("maxZ")] public float MaxZ;
		[XmlElement("height")] public float Height;

		public CSquare() {
		}

		public CSquare( float minX, float minZ, float maxX, float maxZ, float height ) {
			MinX = minX;
			MinZ = minZ;
			MaxX = maxX;
			MaxZ = maxZ;
			Height = height;
		}

		public override ECollideState Collide( Vector3 point, Vector3 velocity, out Vector3 normal, out bool inContact ) {
			inContact = false;
			normal = new Vector3( 0, 1, 0 );
			if ( point.X > MinX && point.X < MaxX && point.Z > MinZ && point.Z < MaxZ && point.Y < Height ) {
				float relativeVelocity = Vector3.Dot( velocity, normal );
				if ( relativeVelocity < 0.0f ) {
					return ECollideState.Colliding;
				}
			}
			return ECollideState.NotColliding;
		}

		public override ECollideState Collide( Vector3 point, Vector3 velocity, out Vector3 normal, out Vector3 intersection ) {
			normal = Vector3.Empty; // TODO
			intersection = Vector3.Empty;
			return ECollideState.NotColliding;
		}

		public override bool Pick( Vector3 rayOrigin, Vector3 rayDirection, out float distance ) {
			distance = float.MaxValue;
			return false; // TODO
		}
	}
}
