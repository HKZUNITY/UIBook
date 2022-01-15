using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HKZ
{
    public class DragButton : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private Action onBeginDrag;
        private Action onDrag;
        private Action onEndDrag;

        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="onBeginDrag"></param>
        /// <param name="onDrag"></param>
        /// <param name="onEndDrag"></param>
        public void Init(Action onBeginDrag, Action onDrag, Action onEndDrag)
        {
            this.onBeginDrag = onBeginDrag;
            this.onDrag = onDrag;
            this.onEndDrag = onEndDrag;
        }
        /// <summary>
        /// 开始拖动
        /// </summary>
        /// <param name="eventData"></param>
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (onBeginDrag!=null)
            {
                onBeginDrag();
            }
        }
        /// <summary>
        /// 持续拖动
        /// </summary>
        /// <param name="eventData"></param>
        public void OnDrag(PointerEventData eventData)
        {
            if (onDrag != null)
            {
                onDrag();
            }
        }
        /// <summary>
        /// 结束拖动
        /// </summary>
        /// <param name="eventData"></param>
        public void OnEndDrag(PointerEventData eventData)
        {
            if (onEndDrag!=null)
            {
                onEndDrag();
            }
        }
    }
}
