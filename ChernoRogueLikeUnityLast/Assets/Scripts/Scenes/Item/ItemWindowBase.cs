using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// アイテム画面用の基底クラス
/// </summary>
public class ItemWindowBase : WindowBase
{
    /// <summary>
    /// アイテムの画面モード
    /// </summary>
    public enum WindowMode
    {
        Normal = 0,
        SetPotItem, // 壺にアイテムを入れるモード
    }

    [Space(20)]

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

    // アイテムウィンドウの種類
    protected ItemManager.ItemWindowType _windowType = ItemManager.ItemWindowType.None;

    // 現在のページ数
    int _currentPageIndex = 0;

    // 現在の画面モード
    public WindowMode CurrentWindowMode { get; set; }

    // 選択中のアイテム
    public ItemData SelectedItem { get; private set; }

    // 最大アイテム数
    public int MaxItemNum
    {
        get { return ItemManager.SHOW_ITEM_NUM * MaxPage; }
    }

    // ページ数
    public virtual int MaxPage { get; }

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

    public override void OnEnable()
    {
        base.OnEnable();

        ItemManager.Instance.OnAddItem += OnUpdateItem;
        ItemManager.Instance.OnRemoveItem += OnUpdateItem;
    }

    public override void OnDisable()
    {
        base.OnDisable();

        ItemManager.Instance.OnAddItem -= OnUpdateItem;
        ItemManager.Instance.OnRemoveItem -= OnUpdateItem;
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
        for (int i = 0; i < ItemManager.SHOW_ITEM_NUM; i++)
        {
            var obj = Instantiate(_itemCellPrefab, _contentsParent);
            obj.name = obj.name + "_" + i;
            obj.SetActive(true);

            // アイテム情報の設定
            var itemCell = obj.GetComponent<ItemCell>();
            if (_itemDataDict.ContainsKey(CurrentPageIndex) && i < _itemDataDict[CurrentPageIndex].Count)
            {
                itemCell.SetUIContents(_itemDataDict[CurrentPageIndex][i]);
            }
            else
            {
                itemCell.SetUIContents(null);
            }

            // セルの選択時の処理を追加
            itemCell.OnClickAction = OnClickItemCell;

            // アイテム一覧に追加
            if (itemCell != null)
            {
                _itemCellList.Add(itemCell);
            }
        }

        // セルの整列
        Canvas.ForceUpdateCanvases();

        // ページャーの設定
        SetUpPager();

        // ナビゲーターの設定
        SetUpNavigator();

        // 初期カーソル位置の設定
        navigationLayer.SetCurrentNavigatorFromIndex(0);
    }

    /// <summary>
    /// アイテム選択時の処理
    /// </summary>
    /// <param name="data"></param>
    public void OnClickItemCell(ItemData data)
    {
        switch (CurrentWindowMode)
        {
            case WindowMode.Normal:
                // メニュー画面を表示
                _itemMenuWindow.Open();
                _itemMenuWindow.SetUp(this, data);

                SelectedItem = data;
                break;

            case WindowMode.SetPotItem:
                // 壺にアイテムを入れる
                AddPutItem(data);
                CurrentWindowMode = WindowMode.Normal;
                break;
        }
    }

    /// <summary>
    /// アイテムのUI更新(現在のページ)
    /// </summary>
    /// <param name="dataList"></param>
    public void UpdateCurrentPageItemUI()
    {
        if (_itemDataDict.ContainsKey(CurrentPageIndex))
        {
            UpdateItemUI(_itemDataDict[CurrentPageIndex]);
        }
        else
        {
            UpdateItemUI(null);
        }
    }

    /// <summary>
    /// アイテムのUI更新
    /// </summary>
    /// <param name="dataList"></param>
    public void UpdateItemUI(List<ItemData> dataList)
    {
        // UIの更新
        for (int i = 0; i < _itemCellList.Count; i++)
        {
            if(dataList != null && i < dataList.Count)
            {
                _itemCellList[i].SetUIContents(dataList[i]);
            }
            else
            {
                _itemCellList[i].SetUIContents(null);
            }
        }

        // ページャーの設定
        SetUpPager();

        // ナビゲーターの設定
        SetUpNavigator();
    }

    /// <summary>
    /// 壺にアイテムを追加
    /// </summary>
    /// <param name="itemData"></param>
    public void AddPutItem(ItemData itemData)
    {
        // [選択中のアイテム]が壺系かどうか
        // or
        // 壺 in 壺 はだめなので[入れたいアイテム]が壺系ならアウト
        if (SelectedItem.Type != ItemData.ItemType.Pot || itemData.Type == ItemData.ItemType.Pot)
        {
            return;
        }

        var potItemList = SelectedItem.PotItemDataList;

        // アイテムがいっぱいなら入れれません
        if (potItemList == null || potItemList.Count >= MaxItemNum)
        {
            return;
        }

        // 壺にアイテムを入れる
        potItemList.Add(itemData);
        ItemManager.Instance.RemoveItem(ItemManager.Instance.CurrentWindowType, itemData);
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

        foreach (var datas in _itemDataDict.Values)
        {
            itemDatalist.AddRange(datas);
        }

        // 一つのリスト化にしたものを返す
        return itemDatalist;
    }

    /// <summary>
    /// 一つにまとめたデータをページごとに分ける
    /// </summary>
    /// <param name="datas"></param>
    protected void SetData(IEnumerable<ItemData> datas)
    {
        // ディクショナリを初期化
        _itemDataDict.Clear();

        // データを ItemManager.SHOW_ITEM_NUM の数づつ_itemDataDictに入れる
        var pageCount = 0;
        while (datas.Any())
        {
            // Take()で指定数づつ取得
            _itemDataDict.Add(pageCount, datas.Take(ItemManager.SHOW_ITEM_NUM).ToList());

            // Skip()で指定分ずらす
            datas = datas.Skip(ItemManager.SHOW_ITEM_NUM);

            pageCount++;
        }
    }

    /// <summary>
    /// アイテムの更新用コールバック
    /// </summary>
    public virtual void OnUpdateItem()
    {
        // アイテム情報を更新
        SetData(ItemManager.Instance.GetPlayerItemList(ItemManager.Instance.CurrentWindowType));

        // UIの更新
        UpdateCurrentPageItemUI();
    }

    /// <summary>
    /// ページャーの設定
    /// </summary>
    private void SetUpPager()
    {
        // ページャーの設定
        var pageNum = _itemDataDict.Count;
        if (pageNum != _pager.GetCount())
        {
            if (pageNum >= CurrentPageIndex)
            {
                _pager.Init(pageNum, CurrentPageIndex);
            }
            else
            {
                _pager.Init(pageNum, pageNum);
            }
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
                if (CurrentPageIndex + 1 <= _pager.GetCount())
                {
                    CurrentPageIndex++;
                }
                break;
        }
    }
    #endregion

    #region サブメニュー関係

    /// <summary>
    /// 壺にアイテムを入れる
    /// </summary>
    public void OnSelectSetPotItem()
    {
        // ウィンドウの画面モードを変更
        CurrentWindowMode = WindowMode.SetPotItem;

        // UIの更新があれば追記


    }

    #endregion
}
