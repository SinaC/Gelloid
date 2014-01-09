using System;
using Microsoft.DirectX;

namespace MyGelloidSharp.test {
	/// <summary>
	/// Summary description for Gelly.
	/// </summary>
	public class CGelly {
		public class _CBond {
			public _CParticle Bond; // Bond particule
			public float Length; // Bond length
			public int Color; // Bond Color

			public override string ToString() {
				return "Bond<Length="+Length+" Color:"+Color+">";
			}
		}
		public class _CParticle {
			public static int MAX_BONDS = 32;

			public Vector3 Position; // Position
			public Vector3 Velocity; // Velocity
			public Vector3 Force; // Force
			public float OneOverMass; // 1/Mass
			public int NbBonds; // Number of bonds to other Particule
			public _CBond[] Bonds; // Bonds to other Particules

			public _CParticle() {
				Position = new Vector3( 0, 0, 0 );
				Velocity = new Vector3( 0, 0, 0 );
				Force = new Vector3( 0, 0, 0 );
				OneOverMass = 1.0f;
				NbBonds = 0;
				Bonds = new _CBond[MAX_BONDS];
				for ( int i = 0; i < MAX_BONDS; i++ )
					Bonds[i] = new _CBond();
			}

			public override string ToString() {
				string bondsStr = "";
				foreach ( _CBond bond in Bonds )
					bondsStr += bond;
				return "Particle<Position="+Position+" Velocity="+Velocity+" Bonds["+NbBonds+"]="+bondsStr+">";
			}
		}

		public static float GELLY_DEF_STIFF = 0.2f;
		public static float GELLY_DEF_DAMPING = 0.999f;
		public static float GELLY_MIN_EXTENSION = 0.005f;
		public int NbParticles;
		public _CParticle[] Particles;
		float Stiffness;
		float Damping;

		public CGelly( int numParts ) {
			Stiffness = GELLY_DEF_STIFF; // Default stiffness of 0.2
			Damping = GELLY_DEF_DAMPING; // Default damping of .999

			NbParticles = numParts;
			Particles = new _CParticle[ NbParticles ];
			for ( int i = 0; i < NbParticles; i++ )
				Particles[i] = new _CParticle();
		}

		public override string ToString() {
			string particlesStr = "";
			foreach ( _CParticle particle in Particles )
				particlesStr += particle;
			return "Gelloid<Stiffness="+Stiffness+" Damping="+Damping+" Particles["+NbParticles+"]="+particlesStr+">";
		}


		// Set the stiffness of the gelly
		public void SetStiffness( float stiffness ) {
			Stiffness = GELLY_DEF_STIFF * stiffness;
		}

		// Set the position of a particle
		public void SetParticule( int idx, Vector3 pos ) {
			Particles[ idx ].Position = pos;
		}

		// Create a bond between 2 particles
		// Connect p1 to p2 with a bond of length BondLength
		public void Connect( int p1, int p2, float BondLength, int color ) {
			int nb = Particles[ p1 ].NbBonds;
			if ( nb < _CParticle.MAX_BONDS / 2 ) {
				Particles[ p1 ].Bonds[ nb ].Bond = Particles[ p2 ];
				Particles[ p1 ].Bonds[ nb ].Length = BondLength;
				Particles[ p1 ].Bonds[ nb ].Color = color;
				Particles[ p1 ].NbBonds++;
			}
		}
		public void Connect( int p1, int p2, int color ) {
			int nb = Particles[ p1 ].NbBonds;
			if ( nb < _CParticle.MAX_BONDS / 2 ) {
				Particles[ p1 ].Bonds[ nb ].Bond = Particles[ p2 ];
				Particles[ p1 ].Bonds[ nb ].Length = ( Particles[p1].Position - Particles[p2].Position ).Length();
				Particles[ p1 ].Bonds[ nb ].Color = color;
				Particles[ p1 ].NbBonds++;
			}
		}

		// Rotate entire gelly
		public void Rotate( Vector3 axis, float angle ) {
			for ( int i = 0; i < NbParticles; i++ )
				Particles[i].Position.TransformNormal( Matrix.RotationAxis( axis, angle ) );
		}

		// Translate entire gelly
		public void Translate( Vector3 trans ) {
			for ( int i = 0; i < NbParticles; i++ )
				Particles[i].Position = Particles[i].Position + trans;
		}

		// Give the gelly some spin
		public void AddAngularMom( Vector3 axis, float spin ) {
			for ( int i = 0; i < NbParticles; i++ ) {
				Vector3 p2 = new Vector3( Particles[i].Position.X, Particles[i].Position.Y, Particles[i].Position.Z );
				p2.TransformNormal( Matrix.RotationAxis( axis, spin ) );
				Particles[i].Velocity += p2 - Particles[i].Position;
			}
		}

		// Give the gelly some velocity
		public void AddVelocity( Vector3 vel ) {
			for ( int i = 0; i < NbParticles; i++ )
				Particles[i].Velocity += vel;
		}

		// Relax the gelly
		public void Relax( Vector3 force ) {
			for ( int i = 0; i < NbParticles; i++ ) {
				_CParticle p1 = Particles[i];
				for ( int j = 0; j < Particles[i].NbBonds; j++ ) {
					_CParticle p2 = p1.Bonds[j].Bond;
					Vector3 bondVector = p2.Position - p1.Position;
					float stretchLength = bondVector.Length();
					float extension = stretchLength - p1.Bonds[j].Length;
					if ( System.Math.Abs( extension ) > GELLY_MIN_EXTENSION ) {
						float forceMag = ( extension / p1.Bonds[j].Length ) * Stiffness;
						Vector3 forceVector = bondVector * ( forceMag / stretchLength );
						p1.Velocity += forceVector;
						p2.Velocity -= forceVector;
					}
				}
			}

			for ( int i = 0; i < NbParticles; i++ ) {
				Particles[i].Velocity = ( Particles[i].Velocity * Damping ) + force;
				Particles[i].Position = Particles[i].Position + Particles[i].Velocity;
			}
		}

		// Detect if any points are below the floor, and move them to the surface
		public void CollideFloor( float fl ) {
			for ( int i = 0; i < NbParticles; i++ )
				if ( Particles[i].Position.Y < fl )
					Particles[i].Position.Y = fl;
		}

		public void CollidePlane( Vector3 normal, float dist ) {
			for ( int i = 0; i < NbParticles; i++ ) {
				_CParticle particle = Particles[i];
				float d = Vector3.Dot( particle.Position, normal );
				if ( d <= dist ) {
					if ( Vector3.Dot( normal, particle.Velocity ) < 0.0f ) {
						// Calculate Vn
						float VdotN = Vector3.Dot( normal, particle.Velocity );
						Vector3 Vn = normal * VdotN;
						// Calculate Vt
						Vector3 Vt = particle.Velocity - Vn;
						// Scale Vn by coefficient of restitution
						Vn = Vn * 0.3f;
						// Set the velocity to be the new impulse
						particle.Velocity = Vt - Vn;
						particle.Position = particle.Position + normal;
					}
				}
			}
		}

		// Detect if any points are inside the sphere, and move them to the surface
		public void CollideSphere( Vector3 center, float radius ) {
			for ( int i = 0; i < NbParticles; i++ ) {
				Vector3 forceVector = Particles[i].Position - center;
				float distance = forceVector.Length();
				if ( distance <= radius ) {
//					forceVector.normalize();
//					float relativeVelocity = forceVector | Particles[i].Velocity;
//					if ( relativeVelocity < 0.0f ) {
//						Vector3 Vn = forceVector * relativeVelocity;
//						Vector3 Vt = Particles[i].Velocity  - Vn;
//						Vn = Vn * 0.1f;
//						Particles[i].Velocity = Vt - Vn;
//						Particles[i].Position = Particles[i].Position + Particles[i].Velocity;
//					}
					Particles[i].Position = center + ( forceVector * ( radius / distance ) );
				}
			}
		}

		static private int CalcPointNum( int x, int y, int z, int xsize, int ysize, int zsize ) {
			int p = z * ysize;
			p = ( p + y ) * xsize;
			p += x;
			return p;
		}

		// Example code to create an arbitary cuboid
		// (string, cloth, or cuboid)
		static public CGelly MakeCuboid( int sizeX, int sizeY, int sizeZ, float gapX, float gapY, float gapZ ) {
			CGelly Gelloid = new CGelly( sizeX * sizeY * sizeZ );
	
			float xoff = ( sizeX - 1.0f ) * gapX / 2.0f;
			float yoff = ( sizeY - 1.0f ) * gapY / 2.0f;
			float zoff = ( sizeZ - 1.0f ) * gapZ / 2.0f;

			int p = 0;
			for ( int z = 0; z < sizeZ; z++ )
				for ( int y = 0; y < sizeY; y++ )
					for ( int x = 0; x < sizeX; x++ )
						Gelloid.SetParticule( p++, new Vector3( (float)x * gapX - xoff, (float)y * gapY - yoff, (float)z * gapZ - zoff ) );

			p=0;
			for ( int z = 0; z < sizeZ; z++ ) {
				for ( int y = 0; y < sizeY; y++ ) {
					for ( int x = 0; x < sizeX; x++ ) {

						for ( int cz = 0; cz <= 2; cz++ ) {
							for ( int cy = 0; cy <= 2; cy++ ) {
								for (int cx = 0; cx <= 2; cx++ ) {
						
									int p2 = CalcPointNum( x+cx, y+cy, z+cz, sizeX, sizeY, sizeZ );
									if ( (x+cx) < sizeX )
										if ( (y+cy) < sizeY )
											if ( (z+cz) < sizeZ )
												if ( p != p2 ) {
													int OnEdge1 = (
														(Convert.ToInt32(    (x)==0         ) << 0) |
														(Convert.ToInt32(    (y)==0         ) << 1) |
														(Convert.ToInt32(    (z)==0         ) << 2) |
														(Convert.ToInt32(    (x)==(sizeX-1) ) << 3) |
														(Convert.ToInt32(    (y)==(sizeY-1) ) << 4) |
														(Convert.ToInt32(    (z)==(sizeZ-1) ) << 5)
														);

													int OnEdge2 = (
														(Convert.ToInt32( (x+cx)==0         ) << 0) |
														(Convert.ToInt32( (y+cy)==0         ) << 1) |
														(Convert.ToInt32( (z+cz)==0         ) << 2) |
														(Convert.ToInt32( (x+cx)==(sizeX-1) ) << 3) |
														(Convert.ToInt32( (y+cy)==(sizeY-1) ) << 4) |
														(Convert.ToInt32( (z+cz)==(sizeZ-1) ) << 5)
														);

													int EdgeColor = 0;
													if ( ( cx < 2 ) && ( cy < 2 ) && ( cz < 2 ) )
														EdgeColor = _NumBits( ( OnEdge1 & OnEdge2) ) > 1 ? 1 : 0;

													//cout << "p1:" << p << "  p2:" << p2
													//		<< "  E1:" << int(OnEdge1) << "  E2:" << int(OnEdge2)
													//		<< "  Shared Bits:" << NumBits((OnEdge1 & OnEdge2))
													//		<< "  colour:" << EdgeColour << endl;

//													Gelloid.Connect(
//																	p,
//																	p2,
//																	gap*(float)System.Math.Sqrt( (float)(cx*cx + cy*cy + cz*cz) ),
//																	EdgeColor
//																	);
													Gelloid.Connect( p, p2, EdgeColor );
												}
								}
							}
						}
						p++;
					}
				}
			}
			/*
			cout << "Assigning Faces" << endl;
			FaceNum = 0;

			z=0;
			for (y=0; y<(ysize-1); y++)
			{
			 for (x=0; x<(xsize-1); x++)
			 {
				p1 = CalcPointNum(x,   y,   0, xsize, ysize, zsize);
				p2 = CalcPointNum(x+1, y,   0, xsize, ysize, zsize);
				p3 = CalcPointNum(x+1, y+1, 0, xsize, ysize, zsize);
				p4 = CalcPointNum(x,   y+1, 0, xsize, ysize, zsize);
				Gelloid.MakeFace(FaceNum++, p1, p4, p2, 1);
				Gelloid.MakeFace(FaceNum++, p2, p4, p3, 1);
				p1 = CalcPointNum(x,   y,   zsize-1, xsize, ysize, zsize);
				p2 = CalcPointNum(x+1, y,   zsize-1, xsize, ysize, zsize);
				p3 = CalcPointNum(x+1, y+1, zsize-1, xsize, ysize, zsize);
				p4 = CalcPointNum(x,   y+1, zsize-1, xsize, ysize, zsize);
				Gelloid.MakeFace(FaceNum++, p1, p2, p4, 9);
				Gelloid.MakeFace(FaceNum++, p2, p3, p4, 9);
			 }
			}

			y=0;
			for (z=0; z<(zsize-1); z++)
			{
			 for (x=0; x<(xsize-1); x++)
			 {
				p4 = CalcPointNum(x,   0, z,   xsize, ysize, zsize);
				p3 = CalcPointNum(x+1, 0, z,   xsize, ysize, zsize);
				p2 = CalcPointNum(x+1, 0, z+1, xsize, ysize, zsize);
				p1 = CalcPointNum(x,   0, z+1, xsize, ysize, zsize);
				Gelloid.MakeFace(FaceNum++, p1, p4, p2, 2);
				Gelloid.MakeFace(FaceNum++, p2, p4, p3, 2);
				p4 = CalcPointNum(x,   ysize-1, z,   xsize, ysize, zsize);
				p3 = CalcPointNum(x+1, ysize-1, z,   xsize, ysize, zsize);
				p2 = CalcPointNum(x+1, ysize-1, z+1, xsize, ysize, zsize);
				p1 = CalcPointNum(x,   ysize-1, z+1, xsize, ysize, zsize);
				Gelloid.MakeFace(FaceNum++, p1, p2, p4, 10);
				Gelloid.MakeFace(FaceNum++, p2, p3, p4, 10);
			 }
			}

			y=0;
			for (z=0; z<(zsize-1); z++)
			{
			 for (y=0; y<(ysize-1); y++)
			 {
				p1 = CalcPointNum(0, y,   z,   xsize, ysize, zsize);
				p2 = CalcPointNum(0, y+1, z,   xsize, ysize, zsize);
				p3 = CalcPointNum(0, y+1, z+1, xsize, ysize, zsize);
				p4 = CalcPointNum(0, y,   z+1, xsize, ysize, zsize);
				Gelloid.MakeFace(FaceNum++, p1, p4, p2, 4);
				Gelloid.MakeFace(FaceNum++, p2, p4, p3, 4);
				p1 = CalcPointNum(xsize-1, y,   z,   xsize, ysize, zsize);
				p2 = CalcPointNum(xsize-1, y+1, z,   xsize, ysize, zsize);
				p3 = CalcPointNum(xsize-1, y+1, z+1, xsize, ysize, zsize);
				p4 = CalcPointNum(xsize-1, y,   z+1, xsize, ysize, zsize);
				Gelloid.MakeFace(FaceNum++, p1, p2, p4, 12);
				Gelloid.MakeFace(FaceNum++, p2, p3, p4, 12);
			 }
			}
			*/
			return Gelloid;
		}

		// Counts the number of bits in an integer
		static private int _NumBits( int b ) {
			int bit = 32768;
			int acc = 0;

			while ( bit > 0 ) {
				if ( ( bit & b ) > 0 )
					acc++;
				bit >>= 1;
			}
			return acc;
		}
	}
}
