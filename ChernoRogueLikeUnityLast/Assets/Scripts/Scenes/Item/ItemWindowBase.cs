﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// アイテム画面用の基底クラス
/// </summary>
public class ItemWindowBase : WindowBase
{
    [Space(20)]

    // 表示するアイテム数
    protected const int SHOW_ITEM_NUM = 10;

    // ページ数
    [SerializeField] protected int _maxPage = 3;

    // ページャー
    [SerializeField] protected PagerControl _pager;

    // セル一覧の親オブジェクト
    [SerializeField] protected Transform _contentsParent;

    // アイテムセル用のプレハブ
    [SerializeField] protected GameObject _itemCellPrefab;

    // メニューウィンドウ
    [SerializeField] protected ItemMenuWindow _itemMenuWindow;

    // アイテムセルのリスト
    protected List<ItemCell> _itemCellList = new List<ItemCell>();

    // ページごとに分かれたアイテム一覧 key:ページ value:アイテムリスト
    protected Dictionary<int, List<ItemData>> _itemDataDict = new Dictionary<int, List<ItemData>>();

    // 現在のページ数
    int _currentPageIndex = 0;

    // 最大アイテム数
    public int MaxItemNum
    {
        get { return SHOW_ITEM_NUM * _maxPage; }
    }

    // ページ数
    public int MaxPage
    {
        get { return _maxPage; }
        set { _maxPage = value; }
    }

    // 現在のページ数
    public int CurrentPageIndex
    {
        get { return _currentPageIndex; }
        set
        {
            // 値が範囲外にいかないようにする
            _currentPageIndex = Mathf.Clamp(value, 0, MaxPage - 1);

            // ページャーの切り替え
            _pager.DisplayIndex = _currentPageIndex;

            // UIの更新
            if (_itemDataDict.ContainsKey(_currentPageIndex))
            {
                UpdateItemUI(_itemDataDict[_currentPageIndex]);
            }
        }
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public virtual void SetUp()
    {
    }

    /// <summary>
    /// アイテム一覧の作成
    /// </summary>
    protected void CreateItemCell()
    {
        // アイテム一覧を削除
        foreach (var cell in _itemCellList)
        {
            Destroy(cell);
        }

        // リストを初期化
        _itemCellList.Clear();
        navigationLayer.RemoveNavigatorAll();

        // 一覧作成
        for (int i = 0; i < SHOW_ITEM_NUM; i++)
        {
            var obj = Instantiate(_itemCellPrefab, _contentsParent);
            obj.name = obj.name + "_" + i;
            obj.SetActive(true);

            // アイテム情報の設定
            var itemCell = obj.GetComponent<ItemCell>();
            itemCell.SetUIContents(_itemDataDict[CurrentPageIndex][i]);

            // セルの選択時の処理を追加
            itemCell.OnClickAction = data =>
            {
                // メニュー画面を表示
                _itemMenuWindow.Open();
                _itemMenuWindow.SetUp(data);
            };

            // アイテム一覧に追加
            if (itemCell != null)
            {
                _itemCellList.Add(itemCell);
            }
        }

        // セルの整列
        Canvas.ForceUpdateCanvases();

        // ナビゲーターの設定
        SetUpNavigator();

        // 初期カーソル位置の設定
        navigationLayer.SetCurrentNavigatorFromIndex(0);
    }

    /// <summary>
    /// アイテムのUI更新(現在のページ)
    /// </summary>
    /// <param name="dataList"></param>
    public void UpdateCurrentPageItemUI()
    {
        UpdateItemUI(_itemDataDict[CurrentPageIndex]);
    }

    /// <summary>
    /// アイテムのUI更新
    /// </summary>
    /// <param name="dataList"></param>
    public void UpdateItemUI(List<ItemData> dataList)
    {
        if (dataList == null || dataList.Count == 0)
        {
            return;
        }

        // UIの更新
        for (int i = 0; i < _itemCellList.Count; i++)
        {
            _itemCellList[i].SetUIContents(dataList[i]);
        }

        // ナビゲーターの設定
        SetUpNavigator();
    }

    /// <summary>
    /// アイテムの追加
    /// </summary>
    /// <param name="data">アイテムID</param>
    /// <returns>結果</returns>
    public bool AddItem(List<int> idList)
    {
        // 追加するものがない
        if (idList == null || idList.Count == 0)
        {
            return false;
        }

        var itemList = new List<ItemData>();

        foreach (var id in idList)
        {
            // idが存在するか確認
            if (!ItemTableHelper.ContainsItemId(id))
            {
                Debug.LogError("ID = " + id + "のデータはマスタデータに存在しないです");
                continue;
            }

            // マスタデータからアイテムの情報を取得する
            itemList.Add(new ItemData(ItemTableHelper.MstItemData[id]));
        }

        // 追加できるものが何もない
        if (itemList.Count == 0)
        {
            return false;
        }

        return AddItem(itemList);
    }

    /// <summary>
    /// アイテムの追加
    /// </summary>
    /// <param name="data">アイテム</param>
    /// <returns>結果</returns>
    public bool AddItem(List<ItemData> dataList)
    {
        var itemList = GetDataList();

        foreach (var data in dataList)
        {
            // アイテムが所持数制限を超えていれば追加しない
            if (IsItemFull(itemList))
            {
                break;
            }

            // 空きがある場所にアイテムの情報を入れる
            var index = itemList.FindIndex(_ => _.Id < 0);
            itemList[index] = data;
        }

        // データをページごとに設定
        SetData(itemList);

        // アイテムのUIを更新
        if (gameObject.activeSelf)
        {
            UpdateCurrentPageItemUI();
        }

        return true;
    }

    /// <summary>
    /// アイテムがいっぱいかどうか
    /// </summary>
    /// <returns></returns>
    public bool IsItemFull(List<ItemData> itemList = null)
    {
        itemList = itemList == null ? GetDataList() : itemList;

        // 一つでも空きがあればfalse
        if (itemList == null || itemList.Any(data => data.Id < 0))
        {
            return false;
        }

        Debug.LogError("アイテムがいっぱいです。");
        return true;
    }

    /// <summary>
    /// 整頓する
    /// </summary>
    public void SortInventory()
    {
        var dataList = GetDataList();

        if (dataList != null)
        {
            // ID順のソートを行う
            dataList.OrderBy(data => data.Id);
        }
    }

    /// <summary>
    /// ぺージごとに分けているデータを取得
    /// </summary>
    /// <returns></returns>
    protected List<ItemData> GetDataList()
    {
        // アイテムデータを一つのリストにする
        List<ItemData> itemDatalist = new List<ItemData>();

        for (int page = 0; page < MaxPage; page++)
        {
            for (int select = 0; select < SHOW_ITEM_NUM; select++)
            {
                try
                {
                    // データがあるものだけリストに追加
                    if (_itemDataDict[page][select] != null)
                    {
                        itemDatalist.Add(_itemDataDict[page][select]);
                    }
                }
                catch
                {
                    return null;
                }
            }
        }

        // 一つのリスト化にしたものを返す
        return itemDatalist;
    }

    /// <summary>
    /// 一つにまとめたデータをページごとに分ける
    /// </summary>
    /// <param name="datas"></param>
    protected void SetData(List<ItemData> datas)
    {
        // ソートしたデータを_itemDataDictに戻す
        for (int i = 0; i < MaxPage; i++)
        {
            var dataList = new List<ItemData>();

            for (int j = 0; j < SHOW_ITEM_NUM; j++)
            {
                // 取得したデータの中身がなくてもリストは用意する
                if (datas != null && (j + SHOW_ITEM_NUM * i) < datas.Count)
                {
                    dataList.Add(datas[j + (SHOW_ITEM_NUM * i)]);
                }
                else
                {
                    dataList.Add(new ItemData(null));
                    continue;
                }
            }

            // ディクショナリに差し替え
            _itemDataDict[i] = dataList;
        }
    }

    #region 入力関係

    /// <summary>
    /// ナビゲーターの設定
    /// </summary>
    protected void SetUpNavigator()
    {
        // エントリーポイントの初期化
        navigationLayer.RemoveNavigatorAll();
        navigationLayer.CurrentNavigator = null;

        foreach (var cell in _itemCellList)
        {
            // 中身のあるオブジェクトのみ選択可能
            if (!cell.IsEmpty)
            {
                navigationLayer.AddNavigator(cell.GetComponent<Navigator>());
            }
        }

        // ナビゲーターの上下方向の設定
        navigationLayer.SetVerticalNavigtor();

        // 最初のカーソル位置を設定
        navigationLayer.SetCurrentNavigatorFromIndex(navigationLayer.GetDefaultIndex());
    }

    /// <summary>
    /// ページ変更
    /// </summary>
    /// <param name="direction"></param>
    public void OnSelectButtonDown(NavigationManager.InputDirection direction)
    {
        // 左右の時はページの切り替え
        switch (direction)
        {
            case NavigationManager.InputDirection.Left:
                if (CurrentPageIndex > 0)
                {
                    CurrentPageIndex--;
                }
                break;

            case NavigationManager.InputDirection.Right:
                if (CurrentPageIndex + 1 <= MaxPage)
                {
                    CurrentPageIndex++;
                }
                break;
        }
    }
    #endregion

    #region サブメニュー関係

    /// <summary>
    /// サブメニューを選択した時の処理
    /// </summary>
    public void OnSelectSubMenu()
    {
        // 選択中のコマンドを実行

    }

    #endregion
}
