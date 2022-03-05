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
        private Dictionary<Type, object> _cacheOfConponents;

        public readonly Transform transform;

        public ViewCompFinder(Transform transform)
        {
            this.transform = transform;
            _cacheOfConponents = new Dictionary<Type, object>();
        }
        
        public T Comp<T>() where T : Component
        {
            var t = typeof(T);
            if (!_cacheOfConponents.TryGetValue(t, out object comp))
            {
                comp = transform.GetComponent<T>();
                if (comp != null)
                {
                    _cacheOfConponents.Add(t, comp);
                }
            }

            return (T)comp;
        }
    }
}