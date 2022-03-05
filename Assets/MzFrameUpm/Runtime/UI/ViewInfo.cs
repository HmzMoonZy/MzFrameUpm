using UnityEngine;

namespace MzFrame
{
    public class ViewInfo
    {
        /// <summary>
        /// UI 界面的预制体.
        /// </summary>
        protected GameObject ViewObject { get; set; }

        /// <summary>
        /// 根据规范, UI 界面本身必须具有一个 Canvas 组件.
        /// </summary>
        protected Canvas ViewCanvas { get; set; }

        /// <summary>
        /// UI 的名称, 必须是唯一标识.
        /// 可以用作查找 Prefab 和 管理的 ID.
        /// </summary>
        protected string ViewName { get; set; }

        /// <summary>
        /// Prefab 的特性, 在 Editor 中配置.
        /// </summary>
        public ViewConfig ViewConfig { get; private set; }

        /// <summary>
        /// UI 在该 SortLayer 下的排序顺序.
        /// </summary>
        public int SortOrder
        {
            get => ViewCanvas.sortingOrder;
            set => ViewCanvas.sortingOrder = value;
        }

        //public Constant.ViewSort Layer
        // {
        //     //get => ViewConfig.
        // }

        /// <summary>
        /// UI 是否是可见的.
        /// </summary>
        public bool IsVisible
        {
            //get => Transform.parent && Transform.parent != UIManager.HideTransform;
            get => true;
        }


        // public UIInfo(GameObject uigo, string name, UIView viewer)
        // {
            // _uigo = uigo;
            // _uigo.name = name;
            // _name = name;
            //
            // _view = viewer;
            //
            // Config = _uigo.GetComponent<UIConfig>();

            // TODO Canvas 的初始化工作由 Editor 保证.
            // _canvas = _uigo.GetComponent<Canvas>();
            // _canvas.renderMode = RenderMode.ScreenSpaceCamera;
            // _canvas.worldCamera = UIManager.UICamera;
            // _canvas.pixelPerfect = true;
            // _canvas.overrideSorting = false;
            // _canvas.sortingLayerID = (int)Layer;
        //}
        
        
        
        #region LifeCycle

        /// <summary>
        /// 当 ViewInfo 被加载时,初始化前被调用.
        /// </summary>
        public virtual void OnCreated()
        {
        }

        /// <summary>
        /// 当 ViewInfo 的可见性为 True 时调用.
        /// </summary>
        public virtual void OnOpening()
        {
        }

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

        /// <summary>
        /// 当 ViewInfo 正准备关闭时调用
        /// </summary>
        public virtual void OnClosing()
        {
        }

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