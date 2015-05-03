using UnityEngine;
using System.Collections;
using Utils;
using Linewars.Control;

namespace Linewars.GameLogic
{
	/// <summary>
	/// This script is responsible for the walking behavior of the heros leader.
	/// </summary>
	public class Walking : MonoBehaviour, IPlayerDirection, GameLogic.IRestartable {

		/// <summary>
		/// The next direction the player wants to go.
		/// </summary>
		private Vector2 nextDirection;
		/// <summary>
		/// The direction where the hero walks to.
		/// </summary>
		private Vector2 direction;

		/// <summary>
		/// size of the board
		/// </summary>
		public Rect maxLevelSize;

		/// <summary>
		/// The speed of the player when a game starts or resets.
		/// </summary>
		private float startSpeed;
		/// <summary>
		/// The player gets faster over time. Here you can set up the speedup.
		/// </summary>
		public float speedup = 0.00001f;
		/// <summary>
		/// Moving speed changes over the time of the game. So it starts with startSpeed and increases by speedup.
		/// </summary>
		public float speed;
		/// <summary>
		/// The current speed is the same as speed but is 0 when there is a fight or at start.
		/// </summary>
		private float currentSpeed;

		/// <summary>
		///  Size of the Grid
		/// </summary>
		public float cellSize = 1;
		public float cellThreshold = 0.05f;
		protected CharacterController characterController;
		/// <summary>
		/// The target transform. This will point player.GetHead()
		/// </summary>
		protected Transform targetTransform;
		public Animator animator;

		protected ControlsManager minput;

		/// <summary>
		/// Distance to next junction on the grid.
		/// </summary>
		private float mod;
		private Vector3 startPosition;

		/// <summary>
		/// A delegate when hit a wall.
		/// </summary>
		public OnHitWall onHitWallListener;

		public delegate void OnHitWall();

		public void Awake(){
			startSpeed = speed;
			MainHelper.Instance.Register(this);
		}

		// Use this for initialization
		void Start () {
			startPosition = transform.position;
			targetTransform = transform;
			MainHelper mm = MainHelper.Instance;
			minput = mm.Get<ControlsManager>();
			GameLogic.GameManager gm = mm.Get<GameLogic.GameManager>();
			gm.Add(this, GameLogic.GameManager.GameState.RESTART);
			enabled = false;
		}

		/// <summary>
		/// Set the Active Hero
		/// </summary>
		/// <param name="target">Target.</param>
		public void SetTarget(GameObject target){
			this.characterController = target.GetComponent<CharacterController>();
			this.animator = target.GetComponent<Animator>();
			this.targetTransform = target.transform;
		}

		public void Restart(){
			speed = startSpeed;
			currentSpeed = 0;
			direction = Vector2.up;	
			targetTransform.position = startPosition;
		}
		
		// Update is called once per frame
		void Update () {

			speed = speed + (speedup * Time.deltaTime);
			Vector2 position2D = new Vector2(targetTransform.position.x,targetTransform.position.y);

			//Save input Direction
			Vector2 inputDirection = Vector2.zero;
			if(minput.Up()){
				inputDirection = Vector2.up;
			}else if(minput.Left()){
				inputDirection = -Vector2.right;
			}else if(minput.Right()){
				inputDirection = Vector2.right;
			}else if(minput.Down()){
				inputDirection = -Vector2.up;
			}
			if(inputDirection != Vector2.zero){
				currentSpeed = speed;
				if(direction + inputDirection != Vector2.zero){
					nextDirection = inputDirection;
				}
			}

			//if we not moveing set anyway
			if(direction == Vector2.zero){
				direction = nextDirection;
				UpdateAnimator();
			}else if(direction != nextDirection){	//the player wahts in a new direction
				UpdateAnimator();

				//calculate the distace to the next cell on the grid.
				if(direction == -Vector2.right){
					mod = (position2D.x - direction.x) % cellSize;
				}else if(direction == Vector2.right){
					mod = cellSize - ((position2D.x + direction.x) % cellSize);
				}else if(direction == Vector2.up){
					mod = cellSize - ((position2D.y + direction.y) % cellSize);
				}else{
					mod = (position2D.y - direction.y) % cellSize;
				}

			}

			Vector2? targetDir = null;
			Vector2 targetPos = Vector2.zero;

			//Avoid overshoot a junction
			if(direction != nextDirection){
				float moveDistance = Time.deltaTime * currentSpeed;
				float remaningDistance = mod - moveDistance;
				if(remaningDistance < 0){
					Vector2 temp = direction * mod;
					targetPos = temp + (nextDirection * -remaningDistance) + position2D;
					targetDir = targetPos - position2D;
					direction = nextDirection;
				}
			}
			//If direction == nextDirection or the player has simply walk more to reach the junction.
			// => Go forward.
			if(!targetDir.HasValue){
				targetDir = direction* Time.deltaTime * currentSpeed;
				targetPos = position2D + targetDir.Value;
			}

			//Test if we are in level bounce
			if(targetPos.x > maxLevelSize.width ||
			   targetPos.x < maxLevelSize.x ||
			   targetPos.y < maxLevelSize.y ||
			   targetPos.y > maxLevelSize.height){
			   HitWall();
			}else{
				characterController.Move(targetDir.Value);
			}

			UpdateLookDirection();
		}

		/// <summary>
		/// Updates direction where the Hero looks to
		/// </summary>
		public void UpdateLookDirection(){
			if(direction == Vector2.right){
				targetTransform.localScale = new Vector3(8,targetTransform.localScale.y,targetTransform.localScale.z);
			}else if(direction == -Vector2.right){
				targetTransform.localScale = new Vector3(-8,targetTransform.localScale.y,targetTransform.localScale.z);
			}
		}

		public void UpdateAnimator(){
			if(currentSpeed == 0){
				animator.SetBool("walking", false);
			}else{
				animator.SetBool("walking", true);
			}
		}

		/// <summary>
		/// If we hit the wall go in the opposite direction.
		/// and inform onHitWallListener.
		/// </summary>
		public void HitWall(){
			direction = -direction;
			nextDirection = direction;
			if(onHitWallListener != null)
				onHitWallListener.Invoke();
		}

		/// <summary>
		/// Gets the current player direction.
		/// </summary>
		/// <returns>The current player direction.</returns>
		public Vector2 GetCurrentPlayerDirection(){
			return direction;
		}
	}
}


