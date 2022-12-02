using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using bambong;
namespace jiho
{
    public class ButtonManager : MonoBehaviour
    {
        [SerializeField] private GameObject exit_obj;
        [SerializeField] private FunctionManager functionManager;
        [SerializeField] private UIManager uiManager;
        private bool isExit = false;

        public void Active(GameObject _exit)
        {
            isExit = !isExit;
            _exit.SetActive(isExit);
        }

        public void OnRestartButtonDown() 
        {
            uiManager.GameOverActive(false);
            TransitionManager.Instance.SceneTransition(E_SceneName.Obstacle_GameScene.ToString());
        }
        public void OnExitButtonDown()
        {
            uiManager.GameOverActive(false);
            TransitionManager.Instance.SceneTransition(E_SceneName.SelectGame.ToString());
        }
    }
}


