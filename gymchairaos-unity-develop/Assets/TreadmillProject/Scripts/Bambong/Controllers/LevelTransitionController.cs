using Gymchair.Core.Mgr;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace bambong
{
    public class LevelTransitionController : MonoBehaviour
    {
        [SerializeField]
        private LevelButtonController[] levelButtonControllers;
        [SerializeField]
        private LevelData levelData;
        [SerializeField]
        private SpeedMenuStartPanelController speedMenuStartPanelController;


        [Header("FOR DEBUGGER")]
        [SerializeField]
        private bool IsPrefabsClear = false;
        private void Awake()
        {
#if UNITY_EDITOR
            if(IsPrefabsClear) 
            {
                PlayerPrefs.DeleteAll();            
            }
#endif
            Init();
        }
        private void Init()
        {
            for(int i = 0; i < levelButtonControllers.Length; ++i) 
            {
                GameManager.Instance.InitLevel(i);
                var text = levelData.levelInfos[i].distance.ToString() + " M";
                levelButtonControllers[i].SetButton((int level)=>
                {
                    speedMenuStartPanelController.SetButton(level);
                },i,text);
                
            }
        }
        public void MenuTransition() 
        {
            SoundMgr.Instance.PlayEffect("touch");
            TransitionManager.Instance.SceneTransition(E_SceneName.SelectGame.ToString());
        }
    }
}

