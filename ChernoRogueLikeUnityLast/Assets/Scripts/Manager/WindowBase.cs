using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*=============================================================================
 * 
 * WindowBase
 * ウィンドウが存在する機能にはこれを必ず継承させる
 * 主な機能はウィンドウの表示・非表示のアニメーションなど
 * その他ほかのウィンドウ全体で必要な機能の追加などがあればここに入れる
 * 
 =============================================================================*/
public class WindowBase : MonoBehaviour
{

    // 画面が閉じているかどうか
    bool _isClose = false;
    
    /// <summary>
    /// 画面が閉じているかどうか
    /// </summary>
    public bool IsClose
    {
        private set { _isClose = value; }
        get { return _isClose; }
    }

    void Awake()
    {

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
        IsClose = true;
    }

    /// <summary>
    /// ウィンドウの表示
    /// </summary>
	public virtual void Open()
    {

    }

    /// <summary>
    /// ウィンドウの非表示
    /// </summary>
    public virtual void Close()
    {
        IsClose = true;
    }
}
