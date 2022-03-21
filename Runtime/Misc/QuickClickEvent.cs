using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MzFrame
{
    public class QuickClickEvent : MonoBehaviour, IPointerClickHandler
    {
        private Action<PointerEventData> _action;

        public void AddListener(Action<PointerEventData> action)
        {
            this._action = action;
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            _action?.Invoke(eventData);
        }
    }
}