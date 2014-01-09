using Microsoft.DirectX;

namespace MyGelloidSharp {
	/// <summary>
	/// Summary description for Utils.
	/// </summary>
	public class CUtils {
		private CUtils() {
			// Singleton
		}

		public static string Vector3ToStr( Vector3 v ) {
			return "Vector3<"+v.X+";"+v.Y+";"+v.Z+">";
		}

		public static float Range( float f, float min, float max ) {
			return ( f < min ) ? min : ( ( f > max ) ? max : f );
		}
	}
}
