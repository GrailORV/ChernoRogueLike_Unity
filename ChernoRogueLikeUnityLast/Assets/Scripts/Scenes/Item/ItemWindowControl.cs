using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemWindowControl : MonoBehaviour
{
    // アイテムのページャー
    [SerializeField] PagerControl _pager = null;

    // アイテムのコントロールクラス
    ItemUIControl _itemUIControl = null;

    // 現在のページ数
    int _currentPageNum = 0;

    // 現在の選択しているインデックス
    int _currentSelectIndex = 0;

    // 現在のページ数
    public int CurrentPageNum
    {
        get { return _currentPageNum; }
        set
        {
            _currentPageNum = value;

            // 値が範囲外にいかないようにする
            if (_currentPageNum >= ItemManager.MAX_PAGE)
            {
                _currentPageNum = 0;
            }
            else if (_currentPageNum < 0)
            {
                _currentPageNum = ItemManager.MAX_PAGE - 1;
            }

            // ページャーの切り替え
            _pager.DisplayIndex = _currentPageNum;

            // ページ切り替え時にカーソルを0にする
            CurrentSelectIndex = 0;

            // UIの更新
            if (_itemUIControl != null)
            {
                _itemUIControl.ChangePage(_currentPageNum);
            }
        }
    }

    // 現在の選択しているインデックス
    public int CurrentSelectIndex
    {
        get { return _currentSelectIndex; }
        set
        {
            // カーソルの調整
            int difference = value - _currentSelectIndex;
            _currentSelectIndex = _itemUIControl.AdjustCursorNum(value, (int)Mathf.Clamp(difference, -1f, 1f));
            
            // UIの更新
            if (_itemUIControl != null)
            {
                _itemUIControl.ChangeCursor(_currentSelectIndex);
            }
        }
    }

    /// <summary>
    /// 初期化処理
    /// </summary>
    public void Init(ItemUIControl itemUIControl)
    {
        // アイテムのコントロールクラスを取得
        _itemUIControl = itemUIControl;

        // ページャーの初期化
        _pager.Init(ItemManager.MAX_PAGE);

        // 初期化
        CurrentSelectIndex = 0;
        CurrentPageNum = 0;
    }

	// Update is called once per frame
	void Update ()
    {
        // キー入力
        KeyControl();
    }

    /// <summary>
    /// キー入力
    /// </summary>
    void KeyControl()
    {
        // TODO 汎用の入力クラスが出来たらそっちのほうが良き


        // 左右キーでページの切り替え
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // 右
            CurrentPageNum++;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // 左
            CurrentPageNum--;
        }

        // 上下キーでカーソルの切り替え
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            // 上
            CurrentSelectIndex--;
        }
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            // 下
            CurrentSelectIndex++;
        }

        // TODO デバッグ
        if(Input.GetKeyDown(KeyCode.A))
        {
            // ソートを行う
            _itemUIControl.SortItemData();
        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            // 追加を行う
            _itemUIControl.AddItem(0);
        }
        if(Input.GetKeyDown(KeyCode.D))
        {
            // 削除を行う
            _itemUIControl.DeleteItem(CurrentSelectIndex, CurrentPageNum);
        }
    }
}
