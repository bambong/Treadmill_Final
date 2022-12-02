using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gymchair.Core
{
    public class Framework : MonoBehaviour
    {
        static Framework instance;
        public static Framework Instance { get => instance; }

        private GameObject _objMgrPool;
        private event Action<object> _managerListener;

        private void Awake()
        {
            if (instance)
            {
                Destroy(instance.gameObject);
            }

            instance = this;
            //DontDestroyOnLoad(gameObject);

            /// <summary>
            /// Init Call Function
            /// </summary>
            /// 

            InitProjectSetting();
            InitObjectPool();
            InitManager();
        }

        /// <summary>
        /// Init Object Pool
        /// </summary>
        private void InitObjectPool()
        {
            if (this._objMgrPool)
                Destroy(this._objMgrPool);

            this._objMgrPool = new GameObject("MgrPool");
            //DontDestroyOnLoad(this._objMgrPool);
        }

        /// <summary>
        /// Init Manager
        /// </summary>
        private void InitManager()
        {
            if (this._objMgrPool)
            {
                this._objMgrPool.AddComponent<Mgr.SceneMgr>();
                this._objMgrPool.AddComponent<Mgr.NetworkMgr>();
                this._objMgrPool.AddComponent<Mgr.ResourceMgr>();
                this._objMgrPool.AddComponent<Mgr.UIMgr>();
                this._objMgrPool.AddComponent<Mgr.DataMgr>();
                this._objMgrPool.AddComponent<Mgr.BluetoothMgr>();
                this._objMgrPool.AddComponent<Mgr.SoundMgr>();
            }
        }

        /// <summary>
        /// Init Project Setting
        /// </summary>
        private void InitProjectSetting()
        {
            Application.targetFrameRate = 60;
            Application.runInBackground = true;
        }

        #region Public Methods
        public void AddedManagerListener(Mgr.Listener.IOnManagerListener listener)
        {
            if (listener != null)
            {
                this._managerListener += listener.OnCoreMessage;
            }
        }

        public void RemoveManagerListener(Mgr.Listener.IOnManagerListener listener)
        {
            if (listener != null)
            {
                this._managerListener -= listener.OnCoreMessage;
            }
        }
        #endregion
    }
}
