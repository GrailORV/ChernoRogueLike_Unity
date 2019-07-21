﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WindowAnimation;

/*=============================================================================
 * 
 * WindowBase
 * ウィンドウが存在する機能にはこれを継承させる(と便利になる？)
 * 主な機能はウィンドウの表示・非表示のアニメーションなど
 * その他ほかのウィンドウ全体で必要な機能の追加などがあればここに入れる
 * 
 =============================================================================*/
public class WindowBase : MonoBehaviour
{
    // アニメーション時間
    [SerializeField] float _animPlayTime  = 0.3f;

    // アニメーション開始の遅延時間
    [SerializeField] float _animDelayTime = 0.0f;

    // アニメーションの種類(デフォルトは拡縮)
    [SerializeField] AnimationBuilder.TweenType _tweenType = AnimationBuilder.TweenType.zoom;


    // RectTransform
    protected RectTransform _rectTransform;

    // Tweenのアニメーション
    protected ITweenAnimation _tweenAnimation = null;

    // 画面が閉じているかどうか
    bool _isClose = true;

    /// <summary>
    /// レクトトランスフォーム
    /// </summary>
    public RectTransform RectT
    {
        protected set { _rectTransform = value; }
        get
        {
            if(_rectTransform == null)
            {
                _rectTransform = gameObject.GetComponent<RectTransform>();
            }
            return _rectTransform;
        }
    }

    /// <summary>
    /// アニメーション
    /// </summary>
    public ITweenAnimation TweenAnimation
    {
        protected set { _tweenAnimation = value; }
        get
        {
            if (_tweenAnimation == null)
            {
                _tweenAnimation = AnimationBuilder.SetUp(_tweenType, RectT, _animPlayTime, _animDelayTime);
            }
            return _tweenAnimation;
        }
    }

    /// <summary>
    /// 画面が閉じているかどうか
    /// </summary>
    public bool IsClose
    {
        private set { _isClose = value; }
        get { return _isClose; }
    }

    protected virtual void Awake()
    {
        // 初期は非表示
        gameObject.SetActive(false);
    }

    /// <summary>
    /// オブジェクト表示時
    /// </summary>
    public virtual void OnEnable()
    {
    }

    /// <summary>
    /// オブジェクト非表示時
    /// </summary>
    public virtual void OnDisable()
    {
    }

    /// <summary>
    /// ウィンドウの表示
    /// </summary>
	public virtual void Open()
    { 
        if(!IsClose || TweenAnimation.IsAnimation)
        {
            return;
        }

        IsClose = false;
        gameObject.SetActive(true);
        TweenAnimation.Open(null);
    }

    /// <summary>
    /// ウィンドウの非表示
    /// </summary>
    public virtual void Close()
    {
        if (IsClose || TweenAnimation.IsAnimation)
        {
            return;
        }

        TweenAnimation.Close(() =>
        {
            IsClose = true;
            gameObject.SetActive(false);
        });
    }
}
