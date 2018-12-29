using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// アイテムのデータのやり取り
/// </summary>
public class ItemDataModifier
{
    // アイテムの項目
    public enum ItemDataHead
    {
        Id = 0,
        Type,
        Name,
    }

    // Itemのデータ用CSVの保存パス
    const string ITEM_DATA_PATH = "Csv/ItemData";

    // 説明文などで読み込まない行の数(上からのみ)
    const int DONT_READ_LINE_NUM = 1;

    /// <summary>
    /// アイテム(.csvファイル)の読み込み
    /// </summary>
    /// <returns>アイテムデータのリスト</returns>
    public static List<ItemData> ReadItemData()
    {
        // アイテムのcsvのテキストデータを読み込む
        var itemDataCsv = Resources.Load(ITEM_DATA_PATH) as TextAsset;
        if (itemDataCsv == null)
        {
            Debug.LogWarning(ITEM_DATA_PATH + "がないです");
            return null;
        }

        // 行で分割する
        var lines = itemDataCsv.text.Split('\n');
        var lineLength = lines.Length;

        if(lineLength <= 0)
        {
            Debug.LogWarning("ItemData.csvの中身が空(\"\")です");
            return null;
        }

        // 最期が空白の行ならカウントしない
        if (lines[lines.Length - 1] == "")
        {
            lineLength--;
        }

        // ItemDataクラスに入れる
        var itemDataList = new List<ItemData>();
        for (int i = 0; i < lineLength; i++)
        {
            //Debug.LogFormat("{0}行目 : {1}", i, lines[i]);

            // 改行文字を削除
            lines[i] = lines[i].Replace("\r", "").Replace("\n", "");

            // 読み込まない行数分は除外
            if (i < DONT_READ_LINE_NUM)
            {
                continue;
            }

            // 項目ごとに分割
            var head = lines[i].Split(',');
            
            // dataに情報を入れていく
            ItemData data = new ItemData();
            {
                // Id
                string text = head[(int)ItemDataHead.Id];
                int id = int.Parse(text);
                data.Id = id;

                // タイプ
                data.Type = head[(int)ItemDataHead.Type];

                // 名前
                data.Name = head[(int)ItemDataHead.Name];
            }

            // リストにdataを追加
            itemDataList.Add(data);
        }

        // データリストを返す
        return itemDataList;
    }

    /// <summary>
    /// アイテム(.csvファイル)の書き込み
    /// </summary>
    /// <param name="dataList">書き込むアイテムの情報 </param>
    public static void WriteItemData(List<ItemData> dataList)
    {
        if (dataList == null)
        {
            Debug.LogWarning("dataListがnullです");
            return;
        }

        // アイテムのcsvのテキストデータを読み込む
        var itemDataCsv = Resources.Load(ITEM_DATA_PATH) as TextAsset;
        if (itemDataCsv == null)
        {
            Debug.LogWarning(ITEM_DATA_PATH + "がないです");
            return;
        }

        // 行で分割する
        var lines = itemDataCsv.text.Split('\n');
        var lineLength = dataList.Count;

        if (lineLength <= 0)
        {
            Debug.LogWarning("ItemData.csvの中身が空(\"\")です");
            return;
        }

        // 最期が空白の行ならカウントしない
        if (lines[lines.Length - 1] == "")
        {
            lineLength--;
        }

        // dataListをcsv用の形にする
        string itemCsvText = "";
        for(int i= 0; i< lines.Length;i++)
        {
            if (i < DONT_READ_LINE_NUM)
            {
                itemCsvText += lines[i] + "\n";
            }
            else
            {
                break;
            }
        }

        for (int i = 0; i < lineLength; i++)
        {
            var data = dataList[i];

            // ItemDataの情報をテキストにいれる
            itemCsvText +=
                data.Id   + "," +
                data.Type + "," +
                data.Name +
                "\n";
        }

        // 書き込みを行う
        var streamWriter = new StreamWriter(Application.dataPath + "/Resources/" + ITEM_DATA_PATH + ".csv", false);
        streamWriter.Write(itemCsvText);
        streamWriter.Flush();
        streamWriter.Close();
        Debug.LogFormat("csv.text = " + itemCsvText);
    }
}
