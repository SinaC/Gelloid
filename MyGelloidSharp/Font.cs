using System;
using System.Drawing;

using Microsoft.DirectX.Direct3D;

namespace MyGelloidSharp {
	public class CFont {
		//{A} System Font
	    readonly System.Drawing.Font _sysFont;
		//Direct3D Font
	    readonly Microsoft.DirectX.Direct3D.Font _d3DFont;

		//{B} CFont constructor
		public CFont(Device d3DDevice, System.Drawing.Font sysFont) {
			_sysFont = sysFont;
            
			_d3DFont = new Microsoft.DirectX.Direct3D.Font(
				//Reference to the Direct3D device
				d3DDevice, 
				//A system font 
				sysFont
				);

		}

		//{C} Text measurement
		public Size MeasureString(string text) {
			Graphics gfx = Graphics.FromHwnd((IntPtr)0);

			SizeF sz = gfx.MeasureString(text, _sysFont);

			gfx.Dispose();
            
			return sz.ToSize();
		}

		//{D} Text drawing
		public void DrawText(string text, int x, int y, Color color, bool center) {
			Size sz = MeasureString(text);
			if (center) {
				x -= sz.Width/2;
				y -= sz.Height/2;
			}
			//DX9.0c : new sprite argument that can be null
			_d3DFont.DrawText(null, text, new Rectangle(x, y, x + sz.Width, y + sz.Height), DrawTextFormat.Left, color.ToArgb());
		}
	}
}