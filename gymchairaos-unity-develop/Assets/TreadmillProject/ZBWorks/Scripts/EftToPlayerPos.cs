using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZB;

namespace ZB
{
    public class EftToPlayerPos : MonoBehaviour
    {
        [SerializeField] ObjectPooling m_pool;
        [SerializeField] Transform m_tf_player;

        public void Play(string name)
        {
            m_pool.Spawn(name, m_tf_player.transform.position);
        }
    }
}