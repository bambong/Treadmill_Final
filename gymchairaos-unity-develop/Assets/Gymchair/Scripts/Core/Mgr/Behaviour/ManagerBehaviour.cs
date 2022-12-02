using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gymchair.Core.Mgr.Behaviour
{
    public class ManagerBehaviour<T> : MonoBehaviour, Listener.IOnManagerListener where T : MonoBehaviour
    {
        static T instance;
        public static T Instance { get => instance; }

        public ManagerBehaviour()
        {
            instance = this as T;

            var framework = Framework.Instance;

            if (framework)
                framework.AddedManagerListener(this);
        }

        public void OnDestroy()
        {
            var framework = Framework.Instance;

            if (framework)
                framework.RemoveManagerListener(this);
        }


        public virtual void OnCoreMessage(object msg)
        {
        }
    }
}