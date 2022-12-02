using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gymchair.Core.Mgr
{
    public class ResourceMgr : Behaviour.ManagerBehaviour<ResourceMgr>
    {
        IEnumerator OnLoadAsync<T>(string name, Action<Result<T>> result) where T : UnityEngine.Object
        {
#if UNITY_EDITOR
            var request = Resources.LoadAsync<T>(name);
            yield return request;
            result?.Invoke(Result<T>.Success(request.asset as T));
#else
            var request = Resources.LoadAsync<T>(name);
            yield return request;
            result?.Invoke(Result<T>.Success(request.asset as T));
        //ResourceRequest request = Resources.LoadAsync<TextAsset>(fileName);
        //yield return request;

        //TextAsset textAsset = request.asset as TextAsset;
        //byte[] data = encrypt.RSADecrypt(textAsset.bytes);
        //AssetBundleCreateRequest createRequest = AssetBundle.LoadFromMemoryAsync(data);
        //yield return createRequest;
        //AssetBundle assetBundle = createRequest.assetBundle;
        //AssetBundleRequest assetRequest = assetBundle.LoadAssetAsync<T>("MyObject");
        //action?.Invoke(Result<T>.Success(assetRequest.asset as T));
#endif
        }

        public void LoadAsync<T>(string name, Action<Result<T>> result) where T : UnityEngine.Object
        {
            StartCoroutine(OnLoadAsync<T>(name, result));
        }

        public static void LoadResourceAsync<T>(string name, Action<Result<T>> result) where T : UnityEngine.Object
        {
            if (Instance)
            {
                Instance.StartCoroutine(Instance.OnLoadAsync<T>(name, result));
            }
            else
            {
                result?.Invoke(Result<T>.Error(eContent.ERROR_UNKNOWN, "instance error"));
            }
        }


        public override void OnCoreMessage(object msg)
        {
        }
    }
}