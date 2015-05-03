using UnityEngine;
using System.Collections;

namespace Utils{

	/// <summary>
	/// Static helper methods.
	/// </summary>
	public class Helper {

		public static int ConvertLayerMaskToNr(LayerMask mask){
			int layerNumber = 0;
			int layer = mask.value;
			while(layer > 0)
			{
				layer = layer >> 1;
				layerNumber++;
			}
			return layerNumber-1;
		}

		public static Rect RectTransformToRect(RectTransform rt){
			Vector3[] points = new Vector3[4];
			rt.GetWorldCorners(points);
			
			float x = points[0].x;
			float height = points[2].y - points[0].y;
			float y = Screen.height - points[0].y - height;
			float width = points[2].x - points[0].x;
			
			return new Rect(x, y,  width, height);
		}
	}
}
