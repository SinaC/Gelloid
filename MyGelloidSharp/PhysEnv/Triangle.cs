using System.Collections.Generic;
using System.Xml.Serialization;

using Microsoft.DirectX;

namespace MyGelloidSharp.PhysEnv {
	public class CTriangle {
		public class CVertex {
			[XmlElement("p")] public int P; // Indices in particle collection
			[XmlElement("Tu")] public float Tu; // Texture coordinates
			[XmlElement("Tv")] public float Tv;

			public CVertex() {
				P = 0;
				Tu = Tv = 0.0f;
			}
		}

		[XmlElement("Vertex")] public CVertex[] Vertices; // Vertices of the triangle
		[XmlIgnore] public Vector3 Normal; // Normal to the triangle

		public CTriangle() {
			Vertices = new CVertex[3];
			Vertices[0] = new CVertex();
			Vertices[1] = new CVertex();
			Vertices[2] = new CVertex();
			Normal = Vector3.Empty;
		}

        public void ComputeNormal(List<CParticle> particles)
        {
			CParticle p1 = particles[Vertices[0].P];
			CParticle p2 = particles[Vertices[1].P];
			CParticle p3 = particles[Vertices[2].P];
			Vector3 v1 = p2.Position - p1.Position;
			Vector3 v2 = p3.Position - p1.Position;
			Normal = Vector3.Cross( v1, v2 );
			Normal.Normalize();
		}
	}
}
