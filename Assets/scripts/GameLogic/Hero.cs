using UnityEngine;
using System.Collections;
using Linewars.Data;

namespace Linewars.GameLogic
{
	/// <summary>
	/// Hero implementation of an InGameEntry.
	/// </summary>
	public class Hero : InGameEntry {

		public override bool IsHero ()
		{
			return true;
		}

		/// <summary>
		/// Subtracts the value from the hitpoints and returns true if the hero has now less than 
		/// one hp.
		/// </summary>
		/// <returns><c>true</c>, if this hero has less then 1 hp <c>false</c> otherwise.</returns>
		/// <param name="value">Value to substact from hp.</param>
		public bool SubstactHP(int value){
			hitPoints -= value;
			return hitPoints < 1;
		}
	}
}


