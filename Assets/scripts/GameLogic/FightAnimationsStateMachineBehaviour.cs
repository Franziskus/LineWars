using UnityEngine;
using System.Collections;

namespace Linewars.GameLogic
{
	/// <summary>
	/// Script for Inform FightAnimation of ended States.
	/// </summary>
	public class FightAnimationsStateMachineBehaviour : StateMachineBehaviour {

		private string stateName;

		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
			this.stateName = animator.GetCurrentAnimatorClipInfo(layerIndex)[0].clip.name;
		}

		public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex){
			FightAnimations fa = animator.gameObject.GetComponent<FightAnimations>();
			if(fa != null)
				fa.EndState(stateName);
		}
	}
}
