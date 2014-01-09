using System;

using Microsoft.DirectX;

namespace MyGelloidSharp {
	public class CIntersection {

		private CIntersection() {
			// Singleton
		}

		//  check if a ray intersects a sphere
		public static bool IntersectsSphere( Vector3 rayOrigin, Vector3 rayDirection, Vector3 center, float radius2 ) {
			Vector3 diff = center - rayOrigin;
			float t = Vector3.Dot( diff, rayDirection );
			if ( t < 0 )
				return false;
			diff -= ( rayDirection * t );
			float dist2 = diff.LengthSq();
			return dist2 <= radius2;
		}

		//  compute intersection between a ray and a sphere
		public static bool IntersectionSphere( Vector3 rayOrigin, Vector3 rayDirection, Vector3 center, float radius2, out float distance ) {
			// We have to solve
			// A t^2 + 2*B t + C = 0
			// with A = square norm of ray.dir
			//      B = ray.dir . ( ray.loc - sphere.loc )
			//  and C = square norm of ( sphere.loc - ray.loc ) - radius^2
			//                -B + (or -) sqrt( B^2 - A * C )
			// solution: t = -----------------------------------
			//                           A
			// if discriminant (B^2- A * C) is negative: no solution
			// A is always strictly positive (= 1 if ray is normalized)
			// if we could assume C > 0, then we could stop if B < 0 and always get the positive solution

			// OPTIMIZATION 1: We assume A = 1.0 (ray direction is normalized)

			Vector3 rayToCenter = center - rayOrigin;
			float B = Vector3.Dot( rayDirection, rayToCenter );
			float C = rayToCenter.LengthSq() - radius2;
			float discriminant = B*B - C;

			distance = float.MaxValue;
			if ( discriminant < 0.0f )
				return false;

			float sqrtDiscr = (float)Math.Sqrt( (float)discriminant );
			distance = B - sqrtDiscr;
			// return smallest positive solution
			if ( distance > 0.0f )
				return true;
			distance = B + sqrtDiscr;
			if ( distance > 0.0f )
				return true;
			return false;
		}

		//  check if a ray intersects a Axis Aligned Bounding Box
		public static bool IntersectsAABB( Vector3 rayOrigin, Vector3 rayDirection, float minX, float minY, float minZ, float maxX, float maxY, float maxZ ) {
			// ray direction is assumed normalized
			float tmin, tmax, tymin, tymax, tzmin, tzmax;
			if ( rayDirection.X >= 0 ) {
				tmin = ( minX - rayOrigin.X ) / rayDirection.X;
				tmax = ( maxX - rayOrigin.X ) / rayDirection.X;
			}
			else {
				tmin = ( maxX - rayOrigin.X ) / rayDirection.X;
				tmax = ( minX - rayOrigin.X ) / rayDirection.X;
			}
			if ( rayDirection.Y >= 0 ) {
				tymin = ( minY - rayOrigin.Y ) / rayDirection.Y;
				tymax = ( maxY - rayOrigin.Y ) / rayDirection.Y;
			}
			else {
				tymin = ( maxY - rayOrigin.Y ) / rayDirection.Y;
				tymax = ( minY - rayOrigin.Y ) / rayDirection.Y;
			}
			if ( ( tmin > tymax ) || ( tymin > tmax ) )
				return false;
			if ( tymin > tmin )
				tmin = tymin;
			if ( tymax < tmax )
				tmax = tymax;
			if ( rayDirection.Z >= 0 ) {
				tzmin = ( minZ - rayOrigin.Z ) / rayDirection.Z;
				tzmax = ( maxZ - rayOrigin.Z ) / rayDirection.Z;
			}
			else {
				tzmin = ( maxZ - rayOrigin.Z ) / rayDirection.Z;
				tzmax = ( minZ - rayOrigin.Z ) / rayDirection.Z;
			}
			if ( ( tmin > tzmax ) || ( tzmin > tmax ) )
				return false;
			if ( tzmin > tmin )
				tmin = tzmin;
			if ( tzmax > tmax )
				tmax = tzmax;
			// return tmin > t0 && tmax < t1      // test distance range [t0, t1]
			return tmin > 0;
		}

		//  compute intersection between a ray and a capped cylinder
		//   generate orthonormal basis
		public static void GenerateOrthonormalBasis( Vector3 W, out Vector3 U, out Vector3 V ) {
			if ( Math.Abs( W.X ) >= Math.Abs( W.Y ) ) {
				// W.x or W.z is the largest magnitude component, swap them
				float invL = (float)(1.0 / Math.Sqrt(W.X*W.X+W.Z*W.Z));
				U = new Vector3( -W.Z * invL, 0.0f, +W.X * invL );
			}
			else {
				// W.t or W.z is the largest magnitude component, swap them
				float invL = (float)(1.0 / Math.Sqrt(W.Y*W.Y+W.Z*W.Z));
				U = new Vector3( 0.0f, +W.Z * invL, -W.Y * invL );
			}
			V = Vector3.Cross( W, U );
		}
		/*
inline void generateOrthonormalBasis( TVector3 &U, TVector3 &V, TVector3 &W, const bool unitLengthW ) {
  if ( !unitLengthW )
	W.normalize();
  if ( fabsf(W[0]) >= fabsf(W[1]) ) {
	// W.x or W.z is the largest magnitude component, swap them
	float invLength = fastInvSqrt(W[0]*W[0]+W[2]*W[2]);
	U[0] = -W[2] * invLength;
	U[1] = 0.0f;
	U[2] = +W[0] * invLength;
  }
  else {
	// W.y or W.z is the largest magnitude component, swap them
	float invLength = fastInvSqrt(W[1]*W[1]+W[2]*W[2]);
	U[0] = 0.0f;
	U[1] = +W[2] * invLength;
	U[2] = -W[1] * invLength;
  }
  V = crossProduct( W, U );
}
			 */
		private static bool IntersectionCappedCylinder( Vector3 rayOrigin, Vector3 rayDirection,
			Vector3 center, Vector3 axis, float height, float radius2, out Vector3 dist ) {
			dist = Vector3.Empty;
			return false;
			/*
			U = axis; V = axis; W = axis;
			generateOrthonormalBasis( U, V, W, true );
			*/
			/*
  // set up quadratic Q(t) = a*t^2 + 2*b*t + c
  TVector3 D( U | ray.direction, V | ray.direction, W | ray.direction );
  //-- DLength is always equal to 1 because U, V, W and ray.direction are normalized
  //--  float DLength = D.magnitude(); // OPTIMIZE: is this always equal to 1 ?
  //--  float invDLength = 1/DLength;
  //--  D *= invDLength;

  TVector3 Diff = ray.origin - center; // OPTIMIZE for camera rays
  TVector3 P( U | Diff, V | Diff, W | Diff ); // OPTIMIZE for camera rays
  float halfHeight = 0.5f * height;
  float inv, A, B, C, Discr, Root, T, T0, T1, Tmp0, Tmp1;
  int iCount = 0;
  
  if ( fabsf(D[2]) >= 1.0f - EPS ) {
#ifdef __DEBUG__
	  printf("parallel to cylinder axis\n");
#endif
	// line is parallel to cylinder axis
	if ( P[0]*P[0] + P[1] * P[1] <= radius2 ) {
	  //--Tmp0 = invDLength / D[2];
	  Tmp0 = 1.0f / D[2];
	  IStack.push( (+halfHeight - P[2])*Tmp0, INTERSECTION_TOP_CAP ); iCount++;
	  IStack.push( (-halfHeight - P[2])*Tmp0, INTERSECTION_BOTTOM_CAP ); iCount++;
#ifdef __DEBUG__
	  printf("intersection found\n");
#endif
	  return true;
	}
	else {
#ifdef __DEBUG__
	  printf("parallel to axis but no intersection with caps\n");
#endif
	  return false;
	}
  }

  if ( fabsf(D[2]) <= EPS ) {
#ifdef __DEBUG__
	printf("perpendicular to cylinder axis\n");
#endif
	// line is perpendicular to axis of cylinder
	if ( fabsf(P[2]) > halfHeight ) {
	  // line is outside the planar caps of cylinder
#ifdef __DEBUG__
	  printf("outside plane caps\n");
#endif
	  return false;
	}
#ifdef __DEBUG__
	  printf("inside planar caps\n");
#endif
	A = D[0]*D[0] + D[1]*D[1];
	B = P[0]*D[0] + P[1]*D[1];
	C = P[0]*P[0] + P[1]*P[1] - radius2;
	Discr = B*B - A*C;
	if ( Discr < 0.0f ) {
	  // line does not intersect cylinder wall
#ifdef __DEBUG__
	  printf("no intersection with cylinder wall\n");
#endif
	  return false;
	}
	else if ( Discr > 0.0f ) {
#ifdef __DEBUG__
	  printf("intersect cylinder wall  2 inter\n");
#endif
	  Root = sqrtf(Discr);
	  //--Tmp0 = invDLength / A;
	  Tmp0 = 1.0f / A;
	  IStack.push( ( -B - Root ) * Tmp0, INTERSECTION_CYLINDER_WALL ); iCount++;
	  IStack.push( ( -B + Root ) * Tmp0, INTERSECTION_CYLINDER_WALL ); iCount++;
	  return true;
	}
	else {
#ifdef __DEBUG__
	  printf("intersect cylinder wall  1 inter\n");
#endif
	  //--intersections[0] = -B * invDLength / A;
	  IStack.push( -B / A,  INTERSECTION_CYLINDER_WALL ); iCount++;
	  return true;
	}
  }
  // test plane intersections first
#ifdef __DEBUG__
  printf("test plane intersection\n");
#endif
  inv = 1.0f / D[2];

  T0 = (+halfHeight - P[2]) * inv;
  Tmp0 = P[0] + T0 * D[0];
  Tmp1 = P[1] + T0 * D[1];
  if ( Tmp0*Tmp0 + Tmp1*Tmp1 <= radius2 ) {
	IStack.push( T0, INTERSECTION_TOP_CAP ); iCount++;
  }

  T1 = (-halfHeight - P[2]) * inv;
  Tmp0 = P[0] + T1 * D[0];
  Tmp1 = P[1] + T1 * D[1];
  if ( Tmp0*Tmp0 + Tmp1*Tmp1 <= radius2 ) {
	IStack.push( T1, INTERSECTION_BOTTOM_CAP ); iCount++;
  }

  if ( iCount == 2 ) {
#ifdef __DEBUG__
  printf("intersect top & bottom\n");
#endif
	// line intersects both top and bottom
	return true;
  }
  // If only 1 intersection, then line must intersect cylinder wall
  // somewhere between caps in a single point.  This case is detected
  // in the following code that tests for intersection between line and
  // cylinder wall.
  
  A = D[0]*D[0] + D[1]*D[1];
  B = P[0]*D[0] + P[1]*D[1];
  C = P[0]*P[0] + P[1]*P[1] - radius2;
  Discr = B*B - A*C;
  if ( Discr < 0.0f ) {
	// line does not intersect cylinder wall
#ifdef __DEBUG__
  printf("no cylinder wall intersection\n");
#endif
	//--assert( iQuantity == 0 ); TODO
	return false;
  }
  else if ( Discr > 0.0f ) {
#ifdef __DEBUG__
  printf("intersect cylinder wall  2 inter\n");
#endif
	Root = sqrtf(Discr);
	inv = 1.0f / A;
	T = ( -B - Root ) * inv;
#ifdef __DEBUG__
	printf("negative root: %5.5f   t0: %5.5f  t1: %5.5f\n", T, T0, T1 );
#endif
	if ( T0 <= T1 ) {
	  if ( T0 <= T && T <= T1 ) {
	IStack.push( T, INTERSECTION_CYLINDER_WALL ); iCount++;
	  }
	}
	else {
	  if ( T1 <= T && T <= T0 ) {
	IStack.push( T, INTERSECTION_CYLINDER_WALL ); iCount++;
	  }
	}
	if ( iCount == 2 ) {
	  // Line intersects one of top/bottom of cylinder and once on
	  // cylinder wall.
#ifdef __DEBUG__
	  printf("intersect one top/bottom and cylinder wall\n");
#endif
	  return true;
	}
	T = ( -B + Root ) * inv;
#ifdef __DEBUG__
	printf("positive root: %5.5f   t0: %5.5f  t1: %5.5f\n", T, T0, T1 );
#endif
	if ( T0 <= T1 ) {
	  if ( T0 <= T && T <= T1 ) {
	IStack.push( T, INTERSECTION_CYLINDER_WALL ); iCount++;
	  }
	}
	else {
	  if ( T1 <= T && T <= T0 ) {
	IStack.push( T, INTERSECTION_CYLINDER_WALL ); iCount++;
	  }
	}
  }
  else {
#ifdef __DEBUG__
  printf("intersect cylinder wall  1 inter\n");
#endif
	T = -B / A;
	if ( T0 <= T1 ) {
	  if ( T0 <= T && T <= T1 ) {
	IStack.push( T, INTERSECTION_CYLINDER_WALL ); iCount++;
	  }
	}
	else {
	  if ( T1 <= T && T <= T0 ) {
	IStack.push( T, INTERSECTION_CYLINDER_WALL ); iCount++;
	  }
	}
  }
  return iCount > 0;
			 */
		}
	}
}