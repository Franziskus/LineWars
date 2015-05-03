using System;
namespace Linewars.GameLogic
{
	/// <summary>
	/// Interface to get informed of GameManager when we switch state so 
	/// the game needs to restart.
	/// </summary>
	public interface IRestartable
	{
		/// <summary>
		/// This method should get called from the GameManager
		/// </summary>
		void Restart();
	}
}


