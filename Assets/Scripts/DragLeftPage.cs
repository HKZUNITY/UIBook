using UnityEngine;

namespace HKZ
{
    public class DragLeftPage : DragPageBase
    {
        public DragLeftPage(UIBook uIBook, BookModels bookModels, TheDraggingPage frontPage, TheDraggingPage backPage, Vector3 startPos)
            : base(uIBook, bookModels, frontPage, backPage, startPos)
        {

        }

        protected override Vector3 GetBookCorner()
        {
            return bookModels.LeftCorner;
        }

        protected override Vector2 GetCilppingMaskPivot()
        {
            return new Vector2(0, bookModels.ClippingPivotY);
        }

        protected override Vector2 GetPagePivot()
        {
            return Vector2.right;
        }

        protected override Vector3 GetValidAngle(float angle)
        {
            return Vector3.forward * (angle + 180);
        }
    }
}
