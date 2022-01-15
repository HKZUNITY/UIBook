using UnityEngine;

namespace HKZ
{
    public class DragRightPage : DragPageBase
    {
        public DragRightPage(UIBook uIBook, BookModels bookModels, TheDraggingPage frontPage, TheDraggingPage backPage, Vector3 startPos)
            : base(uIBook, bookModels, frontPage, backPage, startPos)
        {

        }

        protected override Vector3 GetBookCorner()
        {
            return bookModels.RightCorner;
        }

        protected override Vector2 GetCilppingMaskPivot()
        {
            return new Vector2(1, bookModels.ClippingPivotY);
        }

        protected override Vector2 GetPagePivot()
        {
            return Vector2.zero;
        }

        protected override Vector3 GetValidAngle(float angle)
        {
            return Vector3.forward * angle;
        }
    }
}
