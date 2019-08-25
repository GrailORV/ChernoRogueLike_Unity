using System.Collections;
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

    // 削除時に呼ばれるコールバック
    public System.Action OnDestroyAction = null;

    // RectTransform
    protected RectTransform _rectTransform;

    // Tweenのアニメーション
    protected ITweenAnimation _tweenAnimation = null;

    // 画面が閉じているかどうか
    bool _isClose = true;

    // 表示・非表示のアニメーション中かどうか
    bool _isTweenAnimation = false;

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

    /// <summary>
    /// 表示・非表示のアニメーション中かどうか
    /// </summary>
    public bool IsTweenAnimation
    {
        private set { _isTweenAnimation = value; }
        get { return _isTweenAnimation; }
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
        if(!IsClose || IsTweenAnimation)
        {
            return;
        }

        IsClose = false;
        IsTweenAnimation = true;
        gameObject.SetActive(true);

        // アニメーション
        TweenAnimation.Open(() =>
        {
            // 終了時にフラグを戻す
            IsTweenAnimation = false;
        });
    }

    /// <summary>
    /// ウィンドウの非表示
    /// </summary>
    public virtual void Close()
    {
        if (IsClose || IsTweenAnimation)
        {
            return;
        }

        IsTweenAnimation = true;

        // アニメーション
        TweenAnimation.Close(() =>
        {
            // 終了時にフラグを戻す
            IsTweenAnimation = false;

            IsClose = true;
            gameObject.SetActive(false);
        });
    }

    /// <summary>
    /// 削除時
    /// </summary>
    void OnDestroy()
    {
        if(OnDestroyAction != null)
        {
            OnDestroyAction.Invoke();
        }
    }
}
