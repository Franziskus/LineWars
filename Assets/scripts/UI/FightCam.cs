using UnityEngine;
using System.Collections;

namespace Linewars.Ui
{
	/// <summary>
	/// This script handles the little extra window at a fight
	/// </summary>
	public class FightCam : MonoBehaviour {

		public Camera normalCamera;
		public Vector3 viewPoint;
		private Camera fightCamera;

		private Rect calcRect;

		// Use this for initialization
		void Start () {
			fightCamera = this.GetComponent<Camera>();
			calcRect = new Rect(fightCamera.rect.width / 2, fightCamera.rect.height / 2, fightCamera.rect.width, fightCamera.rect.height);
		}

		/// <summary>
		/// Position the camera on the screen so it overlays the world position.
		/// </summary>
		void PosCameraOnScreen(){
			viewPoint = normalCamera.WorldToViewportPoint(transform.position);
			viewPoint.x = Mathf.Max(0,viewPoint.x - calcRect.x);
			viewPoint.y = Mathf.Max(0,viewPoint.y - calcRect.y);

			viewPoint.x = Mathf.Min(1-calcRect.width,viewPoint.x);
			viewPoint.y = Mathf.Min(1-calcRect.height,viewPoint.y);
			fightCamera.rect = new Rect(viewPoint.x, viewPoint.y, calcRect.width, calcRect.height);
		}

		public void ActivateCam(bool b){
			fightCamera.enabled = b;
			if(b)
				PosCameraOnScreen();
		}

		//Set the position in world space.
		public void SetPosition(Vector3 t){
			transform.position = new Vector3(t.x,t.y,transform.position.z);
		}
	}
}


