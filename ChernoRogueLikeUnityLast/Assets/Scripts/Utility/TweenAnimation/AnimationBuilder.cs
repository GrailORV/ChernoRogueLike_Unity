using DG.Tweening;
using UnityEngine;

namespace WindowAnimation
{
    public class AnimationBuilder
    {
        /// <summary>
        /// アニメーションのタイプ
        /// </summary>
        public enum TweenType
        {
            none            = 0,    // アニメーションなし
            zoom            = 1,    // 拡縮
            zoom_vertcal    = 2,    // 拡縮(縦)
            zoom_horizontal = 3,    // 拡縮(横)
        }

        /// <summary>
        /// アニメーションの設定
        /// </summary>
        /// <returns>Instance</returns>
        public static ITweenAnimation SetUp(TweenType type, RectTransform rectT, float time, float delay)
        {
            ITweenAnimation animation = null;

            // type によってアニメーションの種類を変更させる
            switch (type)
            {
                case TweenType.none:
                    animation = new TweenAnimationZoom();
                    animation.SetUp(rectT, 0f, 0f);
                    break;
                case TweenType.zoom:
                    animation = new TweenAnimationZoom();
                    animation.SetUp(rectT, time, delay);
                    break;
                case TweenType.zoom_vertcal:
                    animation = new TweenAnimationZoomVertical();
                    animation.SetUp(rectT, time, delay);
                    break;
                case TweenType.zoom_horizontal:
                    animation = new TweenAnimationZoomHorizontal();
                    animation.SetUp(rectT, time, delay);
                    break;
                default:
                    break;
            }

            return animation;
        }
    }

}
