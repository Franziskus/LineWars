using UnityEngine;
using System.Collections;
using Linewars.GameLogic;

namespace Linewars.Control{
	/// <summary>
	/// With Keyboard or D-Pad. 
	/// This version uses AD to move and QE to select Hero.
	/// So AD depence to the direction the Player (Hero Avatar) is facing.
	/// </summary>
	public class KeyboardControlled2 : IControl {

		public Walking player;

		public UnityEngine.UI.Text text;

		public void Activate(bool on){
			player = Utils.MainHelper.Instance.Get<Walking>();
		}

		private bool KeyLeft(){
			return Input.GetButtonDown("Left") || Input.GetAxis("Horizontal") < -0.9;
		}
		
		private bool KeyRight(){
			return Input.GetButtonDown("Right") || Input.GetAxis("Horizontal") > 0.9;
		}

		public bool Left(){
			return (player.GetCurrentPlayerDirection() == Vector2.up && KeyLeft()) ||
			        (player.GetCurrentPlayerDirection() ==-Vector2.up && KeyRight());
		}
		
		public bool Right(){
				return (player.GetCurrentPlayerDirection() == Vector2.up && KeyRight()) ||
					(player.GetCurrentPlayerDirection() ==-Vector2.up && KeyLeft());
		}
		
		public bool Up(){
						return (player.GetCurrentPlayerDirection() == Vector2.right && KeyLeft()) ||
					        (player.GetCurrentPlayerDirection() ==-Vector2.right && KeyRight());
		}
		
		public bool Down(){
						return (player.GetCurrentPlayerDirection() == Vector2.right && KeyRight()) ||
								(player.GetCurrentPlayerDirection() ==-Vector2.right && KeyLeft());
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
