using UnityEngine;
using System.Collections;

namespace Linewars.GameLogic
{
	/// <summary>
	/// Interface for returning the current direction of the Player (Active Hero Avatar)
	/// </summary>
	public interface IPlayerDirection {

		Vector2 GetCurrentPlayerDirection();
	}
}
