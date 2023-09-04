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
            Managers.Scene.LoadScene(E_SceneName.SelectGame);
        }
    }
}