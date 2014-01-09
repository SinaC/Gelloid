using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.DirectX;

namespace MyGelloidSharp.Tesselation {
	
	public class CTesselatedSphere {
		public enum ESubdivisionKind { EdgeToEdge, VertexToEdge, Barycentric };

		public class CTriangle {
			public int P1, P2, P3; // Indices in points list

			public CTriangle( int p1, int p2, int p3 ) {
				P1 = p1;
				P2 = p2;
				P3 = p3;
			}
		}

		private class CMidPoint {
			public readonly int P1; // p1 < p2
		    public readonly int P2; // p1 < p2
		    public readonly int P; // index of middle of p1 and p2

			public CMidPoint( int p1, int p2, int p ) {
				P1 = p1;
				P2 = p2;
				P = p;
			}
		}

		public List<Vector3> Points;
		public ArrayList Triangles;

		public CTesselatedSphere() {
            Points = new List<Vector3>();
			Triangles = new ArrayList();
		}

		// Search in midList if middle of p1 and p2 already exists
		private static bool MidExists( ArrayList midList, int p1, int p2, out int mid ) {
			if ( p1 > p2 ) { // p1 must be < p2
				int tmp = p1;
				p1 = p2;
				p2 = tmp;
			}
			mid = 0;
			foreach ( CMidPoint midPoint in midList )
				if ( p1 == midPoint.P1 && p2 == midPoint.P2 ) {
					mid = midPoint.P;
					return true;
				}
			return false;
		}

		private static CTesselatedSphere Subdivide( CTesselatedSphere inSphere, ESubdivisionKind kind ) {
			CTesselatedSphere outSphere = new CTesselatedSphere();

			// Normalisation, gives points on the unit sphere

			// Copy previous points
			outSphere.Points = new List<Vector3>( inSphere.Points );
			// Split triangles
			ArrayList midList = new ArrayList();
			foreach ( CTriangle triangle in inSphere.Triangles ) {
				Vector3 p1 = outSphere.Points[triangle.P1];
				Vector3 p2 = outSphere.Points[triangle.P2];
				Vector3 p3 = outSphere.Points[triangle.P3];
				if ( kind == ESubdivisionKind.Barycentric ) {
					// Compute gravity center and create 3 triangles with vertices and gravity center
					Vector3 center = Vector3.Normalize( ( p1 + p2 + p3 ) * ( 1.0f / 3.0f ) );
				    outSphere.Points.Add(center);
					int c = outSphere.Points.Count-1;
					outSphere.Triangles.Add( new CTriangle( triangle.P1, triangle.P2, c ) );
					outSphere.Triangles.Add( new CTriangle( triangle.P2, triangle.P3, c ) );
					outSphere.Triangles.Add( new CTriangle( triangle.P3, triangle.P1, c ) );
				}
				else {
					// Search if mid point already exists, if it doesn't exist, create it
					int midId12;
					if ( !MidExists( midList, triangle.P1, triangle.P2, out midId12 ) ) {
						// compute mid points
						Vector3 mid = Vector3.Normalize( ( p1 + p2 ) * 0.5f );
					    outSphere.Points.Add(mid);
                        midId12 = outSphere.Points.Count - 1;
						midList.Add( new CMidPoint( triangle.P1, triangle.P2, midId12 ) );
					}
					int midId13;
					if ( !MidExists( midList, triangle.P1, triangle.P3, out midId13 ) ) {
						// compute mid points
						Vector3 mid = Vector3.Normalize( ( p1 + p3 ) * 0.5f );
					    outSphere.Points.Add(mid);
                        midId13 = outSphere.Points.Count - 1;
						midList.Add( new CMidPoint( triangle.P1, triangle.P3, midId13 ) );
					}
					int midId23;
					if ( !MidExists( midList, triangle.P2, triangle.P3, out midId23 ) ) {
						// compute mid points
						Vector3 mid = Vector3.Normalize( ( p2 + p3 ) * 0.5f );
					    outSphere.Points.Add(mid);
                        midId23 = outSphere.Points.Count - 1;
						midList.Add( new CMidPoint( triangle.P2, triangle.P3, midId23 ) );
					}
					// Create new triangles
					if ( kind == ESubdivisionKind.EdgeToEdge ) {
						//       /\
						//      /  \
						//     /____\
						//    /\    /\
						//   /  \  /  \
						//  /____\/____\
						outSphere.Triangles.Add( new CTriangle( triangle.P1, midId12, midId13 ) );
						outSphere.Triangles.Add( new CTriangle( midId12, triangle.P2, midId23 ) );
						outSphere.Triangles.Add( new CTriangle( midId13, midId12, midId23 ) );
						outSphere.Triangles.Add( new CTriangle( midId13, midId23, triangle.P3 ) );
					}
					else if ( kind == ESubdivisionKind.VertexToEdge ) {
						// Compute gravity center and create 6 triangles with vertices, middle edges and gravity center
						Vector3 center = Vector3.Normalize( ( p1 + p2 + p3 ) * ( 1.0f / 3.0f ) );
					    outSphere.Points.Add(center);
                        int c = outSphere.Points.Count - 1;
						outSphere.Triangles.Add( new CTriangle( triangle.P1, midId12, c ) );
						outSphere.Triangles.Add( new CTriangle( midId12, triangle.P2, c ) );
						outSphere.Triangles.Add( new CTriangle( triangle.P2, midId23, c ) );
						outSphere.Triangles.Add( new CTriangle( midId23, triangle.P3, c ) );
						outSphere.Triangles.Add( new CTriangle( triangle.P3, midId13, c ) );
						outSphere.Triangles.Add( new CTriangle( midId13, triangle.P1, c ) );
					}
				}
			}
			return outSphere;
		}

		public static CTesselatedSphere CreateFromOctahedron( float radius, int steps, ESubdivisionKind kind ) {
			CTesselatedSphere sphere = new CTesselatedSphere();

			// Points
			// 0: X-
			// 1: X+
			// 2: Y-
			// 3: Y+
			// 4: Z-
			// 5: Z+
			sphere.Points.Add( new Vector3( -1, 0, 0 ) );
			sphere.Points.Add( new Vector3( +1, 0, 0 ) );
			sphere.Points.Add( new Vector3( 0, -1, 0 ) );
			sphere.Points.Add( new Vector3( 0, +1, 0 ) );
			sphere.Points.Add( new Vector3( 0, 0, -1 ) );
			sphere.Points.Add( new Vector3( 0, 0, +1 ) );

			//			// Triangles
			//			// X+, Z+, Y+
			//			// Y+, Z+, X-
			//			// X-, Z+, Y-
			//			// Y-, Z+, X+
			//			// X+, Y+, Z-
			//			// Y+, X-, Z-
			//			// X-, Y-, Z-
			//			// Y-, X+, Z-
			sphere.Triangles.Add( new CTriangle( 1, 5, 3 ) );
			sphere.Triangles.Add( new CTriangle( 3, 5, 0 ) );
			sphere.Triangles.Add( new CTriangle( 0, 5, 2 ) );
			sphere.Triangles.Add( new CTriangle( 2, 5, 1 ) );
			sphere.Triangles.Add( new CTriangle( 1, 3, 4 ) );
			sphere.Triangles.Add( new CTriangle( 3, 0, 4 ) );
			sphere.Triangles.Add( new CTriangle( 0, 4, 2 ) );
			sphere.Triangles.Add( new CTriangle( 2, 1, 4 ) );

			// Subdivide
			for ( int i = 1; i < steps; i++ )
				sphere = Subdivide( sphere, kind );

			// Apply radius
			for ( int i = 0; i < sphere.Points.Count; i++ )
				sphere.Points[i] = sphere.Points[i] * radius;

			return sphere;
		}

		public static CTesselatedSphere CreateFromTetrahedron( float radius, int steps, ESubdivisionKind kind ) {
			CTesselatedSphere sphere = new CTesselatedSphere();

			// Points
			float sqrt3 = (float)Math.Sqrt(3);
			// 0: +++  PPP
			// 1: --+  MMP
			// 2: -+-  MPM
			// 3: +--  PMM
			sphere.Points.Add( Vector3.Normalize( new Vector3( sqrt3, sqrt3, sqrt3 ) ) );
			sphere.Points.Add( Vector3.Normalize( new Vector3( -sqrt3, -sqrt3, sqrt3 ) ) );
			sphere.Points.Add( Vector3.Normalize( new Vector3( -sqrt3, sqrt3, -sqrt3 ) ) );
			sphere.Points.Add( Vector3.Normalize( new Vector3( sqrt3, -sqrt3, -sqrt3 ) ) );

			// Triangles
			// PPP, MMP, MPM
			// PPP, PMM, MMP
			// MPM, MMP, PMM
			// PMM, PPP, MPM
			sphere.Triangles.Add( new CTriangle( 0, 1, 2 ) );
			sphere.Triangles.Add( new CTriangle( 0, 3, 1 ) );
			sphere.Triangles.Add( new CTriangle( 2, 1, 3 ) );
			sphere.Triangles.Add( new CTriangle( 3, 0, 2 ) );
			
			// Subdivide
			for ( int i = 1; i < steps; i++ )
				sphere = Subdivide( sphere, kind );

			// Apply radius
			for ( int i = 0; i < sphere.Points.Count; i++ )
				sphere.Points[i] = sphere.Points[i] * radius;

			return sphere;
		}

		public static CTesselatedSphere CreateFromIcosahedron( float radius, int steps, ESubdivisionKind kind ) {
			CTesselatedSphere sphere = new CTesselatedSphere();

			// Points
			float t = ( 1.0f + (float)Math.Sqrt( 5.0 ) ) / 2.0f;
			float tau = t / (float)Math.Sqrt( 1.0 + t*t ); 
			float one = 1.0f / (float)Math.Sqrt(1.0 + t*t );

			// Triangles
			// 0: ZA(tau,one,0)
			// 1: ZB(-tau,one,0)
			// 2: ZC(-tau,-one,0)
			// 3: ZD(tau,-one,0)
			// 4: YA(one,0,tau)
			// 5: YB(one,0,-tau)
			// 6: YC(-one,0,-tau)
			// 7: YD(-one,0,tau)
			// 8: XA(0,tau,one)
			// 9: XB(0,-tau,one)
			//10: XC(0,-tau,-one)
			//11: XD(0,tau,-one)
			sphere.Points.Add( Vector3.Normalize( new Vector3( tau, one, 0 ) ) );
			sphere.Points.Add( Vector3.Normalize( new Vector3( -tau, one, 0 ) ) );
			sphere.Points.Add( Vector3.Normalize( new Vector3( -tau, -one, 0 ) ) );
			sphere.Points.Add( Vector3.Normalize( new Vector3( tau, -one, 0 ) ) );
			sphere.Points.Add( Vector3.Normalize( new Vector3( one, 0, tau ) ) );
			sphere.Points.Add( Vector3.Normalize( new Vector3( one, 0, -tau ) ) );
			sphere.Points.Add( Vector3.Normalize( new Vector3( -one, 0, -tau ) ) );
			sphere.Points.Add( Vector3.Normalize( new Vector3( -one, 0, tau ) ) );
			sphere.Points.Add( Vector3.Normalize( new Vector3( 0, tau, one ) ) );
			sphere.Points.Add( Vector3.Normalize( new Vector3( 0, -tau, one ) ) );
			sphere.Points.Add( Vector3.Normalize( new Vector3( 0, -tau, -one ) ));
			sphere.Points.Add( Vector3.Normalize( new Vector3( 0, tau, -one ) ) );

			// Triangles
			// YA, XA, YD
			// YA, YD, XB
			// YB, YC, XD
			// YB, XC, YC

			// ZA, YA, ZD
			// ZA, ZD, YB
			// ZC, YD, ZB
			// ZC, ZB, YC

			// XA, ZA, XD
			// XA, XD, ZB
			// XB, XC, ZD
			// XB, ZC, XC

			// XA, YA, ZA
			// XD, ZA, YB
			// YA, XB, ZD
			// YB, ZD, XC

			// YD, XA, ZB
			// YC, ZB, XD
			// YD, ZC, XB
			// YC, XC, ZC
			sphere.Triangles.Add( new CTriangle( 4, 8, 7 ) );
			sphere.Triangles.Add( new CTriangle( 4, 7, 9 ) );
			sphere.Triangles.Add( new CTriangle( 5, 6, 11 ) );
			sphere.Triangles.Add( new CTriangle( 5, 10, 6 ) );

			sphere.Triangles.Add( new CTriangle( 0, 4, 3 ) );
			sphere.Triangles.Add( new CTriangle( 0, 3, 5 ) );
			sphere.Triangles.Add( new CTriangle( 2, 7, 1 ) );
			sphere.Triangles.Add( new CTriangle( 2, 1, 6 ) );

			sphere.Triangles.Add( new CTriangle( 8, 0, 11 ) );
			sphere.Triangles.Add( new CTriangle( 8, 11, 1 ) );
			sphere.Triangles.Add( new CTriangle( 9, 10, 3 ) );
			sphere.Triangles.Add( new CTriangle( 9, 2, 10 ) );

			sphere.Triangles.Add( new CTriangle( 8, 4, 0 ) );
			sphere.Triangles.Add( new CTriangle( 11, 0, 5 ) );
			sphere.Triangles.Add( new CTriangle( 4, 9, 3 ) );
			sphere.Triangles.Add( new CTriangle( 5, 3, 10 ) );

			sphere.Triangles.Add( new CTriangle( 7, 8, 1 ) );
			sphere.Triangles.Add( new CTriangle( 6, 1, 11 ) );
			sphere.Triangles.Add( new CTriangle( 7, 2, 9 ) );
			sphere.Triangles.Add( new CTriangle( 6, 10, 2 ) );

			// Subdivide
			for ( int i = 1; i < steps; i++ )
				sphere = Subdivide( sphere, kind );

			// Apply radius
			for ( int i = 0; i < sphere.Points.Count; i++ )
				sphere.Points[i] = sphere.Points[i] * radius;

			return sphere;
		}
	}
}
