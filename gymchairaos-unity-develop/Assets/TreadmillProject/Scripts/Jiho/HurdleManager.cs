using jiho;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jiho
{
    public class HurdleManager : MonoBehaviour
    {
        [SerializeField] private FunctionManager function;

        private void HurdleAttack(int _count)
        {
            function.PlayerHit(_count);
        }

        public void Hurdle(int _count)
        {
            HurdleAttack(_count);
        }
    }

}

