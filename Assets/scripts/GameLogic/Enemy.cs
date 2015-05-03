using UnityEngine;
using System.Collections;

namespace Linewars.GameLogic
{
	/// <summary>
	/// An InGame Entry that represents a bad guy.
	/// </summary>
	public class Enemy : InGameEntry {

		private Player player;
		
		public override void Start(){
			base.Start();
			player = Utils.MainHelper.Instance.Get<GameLogic.Player>();
		}

		/// <summary>
		/// Execute fight when hit by player.
		/// </summary>
		/// <param name="other">Other.</param>
		void OnTriggerEnter(Collider other) {
			if(player != null){
				if(player.GetHead().GetComponent<Collider>() == other){
					player.Fight(this);
				}
			}
		}
	}
}


