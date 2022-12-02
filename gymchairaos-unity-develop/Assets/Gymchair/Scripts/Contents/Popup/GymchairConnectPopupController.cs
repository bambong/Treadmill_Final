using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gymchair.Contents.Popup
{
    public class GymchairConnectPopupController : MonoBehaviour
    {
        [SerializeField] GameObject _wait;
        [SerializeField] GameObject _success;
        [SerializeField] GameObject _spin;

        Vector3 _spinEuler = Vector3.zero;

        public static GymchairConnectPopupController Create()
        {
            GameObject prefabs = Resources.Load<GameObject>("Prefabs/GymchairConnectPopup");
            GameObject obj = Instantiate(prefabs);
            GymchairConnectPopupController script = obj.GetComponent<GymchairConnectPopupController>();
            return script;
        }

        private void Update()
        {
            if (this._wait.activeSelf)
            {
                this._spinEuler += Vector3.forward * -360 * Time.deltaTime;
                this._spin.transform.rotation = Quaternion.Euler(_spinEuler);
            }
        }

        public void ShowSuccess()
        {
            this._wait.SetActive(false);
            this._success.SetActive(true);
        }
    }
}