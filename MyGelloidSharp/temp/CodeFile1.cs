using System;
using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace MyGelloidSharp.test {
	// Shopping list class which will be serialized
	[XmlRoot("shoppingList")]
	public class ShoppingList {
		private ArrayList listShopping;

		public ShoppingList() {
			listShopping = new ArrayList();
		}

		[XmlElement("item")]
		public myItem[] Items {
			get {
				myItem[] items = new myItem[ listShopping.Count ];
				listShopping.CopyTo( items );
				return items;
			}
			set {
				if( value == null ) return;
				myItem[] items = (myItem[])value;
				listShopping.Clear();
				foreach( myItem item in items )
					listShopping.Add( item );
			}
		}

		public int AddItem( myItem item ) {
			return listShopping.Add( item );
		}
	}

	// Items in the shopping list
	public class myItem {
		[XmlAttribute("name")] public string name;
		[XmlAttribute("price")] public double price;
		public Vector3 pos;

		public myItem() {
		}

		public myItem( string Name, double Price, Vector3 Pos ) {
			name = Name;
			price = Price;
			pos = Pos;
		}
	}
}