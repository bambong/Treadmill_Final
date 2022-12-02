using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bambong
{
    public class PanelController : MonoBehaviour
    {
        public virtual void Open()
        {
            gameObject.SetActive(true);
        }
        public virtual void Close()
        {
            gameObject.SetActive(false);
        }
    }
}
