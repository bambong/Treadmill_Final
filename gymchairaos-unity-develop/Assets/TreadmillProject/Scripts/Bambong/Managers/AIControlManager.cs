using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace bambong 
{
    public class AIControlManager : GameObjectSingletonDestroy<AIControlManager>, IInit
    {
        [SerializeField]
        private GameObject aiCharPrefab;

        [SerializeField]
        private GameObject aiRivalPrefab;
        [SerializeField]
        private int aiInstanceCount = 3;
        [SerializeField]
        private int airivalCount = 1;
        [SerializeField]
        private List<Material> mats;

        private List<AI_CharController> characterAIs = new List<AI_CharController>();
        private bool isInit;
        private readonly float CharSpaceX = 3;
        public void Init()
        {
            if(isInit)
            {
                return;
            }
            aiInstanceCount = GameSceneManager.Instance.GetCurLevelInfo().AI_Dtatas.Count - airivalCount;
            
            isInit = true;
            GenerateAI();
        }
        private void Awake()
        {
            Init();
        }
        public void GenerateAI() 
        {
            List<int> pickColor = Enumerable.Range(0,aiInstanceCount+airivalCount).ToList();
            var rot = GameSceneManager.Instance.Player.transform.rotation;
            var pos = GameSceneManager.Instance.Player.transform.position;
           
            for (int i = 0; i < airivalCount; ++i)
            {
                pos.x -= CharSpaceX;

                var pickColorIndex = Random.Range(0, pickColor.Count);
                var colorIndex = pickColor[pickColorIndex];
                pickColor.RemoveAt(pickColorIndex);

                var aiController = Instantiate(aiRivalPrefab, pos, rot, null).GetComponent<AI_Rival>();
                aiController.Init(mats[colorIndex], GameSceneManager.Instance.GetCurLevelInfo().AI_Dtatas[i]);
                characterAIs.Add(aiController);
            }

            for (int i = 0; i < aiInstanceCount; ++i) 
            {
                pos.x -= CharSpaceX;

                var pickColorIndex = Random.Range(0,pickColor.Count);
                var colorIndex = pickColor[pickColorIndex];
                pickColor.RemoveAt(pickColorIndex);
                
                var aiController = Instantiate(aiCharPrefab,pos,rot,null).GetComponent<AI_CharController>();
                aiController.Init(mats[colorIndex],GameSceneManager.Instance.GetCurLevelInfo().AI_Dtatas[i]);
                characterAIs.Add(aiController);
            }
        }
        public void SetStateAIsRun()
        {
            foreach(var ai in characterAIs)
            {
                ai.StateController.ChangeState(AI_Run.Instance);
            }
        }
        public void SetStateAIsStop()
        {
            foreach(var ai in characterAIs)
            {
                ai.StateController.ChangeState(AI_Stop.Instance);
            }
        }
        public void SetStateAIsIdle()
        {
            foreach(var ai in characterAIs)
            {
                ai.StateController.ChangeState(AI_Idle.Instance);
            }
        }

    }
}

