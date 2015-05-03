using UnityEngine;
using System.Collections;

namespace Linewars.Control{
	/// <summary>
	/// Manager to handle the different controls.
	/// Acts as a facade to the rest of the application.
	/// </summary>
	public class ControlsManager : MonoBehaviour {

		/// <summary>
		/// Reference to touch controls. Because this needs some extra setup.
		/// </summary>
		public TouchControlled touchControlled;

		protected IControl currentControl;

		/// <summary>
		/// Setup Controls dependent on mobile phone or everything else.
		/// </summary>
		public void Start(){
			if(Application.isMobilePlatform){
				Activate(touchControlled);
			}else{
				touchControlled.Activate(false);
				Activate( new KeyboardControlled2());
			}
		}

		/// <summary>
		/// Is it currently touch controlled.
		/// </summary>
		/// <returns><c>true</c> if this instance is touch controlled; otherwise, <c>false</c>.</returns>
		public bool IsTouchControlled(){
			return currentControl == touchControlled;
		}

		/// <summary>
		/// Is it currently controlled by keyboard or game controller via WASD and QE.
		/// </summary>
		/// <returns><c>true</c> If this instance is normal keyboard controlled; otherwise, <c>false</c>.</returns>
		public bool IsNormalKeyboard(){
			return currentControl.GetType() == typeof(KeyboardControlled);
		}

		/// <summary>
		/// Is it currently controlled by keyboard or game controller via AD and QE.
		/// </summary>
		/// <returns><c>true</c> If this instance is alternative keyboard controlled; otherwise, <c>false</c>.</returns>
		public bool IsAltKeyboard(){
			return currentControl.GetType() == typeof(KeyboardControlled2);
		}

		/// <summary>
		/// Activate a specified control.
		/// </summary>
		/// <param name="newControl">New control.</param>
		public void Activate(IControl newControl){
			if(currentControl != null){
				if(currentControl != newControl){
					currentControl.Activate(false);
					currentControl = newControl;
					currentControl.Activate(true);
				}
			}else{
				currentControl = newControl;
				currentControl.Activate(true);
			}
		}

		public bool Left(){
			return currentControl.Left();
		}

		public bool Right(){
			return currentControl.Right();
		}

		public bool Up(){
			return currentControl.Up();
		}

		public bool Down(){
			return currentControl.Down();
		}

		public bool Next(){
			return currentControl.Next();
		}

		public bool Previous(){
			return currentControl.Previous();
		}

		public bool Menu(){
			return currentControl.Menu();
		}
	}
}


