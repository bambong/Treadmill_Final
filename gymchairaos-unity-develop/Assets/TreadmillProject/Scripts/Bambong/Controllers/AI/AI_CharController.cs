using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bambong 
{
    public class AI_CharController : MonoBehaviour
    {
        [SerializeField]
        private GameObject destination;

        [SerializeField]
        private Animator animator;


        private WheelChairAnimateController animateController;
        private AI_StateController stateController;

        private SpeedRevisionData speedRevisionData;
        private Color color;
        private float moveSpeed =1f;
        private float curMoveSpeed =1f;
        public AI_StateController StateController { get => stateController; }
        private Coroutine speedChangeCo;
        public void Init(Color color, SpeedRevisionData revisionData)
        {
            this.color = color;
            speedRevisionData = revisionData;
            SetMeshMat();


        }
        private void SetMeshMat() 
        {
            var meshs = transform.GetComponentsInChildren<MeshRenderer>();

            foreach(var mesh in meshs)
            {
                mesh.material.color = color;
            }
        }
        private void Awake()
        {
        
            animateController = new WheelChairAnimateController(animator);
            stateController = new AI_StateController(this);
        }
        public void MoveUpdate() 
        {
            //var dir = destination.transform.position - transform.position;
            //var speed = speedRevisionData.GetCollectAreaData(GameSceneManager.Instacne.GetCurDistanceRatio(transform));
            animateController.SetMoveAnimSpeed(curMoveSpeed / GameSceneManager.Instance.GetAIAnimSpeedRevision());
            transform.position += transform.forward * curMoveSpeed * Time.deltaTime;
            ChangeSpeed();
        }
  
        private void ChangeSpeed() 
        {
            var speedData = speedRevisionData.GetCollectAreaData(GameSceneManager.Instance.GetCurDistanceRatio(transform));
            if(moveSpeed != speedData.speed)
            {
                moveSpeed = speedData.speed;
                if(speedChangeCo != null) 
                {
                    StopCoroutine(speedChangeCo);
                }
                speedChangeCo = StartCoroutine(SpeedIncrease(speedData));                   
            }
        
        }
        public IEnumerator SpeedIncrease(SpeedChangeData speedData) 
        {
            float process = 0;
            float startSpeed = curMoveSpeed;
            while(process < speedData.time) 
            {
                var td = Time.deltaTime;
                curMoveSpeed =  Mathf.Lerp(startSpeed,moveSpeed,(process / speedData.time));
                process += td;
                yield return null;
            }
            curMoveSpeed = speedData.speed;
            speedChangeCo = null;
        }
        public void AnimateMove(bool trigger)
        {
            animateController.PlayAnimateMove(trigger);
        }
        private void Update()
        {
            stateController.Update();
        }
        private void FixedUpdate()
        {
            stateController.FixedUpdate();
        }
    }
}
