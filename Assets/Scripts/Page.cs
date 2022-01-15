using System;
using UnityEngine;
using UnityEngine.UI;

namespace HKZ
{
    public class Page : MonoBehaviour
    {
        //引用
        public Shadow Shadow
        {
            get;
            private set;
        }
        //组件
        private Func<int, Sprite> getSprite;
        private Image img_Page;
        protected RectTransform rect;
        //数据
        /// <summary>
        /// 页数
        /// </summary>
        private int id;
        public int ID
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
                ChangeSprite(value);
            }
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="getSprite"></param>
        public virtual void Init(Func<int,Sprite> getSprite)
        {
            rect = GetComponent<RectTransform>();
            this.getSprite = getSprite;
            img_Page = GetComponent<Image>();
            Shadow = transform.GetChild(0).gameObject.AddComponent<Shadow>();
        }
        /// <summary>
        /// 赋值Sprite
        /// </summary>
        /// <param name="id"></param>
        private void ChangeSprite(int id)
        {
            img_Page.sprite = getSprite(id);
        }
        /// <summary>
        /// 设置活跃状态
        /// </summary>
        /// <param name="isActive"></param>
        public void SetActiveState(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
        /// <summary>
        /// 设置父物体
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="worldPosStays"></param>
        public void SetParent(Transform parent,bool worldPosStays)
        {
            transform.SetParent(parent, worldPosStays);
        }
    }
}
