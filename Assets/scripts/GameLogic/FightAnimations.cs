using UnityEngine;
using System.Collections;

namespace Linewars.GameLogic
{
	/// <summary>
	/// Script for easy access to fight animation system.
	/// </summary>
	public class FightAnimations : MonoBehaviour {

		public Animator animator;

		public bool shield;
		public int attack;

		public void EndAnimation(string name){
			attack = 0;
			shield = false; 
			animator.SetBool("shield", shield);
			animator.SetInteger("attack", attack);
		}

		public void PlayShieldAnimation(){
			shield = true;
			animator.SetBool("shield", shield);
		}

		public void PlayAttack(){
			attack = Random.Range(1,3);
			animator.SetInteger("attack", attack);
		}

		public bool IsPlayAnimation(){
			return shield || attack > 0;
		}

		void Start () {
			animator = GetComponent<Animator>();	
		}
	}
}
