using UnityEngine;
using System.Collections;


namespace Linewars.GameLogic
{
	/// <summary>
	/// This script follow an other transform object.
	/// </summary>

	[RequireComponent(typeof (CharacterController))]
	[RequireComponent(typeof (Animator))]
	public class Follow : MonoBehaviour {

		public Transform target;
		public const float epsilon = 0.05f;
		public float minDistance;
		public float speed;
		public CharacterController characterController;
		public Animator animator;
		private Vector3 direction;
		// Use this for initialization
		void Start () {
			characterController = GetComponent<CharacterController>();
			animator = GetComponent<Animator>();
		}
		
		// Update is called once per frame
		void Update () {
			Vector3 direction = target.position - transform.position;
			float distance = direction.magnitude;

			UpdateLookDirection(direction);

			if(distance <= minDistance){
				direction = -direction.normalized * (minDistance - distance);
				animator.SetBool("walking", true);
			}else if(distance > minDistance + epsilon){
				direction = direction.normalized * Time.deltaTime * speed;
				animator.SetBool("walking", true);
			}else{
				animator.SetBool("walking", false);
				return;
			}
			Debug.DrawRay(transform.position, direction);
			characterController.Move(direction);
		}

		/// <summary>
		/// Look in a specific direction.
		/// </summary>
		/// <param name="direction">Direction.</param>
		public void UpdateLookDirection(Vector3 direction){
			if(direction.x > 0){
				transform.localScale = new Vector3(8,transform.localScale.y,transform.localScale.z);
			}else{
				transform.localScale = new Vector3(-8,transform.localScale.y,transform.localScale.z);
			}
		}
		
	}
}


