using UnityEngine;
using Linewars.GameLogic;
using System.Collections;

namespace Linewars.Control{
	///<summary>
	/// Area in the middle of the screen to pause on touch diverses.
	/// Its also possible to hit ESC on keyboard or start on controller.
	///</summary>
	public class PauseMenu : MonoBehaviour {

		protected ControlsManager minput;
		protected GameManager gameManager;

		void Start () {
			Utils.MainHelper mh = Utils.MainHelper.Instance;
			minput = mh.Get<ControlsManager>();
			gameManager = mh.Get<GameManager>();
		}
		

		void Update () {
			if(minput.Menu()){
				Pause();
			}
		}

		public void Pause(){
			gameManager.SwitchState(GameManager.GameState.PAUSE);
		}
	}
}
