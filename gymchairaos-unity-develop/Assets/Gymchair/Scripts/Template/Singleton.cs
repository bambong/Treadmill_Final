using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gymchair.Template
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        static T instance;
        public static T Instance { get
            {
                if (instance == null)
                {
                    GameObject obj = GameObject.Find("SingletonPool");

                    if (!obj)
                    {
                        obj = new GameObject("SingletonPool");
                        DontDestroyOnLoad(obj);
                    }

                    instance = obj.AddComponent<T>();
                }

                return instance;
            }
        }
    }
}
