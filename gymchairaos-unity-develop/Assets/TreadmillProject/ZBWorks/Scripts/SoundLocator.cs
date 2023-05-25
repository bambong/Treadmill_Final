using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gymchair.Core.Mgr;

namespace ZB
{
    public class SoundLocator : MonoBehaviour
    {
        public static SoundLocator Instance;

        [SerializeField] string m_bgmID;

        public void PlayBgm(string id)
        {
#if !UNITY_EDITOR || SOUND_TEST
            if (SoundMgr.Instance != null) 
                SoundMgr.Instance.PlayBGM(id);
#endif
        }

        public void StopBgm()
        {
#if !UNITY_EDITOR || SOUND_TEST
            if (SoundMgr.Instance != null)
                SoundMgr.Instance.StopBGM();
#endif
        }

        public void PlaySfx(string id)
        {
#if !UNITY_EDITOR || SOUND_TEST
            if (SoundMgr.Instance != null)
                SoundMgr.Instance.PlayEffect(id);
#endif
        }
        void Start()
        {
            Instance = this;

#if !UNITY_EDITOR || SOUND_TEST
            if (SoundMgr.Instance != null)
                SoundMgr.Instance.PlayBGM(m_bgmID);
#endif
        }
    }
}