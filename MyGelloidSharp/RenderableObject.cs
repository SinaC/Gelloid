using System.Xml.Serialization;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace MyGelloidSharp {
	public abstract class CRenderableObject: CNamedObject {
		[XmlIgnore] protected Device Device;
		[XmlIgnore] protected CFont Font;

	    protected CRenderableObject() {
		}

	    protected CRenderableObject( string name ) : base( name ) {
		}

		public virtual void InitializeGraphics( Device device, CFont font ) {
			Device = device;
			Font = font;
		}
		public virtual void Render() {
		}

		public abstract bool Pick( Vector3 rayOrigin, Vector3 rayDirection, out float distance );

	    [XmlIgnore]
	    public Material Material { get; set; }
	}
}