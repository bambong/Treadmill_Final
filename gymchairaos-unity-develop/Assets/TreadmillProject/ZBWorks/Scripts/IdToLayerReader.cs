using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZB;


namespace ZB
{
    public class IdToLayerReader : MonoBehaviour
    {
        public static IdToLayerReader Instance;

        [SerializeField] Set[] sets;

        public int IdToLayer(string ID)
        {
            for (int i = 0; i < sets.Length; i++)
            {
                if (sets[i].ID == ID)
                {
                    return sets[i].Layer;
                }
            }
            return -1;
        }

        void Awake()
        {
            Instance = this;
        }

        [System.Serializable]
        public class Set
        {
            public string ID;
            public int Layer;
        }
    }
}