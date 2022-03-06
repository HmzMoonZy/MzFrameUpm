using System;
using System.Collections.Generic;
using UnityEngine;
namespace MzFrame
{
    /// <summary>
    /// 这是用以查找组件的结构体。
    /// </summary>
    public struct ViewCompFinder
    {
        private readonly Dictionary<Type, object> _cacheOfComponents;

        private readonly Transform _transform;
        public ViewCompFinder(Transform transform)
        {
            this._transform = transform;
            _cacheOfComponents = new Dictionary<Type, object>();
        }
        
        public T Comp<T>() where T : MonoBehaviour
        {
            var t = typeof(T);
            if (!_cacheOfComponents.TryGetValue(t, out object comp))
            {
                comp = _transform.GetComponent<T>();
                if (comp != null)
                {
                    _cacheOfComponents.Add(t, comp);
                }
            }

            return (T)comp;
        }
    }
}