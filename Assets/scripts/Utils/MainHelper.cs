using System;
using System.Collections.Generic;

namespace Utils
{
    /// <summary>
    /// This is the main helper class. Scripts can register here by themselve (for example in there awake method).
    /// Usually you only need it to get a reference of an object and we take the current type.
    /// Sometimes it can be interesting to register subclass with the type of a super class. 
    /// With techniques like this you can create an inversion of control.
    /// </summary>
    public class MainHelper : UnityEngine.MonoBehaviour
    {

        private static MainHelper _instance;
        private readonly Dictionary<Type, Object> _registerComp = new Dictionary<Type, Object>();
        /// <summary>
        /// returns instance
        /// </summary>
        public static MainHelper Instance
        {
            get
            {

                if (!_instance)
                {
                    var mainObj = UnityEngine.GameObject.Find("MainHelper");

                    if (!mainObj)
                    {
                        mainObj = new UnityEngine.GameObject("MainHelper");
                    }
                    else
                    {
                        _instance = mainObj.GetComponent<MainHelper>();
                    }

                    if (_instance == null)
                    {
                        _instance = mainObj.AddComponent<MainHelper>();
                    }


                }
                return _instance;
            }
        }

        /// <summary>
        /// Registers this object with a specific type.
        /// The type should be at least a super class or an interface of this class.
        /// </summary>
        /// <param name="ntype"></param>
        /// <param name="obj"></param>
        public void Register(Type ntype, object obj)
        {  
            _registerComp.Add(ntype, obj);
            //UnityEngine.Debug.Log("Register: " + ntype);
        }

        /// <summary>
        /// Registers this object. It can be found by his own type.
        /// </summary>
        /// <param name="obj"></param>
        public void Register(object obj)
        {
            Type ntype = obj.GetType();
            _registerComp.Add(ntype, obj);
        }

        /// <summary>
        /// Get a registered object by type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Get<T>()
        {
            Type ntype = typeof(T);
            //UnityEngine.Debug.Log("Get " + ntype + " it\'s in?" + _registerComp.ContainsKey(ntype));
            if (_registerComp.ContainsKey(ntype))
                return (T)_registerComp[ntype];
            return default(T);
        }

    }
}


