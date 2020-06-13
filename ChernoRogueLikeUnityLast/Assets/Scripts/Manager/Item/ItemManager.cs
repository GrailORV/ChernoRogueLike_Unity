using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemManager : SingletonMonoBehaviour<ItemManager>
{
    /*
    /// <summary>
    /// Instance生成時に呼ばれる処理
    /// </summary>
    protected override void Awake()
    {
        base.Awake();

        // アイテムのマスターデータの取得
        //_mstItemData = ItemDataModifier.GetMstItemData();

        // 全キャラクターのアイテム情報を取得
        // TODO ID管理と仮定しているので変更の可能性あり…現状はプレイヤーのみ
        //var dataList = ItemDataModifier.ReadItemData();
        //_itemDataDict.Add(0, dataList);
    }

    /// <summary>
    /// アイテム情報の取得
    /// </summary>
    /// <param name="charaId"></param>
    /// <returns></returns>
    public List<ItemData> GetItemData(int charaId)
    {
        // 指定したキャラIDのアイテムリストが存在するかどうか
        if(!_itemDataDict.ContainsKey(charaId))
        {
            Debug.LogError("<color=red>charaIDが[" + charaId + "]のアイテムリストが存在しません</color>");
            return null;
        }

        return _itemDataDict[charaId];
    }

    /// <summary>
    /// アイテムの追加
    /// </summary>
    /// <param name="charaId">キャラID</param>
    /// <param name="data">アイテムID</param>
    /// <returns>結果</returns>
    public bool AddItem(int charaId, int itemId)
    {
        // マスタデータが存在するか
        if (_mstItemData == null)
        {
            Debug.LogError("<color=red>アイテムのマスタデータがないので追加できません</color>");
            return false;
        }

        // idが存在するか確認
        if (!_mstItemData.ContainsKey(itemId))
        {
            Debug.LogError("<color=red>ID = " + itemId + "のデータはマスタデータに存在しないです</color>");
            return false;
        }

        // マスタデータからアイテムの情報を取得する
        var data = _mstItemData[itemId];
        return AddItem(charaId, data);
    }

    /// <summary>
    /// アイテムの追加
    /// </summary>
    /// <param name="charaId">キャラID</param>
    /// <param name="data">アイテムの情報</param>
    /// <returns>結果</returns>
    public bool AddItem(int charaId, ItemData data)
    {
        bool result = false;

        // キャラクターが存在するか
        if (!_itemDataDict.ContainsKey(charaId))
        {
            Debug.LogError("<color=red>charaIDが[" + charaId + "]のアイテムリストが存在しません</color>");
            return result;
        }

        // データが存在するか
        if (data == null)
        {
            Debug.LogError("<color=red>追加したいデータが存在しません</color>");
            return result;
        }

        // 該当するキャラにアイテムを追加
        var dataList = _itemDataDict[charaId];
        for (int i = 0; i < MAX_ITEM; i++)
        {
            // 空きがあるか確認
            if(0 < dataList[i].Id)
            {
                continue;
            }

            // アイテム追加
            dataList[i] = data;
            result = true;
            break;
        }

        // UIの更新
        return result;
    }

    /// <summary>
    /// アイテムの削除
    /// </summary>
    /// <param name="charaId">キャラID</param>
    /// <param name="itemId">アイテムID</param>
    /// <returns></returns>
    public bool DeleteItem(int charaId, int itemId)
    {
        // マスタデータが存在するか
        if (_mstItemData == null)
        {
            Debug.LogError("<color=red>アイテムのマスタデータがないので追加できません</color>");
            return false;
        }

        // idが存在するか確認
        if (!_mstItemData.ContainsKey(itemId))
        {
            Debug.LogError("<color=red>ID = " + itemId + "のデータはマスタデータに存在しないです</color>");
            return false;
        }

        // マスタデータからアイテムの情報を取得する
        var data = _mstItemData[itemId];
        return DeleteItem(charaId, data);
    }

    /// <summary>
    /// アイテムの削除
    /// </summary>
    /// <param name="charaId">キャラID</param>
    /// <param name="data">アイテムの情報</param>
    /// <returns>結果</returns>
    public bool DeleteItem(int charaId, ItemData data)
    {
        bool result = false;

        // キャラクターが存在するか
        if (!_itemDataDict.ContainsKey(charaId))
        {
            Debug.LogError("<color=red>charaIDが[" + charaId + "]のアイテムリストが存在しません</color>");
            return result;
        }

        // データが存在するか
        if (data == null)
        {
            Debug.LogError("<color=red>削除したいデータが存在しません</color>");
            return result;
        }

        // 該当するキャラのアイテムを削除
        var dataList = _itemDataDict[charaId];
        result = dataList.Remove(data);

        if(!result)
        {
            Debug.LogError("<color=red>削除したいデータを所持していません</color>");
            return result;
        }

        // 空のデータを追加
        //dataList.Add(new ItemData());
        return result;
    }

    /// <summary>
    /// プレイヤーのアイテム追加
    /// </summary>
    /// <returns></returns>
    public bool AddPlayerItem(int itemId)
    {
        return AddItem(0, itemId);
    }

    /// <summary>
    /// プレイヤーのアイテム追加
    /// </summary>
    /// <returns></returns>
    public bool AddPlayerItem(ItemData data)
    {
        return AddItem(0, data);
    }

    /// <summary>
    /// プレイヤーのアイテム削除
    /// </summary>
    /// <param name="itemId"></param>
    /// <returns></returns>
    public bool DeletePlayerItem(int itemId)
    {
        return DeleteItem(0, itemId);
    }

    /// <summary>
    /// プレーヤーのアイテム削除
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public bool DeletePlayerItem(ItemData data)
    {
        return DeleteItem(0, data);
    }
    */
}
