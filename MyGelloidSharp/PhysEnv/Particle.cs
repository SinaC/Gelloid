using System.Xml.Serialization;

using Microsoft.DirectX;

namespace MyGelloidSharp.PhysEnv
{
    public class CParticle
    {
        // Default particle mass
        [XmlIgnore] public const float DefaultOneOverMass = 1.0f;
        // Velocity Threshold that decides between Static and Kinetic Friction
        [XmlIgnore] public const float StaticFrictionThreshold = 0.03f;

        [XmlElement("position")] public Vector3 Position; // Position of Particle
        [XmlElement("velocity")] public Vector3 Velocity; // Velocity of Particle
        [XmlElement("force")] public Vector3 Force; // Force acting on Particle
        [XmlElement("oneOverMass")] public float OneOverMass; // 1 / Mass of Particle

        [XmlElement("inContact")] public bool InContact; // If true, particle is in contact with a collider
        [XmlElement("normalAtContact")] public Vector3 NormatAtContact; // Normal at contact point, if in contact

        [XmlElement("normal")] public Vector3 Normal; // Normal to this point, computed using triangles normal averaged sum of adjacent triangles normal

        [XmlElement("notMovable")] public bool NotMovable; // If true, particle doesn't move

        public CParticle()
        {
            Position = Vector3.Empty;
            Velocity = Vector3.Empty;
            Force = Vector3.Empty;
            OneOverMass = DefaultOneOverMass;

            InContact = false;
            NormatAtContact = Vector3.Empty;

            Normal = Vector3.Empty;

            NotMovable = false;
        }

        public void ResetForce()
        {
            // Reset force
            Force = Vector3.Empty;
        }

        public void ApplyGravity(Vector3 gravity)
        {
            // Apply Gravity (depends on mass)
            Force += (gravity*OneOverMass);
        }

        public void ApplyMouse(Vector3 mouseForce, float mouseKs)
        {
            // Apply Mouse, DOESN'T WORK FOR THE MOMENT
            Vector3 deltaP = Position - mouseForce;
            float dist = deltaP.Length();
            if (dist > 0.0f)
            {
                float hTerm = dist*mouseKs;
                deltaP *= ((1.0f/dist)*(-hTerm)); // OPTIMIZATION: no need to divide by dist and no need to multiply HTerm by dist
                Force += deltaP;
            }
        }

        public void ApplyDamping(float Kd)
        {
            // Apply Damping (depends on velocity)
            Force += (Velocity*(-Kd));
        }

        public void ApplyWind(Vector3 windDirection, float windSpeed)
        {
            // Apply Wind (depends on normal)
            Vector3 windForce = Normal*windSpeed*Vector3.Dot(Normal, windSpeed*windDirection - Velocity);
            Force += windForce;
        }

        public void ApplyFriction(float CSf, float CKf)
        {
            // Apply Friction (depends on normal at contact, if contact)
            if (InContact)
            {
                // Calculate Fn
                float FdotN = Vector3.Dot(NormatAtContact, Force);
                // Calculate Vt Velocity Tangent to Normal Plane
                float VdotN = Vector3.Dot(NormatAtContact, Velocity);
                Vector3 Vn = NormatAtContact*VdotN;
                Vector3 Vt = Velocity - Vn;
                float VMag2 = Vt.LengthSq();
                // Check if Velocity is faster then threshold
                if (VMag2 > StaticFrictionThreshold)
                {
                    // Use Kinetic Friction model
                    Vt.Normalize();
                    Vt = Vt*(FdotN*CKf);
                    Force += Vt;
                }
                else
                {
                    // Use Static Friction Model
                    VMag2 = VMag2/StaticFrictionThreshold;
                    Vt.Normalize();
                    Vt = Vt*(FdotN*CSf*VMag2); // Scale Friction by Velocity
                    Force += Vt;
                }
                InContact = false; // friction handled
            }
        }

        // Compute new position and new velocity
        public void Integrate(CParticle initial, CParticle source, float deltaTime)
        {
            if (NotMovable)
                return;
            Velocity = initial.Velocity + (source.Force*(deltaTime*initial.OneOverMass));
            Position = initial.Position + (deltaTime*source.Velocity);
        }

        public void Copy(CParticle particle)
        {
            Position = new Vector3(particle.Position.X, particle.Position.Y, particle.Position.Z);
            Velocity = new Vector3(particle.Velocity.X, particle.Velocity.Y, particle.Velocity.Z);
            Force = new Vector3(particle.Force.X, particle.Force.Y, particle.Force.Z);
            OneOverMass = particle.OneOverMass;
            Normal = new Vector3(particle.Normal.X, particle.Normal.Y, particle.Normal.Z);
            NotMovable = particle.NotMovable;
            InContact = particle.InContact;
            NormatAtContact = particle.NormatAtContact;
        }
    }
}