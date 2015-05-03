using UnityEngine;
using System.Collections;

namespace Utils
{
	public class TestSize : MonoBehaviour {

		public RectTransform leftButton;

		private int screenHeight;

		public void Awake(){
			leftButton = this.GetComponent<RectTransform>();
			screenHeight = Screen.height;
		}

		void OnGUI(){


			Rect r = Utils.Helper.RectTransformToRect(leftButton);
			GUI.Box(r, leftButton.name);
		}
	}
}
