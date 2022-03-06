using System;
using UnityEngine;

namespace MzFrame
{
    public class ViewInfo
    {
        /// <summary>
        /// UI 界面的预制体.
        /// </summary>
        public GameObject ViewObject { get; private set; }

        /// <summary>
        /// 根据规范, UI 界面本身必须具有一个 Canvas 组件.
        /// </summary>
        public Canvas ViewCanvas { get; private set; }

        /// <summary>
        /// UI 的名称, 必须是唯一标识.
        /// 可以用作查找 Prefab 和 管理的 ID.
        /// </summary>
        public string ViewName { get; private set; }

        /// <summary>
        /// Prefab 的特性, 在 Editor 中配置.
        /// </summary>
        public ViewConfig ViewConfig { get; private set; }

        /// <summary>
        /// View 在该 SortLayer 下的排序顺序.
        /// </summary>
        public int SortOrder
        {
            get => ViewCanvas.sortingOrder;
            set => ViewCanvas.sortingOrder = value;
        }

        /// <summary>
        /// View 所在的层级.
        /// </summary>
        public Constant.ViewSort Layer => ViewConfig.Layer;
        
        /// <summary>
        /// UI 是否是可见的.
        /// </summary>
        public bool IsVisible
        {
            get
            {
                var parent = ViewObject.transform.parent;
                return parent != null && parent != ViewManager.HiddenTransform;
            }
        }

        public ViewInfo(GameObject viewGo, string viewName)
         {
             ViewObject = viewGo;
             ViewName = viewName;
             //PrefabName = ViewName.Replace("Info", "");

             ViewCanvas = viewGo.GetComponent<Canvas>();
             ViewConfig = viewGo.GetComponent<ViewConfig>();
         }
         
        #region LifeCycle

        /// <summary>
        /// 当 ViewInfo 被加载时,初始化前被调用.
        /// </summary>
        public virtual void OnCreated()
        {
        }

        /*/// <summary>
        /// 当 ViewInfo 的可见性为 True 时调用.
        /// </summary>
        public virtual void OnOpening()
        {
        }*/

        /// <summary>
        /// 当 ViewInfo 被展示后调用.
        /// </summary>
        /// <param name="args"></param>
        public virtual void OnOpened(params object[] args)
        {
        }

        /// <summary>
        /// 由 UIManager 每帧调用
        /// </summary>
        public virtual void Update(float deltaTime)
        {
        }

        /*/// <summary>
        /// 当 ViewInfo 正准备关闭时调用
        /// </summary>
        public virtual void OnClosing()
        {
        }*/

        /// <summary>
        /// 当 ViewInfo 被关闭时调用,不论它是 Hidden 还是 Destroy.
        /// </summary>
        public virtual void OnClose()
        {
        }

        /// <summary>
        /// 如果 ViewInfo 被关闭后被缓存,则调用
        /// </summary>
        public virtual void OnHidden()
        {
        }

        /// <summary>
        /// 如果 ViewInfo 关闭后不被缓存,则调用
        /// </summary>
        public virtual void OnDestroy()
        {
        }

        #endregion
    }
}