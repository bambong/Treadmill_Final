using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bambong
{
    public class PausePanelController : PanelController
    {
        public override void Open()
        {
            Time.timeScale = 0;
            base.Open();
        }
        public override void Close()
        {
            Time.timeScale = 1;
            base.Close();
        }

    }

}

