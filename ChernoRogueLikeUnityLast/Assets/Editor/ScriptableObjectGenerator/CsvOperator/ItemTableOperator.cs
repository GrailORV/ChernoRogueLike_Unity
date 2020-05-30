using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using System;
using System.IO;

public class ItemTableOperator : AssetPostprocessor
{
    // csvファイルが存在するPath
    static readonly string CSV_FILE_PATH = "Assets/Resources/Csv/ItemTable.csv";

    // ScriptableObjectを生成するPath
    static readonly string SCRIPTABLE_OBJECT_EXPORT_PATH = "Assets/Resources/MasterData/ItemTable.asset";

    // Keyの列を表す記号
    static readonly string CSV_KEY_SYMBOL = "##";

    // 読み込みたくない行を表す記号
    static readonly string CSV_IGNORE_KEY = "//";

    /// <summary>
    /// すべてのアセットがインポートされた後に呼ばれる処理
    /// </summary>
    /// <param name="importedAssets"></param>
    /// <param name="deletedAssets"></param>
    /// <param name="movedAssets"></param>
    /// <param name="movedFromAssetPaths"></param>
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        // 自分自身が作成されていればScriptableObjectを作成する
        foreach (var path in importedAssets)
        {
            if (path != CSV_FILE_PATH)
            {
                continue;
            }

            // ScriptableObjectにデータを設定
            // パスをもとにcsvファイルを読み込む
            string csvtext = string.Empty;

            try
            {
                csvtext = File.ReadAllText(CSV_FILE_PATH);
            }
            catch (System.Exception)
            {
                Debug.LogError("csvファイルが読み込めませんでした。\npath = " + CSV_FILE_PATH);
                return;
            }

            // ScriptableObjectを作成or更新
            var asset = (ItemTableData)AssetDatabase.LoadAssetAtPath(SCRIPTABLE_OBJECT_EXPORT_PATH, typeof(ItemTableData));

            // ScriptableObjectが無ければ新規作成
            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance<ItemTableData>();
                AssetDatabase.CreateAsset(asset, SCRIPTABLE_OBJECT_EXPORT_PATH);
            }

            // リストの初期化
            asset.dataList.Clear();

            // テキストを読み込み、データを挿入していく
            var stringReader = new StringReader(csvtext);
            var isStart = false;
            while (stringReader.Peek() > -1)
            {
                // 一行ずつ取り出す
                var line = stringReader.ReadLine();

                // 改行コード削除
                line = line.Replace("\n", "").Replace("¥r", "");

                // CSV_KEY_SYMBOL が入っている行がスタート位置なのでそこまで検索
                if (!isStart)
                {
                    // 見つけるまで繰り返し
                    if (line.Contains(CSV_KEY_SYMBOL))
                    {
                        // 見つけたら次の行から検索できるようにする
                        isStart = true;
                    }

                    continue;
                }

                // コメントアウト行ならその行は読み込まない
                if (line.Contains(CSV_IGNORE_KEY))
                {
                    continue;
                }

                // カンマ区切りで分割
                var cells = line.Split(',');
                if (cells.Length == 0)
                {
                    continue;
                }

                // データを挿入
                var data = new ItemTableData.Data();
                data.id = int.Parse(cells[0]);
                data.type = cells[1];
                data.name = cells[2];

                // データを挿入
                asset.dataList.Add(data);
            }

            // 更新
            ScriptableObject obj = AssetDatabase.LoadAssetAtPath(SCRIPTABLE_OBJECT_EXPORT_PATH, typeof(ScriptableObject)) as ScriptableObject;
            EditorUtility.SetDirty(obj);
        }
    }
}
