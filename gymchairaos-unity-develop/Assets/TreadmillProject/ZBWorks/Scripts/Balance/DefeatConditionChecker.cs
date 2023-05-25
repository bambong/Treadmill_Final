using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ZB.Balance
{
    public class DefeatConditionChecker : MonoBehaviour
    {
        [SerializeField] UnityEvent m_uEvent_OnDefeat;

        public void Defeat()
        {
            m_uEvent_OnDefeat.Invoke();
        }
    }
}