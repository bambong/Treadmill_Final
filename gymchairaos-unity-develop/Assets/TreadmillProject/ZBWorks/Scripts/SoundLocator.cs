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
            Managers.Sound.PlayBGM(id);
#endif
        }

        public void StopBgm()
        {
#if !UNITY_EDITOR || SOUND_TEST
            Managers.Sound.StopBGM();
#endif
        }

        public void PlaySfx(string id)
        {
#if !UNITY_EDITOR || SOUND_TEST
            Managers.Sound.PlayEffect(id);
#endif
        }
        void Start()
        {
            Instance = this;

#if !UNITY_EDITOR || SOUND_TEST
            Managers.Sound.PlayBGM(m_bgmID);
#endif
        }
    }
}