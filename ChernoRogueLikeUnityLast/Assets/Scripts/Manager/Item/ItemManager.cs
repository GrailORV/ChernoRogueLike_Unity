using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class ItemManager : SingletonMonoBehaviour<ItemManager>
{
    /// <summary>
    /// アイテム画面の種類
    /// </summary>
    public enum ItemWindowType
    {
        None = -1,

        Item,       // 所持アイテム
        Inventory,  // 倉庫
        Shop,       // ショップ

        Max,
    }

    // 表示するアイテム数
    public const int SHOW_ITEM_NUM = 10;

    // アイテムの最大ページ数
    public const int MAX_PAGE_NUM_POT       =    1;    // 壺
    public const int MAX_PAGE_NUM_ITEM      =    3;    // 所持品
    public const int MAX_PAGE_NUM_INVENTORY =   10;    // 倉庫
    public const int MAX_PAGE_NUM_SHOP      =    1;    // ショップ

    // プレイヤーのアイテム用ディクショナリ
    private Dictionary<ItemWindowType, List<ItemData>> _playerItemDataDict = new Dictionary<ItemWindowType, List<ItemData>>();

    // 現在開いているアイテム画面
    public ItemWindowType CurrentWindowType { get; set; }

    // アイテムが追加されたときに呼ばれるコールバック
    public event Action OnAddItem;

    // アイテムが削除されたときに呼ばれるコールバック
    public event Action OnRemoveItem;


    /// <summary>
    /// Instance生成時に呼ばれる処理
    /// </summary>
    protected override void Awake()
    {
        base.Awake();

        // 初期化
        Init();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    void Init()
    {
        // マスタデータの読み込み
        ItemTableHelper.Load();

        // アイテム情報の読み込み
        Load();
    }

    /// <summary>
    /// アイテム情報のセーブ
    /// </summary>
    public void Save()
    {

    }

    /// <summary>
    /// アイテム情報のロード
    /// </summary>
    public void Load()
    {
        // まだロード処理内からただの初期化
        _playerItemDataDict.Clear();

        foreach (ItemWindowType key in Enum.GetValues(typeof(ItemWindowType)))
        {
            if (key == ItemWindowType.None || key == ItemWindowType.Max)
            {
                continue;
            }

            _playerItemDataDict.Add(key, new List<ItemData>());
        }
    }

    /// <summary>
    /// プレイヤーのアイテムを取得する
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public List<ItemData> GetPlayerItemList(ItemWindowType type)
    {
        if (type == ItemWindowType.None || type == ItemWindowType.Max)
        {
            Debug.LogError("指定する ItemWindowType が「" + type + "」です");
            return null;
        }
        if (!_playerItemDataDict.ContainsKey(type))
        {
            Debug.LogErrorFormat("「{0}」のキーがDisctionaryに追加されていません", type);
            return null;
        }
        if (_playerItemDataDict[type] == null)
        {
            Debug.LogErrorFormat("「{0}」のアイテムリストが null です", type);
            return null;
        }

        return _playerItemDataDict[type];
    }

    /// <summary>
    /// アイテムを直接設定する
    /// </summary>
    /// <param name="type"></param>
    /// <param name="itemDataList"></param>
    public void SetPlayerItemList(ItemWindowType type, List<ItemData> itemDataList)
    {
        if (type == ItemWindowType.None || type == ItemWindowType.Max)
        {
            Debug.LogError("指定する ItemWindowType が「" + type + "」です");
            return;
        }
        if (!_playerItemDataDict.ContainsKey(type))
        {
            Debug.LogErrorFormat("「{0}」のキーがDisctionaryに追加されていません", type);
            return;
        }

        _playerItemDataDict[type] = itemDataList;
    }

    /// <summary>
    /// アイテムがいっぱいかどうか
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public bool CheckItemFull(ItemWindowType type)
    {
        var maxPageNum = 0;

        switch (type)
        {
            case ItemWindowType.Item:
                maxPageNum = MAX_PAGE_NUM_ITEM;
                break;

            case ItemWindowType.Inventory:
                maxPageNum = MAX_PAGE_NUM_INVENTORY;
                break;

            case ItemWindowType.Shop:
                maxPageNum = MAX_PAGE_NUM_SHOP;
                break;

            default:
                return false;
        }

        var itemList = GetPlayerItemList(type);
        return itemList.Count >= SHOW_ITEM_NUM * maxPageNum;
    }

    /// <summary>
    /// アイテムの追加(id)
    /// </summary>
    /// <param name="type"></param>
    /// <param name="itemId"></param>
    /// <param name="isCallBack"></param>
    /// <returns></returns>
    public bool AddItem(ItemWindowType type, int itemId, bool isCallBack = true)
    {
        // idからアイテムの情報を取得
        if (!ItemTableHelper.ContainsItemId(itemId))
        {
            Debug.LogError("id = " + itemId + " のアイテムはテーブルに存在していません");
            return false;
        }


        return AddItem(type, new ItemData(ItemTableHelper.GetItemData(itemId)), isCallBack);
    }

    /// <summary>
    /// アイテムの追加(id)(複数)
    /// </summary>
    /// <param name="type"></param>
    /// <param name="idList"></param>
    /// <returns></returns>
    public bool AddItem(ItemWindowType type, IReadOnlyList<int> idList)
    {
        if (idList == null)
        {
            Debug.LogError("追加したいアイテムのidリストが null です");
            return false;
        }

        var result = false;

        foreach (var id in idList)
        {
            result = AddItem(type, id, false);

            // 失敗した時点で終了
            if (!result)
            {
                break;
            }
        }

        OnAddItem.SafeInvoke();

        return result;
    }

    /// <summary>
    /// アイテムの追加
    /// </summary>
    /// <param name="type"></param>
    /// <param name="itemData"></param>
    /// <returns></returns>
    public bool AddItem(ItemWindowType type, ItemData itemData, bool isCallBack = true)
    {
        if (itemData == null)
        {
            Debug.LogError("追加したいアイテムのデータが null です");
            return false;
        }

        // いっぱいでなければ入れる
        if (CheckItemFull(type))
        {
            return false;
        }

        var itemList = GetPlayerItemList(type);
        itemList.Add(itemData);

        if (isCallBack)
        {
            OnAddItem.SafeInvoke();
        }

        return true;
    }

    /// <summary>
    /// アイテムの追加(複数)
    /// </summary>
    /// <param name="type"></param>
    /// <param name="itemDataList"></param>
    /// <returns></returns>
    public bool AddItem(ItemWindowType type, IReadOnlyList<ItemData> itemDataList)
    {
        if (itemDataList == null)
        {
            Debug.LogError("追加したいアイテムのデータリストが null です");
            return false;
        }

        var result = false;

        foreach (var data in itemDataList)
        {
            result = AddItem(type, data, false);

            // 失敗した時点で終了
            if(!result)
            {
                break;
            }
        }

        OnAddItem.SafeInvoke();

        return result;
    }

    /// <summary>
    /// アイテムの削除
    /// </summary>
    /// <param name="type"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public bool RemoveItem(ItemWindowType type, ItemData itemData, bool isCallBack = true)
    {
        if (itemData == null)
        {
            Debug.LogError("削除したいアイテムのデータが null です");
            return false;
        }

        var result = false;
        var itemList = GetPlayerItemList(type);

        for (int i = 0; i < itemList.Count; i++)
        {
            var data = itemList[i];

            // 該当するデータの場合は削除
            if (data == itemData)
            {
                itemList.Remove(itemData);
                result = true;
                break;
            }
        }

        if (isCallBack)
        {
            OnRemoveItem.SafeInvoke();
        }

        return result;
    }

    /// <summary>
    /// アイテムの削除(複数)
    /// </summary>
    /// <param name="type"></param>
    /// <param name="itemDataList"></param>
    /// <returns></returns>
    public bool RemoveItem(ItemWindowType type, IReadOnlyList<ItemData> itemDataList)
    {
        if (itemDataList == null)
        {
            Debug.LogError("削除したいアイテムのデータリストが null です");
            return false;
        }

        var result = false;

        foreach (var data in itemDataList)
        {
            result = RemoveItem(type, data);

            // 失敗した時点で終了
            if (!result)
            {
                break;
            }
        }

        OnRemoveItem.SafeInvoke();

        return result;
    }
}
