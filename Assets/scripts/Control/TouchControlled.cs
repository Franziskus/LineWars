using UnityEngine;
using System.Collections;
using Utils;
using Linewars.GameLogic;

namespace Linewars.Control{
	/// <summary>
	/// Touchscreen controlled. 
	/// This version uses the position of buttons on the screen to move and select the Hero.
	/// So left and right depence on the direction the Player (Hero Avatar) is facing.
	/// </summary>
	public class TouchControlled : MonoBehaviour, IControl {

		public RectTransform leftButton;
		private Rect leftButtonRec;
		public bool leftButtonReset;
		public RectTransform rightButton;
		private Rect rightButtonRec;
		public bool rightButtonReset;

		public RectTransform altLeftButton;
		private Rect altLeftButtonRec;
		public bool altLeftButtonReset;
		public RectTransform altRightButton;
		private Rect altRightButtonRec;
		public bool altRightButtonReset;

		public Walking player;

		public void Awake(){
			leftButtonRec = Helper.RectTransformToRect(leftButton);
			rightButtonRec = Helper.RectTransformToRect(rightButton);
			altLeftButtonRec = Helper.RectTransformToRect(altLeftButton);
			altRightButtonRec = Helper.RectTransformToRect(altRightButton);
		}


		/// <summary>
		/// Show buttons when Activate(true) and hide when Activate(false)
		/// </summary>
		/// <param name="on">Show or hide Buttons</param>
		public void Activate(bool on){
			leftButton.gameObject.SetActive(on);
			rightButton.gameObject.SetActive(on);
			altLeftButton.gameObject.SetActive(on);
			altRightButton.gameObject.SetActive(on);
		}

		/// <summary>
		/// This method tests if there is a touch on the rectangle.
		/// It also takes a reset boolean. To determinate if the player lifted the touch.
		/// 
		/// Is
		/// buttomReset = false and finger is up 
		/// => return false and buttomReset = false
		/// 
		/// Is
		/// buttomReset = false and finger is down 
		/// => return true and buttomReset = true
		/// 
		/// Is
		/// buttomReset = true and finger is down 
		/// => return false and buttomReset = true
		/// 
		/// Is
		/// buttomReset = true and finger is up 
		/// => return false and buttomReset = false
		/// 
		/// </summary>
		/// <returns><c>true</c> has pressed the area</returns>
		/// <param name="rec">Rec where to test the click.</param>
		/// <param name="buttomReset">this variable will set and read. See method description</param>
		private bool IsClicked(Rect rec, ref bool buttomReset){
			Vector3 buttomLocalTouchPoint = Vector3.one;
			if(!buttomReset){			
				foreach (Touch touch in Input.touches)
				{
					if (rec.Contains(touch.position)
					    && touch.phase != TouchPhase.Ended)
					{
						buttomReset = true;
						return true;
					}
				}
			}else{				
				foreach (Touch touch in Input.touches)
				{
					if (rec.Contains(touch.position)
					    && touch.phase == TouchPhase.Ended)
					{
						buttomReset = false;
					}
				}
			}
			return false;
		}
		
		public bool Left(){
			return (player.GetCurrentPlayerDirection() == Vector2.up && IsClicked(leftButtonRec, ref leftButtonReset)) ||
				(player.GetCurrentPlayerDirection() ==-Vector2.up && IsClicked(rightButtonRec, ref rightButtonReset));
		}
		
		public bool Right(){
			return (player.GetCurrentPlayerDirection() == Vector2.up && IsClicked(rightButtonRec, ref rightButtonReset)) ||
				(player.GetCurrentPlayerDirection() ==-Vector2.up && IsClicked(leftButtonRec, ref leftButtonReset));
		}
		
		public bool Up(){
			return (player.GetCurrentPlayerDirection() == Vector2.right && IsClicked(leftButtonRec, ref leftButtonReset)) ||
				(player.GetCurrentPlayerDirection() ==-Vector2.right && IsClicked(rightButtonRec, ref rightButtonReset));
		}
		
		public bool Down(){
			return (player.GetCurrentPlayerDirection() == Vector2.right && IsClicked(rightButtonRec, ref rightButtonReset)) ||
				(player.GetCurrentPlayerDirection() ==-Vector2.right && IsClicked(leftButtonRec, ref leftButtonReset));
		}
		
		public bool Next(){
			return IsClicked(altRightButtonRec, ref altRightButtonReset);
		}
		
		public bool Previous(){
			return IsClicked(altLeftButtonRec, ref altLeftButtonReset);
		}
		
		public bool Menu(){
			return false;
		}


	}
}



