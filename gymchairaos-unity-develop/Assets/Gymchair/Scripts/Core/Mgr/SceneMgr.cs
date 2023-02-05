using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gymchair.Core.Mgr
{
    public enum eScene
    {
        Init,
        Login,
        Main,
        Join,
        Game,
        Record,
        Result,
        TargetResult,
        TestResult,
        Modify
    }

    public class SceneMgr : Behaviour.ManagerBehaviour<SceneMgr>
    {
        /// <summary>
        /// RuntimeInitializeOnLoadMethod
        /// First Init Scene Load
        /// </summary>
        [RuntimeInitializeOnLoadMethod]
        static void FirstLoad()
        {
            var scene = SceneManager.GetActiveScene();

            if (scene != null && !scene.name.Equals(eScene.Init.ToString()))
            {
                SceneManager.LoadScene(eScene.Init.ToString());
            }
        }

        List<string> _scenes = new List<string>();
        eScene _startScene = eScene.Login;

        private void Awake()
        {
            LoadSceneAsync(this._startScene.ToString(), LoadSceneMode.Additive);
        }

        /// <summary>
        /// 이후 구조는 선 로드 후 처리 방식으로 변경 예정.
        /// 구조적인 수정 필요.
        /// </summary>
        IEnumerator OnLoadScene(string name, LoadSceneMode mode, Action success)
        {
            if (mode == LoadSceneMode.Single)
            {
                lock (this._scenes)
                {
                    this._scenes.Clear();
                }
            }

            var async = SceneManager.LoadSceneAsync(name, mode);
            yield return async;

            lock (this._scenes)
            {
                this._scenes.Add(name);
            }

            success?.Invoke();
        }

        IEnumerator OnUnLoadScene(string name, Action success)
        {
            this._scenes.Remove(name);
            yield return SceneManager.UnloadSceneAsync(name);
            success?.Invoke();
        }

        public void LoadSceneAsync(string name, LoadSceneMode mode = LoadSceneMode.Single, Action success = null)
        {
            StartCoroutine(OnLoadScene(name, mode, success));
        }

        public void UnLoadSceneAsync(string name, Action success = null)
        {
            StartCoroutine(OnUnLoadScene(name, success));
        }

        public void SetActiveScene(string name)
        {
            if (this._scenes.Contains(name))
            {
                Scene scene = SceneManager.GetSceneByName(name);
                SceneManager.SetActiveScene(scene);
            }
        }

        public string GetActiveScene()
        {
            Scene activeScene = SceneManager.GetActiveScene();
            return activeScene.name;
        }

        public override void OnCoreMessage(object msg)
        {
        }
    }
}