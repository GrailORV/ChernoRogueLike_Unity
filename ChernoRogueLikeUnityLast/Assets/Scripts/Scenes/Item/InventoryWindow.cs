using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryWindow : WindowBase
{
    // ページ数
    readonly int MAX_PAGE = 4;

    // 総アイテム所持数
    readonly int MAX_ITEM = 40;

    [Space(20)]

    // ページャー
    [SerializeField] PagerControl _pager;

    // アイテムセルのリスト
    [SerializeField] List<ItemCell> _itemCellList = new List<ItemCell>();

    // 所持するアイテム一覧
    List<ItemData> _itemDataList = new List<ItemData>();

    // ページごとに分かれたアイテム一覧
    Dictionary<int, List<ItemData>> _itemDataDict = new Dictionary<int, List<ItemData>>();

    // 現在のページ数
    int _currentPageNum = 0;

    // 現在のページ数
    public int CurrentPageNum
    {
        get { return _currentPageNum; }
        set
        {
            // 値が範囲外にいかないようにする
            _currentPageNum = Mathf.Clamp(value, 0, MAX_PAGE - 1);

            // ページャーの切り替え
            _pager.DisplayIndex = _currentPageNum;

            // UIの更新
            UpdateItemUI(_itemDataDict[_currentPageNum]);
        }
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public void SetUp()
    {
        // ページャーの初期化
        _pager.Init(MAX_PAGE);

        // 初期化
        CurrentPageNum = 0;
    }

    /// <summary>
    /// アイテムのUI更新
    /// </summary>
    /// <param name="dataList"></param>
    public void UpdateItemUI(List<ItemData> dataList)
    {
        for (int i = 0; i < _itemCellList.Count; i++)
        {
            _itemCellList[i].SetUIContents(dataList[i]);
        }
    }

    /// <summary>
    /// アイテムの追加
    /// </summary>
    /// <param name="charaId">キャラID</param>
    /// <param name="data">アイテムID</param>
    /// <returns>結果</returns>
    public bool AddItem(int charaId, int itemId)
    {
        //// マスタデータが存在するか
        //if (_mstItemData == null)
        //{
        //    Debug.LogError("<color=red>アイテムのマスタデータがないので追加できません</color>");
        //    return false;
        //}

        //// idが存在するか確認
        //if (!_mstItemData.ContainsKey(itemId))
        //{
        //    Debug.LogError("<color=red>ID = " + itemId + "のデータはマスタデータに存在しないです</color>");
        //    return false;
        //}

        //// マスタデータからアイテムの情報を取得する
        //var data = _mstItemData[itemId];
        //return AddItem(charaId, data);
        return false;
    }

    /// <summary>
    /// ぺージごとに分けているデータを取得
    /// </summary>
    /// <returns></returns>
    List<ItemData> GetDataList()
    {
        // アイテムデータを一つのリストにする
        List<ItemData> itemDatalist = new List<ItemData>();
        for (int page = 0; page < ItemManager.MAX_PAGE; page++)
        {
            for (int select = 0; select < ItemManager.MAX_SHOW_ITEM; select++)
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
                    Debug.LogError("リスト化失敗？：" + e);
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
        if (datas == null)
        {
            Debug.LogError("データがありません");
            return;
        }
        if (datas.Count == ItemManager.MAX_SHOW_ITEM * ItemManager.MAX_PAGE)
        {
            Debug.LogError("データが足りません");
            return;
        }

        // ソートしたデータを_itemDataDictに戻す
        for (int i = 0; i < ItemManager.MAX_PAGE; i++)
        {
            var dataList = new List<ItemData>();

            for (int j = 0; j < ItemManager.MAX_SHOW_ITEM; j++)
            {
                // 取得したデータの中身がなくてもリストは用意する
                if ((j + ItemManager.MAX_SHOW_ITEM * i) < datas.Count)
                {
                    dataList.Add(datas[j + (ItemManager.MAX_SHOW_ITEM * i)]);
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
}
