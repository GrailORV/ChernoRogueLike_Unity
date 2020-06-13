using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ItemTableHelper
{
    /// <summary>
    /// アイテムのマスターデータ
    /// key:id value:ItemTableData.Data
    /// </summary>
    public static Dictionary<int, ItemTableData.Data> MstItemData { get; private set; }

    /// <summary>
    /// テーブルの読み込み
    /// </summary>
    public static void Load()
    {
        var dataList = Resources.Load<ItemTableData>("MasterData/ItemTable").dataList;

        // リストディクショナリに変換する
        MstItemData = dataList.ToDictionary(data => data.id);
    }

    /// <summary>
    /// アイテムがあるかどうか(ID 基準)
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static bool ContainsItemId(int id)
    {
        return MstItemData.ContainsKey(id);
    }

    /// <summary>
    /// マスタデータからアイテムを取得
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static ItemTableData.Data GetItemData(int id)
    {
        if(MstItemData.ContainsKey(id))
        {
            return MstItemData[id];
        }
        else
        {
            return null;
        }
    }
}
