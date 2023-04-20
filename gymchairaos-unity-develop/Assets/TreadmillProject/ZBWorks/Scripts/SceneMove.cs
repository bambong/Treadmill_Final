using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZB
{
    public class SceneMove : MonoBehaviour
    {
        public void OnBtnClikced_SelectGame()
        {
            Time.timeScale = 1;
            bambong.TransitionManager.Instance.SceneTransition(bambong.E_SceneName.SelectGame.ToString());
        }
    }
}