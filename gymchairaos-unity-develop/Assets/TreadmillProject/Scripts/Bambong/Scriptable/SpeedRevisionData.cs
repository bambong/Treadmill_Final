using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bambong 
{
    [Serializable]
    public struct SpeedChangeData
    {
        public float ratio; // ��ŭ�� �Ÿ����� ��ȭ�Ұ��� 
        public float time; // ��ȭ�ϴµ� ���� �ɸ��� �ð�
        public float speed; // � �ӵ��� ��ȭ �Ұ���
    }


    [CreateAssetMenu(fileName = "SpeedRevisionData",menuName = "Scriptable/SpeedRevisionData",order = 0)]
    public class SpeedRevisionData : ScriptableObject
    {
        public List<SpeedChangeData> pairs;

        public SpeedChangeData GetCollectAreaData(float ratio)  // ��ü ���̽� �� ��ŭ �Դ��� 
        {
            foreach(var data in pairs) 
            {
                if( data.ratio < ratio) 
                {
                    continue;
                }
                return data;  // ������ �˸��� �����͸� ��ȯ
            }
            return pairs[pairs.Count - 1];
        }


    }

}
