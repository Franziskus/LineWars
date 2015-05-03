using UnityEngine;
using System.Collections;

namespace Linewars.Control{
	/// <summary>
	/// With Keyboard or D-Pad. 
	/// This version uses WASD to move and QE to select Hero.
	/// </summary>
	public class KeyboardControlled : IControl {

		public void Activate(bool on){
		}

		public bool Left(){
			return Input.GetButton("Left") || Input.GetAxis("Horizontal") < -0.9;
		}
		
		public bool Right(){
			return Input.GetButton("Right") || Input.GetAxis("Horizontal") > 0.9;
		}
		
		public bool Up(){
			return Input.GetButton("Up") || Input.GetAxis("Vertical") > 0.9;
		}
		
		public bool Down(){
			return Input.GetButton("Down") || Input.GetAxis("Vertical") < -0.9;
		}
		
		public bool Next(){
			return Input.GetButtonDown("Next");
		}
		
		public bool Previous(){
			return Input.GetButtonDown("Previous");
		}
		
		public bool Menu(){
			return Input.GetButtonDown("Pause");
		}
	}
}
