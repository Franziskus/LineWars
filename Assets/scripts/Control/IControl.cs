using UnityEngine;
using System.Collections;

namespace Linewars.Control{
	/// <summary>
	/// Interface for keyboard control and touch control classes
	/// </summary>
	public interface IControl{

		void Activate(bool on);

		bool Left();
		
		bool Right();
		
		bool Up();
		
		bool Down();
		
		bool Next();
		
		bool Previous();
		
		bool Menu();
	}
}
