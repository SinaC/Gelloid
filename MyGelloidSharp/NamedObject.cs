using System.Xml.Serialization;

namespace MyGelloidSharp {
	public class CNamedObject {
		[XmlElement("Name")] public string Name = "not specified";

		public CNamedObject() {
		}

		public CNamedObject( string name ) {
			this.Name = name;
		}
	}
}
