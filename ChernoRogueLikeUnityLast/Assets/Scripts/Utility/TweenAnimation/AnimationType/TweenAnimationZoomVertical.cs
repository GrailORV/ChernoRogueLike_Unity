﻿using System;
using UnityEngine;

using DG.Tweening;

namespace WindowAnimation
{
    public class TweenAnimationZoomVertical : ITweenAnimation
    {
        /// <summary>アニメーションの時間</summary>
        public float AnimationTime { set; get; }

        /// <summary>遅延時間</summary>
        public float DelayTime { set; get; }

        // アニメーション用のレクとトランスフォーム
        RectTransform _rectT;

        // 初期化用のスケール
        Vector2 _defaultScale;

        /// <summary>
        /// 設定
        /// </summary>
        /// <param name="time"></param>
        /// <param name="delay"></param>
        public void SetUp(RectTransform rectT, float time, float delay)
        {
            _rectT = rectT;
            _defaultScale = rectT.localScale;

            AnimationTime = time < 0 ? 0 : time;
            DelayTime = delay < 0 ? 0 : delay;
        }

        /// <summary>
        /// 開くTweenアニメーション
        /// </summary>
        public void Open(Action onComplete)
        {
            // スケールの初期化
            _rectT.localScale = new Vector2(1f, 0f);

            // 拡大アニメーションを行う
            var tween = _rectT.DOScale(_defaultScale, AnimationTime);
            tween.SetDelay(DelayTime);

            // コールバックの設定
            tween.OnComplete(() =>
            {
                if (onComplete != null)
                {
                    onComplete();
                }
            });
        }

        /// <summary>
        /// 閉じるTweenアニメーション
        /// </summary>
        public void Close(Action onComplete)
        {
            // スケールの初期化
            _rectT.localScale = _defaultScale;

            // 縮小アニメーションを行う
            var tween = _rectT.DOScale(new Vector2(1f, 0f), AnimationTime);
            tween.SetDelay(DelayTime);

            // コールバックの設定
            tween.OnComplete(() =>
            {
                if (onComplete != null)
                {
                    onComplete();
                }
            });
        }
    }
}