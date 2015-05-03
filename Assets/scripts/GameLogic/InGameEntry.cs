using UnityEngine;
using System.Collections;
using Linewars.Data;

namespace Linewars.GameLogic
{
	/// <summary>
	/// An instance of this class represents an object on the board.
	/// It's the superclass for Hero and Enemy
	/// </summary>
	public class InGameEntry : MonoBehaviour {

		/// <summary>
		///	Return types for the fight between entities.
		/// </summary>
		public enum FightMove{
			SHIELD, DEAD, SUPER_SHIELD, SUPER_DEAD
		}

		/// <summary>
		/// Reference for the entry from the Database.
		/// Should not be changed.
		/// </summary>
		private Entry entry;

		/// <summary>
		/// Current hit points.
		/// </summary>
		public int hitPoints;


		/// <summary>
		/// Reference to  fight animations class.
		/// </summary>
		public FightAnimations fightAnimations;

		public Texture2D texture{
			get{
				return entry.texture;
			}
		}

		/// <summary>
		/// Gets the name of the entity.
		/// </summary>
		/// <value>The name of the entity.</value>
		public string charName{
			get{
				return entry.charName;
			}
		}

		/// <summary>
		/// Gets the maximum/start health.
		/// </summary>
		/// <value>The maximum health.</value>
		public int startHealth{
			get{
				return entry.heart;
			}
		}

		/// <summary>
		/// Calculate the points of the entry.
		/// </summary>
		/// <value>The points.</value>
		public int points{
			get{
				return entry.shield + entry.sword + entry.heart;
			}
		}

		public virtual void Start(){
			fightAnimations = GetComponentInChildren<FightAnimations>();
		}

		/// <summary>
		/// Fights the round. This instance gets attacked by the enemy.
		/// </summary>
		/// <returns>I'm DEAD or I am shield some dmg</returns>
		/// <param name="enemy">Enemy.</param>
		public FightMove FightRound(InGameEntry enemy){
			int attackValue = enemy.entry.sword;
			bool super = enemy.IsHero() && entry.characterType == enemy.entry.characterType;
			if(super){
				attackValue *= 2;
			}
			int damage = attackValue - entry.shield;
			damage = Mathf.Max(damage, 1);
			int result = hitPoints - damage;
			//Debug.Log(entry + "(defence) vs \n"+enemy.entry+"(old HP:"+hitPoints+")(attacker) \nDMG: "+ damage + " result: "+result);
			if(result > 0){
				hitPoints = result;
				return super?FightMove.SUPER_SHIELD:FightMove.SHIELD;
			}else{
				hitPoints = 0;
				return super?FightMove.SUPER_DEAD:FightMove.DEAD;
			}
		}

		public void RefillHP(int amount){
			if(IsHero()){
				//hitPoints = Mathf.Min(hitPoints + amount, entry.heart);
				int refiel = hitPoints + amount;
				if(refiel > entry.heart)
					hitPoints = entry.heart;
				else
					hitPoints = refiel;
			}
		}

		/// <summary>
		/// Determines whether this instance is hero.
		/// Should be overwritten by Hero class
		/// </summary>
		/// <returns><c>true</c> if this instance is hero; otherwise, <c>false</c>.</returns>
		public virtual bool IsHero(){
			return false;
		}

		/// <summary>
		/// Sets the entry. Should called after creating this instance
		/// </summary>
		/// <param name="e">E.</param>
		public virtual void SetEntry(Entry e){
			entry = e;
			hitPoints = entry.heart;
		}
	}
}


