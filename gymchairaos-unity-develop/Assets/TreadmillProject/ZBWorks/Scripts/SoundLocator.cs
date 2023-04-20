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
            //try
            //{
                SoundMgr.Instance.PlayBGM(id);
            //}
            //catch { }
        }

        public void StopBgm()
        {
           // try
           // {
                SoundMgr.Instance.StopBGM();
           // }
           // catch { }
        }

        public void PlaySfx(string id)
        {
          //  try
       //     {
                SoundMgr.Instance.PlayEffect(id);
       //     }
          //  catch { }
        }

        void Start()
        {
            //게임실행 음악
          //  try
           // {
                SoundMgr.Instance.PlayBGM("bgm_Obstacle");
         //   }
         //   catch { }
        }
    }
}