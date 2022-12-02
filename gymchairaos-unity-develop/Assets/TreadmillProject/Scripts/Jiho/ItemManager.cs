using jiho;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace jiho
{
    public class ItemManager : MonoBehaviour
    {
        [SerializeField] private FunctionManager function;

        private void UseItem(int _count)
        {
            function.PlusItem(_count);
        }

        public void GetItem(int _count)
        {
            UseItem(_count);
        }
    }
}

