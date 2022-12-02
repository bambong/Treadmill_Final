using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gymchair.Core.Mgr.Listener
{
    public interface IOnManagerListener
    {
        void OnCoreMessage(object msg);
    }
}