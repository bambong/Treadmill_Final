using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bambong 
{
    public class EndPanelController : PanelController
    {
        private readonly float VISIBLE_TIME = 0.5f;

        public override void Open()
        {
            base.Open();
            StartCoroutine(WaitClose());
        }

        IEnumerator WaitClose() 
        {
            yield return new WaitForSeconds(VISIBLE_TIME);

            Close();
        
        }

        public override void Close()
        {
            base.Close();
            UISceneManager.Instance.ClearPanelOpen();
        }



    }

}
