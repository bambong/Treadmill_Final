using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bambong 
{
    [Serializable]
    public struct SpeedChangeData
    {
        public float ratio;
        public float time;
        public float speed;
    }
    


    [CreateAssetMenu(fileName = "SpeedRevisionData",menuName = "Scriptable/SpeedRevisionData",order = 0)]
    public class SpeedRevisionData : ScriptableObject
    {
        public List<SpeedChangeData> pairs;

        public SpeedChangeData GetCollectAreaData(float key) 
        {
            foreach(var data in pairs) 
            {
                if( data.ratio < key) 
                {
                    continue;
                }
                return data;
            }
            return pairs[pairs.Count - 1];
        }


    }

}
