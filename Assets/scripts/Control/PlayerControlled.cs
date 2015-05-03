using UnityEngine;
using System.Collections;

namespace Linewars.Control{
    public abstract class PlayerControlled : MonoBehaviour
    {
		
		public abstract Vector3 GetPosBehind();
	}
}
