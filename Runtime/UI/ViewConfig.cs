using Sirenix.OdinInspector;
using UnityEngine;

namespace MzFrame
{
    public class ViewConfig : MonoBehaviour
    {
        [Title("层级")]
        [EnumPaging]
        public Constant.ViewSort Layer;

        [Title("自动遮罩")]
        public bool EnableAutoMask = true;
        [ShowIf("EnableAutoMask")]
        public bool ClickMaskTriggerClose = false;

        [Title("动画")]
        public bool EnableAutoAnimation = false;

        [Title("是否缓存")]
        public bool IsCache = true;


        #if UNITY_EDITOR
        [Button(ButtonSizes.Large)]
        private void 编写逻辑层()
        {
            Debug.Log("CCCC!");
        }
        #endif
    }
}