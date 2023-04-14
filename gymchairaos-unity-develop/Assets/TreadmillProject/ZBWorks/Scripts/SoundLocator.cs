using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gymchair.Core.Mgr;

namespace ZB
{
    public class SoundLocator : MonoBehaviour
    {
        public void PlayBgm(string id)
        {
            SoundMgr.Instance.PlayBGM(id);
        }

        public void StopBgm()
        {
            SoundMgr.Instance.StopBGM();
        }

        public void PlaySfx(string id)
        {
            SoundMgr.Instance.PlayEffect(id);
        }

        void Start()
        {
            //���ӽ��� ����
            SoundMgr.Instance.PlayBGM("bgm_Obstacle");
        }
    }
}