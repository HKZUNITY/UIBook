using System;
using UnityEngine;

namespace HKZ
{
    public interface IDragPage
    {
        void BeginDragPage(Vector3 point);

        void UpdateDrag();

        void EndDragPage(Action complete);
    }

    public abstract class DragPageBase
    {
        private UIBook uIBook;
        protected BookModels bookModels;
        private TheDraggingPage frontPage;
        private TheDraggingPage backPage;
        private Vector3 startPos;
        private RectTransform clippingMask;

        public DragPageBase(UIBook uIBook, BookModels bookModels,
            TheDraggingPage frontPage, TheDraggingPage backPage, Vector3 startPos)
        {
            this.uIBook = uIBook;
            this.bookModels = bookModels;
            this.frontPage = frontPage;
            this.backPage = backPage;
            this.startPos = startPos;
            clippingMask = uIBook.GetClippingMask();
        }
        /// <summary>
        /// 开始拖拽
        /// </summary>
        /// <param name="point"></param>
        public void BeginDragPage(Vector3 point)
        {
            //设置中心点
            clippingMask.pivot = GetCilppingMaskPivot();

            bookModels.ClickPoint = point;

            frontPage.BeginDragPage(startPos, GetPagePivot());
            backPage.BeginDragPage(startPos, GetPagePivot());
        }
        protected abstract Vector2 GetCilppingMaskPivot();

        protected abstract Vector2 GetPagePivot();

        /// <summary>
        /// 拖拽过程
        /// </summary>
        public void UpdateDrag()
        {
            bookModels.ClickPoint = uIBook.GetClickPos();

            backPage.SetParent(clippingMask, true);
            frontPage.SetParent(uIBook.transform, true);

            bookModels.CurrentPageCorner = uIBook.CulculateDraggingCorner(bookModels.ClickPoint);

            Vector3 bottomCrossPoint = UpdateClippingMask();

            UpdateBackSide(bottomCrossPoint);

            frontPage.SetParent(clippingMask, true);
            frontPage.ResetShadowDate();
            frontPage.transform.SetAsFirstSibling();

            backPage.SetShadowFollow(clippingMask);
        }

        private Vector3 UpdateClippingMask()
        {
            Vector3 bottomCrossPoint;
            float angle = uIBook.CulculateFoldAngle(bookModels.CurrentPageCorner, GetBookCorner(), out bottomCrossPoint);
            if (angle > 0)
            {
                angle = angle - 90;
            }
            else
            {
                angle = angle + 90;
            }

            clippingMask.eulerAngles = Vector3.forward * angle;
            clippingMask.localPosition = bottomCrossPoint;

            return bottomCrossPoint;
        }

        protected abstract Vector3 GetBookCorner();

        private void UpdateBackSide(Vector3 bottonCrossPoint)
        {
            backPage.transform.position = uIBook.Local2WorldPos(bookModels.CurrentPageCorner);
            Vector3 offset = bottonCrossPoint - bookModels.CurrentPageCorner;

            float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
            backPage.transform.eulerAngles = GetValidAngle(angle);
        }

        protected abstract Vector3 GetValidAngle(float angle);

        public void EndDragPage(Action complete)
        {
            Vector3 corner;
            if (bookModels.CurrentPageCorner.x>bookModels.BottomCenter.x)
            {
                corner = bookModels.RightCorner;
            }
            else
            {
                corner = bookModels.LeftCorner;
            }
            uIBook.FlipAni(corner, complete);
        }
    }
}
