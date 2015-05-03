using UnityEngine;
using System.Collections;

namespace Linewars.Ui
{
	/// <summary>
	/// This script ensure that the attached sprite component will fill the complete camera.
	/// </summary>
	public class SameAsCameraSize : MonoBehaviour {

		public Camera copyFrom;

		// Use this for initialization
		void Start () {
			ResizeSpriteToScreen();
		}


		void ResizeSpriteToScreen() {
			SpriteRenderer sr = GetComponent<SpriteRenderer>();
			if (sr == null) return;

			float width = sr.sprite.bounds.size.x;
			float height = sr.sprite.bounds.size.y;
			
			float worldScreenHeight = copyFrom.orthographicSize * 2.0f;
			float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;


			Vector3 scale = new Vector3(worldScreenWidth / width, worldScreenHeight / height, 1);
			transform.localScale = scale;

		}
	}
}
