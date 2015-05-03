using UnityEngine;
using System.Collections;
using Linewars.GameLogic;

namespace Linewars.GameLogic
{
	/// <summary>
	/// A script to handle collect hero from the board.
	/// </summary>
	public class CollectHero : MonoBehaviour {

		public CapsuleCollider capsuleCollider;

		public CharacterController characterController;
		/// <summary>
		/// A delegate that gets called when this is collected.
		/// </summary>
		public OnCollect onCollectListener;

		private Player player;

		void Start(){
			player = Utils.MainHelper.Instance.Get<GameLogic.Player>();
		}

		public delegate void OnCollect(GameObject me);

		/// <summary>
		/// If the Player avatar hits this collider. It will be added to the
		/// Hero line.
		/// </summary>
		/// <param name="other">Other.</param>
		void OnTriggerEnter(Collider other) {
			if(player != null){
				//Debug.Log("Test "+
				//          ((player.GetHead() == null)?"null":player.GetHead().name) + " " + 
				//          ((player.GetHead() == null)?"null":player.GetHead().collider.ToString())+ " == "+
				//          ((other == null)?"null":other.name)+ " "+
				//          ((other == null)?"null":other.ToString()));
				if(player.GetHead().GetComponent<Collider>() == other){
					player.AddHero(gameObject.GetComponent<GameLogic.Hero>());
				}
			}
		}

		/// <summary>
		/// Method for the Player script to disable all stuff what is not needed anymore after pickup.
		/// </summary>
		public void CollectedByPlayer(){
			capsuleCollider.enabled = false;
			characterController.enabled = true;
			if(onCollectListener != null)
				onCollectListener.Invoke(gameObject);
			this.enabled = false;
			transform.GetChild(0).gameObject.SetActive(true);
		}

	}
}


