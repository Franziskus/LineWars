using UnityEngine;
using System.Collections.Generic;
using Linewars.Data;
using Utils;
using Linewars.Control;
using Linewars.Ui;

namespace Linewars.GameLogic
{

	/// <summary>
	/// It takes care of the list of heros and also switches the head
	/// of the heros.
	/// It also takes care of ordering a fight.
	/// </summary>
	public class Player : MonoBehaviour, IRestartable, IInformEnable {

		private List<Hero> heros = new List<Hero>(); 
		public LayerMask headLayer;
		public LayerMask tailLayer;
		public Walking walking;
		public int hpForHitWall = 1;

		private int currentHeroNr;
		private int currentFightCounter = -1;
		private ControlsManager controlsManager;
		private GameManager gameManager;
		private InGameEntry enemy;

		public FightCam fightCam;
		public InGameHud inGameHud;

		void Awake(){
			MainHelper.Instance.Register(this);
		}

		void Start () {
			Utils.MainHelper mh = MainHelper.Instance;
			gameManager = mh.Get<GameManager>();
			controlsManager = mh.Get<ControlsManager>();
			gameManager.Add(this, GameLogic.GameManager.GameState.RUN);
			gameManager.Add(this, GameLogic.GameManager.GameState.RESTART);
			inGameHud = mh.Get<InGameHud>();
			walking.onHitWallListener += HitWall;
			Restart();
		}
		
		/// <summary>
		/// When there is a fight (fightcounter is > -1) play animations and calculate fights.
		/// otherwise enable waking script and check if the user wants to switch the head of the line.
		/// </summary>
		void Update () {
			if(!IsPlayFightAnimation()){
				if(currentFightCounter < 0){
					fightCam.ActivateCam(false);
					walking.enabled = true;
					if(controlsManager.Next()){
						NextHero();
					}else if(controlsManager.Previous()){
						PreviousHero();
					}
				}else{
					Fight(enemy, ++currentFightCounter);
				}
			}
		}

		/// <summary>
		/// Remove all Heros and order a new start Hero from SpawnManager.
		/// </summary>
		public void Restart(){
			foreach(Hero h in heros){
				h.transform.position = Vector3.zero;
				GameObject.Destroy(h.gameObject);
			}
			heros.Clear();
			Utils.MainHelper.Instance.Get<SpawnManager>().Restart();
			Hero nh = Utils.MainHelper.Instance.Get<SpawnManager>().GetStartHeroEntry();
			AddHero(nh);
			ActivateHero(0);
			currentFightCounter = -1;
			fightCam.ActivateCam(false);
		}

		void OnEnable(){
		}
	
		/// <summary>
		/// Enable and disable walking.
		/// </summary>
		/// <param name="enable">Set to <c>true</c> enable.</param>
		public void OnEnable(bool enable){
			if(enable){
				if(currentFightCounter >= 0){
					walking.enabled = false;
				}else{
					walking.enabled = true;
				}
			}else{
				walking.enabled = false;
			}
		}
	
		/// <summary>
		/// Activates the hero to become the head of the line.
		/// </summary>
		/// <param name="nr">Nr.</param>
		private void ActivateHero(int nr){
			Hero h = heros[nr];
			Follow f = h.GetComponent<Follow>();
			f.enabled = false;
			h.gameObject.layer = Helper.ConvertLayerMaskToNr(headLayer);
			walking.SetTarget(h.gameObject);
			inGameHud.SetHP(h.hitPoints);
		}

		/// <summary>
		/// Deactivates scripts that make a hero the head.
		/// </summary>
		/// <param name="nr">Nr.</param>
		private void DeactivateHero(int nr){
			Hero h = heros[nr];
			Follow f = h.GetComponent<Follow>();
			int next = (nr+1 >= heros.Count)?0:nr+1;
			f.target = heros[next].transform;
			h.gameObject.layer = Helper.ConvertLayerMaskToNr(tailLayer);
			f.enabled = true;
		}

		public int HeroCount(){
			return heros.Count;
		}

		/// <summary>
		/// Adds the hero to the end of the line.
		/// </summary>
		/// <param name="h">The height.</param>
		public void AddHero(Hero h){
			int next = currentHeroNr;
			if(heros.Count != 0){
				Follow f = h.GetComponent<Follow>();
				Follow fHead = GetHead().GetComponent<Follow>();
				f.target = fHead.target;
				fHead.target = h.transform;
				next++;
				next = next % heros.Count;
				if(next <= currentHeroNr)
					currentHeroNr++;
			}else{
				Follow f = h.GetComponent<Follow>();
				f.target = h.transform;
			}
			CollectHero ch = h.GetComponent<CollectHero>();
			ch.CollectedByPlayer();
			heros.Insert(next, h);
			DeactivateHero(next);
		}

		/// <summary>
		/// Removes the head of the line and destroy the GameObject.
		/// </summary>
		public void RemoveHead(){
			if(heros.Count > 1){
				Hero h = heros[currentHeroNr];
				heros.RemoveAt(currentHeroNr);
				for(int i = 0; i < heros.Count-1; i++){
					heros[i].GetComponent<Follow>().target = heros[i+1].transform;
				}
				heros[heros.Count-1].GetComponent<Follow>().target = heros[0].transform;
				if(currentHeroNr >= heros.Count)
					currentHeroNr = 0;
				ActivateHero(currentHeroNr);
				GameObject.Destroy(h.gameObject);
			}else{
				Debug.Log("End Game");
				gameManager.SwitchState(GameManager.GameState.END);
				//End Game
			}
		}

		/// <summary>
		/// Switch to next hero.
		/// </summary>
		public void NextHero(){
			int oldNr = currentHeroNr;
			currentHeroNr++;
			currentHeroNr = currentHeroNr % heros.Count;
			if(oldNr != currentHeroNr){
				DeactivateHero(oldNr);
				ActivateHero(currentHeroNr);
			}
		}

		/// <summary>
		/// Switch to previouses hero.
		/// </summary>
		public void PreviousHero(){
			int oldNr = currentHeroNr;
			currentHeroNr--;
			if(currentHeroNr < 0)
				currentHeroNr = heros.Count -1;
			if(oldNr != currentHeroNr){
				DeactivateHero(oldNr);
				ActivateHero(currentHeroNr);
			}
		}

		/// <summary>
		/// Gets the head gamobject.
		/// </summary>
		/// <returns>The head.</returns>
		public GameObject GetHead(){
			return heros[currentHeroNr].gameObject;
		}

		/// <summary>
		/// Hits the wall.
		/// </summary>
		public void HitWall(){
			bool dead = heros[currentHeroNr].SubstactHP(hpForHitWall);
			inGameHud.SetHP(heros[currentHeroNr].hitPoints);
			if(dead)
				RemoveHead();
		}
		/// <summary>
		/// Is the hero or an enemy playing a fight animation.
		/// </summary>
		/// <returns>Is the Hero or an enemy playing a fight animation.</returns>
		private bool IsPlayFightAnimation(){
			return (enemy != null && enemy.fightAnimations != null && enemy.fightAnimations.IsPlayAnimation()) ||
				heros[currentHeroNr] != null && heros[currentHeroNr].fightAnimations != null && heros[currentHeroNr].fightAnimations.IsPlayAnimation();
		}

		/// <summary>
		/// Start a fight against the specified enemy.
		/// </summary>
		/// <param name="enemy">Enemy.</param>
		public void Fight(InGameEntry enemy){
			Vector3 dir = (GetHead().transform.position - enemy.transform.position) / 2;
			fightCam.SetPosition(enemy.transform.position + dir);
			fightCam.ActivateCam(true);
			Fight(enemy, 0);
			walking.enabled = false;
		}

		/// <summary>
		/// Execute a fight round against a specified enemy.
		/// The fightCounter indicates the round.
		/// If the fightCounter is even the hero will attak.
		/// If is uneven the bad guy will attack.
		/// </summary>
		/// <param name="enemy">Enemy.</param>
		/// <param name="fightCounter">Fight counter.</param>
		public void Fight(InGameEntry enemy, int fightCounter){
			this.enemy = enemy;
			currentFightCounter = fightCounter;
			GameLogic.InGameEntry.FightMove result;
			if(currentFightCounter % 2 != 1){
				result = enemy.FightRound(heros[currentHeroNr]);
				heros[currentHeroNr].fightAnimations.PlayAttack();
				if(result == GameLogic.InGameEntry.FightMove.DEAD || result == GameLogic.InGameEntry.FightMove.SUPER_DEAD){
					GameObject.Destroy(this.enemy.gameObject, 0.5f);
				}else{
					enemy.fightAnimations.PlayShieldAnimation();
				}
			}else{
				result = heros[currentHeroNr].FightRound(enemy);
				enemy.fightAnimations.PlayAttack();
				if(result == GameLogic.InGameEntry.FightMove.DEAD || result == GameLogic.InGameEntry.FightMove.SUPER_DEAD){
					RemoveHead();
				}else{
					heros[currentHeroNr].fightAnimations.PlayShieldAnimation();
					inGameHud.SetHP(heros[currentHeroNr].hitPoints);
				}
			}
			if(result == GameLogic.InGameEntry.FightMove.DEAD || result == GameLogic.InGameEntry.FightMove.SUPER_DEAD){
				currentFightCounter = -1;
				inGameHud.AddPoints(this.enemy.points);
				heros[currentHeroNr].RefillHP(this.enemy.startHealth);
				this.enemy = null;
			}
		}

		public bool IsInFight(){
			return currentFightCounter >= 0;
		}
	}
}

