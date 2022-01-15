using System;
using UnityEngine;

namespace HKZ
{
    public class TheDraggingPage : Page
    {
        public override void Init(Func<int, Sprite> getSprite)
        {
            base.Init(getSprite);
            SetActiveState(false);
        }
        /// <summary>
        /// 开始拖动
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="pivot"></param>
        public void BeginDragPage(Vector3 pos,Vector2 pivot)
        {
            SetActiveState(true);
            rect.pivot = pivot;
            transform.position = pos;
            
            transform.eulerAngles = Vector3.zero;
        }
        /// <summary>
        /// 初始化Shadow的大小
        /// </summary>
        /// <param name="size"></param>
        public void InitShadow(Vector2 size)
        {
            Shadow.Init(size);
        }
        /// <summary>
        /// 让shadow跟随书页
        /// </summary>
        /// <param name="target"></param>
        public void SetShadowFollow(Transform target)
        {
            Shadow.SetShadowFollow(target);
        }

        public void ResetShadowDate()
        {
            Shadow.ResetShadowDate();
        }
    }
}
