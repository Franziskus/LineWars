using System;
namespace Linewars.GameLogic
{
	/// <summary>
	/// Interface to get informed of GameManager when we switch state.
	/// </summary>
	public interface IInformEnable
	{
		void OnEnable(bool enable);
	}
}



