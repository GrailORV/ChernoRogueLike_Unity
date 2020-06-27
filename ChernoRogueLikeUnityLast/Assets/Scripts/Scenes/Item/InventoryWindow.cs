using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryWindow : WindowBase
{
    [Space(20)]

    // ページ数
    const int MAX_PAGE = 4;

    // 表示するアイテム数
    const int SHOW_ITEM_NUM = 10;

    // 総アイテム所持数
    const int MAX_ITEM = MAX_PAGE * SHOW_ITEM_NUM;

    // ページャー
    [SerializeField] PagerControl _pager;

    // セル一覧の親オブジェクト
    [SerializeField] Transform _contentsParent;

    // アイテムセル用のプレハブ
    [SerializeField] GameObject _itemCellPrefab;

    // アイテムセルのリスト
    List<ItemCell> _itemCellList = new List<ItemCell>();

    // 所持するアイテム一覧
    //List<ItemData> _itemDataList = new List<ItemData>();

    // ページごとに分かれたアイテム一覧
    Dictionary<int, List<ItemData>> _itemDataDict = new Dictionary<int, List<ItemData>>();

    // 現在のページ数
    int _currentPageIndex = 0;

    // 現在のページ数
    public int CurrentPageIndex
    {
        get { return _currentPageIndex; }
        set
        {
            // 値が範囲外にいかないようにする
            _currentPageIndex = Mathf.Clamp(value, 0, MAX_PAGE - 1);

            // ページャーの切り替え
            _pager.DisplayIndex = _currentPageIndex;

            // UIの更新
            if(_itemDataDict.ContainsKey(_currentPageIndex))
            {
                UpdateItemUI(_itemDataDict[_currentPageIndex]);
            }
        }
    }

    public override void OnEnable()
    {
        base.OnEnable();
    }

    public override void OnDisable()
    {
        base.OnDisable();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public void SetUp()
    {
        // ページャーの初期化
        _pager.Init(MAX_PAGE);

        // 初期化
        CurrentPageIndex = 0;

        // アイテム情報の更新
        SetData(GetDataList());

        // アイテム一覧ないなら作成
        if(_itemCellList == null || _itemCellList.Count != SHOW_ITEM_NUM)
        {
            CreateItemCell();
        }
        // 作成済みならUIの更新
        else
        {
            UpdateCurrentPageItemUI();
        }
    }

    /// <summary>
    /// アイテム一覧の作成
    /// </summary>
    void CreateItemCell()
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

            // アイテム一覧に追加
            if (itemCell != null)
            {
                _itemCellList.Add(itemCell);
            }

            // ナビゲーターの追加
            navigationLayer.AddNavigator(itemCell.GetComponent<Navigator>());
        }

        // セルの整列
        Canvas.ForceUpdateCanvases();

        // ナビゲーターの上下方向の設定
        navigationLayer.SetVerticalNavigtor();

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

        for (int i = 0; i < _itemCellList.Count; i++)
        {
            _itemCellList[i].SetUIContents(dataList[i]);
        }
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

            itemList.Add(data);
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

        if (itemList.Count >= MAX_ITEM)
        {
            Debug.LogError("アイテムがいっぱいです。");
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// ぺージごとに分けているデータを取得
    /// </summary>
    /// <returns></returns>
    List<ItemData> GetDataList()
    {
        // アイテムデータを一つのリストにする
        List<ItemData> itemDatalist = new List<ItemData>();
        for (int page = 0; page < MAX_PAGE; page++)
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
                catch (System.Exception e)
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
    void SetData(List<ItemData> datas)
    {
        // ソートしたデータを_itemDataDictに戻す
        for (int i = 0; i < MAX_PAGE; i++)
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
                    dataList.Add(null);
                    continue;
                }
            }

            // ディクショナリに差し替え
            _itemDataDict[i] = dataList;
        }
    }

    #region 入力処理

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
                if (CurrentPageIndex + 1 <= MAX_PAGE)
                {
                    CurrentPageIndex++;
                }
                break;
        }
    }
    #endregion
}
