using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DramaTableHelper
{
    /// <summary>
    /// ドラマのマスターデータ
    /// key:code value:同じcodeのデータをindex毎にDictinaryに入れる
    /// </summary>
    public static Dictionary<int, Dictionary<int, DramaTableData.Data>> MstDramaData { get; private set; }

    public static void Load()
    {
        if (MstDramaData != null)
        {
            return;
        }

        var dataList = Resources.Load<DramaTableData>("MasterData/DramaTable").dataList;

        MstDramaData = new Dictionary<int, Dictionary<int, DramaTableData.Data>>();

        // リスト→ディクショナリに変換
        foreach (var data in dataList)
        {
            if(MstDramaData.ContainsKey(data.code))
            {
                // すでにあるならindexを見て中に入れる
                var dramaDict = MstDramaData[data.code];

                if (!MstDramaData[data.code].ContainsKey(data.index))
                {
                    MstDramaData[data.code].Add(data.index, data);
                }
            }
            else
            {
                MstDramaData.Add(data.code, new Dictionary<int, DramaTableData.Data>());
                MstDramaData[data.code].Add(data.index, data);
            }
        }
    }

    /// <summary>
    /// codeを渡してマスターデータからドラマを取得
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    public static Dictionary<int, DramaTableData.Data> GetDramaData(int code)
    {
        if(MstDramaData.ContainsKey(code))
        {
            return MstDramaData[code];
        }
        else
        {
            return null;
        }
    }
}
