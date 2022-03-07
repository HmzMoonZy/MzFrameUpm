using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace MzFrameUpm
{
    public struct ResourcesRequest<T>
    {
        public string assetName;
        public List<string> labels;
        public Action<T> onLoad;
    }

    /// <summary>
    /// 基于 Addressable Assets System 的加载框架.
    /// </summary>
    public static class ResourcesManager
    {
        /// <summary>
        /// 
        /// </summary>
        public static GameObject Instantiate(ResourcesRequest<GameObject> request, Transform parent = null,
            bool instantiateInWorldSpace = false)
        {
            var handle = Addressables.InstantiateAsync(request.assetName, parent, instantiateInWorldSpace);
            handle.WaitForCompletion();
            if (handle.Status == AsyncOperationStatus.Succeeded && handle.IsDone)
            {
                return handle.Result;
            }

            Debug.LogError(handle.OperationException.Message);
            return null;
        }
    }
}