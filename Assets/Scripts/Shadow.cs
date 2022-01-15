using UnityEngine;
using UnityEngine.UI;

namespace HKZ
{
    public class Shadow : MonoBehaviour
    {
        //组件
        private RectTransform rect;
        private Image image;
        private Color defaultColor = new Color(1, 1, 1, 0.5f);
        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="size"></param>
        public void Init(Vector2 size)
        {
            rect = GetComponent<RectTransform>();
            image = GetComponent<Image>();
            rect.sizeDelta = size;//设置大小
            image.color = defaultColor;//设置颜色
        }
        /// <summary>
        /// 设置shadow的位置
        /// </summary>
        /// <param name="target"></param>
        public void SetShadowFollow(Transform target)
        {
            transform.position = target.position;
            transform.rotation = target.rotation;
        }

        public void ResetShadowDate()
        {
            rect.anchoredPosition = Vector2.zero;
            rect.localEulerAngles = Vector2.zero;
        }
    }
}
