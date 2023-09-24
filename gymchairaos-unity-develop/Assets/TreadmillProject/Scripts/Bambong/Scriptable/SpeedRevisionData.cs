using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bambong 
{
    [Serializable]
    public struct SpeedChangeData
    {
        public float ratio; // 얼만큼의 거리에서 변화할건지 
        public float time; // 변화하는데 까지 걸리는 시간
        public float speed; // 어떤 속도로 변화 할건지
    }


    [CreateAssetMenu(fileName = "SpeedRevisionData",menuName = "Scriptable/SpeedRevisionData",order = 0)]
    public class SpeedRevisionData : ScriptableObject
    {
        public List<SpeedChangeData> pairs;

        public SpeedChangeData GetCollectAreaData(float ratio)  // 전체 레이스 중 얼만큼 왔는지 
        {
            foreach(var data in pairs) 
            {
                if( data.ratio < ratio) 
                {
                    continue;
                }
                return data;  // 범위에 알맞은 데이터를 반환
            }
            return pairs[pairs.Count - 1];
        }


    }

}
