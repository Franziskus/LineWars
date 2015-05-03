using UnityEngine;
using System.Collections;

namespace Linewars.GameLogic
{
	/// <summary>
	/// Game manager is responsible for handle Gamestates.
	/// For example when the user hits play the state will switch from
	/// start to run (actually to restart).
	/// The important thing is that scripts can register here for
	/// beeing a menu script (active in pause) or a level script (active in game)
	/// and also restart script (gets informed on restart)
	/// </summary>
	public class GameManager : MonoBehaviour {

		private MonoBehaviour[] levelScripts = new MonoBehaviour[0];
		private MonoBehaviour[] restartScripts = new MonoBehaviour[0];
		private MonoBehaviour[] menuScripts = new MonoBehaviour[0];

		public enum GameState{
			RESTART = 0,
			PAUSE = 1,
			RUN = 2,
			STARTED = 3,
			END = 4
		}

		private GameState currentState = GameState.STARTED;

		/// <summary>
		/// Switchs the state and informs register scripts.
		/// </summary>
		/// <param name="gs">Gs.</param>
		public void SwitchState(GameState gs){
			if(gs == GameState.RESTART){
				currentState = GameState.RESTART;
				RestartScripts();
				gs = GameState.RUN;
			}
			currentState = gs;
			if(gs == GameState.END){
				currentState = gs;
				RestartScripts();
				SetActive(menuScripts, true);
				SetActive(levelScripts, false);
			}else if(gs == GameState.PAUSE){
				SetActive(menuScripts, true);
				SetActive(levelScripts, false);
			}else if(gs == GameState.RUN){
				SetActive(menuScripts, false);
				SetActive(levelScripts, true);
			}else{
				Debug.LogError("Unknown GameState "+gs);
			}
			currentState = gs;
		}

		public GameState GetState(){
			return currentState;
		}

		private void RestartScripts(){
			foreach(MonoBehaviour mb in restartScripts){
				((IRestartable)mb).Restart();
			}
		}

		/// <summary>
		/// Add the script to a state.
		/// When gs is Run or Pause the MonoBehaviour needs to implement IInformEnable
		/// When gs is Restart then the MonoBehaviour needs to implement IRestartable
		/// </summary>
		/// <param name="mb">MonoBehaviour with IInformEnable or IRestartable</param>
		/// <param name="gs">GameState register to</param>
		public void Add(MonoBehaviour mb, GameState gs){
			MonoBehaviour[] org = null;
			switch(gs){
				case GameState.RUN: org = levelScripts; break;
				case GameState.RESTART: org = restartScripts; break;
				case GameState.PAUSE: org = menuScripts; break;
			}
			MonoBehaviour[] temp = new MonoBehaviour[org.Length + 1];
			System.Array.Copy(org, temp,org.Length);
			temp[org.Length] = mb;
			switch(gs){
				case GameState.RUN: levelScripts = temp; break;
				case GameState.RESTART: restartScripts = temp; break;
				case GameState.PAUSE: menuScripts = temp; break;
			}
		}

		/// <summary>
		/// Help method to inform an array of IInformEnable with one value
		/// </summary>
		/// <param name="mbs">Mbs.</param>
		/// <param name="value">If set to <c>true</c> value.</param>
		private void SetActive(MonoBehaviour[] mbs, bool value){
			foreach(MonoBehaviour mb in mbs){
				mb.enabled = value;
				if(mb is IInformEnable){
					((IInformEnable)mb).OnEnable(value);
				}
			}
		}

		/// <summary>
		/// If an event pauses the game. For example user gets a phone call. We put all scripts in pause state too
		/// </summary>
		/// <param name="pauseStatus">If set to <c>true</c> then the application goes to pause state.</param>
		void OnApplicationPause(bool pauseStatus) {
			if(currentState == GameState.RUN){
				SetActive(menuScripts, pauseStatus);
				SetActive(levelScripts, !pauseStatus);
				currentState = GameState.PAUSE;
			}
		}
	}
}



