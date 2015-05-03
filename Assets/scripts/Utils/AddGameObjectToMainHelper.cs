using UnityEngine;
using System.Collections;

namespace Utils
{
	/// <summary>
	/// Helper script to automatically register scripts to MainHelper
	/// </summary>
    public class AddGameObjectToMainHelper : MonoBehaviour
    {

        public GameObject RegisterAllMonoBehavierChilds;

        private void Awake()
        {
            MonoBehaviour[] monos = RegisterAllMonoBehavierChilds.GetComponents<MonoBehaviour>();
            for (int i = 0; i < monos.Length; i++)
            {
                MonoBehaviour m = monos[i];
                MainHelper.Instance.Register(m.GetType(), m);
            }
           
        }
    }
}

