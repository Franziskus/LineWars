using UnityEngine;
using System.Collections;

namespace Linewars.GameLogic
{
	/// <summary>
	/// This class can register at GameManager as a IInformEnable.
	/// So when OnEnable(bool enable) is called from GameManager this 
	/// Script will set this to the targetGameObject. So it will
	/// activate this in hierarchy.
	/// </summary>
	public class ActivateGameObject : MonoBehaviour, IInformEnable {

		public GameObject targetGameObject;
		public GameLogic.GameManager.GameState registerGameState;

		void Start(){
			Utils.MainHelper.Instance.Get<GameLogic.GameManager>().Add(this, registerGameState);
		}

		public void OnEnable() {
		}

		public void OnEnable(bool enable) {
			targetGameObject.SetActive(enable);
		}

	}
}
