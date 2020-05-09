﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Navigator : MonoBehaviour
{
    [SerializeField] Navigator _up;
    [SerializeField] Navigator _down;
    [SerializeField] Navigator _left;
    [SerializeField] Navigator _right;

    // 選択したときにアクティブになるオブジェクト
    [SerializeField] List<GameObject> _cursorList = new List<GameObject>();

    public void OnHover(bool isHover)
    {
        // カーソル状態の変更
        SetCursorActive(isHover);
    }

    /// <summary>
    /// 上下左右のナビゲーターの取得
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public Navigator GetDirectionNavigator(NavigationManager.InputDirection direction)
    {
        Navigator navi = null;

        switch (direction)
        {
            case NavigationManager.InputDirection.Up:
                navi = _up;
                break;

            case NavigationManager.InputDirection.Down:
                navi = _down;
                break;

            case NavigationManager.InputDirection.Left:
                navi = _left;
                break;

            case NavigationManager.InputDirection.Right:
                navi = _right;
                break;
        }

        return navi;
    }

    /// <summary>
    /// ナビゲーターの設定
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="navigator"></param>
    public void SetNavigator(NavigationManager.InputDirection direction, Navigator navigator)
    {
        switch (direction)
        {
            case NavigationManager.InputDirection.Up:
                _up = navigator;
                break;

            case NavigationManager.InputDirection.Down:
                _down = navigator;
                break;

            case NavigationManager.InputDirection.Left:
                _left = navigator;
                break;

            case NavigationManager.InputDirection.Right:
                _right = navigator;
                break;
        }
    }

    /// <summary>
    /// 縦方向のナビゲーターの設定
    /// </summary>
    /// <param name="up"></param>
    /// <param name="down"></param>
    public void SetVerticalNavigator(Navigator up, Navigator down)
    {
        _up = up;
        _down = down;
    }

    /// <summary>
    /// 横方向のナビゲーター設定
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    public void SetHorizontalNavigator(Navigator left, Navigator right)
    {
        _left = left;
        _right = right;
    }

    /// <summary>
    /// 全方向のナビゲーターの設定
    /// </summary>
    /// <param name="up"></param>
    /// <param name="down"></param>
    /// <param name="left"></param>
    /// <param name="right"></param>
    public void SetAllNavigator(Navigator up, Navigator down, Navigator left, Navigator right)
    {
        _up = up;
        _down = down;
        _left = left;
        _right = right;
    }

    /// <summary>
    /// ナビゲーターの全解除
    /// </summary>
    public void RemoveAllNavigator()
    {
        _up = null;
        _down = null;
        _left = null;
        _right = null;
    }

    /// <summary>
    /// カーソルの表示
    /// </summary>
    /// <param name="isActive"></param>
    public void SetCursorActive(bool isActive)
    {
        foreach (var cursor in _cursorList)
        {
            // アクティブ状態に変更がないなら次へ
            if(cursor.activeSelf == isActive)
            {
                continue;
            }

            cursor.SetActive(isActive);
        }
    }
}
