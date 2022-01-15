using System;
using System.Collections;
using UnityEngine;

namespace HKZ
{
    public class UIBook : MonoBehaviour
    {
        //组件
        private RectTransform rect;
        private Sprite[] bookSprites;//存放所以的图片（书页）
        private RectTransform clippingMask;

        //引用
        /// <summary>
        /// 正在翻页的书页的左面
        /// </summary>
        private TheDraggingPage leftSideOfPage;
        /// <summary>
        /// 正在翻页的书页的右面
        /// </summary>
        private TheDraggingPage rightSideOfPage;
        /// <summary>
        /// 左侧静止页面
        /// </summary>
        private Page leftPage;
        /// <summary>
        /// 右侧静止页面
        /// </summary>
        private Page rightPage;
        private DragPageBase dragPage;
        /// <summary>
        /// 数据类
        /// </summary>
        private BookModels bookModels;

        //数据
        private int currentLeftId;//当前左边页数
        private float aniDuration = 0.5f;
        private bool isDragging;

        public int CurrentLeftId
        {
            get
            {
                return currentLeftId;
            }

            set
            {
                currentLeftId = value;

                if (currentLeftId < -1)
                {
                    currentLeftId = -1;
                }
                else if (currentLeftId > bookSprites.Length - 1)
                {
                    currentLeftId = bookSprites.Length - 1;
                }
            }
        }
        private void Start()
        {
            Canvas canvas;

            InitAddComponent(out canvas);//初始化     

            InitDate(canvas);

            UpdateID(isDragging);
        }
        /// <summary>
        /// 初始化,找到子项并添加相应的脚本（正在翻页，静态页面）
        /// </summary>
        private void InitAddComponent(out Canvas canvas)
        {
            rect = GetComponent<RectTransform>();//获取组件
            canvas = null;
            foreach (Canvas c in rect.GetComponentsInParent<Canvas>())
            {
                if (c.isRootCanvas)
                {
                    canvas = c;
                    break;
                }
            }

            clippingMask = rect.Find("ClippingMask").GetComponent<RectTransform>();
            rightPage = rect.Find("RightPage").gameObject.AddComponent<Page>();
            leftPage = rect.Find("LeftPage").gameObject.AddComponent<Page>();
            rightSideOfPage = rect.Find("RightSide").gameObject.AddComponent<TheDraggingPage>();
            leftSideOfPage = rect.Find("LeftSide").gameObject.AddComponent<TheDraggingPage>();

            rect.Find("RightDragButton").gameObject.AddComponent<DragButton>().
                Init(OnMouseDragRightPage, OnUpdatePage, OnEndDragRightPage);
            rect.Find("LeftDragButton").gameObject
                      .AddComponent<DragButton>()
                      .Init(OnMouseDragLeftPage, OnUpdatePage, OnEndDragLeftPage);

            rightPage.Init(GetSprite);
            leftPage.Init(GetSprite);
            rightSideOfPage.Init(GetSprite);
            leftSideOfPage.Init(GetSprite);
        }

        private Sprite GetSprite(int index)
        {
            if (index>=0&&index<bookSprites.Length)
            {
                return bookSprites[index];
            }
            return null;
        }

        /// <summary>
        /// 初始化数据（加载图片，设置整个UIBook大小）
        /// </summary>
        private void InitDate(Canvas canvas)
        {
            bookModels = new BookModels();//创建并初始化

            bookSprites = Resources.LoadAll<Sprite>("Book");//加载所有的图片（书页）

            if (bookSprites.Length > 0)
            {
                rect.sizeDelta = new Vector2(bookSprites[0].rect.width * 2, bookSprites[0].rect.height);//设置UIBook大小
            }

            CurrentLeftId = -1;
            isDragging = false;

            float scaleFactor = 1;//默认图片缩放比例为1
            if (canvas != null)
            {
                scaleFactor = canvas.scaleFactor;
            }
            //计算屏幕上书页的显示尺寸
            float pageWidth = rect.rect.width * scaleFactor / 2; ;//书页宽度
            float pageHeight = rect.rect.height * scaleFactor;//书页高度

            //底边中心点的获取
            Vector3 pos = rect.position + Vector3.down * pageHeight / 2;
            bookModels.BottomCenter = World2LocalPos(pos);
            //右下角坐标
            pos = rect.position + Vector3.down * pageHeight / 2 + Vector3.right * pageWidth;
            bookModels.RightCorner = World2LocalPos(pos);
            //上边中心点的获取
            pos = rect.position + Vector3.up * pageHeight / 2;
            bookModels.TopCenter = World2LocalPos(pos);
            //左下角坐标的获取
            pos = rect.position + Vector3.down * pageHeight / 2 + Vector3.left * pageWidth;
            bookModels.LeftCorner = World2LocalPos(pos);

            float width = rect.rect.width / 2;
            float height = rect.rect.height;
            //每页的宽度
            bookModels.PageWidth = width;
            //对角线的计算
            bookModels.PageDiagonal = Mathf.Sqrt(Mathf.Pow(width, 2) + Mathf.Pow(height, 2));
            //设置ClippingMask的大小
            clippingMask.sizeDelta = new Vector2(bookModels.PageDiagonal, bookModels.PageDiagonal + bookModels.PageWidth);
            //反转切面的轴心点Y值
            bookModels.ClippingPivotY = bookModels.PageWidth / clippingMask.sizeDelta.y;

            leftSideOfPage.InitShadow(new Vector2(bookModels.PageDiagonal, bookModels.PageDiagonal));
            rightSideOfPage.InitShadow(new Vector2(bookModels.PageDiagonal, bookModels.PageDiagonal));
        }

        /// <summary>
        /// 获取clippingMask
        /// </summary>
        /// <returns></returns>
        public RectTransform GetClippingMask()
        {
            return clippingMask;
        }
        /// <summary>
        /// 世界坐标转换为local坐标
        /// </summary>
        /// <param name="world"></param>
        /// <returns></returns>
        private Vector3 World2LocalPos(Vector3 world)
        {
            return rect.InverseTransformPoint(world);
        }
        /// <summary>
        /// Local坐标转化世界坐标
        /// </summary>
        /// <param name="local"></param>
        /// <returns></returns>
        public Vector3 Local2WorldPos(Vector3 local)
        {
            return rect.TransformPoint(local);
        }

        private void UpdateID(bool isDragging)
        {
            if (isDragging)
            {
                leftPage.ID = CurrentLeftId;
                leftSideOfPage.ID = CurrentLeftId + 1;
                rightSideOfPage.ID = CurrentLeftId + 2;
                rightPage.ID = CurrentLeftId + 3;
            }
            else
            {
                leftPage.ID = CurrentLeftId;
                rightPage.ID = CurrentLeftId + 1;
            }
        }

        //开始拖动
        private void OnMouseDragRightPage()
        {
            if (CurrentLeftId<bookSprites.Length-1)
            {
                isDragging = true;
                dragPage = new DragRightPage(this, bookModels, leftSideOfPage, rightSideOfPage, rightPage.transform.position);
                dragPage.BeginDragPage(World2LocalPos(Input.mousePosition));
                UpdateID(isDragging);
            }
        }
        private void OnMouseDragLeftPage()
        {
            if (CurrentLeftId > 0)
            {
                dragPage = new DragLeftPage(this, bookModels, rightSideOfPage, leftSideOfPage, leftPage.transform.position);
                dragPage.BeginDragPage(World2LocalPos(Input.mousePosition));
                isDragging = true;
                CurrentLeftId -= 2;
                UpdateID(isDragging);
            }
        }
        //持续拖动
        private void OnUpdatePage()
        {
            if (isDragging)
            {
                dragPage.UpdateDrag();
            }
        }
        //结束拖动
        private void OnEndDragRightPage()
        {
            if (isDragging)
            {
                isDragging = false;
                bool isLeft = JudgeCornerIsLeft();
                dragPage.EndDragPage(() =>AniEnd(isLeft));
            }
        }
        //结束拖动
        private void OnEndDragLeftPage()
        {
            if (isDragging)
            {
                isDragging = false;
                bool isLeft = JudgeCornerIsLeft();
                dragPage.EndDragPage(() => AniEnd(isLeft));
            }
        }

        private bool JudgeCornerIsLeft()
        {
            return bookModels.CurrentPageCorner.x < bookModels.BottomCenter.x;
        }

        private void AniEnd(bool isLeft)
        {
            if (isLeft)
            {
                CurrentLeftId += 2;
            }
        }

        /// <summary>
        /// 获取点击位置
        /// </summary>
        /// <returns></returns>
        public Vector3 GetClickPos()
        {
            if (isDragging)
            {
                return World2LocalPos(Input.mousePosition);
            }
            else
            {
                return bookModels.ClickPoint;
            }
        }
        /// <summary>
        /// 计算拖拽角的位置
        /// </summary>
        /// <param name="click"></param>
        public Vector3 CulculateDraggingCorner(Vector3 click)
        {
            Vector3 corner = Vector3.zero;

            corner = LimitBotomCenter(click);

            return LimitTopCenter(corner);
        }
        /// <summary>
        /// 获取底边中心点
        /// </summary>
        /// <param name="click"></param>
        /// <returns></returns>
        private Vector3 LimitBotomCenter(Vector3 click)
        {
            Vector3 offset = click - bookModels.BottomCenter;
            float radians = Mathf.Atan2(offset.y, offset.x);

            Vector3 cornerLimit = new Vector3(
                bookModels.PageWidth * Mathf.Cos(radians)
                , bookModels.PageWidth * Mathf.Sin(radians))
                + bookModels.BottomCenter;

            float distance = Vector2.Distance(click, bookModels.BottomCenter);

            if (distance < bookModels.PageWidth)
            {
                return click;
            }
            else
            {
                return cornerLimit;
            }
        }
        /// <summary>
        /// 获取上边中心点
        /// </summary>
        /// <param name="click"></param>
        /// <returns></returns>
        private Vector3 LimitTopCenter(Vector3 corner)
        {
            Vector3 offset = corner - bookModels.TopCenter;
            float radians = Mathf.Atan2(offset.y, offset.x);

            Vector3 cornerLimit = new Vector3(
               bookModels.PageDiagonal * Mathf.Cos(radians)
               , bookModels.PageDiagonal * Mathf.Sin(radians))
               + bookModels.TopCenter;

            float distance = Vector2.Distance(corner, bookModels.TopCenter);

            if (distance > bookModels.PageDiagonal)
                return cornerLimit;

            return corner;
        }
        /// <summary>
        /// 计算折叠线的角度数
        /// </summary>
        /// <param name="corner"></param>
        /// <param name="bookCorner"></param>
        /// <returns></returns>
        public float CulculateFoldAngle(Vector3 corner, Vector3 bookCorner, out Vector3 bottomCrossPoint)
        {
            Vector3 twoCornerCenter = (corner + bookCorner) / 2;
            Vector3 offset = bookCorner - twoCornerCenter;
            float randians = Mathf.Atan2(offset.y, offset.x);
            float offsetX = twoCornerCenter.x - offset.y * Mathf.Tan(randians);
            offsetX = LimitOffsetX(offsetX, bookCorner, bookModels.BottomCenter);
            bottomCrossPoint = new Vector3(offsetX, bookModels.BottomCenter.y);

            offset = bottomCrossPoint - twoCornerCenter;
            return Mathf.Atan(offset.y / offset.x) * Mathf.Rad2Deg; //randians to degress
        }

        private float LimitOffsetX(float offsetX, Vector3 bookCorner, Vector3 bottomCenter)
        {
            if (offsetX > bottomCenter.x && bottomCenter.x > bookCorner.x)
                return bottomCenter.x;

            if (offsetX < bottomCenter.x && bottomCenter.x < bookCorner.x)
                return bottomCenter.x;

            return offsetX;
        }

        public void FlipAni(Vector3 target, Action onComplete)
        {
            StartCoroutine(PageAni(target, aniDuration, ()=>
            {
                if (onComplete!=null)
                {
                    onComplete();
                }
                ResetDate();
            }));
        }

        private void ResetDate()
        {
            UpdateID(isDragging);
            leftSideOfPage.SetActiveState(false);
            rightSideOfPage.SetActiveState(false);
        }

        private IEnumerator PageAni(Vector3 target,float duration,Action onComplete)
        {
            Vector3 offset = (target - bookModels.ClickPoint) / duration;
            float symbol = (target - bookModels.ClickPoint).x;
            yield return new WaitUntil(() =>
            {
                bookModels.ClickPoint += offset * Time.deltaTime;
                dragPage.UpdateDrag();
                if (symbol>0)
                {
                    return bookModels.ClickPoint.x >= target.x;
                }
                else
                {
                    return bookModels.ClickPoint.x <= target.x;
                }
            });
            if (onComplete!=null)
            {
                onComplete();
            }
        }
    }
}
