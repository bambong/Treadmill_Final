using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace bambong 
{
    public class DestinationUIController : MonoBehaviour
    {
        [SerializeField]
        private RectTransform flagArea;
        [SerializeField]
        private Image playerFlag;
        
        [SerializeField]
        private DestinationController destinationController;

        private PlayerController player;
        private float roadDistance;
        private float distanceRatio;
        public void Init()
        {
            player = GameSceneManager.Instance.Player;
            roadDistance = flagArea.sizeDelta.x;
            distanceRatio = roadDistance / GameSceneManager.Instance.CurRoadDistance;
        }
 
        public void UpdateActive()
        {
            var targetPosX = (GameSceneManager.Instance.CurRoadDistance - (destinationController.transform.position.z - player.transform.position.z)) * distanceRatio;
            var pos = playerFlag.rectTransform.anchoredPosition;

            pos.x = Mathf.Min(roadDistance, targetPosX);
            playerFlag.rectTransform.anchoredPosition = pos;
        }


    }

}
