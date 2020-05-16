using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemTableHelper
{
    /// <summary>
    /// アイテムのマスターデータ
    /// </summary>
    public static List<ItemData> MstItemData { get; private set; }

    /// <summary>
    /// テーブルの読み込み
    /// </summary>
    public static void Load()
    {
        //MstItemData = Resources.Load<ItemTabe>("MasterData/ItemTable").
    }
}
