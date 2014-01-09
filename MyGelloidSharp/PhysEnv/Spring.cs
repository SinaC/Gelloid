using System.Collections.Generic;
using System.Xml.Serialization;

using Microsoft.DirectX;

namespace MyGelloidSharp.PhysEnv {
	public class CSpring {
		[XmlIgnore] public const float DefaultKs = 5.0f; // Hook's Spring constant
		[XmlIgnore] public const float DefaultKd = 0.1f; // Spring Damping constant

		public enum ESpringKind { Structural, Shear, Bend, Manual };

		[XmlElement("p1")] public int P1Index; // Particles bonded by Spring (indices in a particle collection)
		[XmlElement("p2")] public int P2Index;
		[XmlElement("restLength")] public float RestLength; // Length of Spring at rest
		[XmlElement("Ks")] public float Ks; // Spring constant
		[XmlElement("Kd")] public float Kd; // Spring damping
		[XmlElement("maxExtension")] public float MaxExtension; // Maximal extension: current/rest must be < maxExtension, otherwise -> broken
		[XmlElement("broken")] public bool Broken; // Is spring broken
		[XmlElement("kind")] public ESpringKind Kind; // Spring kind

		public CSpring() {
			P1Index = P2Index = 0;
			RestLength = 1.0f;
			Ks = DefaultKs;
			Kd = DefaultKd;
			MaxExtension = 100.0f;
			Broken = false;
			Kind = ESpringKind.Structural;
		}

		public void Apply( List<CParticle> particles ) {
			if ( !Broken ) {
				CParticle p1 = particles[P1Index];
				CParticle p2 = particles[P2Index];
				Vector3 deltaP = p1.Position - p2.Position;
				float dist = deltaP.Length();

				float extension = dist - RestLength;
				if ( extension / RestLength > MaxExtension ) // If maximum extension is reached, we broke the spring
					Broken = true;
				else {
					float hTerm = extension * Ks;

					Vector3 deltaV = p1.Velocity - p2.Velocity;
					float dTerm = ( Vector3.Dot( deltaV, deltaP ) * Kd ) / dist;
	
					Vector3 springForce = deltaP * ( 1.0f / dist );
					springForce = springForce * (-(hTerm + dTerm));

					if ( !p1.NotMovable )
						p1.Force = p1.Force + springForce;
					if ( !p2.NotMovable )
						p2.Force = p2.Force - springForce;
				}
			}
		}
	}
}