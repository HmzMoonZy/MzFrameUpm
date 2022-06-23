using System;
using System.Collections.Generic;
using UnityEngine;
namespace MzFrame
{
    /// <summary>
    /// 这是用以查找组件的结构体。
    /// </summary>
    public class ViewCompFinder
    {
        public readonly Transform transform;

        public readonly GameObject gameObject;
        
        private readonly Dictionary<Type, object> _cacheOfComponents;
        
        public ViewCompFinder(Transform transform)
        {
            this.transform = transform;
            this.gameObject = transform.gameObject;
            _cacheOfComponents = new Dictionary<Type, object>();
        }
        
        public T Comp<T>() where T : Component
        {
            var t = typeof(T);
            if (!_cacheOfComponents.TryGetValue(t, out object comp))
            {
                comp = transform.GetComponent<T>();
                if (comp != null)
                {
                    _cacheOfComponents.Add(t, comp);
                }
            }

            return (T)comp;
        }

        public ViewCompFinder Instantiate(Transform parent, bool worldPositionStays = false)
        {
           var t = UnityEngine.Object.Instantiate(gameObject, parent, worldPositionStays).transform;
           return new ViewCompFinder(t);
        }
    }
}