using System;
using UnityEngine;

namespace MzFrame
{
    public class ViewUpdater : MonoBehaviour
    {
        private void Update()
        {
            ViewManager.Update();
        }
    }
}