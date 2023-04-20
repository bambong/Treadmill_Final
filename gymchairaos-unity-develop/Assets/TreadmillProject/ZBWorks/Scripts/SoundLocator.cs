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
#if !UNITY_EDITOR || SOUND_TEST
            SoundMgr.Instance.PlayBGM(id);
#endif
        }

        public void StopBgm()
        {
#if !UNITY_EDITOR || SOUND_TEST
            SoundMgr.Instance.StopBGM();
#endif
        }

        public void PlaySfx(string id)
        {
#if !UNITY_EDITOR || SOUND_TEST
            SoundMgr.Instance.PlayEffect(id);
#endif
        }
        void Start()
        {
#if !UNITY_EDITOR || SOUND_TEST
            SoundMgr.Instance.PlayBGM("bgm_Obstacle");
#endif
        }
    }
}