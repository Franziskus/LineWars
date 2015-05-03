using UnityEngine;
using System.Collections;

namespace Utils
{
	public class TestSize : MonoBehaviour {

		public RectTransform leftButton;

		public void Awake(){
			leftButton = this.GetComponent<RectTransform>();
		}

		void OnGUI(){


			Rect r = Utils.Helper.RectTransformToRect(leftButton);
			GUI.Box(r, leftButton.name);
		}
	}
}
