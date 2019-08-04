using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// セーブデータの管理クラス
/// </summary>
public class SaveManager : SaveData
{
    /// <summary>
    /// データをJsonに変換して書き込み
    /// </summary>
    /// <param name="filePath">保存先のパス</param>
    /// <param name="saveData">セーブしたいデータ</param>
    public static void Save(string filePath, SaveData saveData, SaveDataList.SaveDataType type)
    {
        // セーブデータを取得
        var path = Resources.Load(playerSaveData);
        // パスが取得できなかったもしくは保存したいデータがないなら中止
        if (path == null || saveData == null) return;

        switch (type)
        {
            // プレイヤーデータ（レベルや名前など）
            case SaveDataList.SaveDataType.PlayerData:
                FileStream fs = new FileStream(path.ToString(), FileMode.Create, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(JsonUtility.ToJson(saveData));
                sw.Flush();
                sw.Close();
                break;

            case SaveDataList.SaveDataType.ItemData:
                break;
        }
    }

    /// <summary>
    /// データロード
    /// </summary>
    /// <param name="filePath">読み込みたいファイルのパス</param>
    /// <returns></returns>
    public static SaveData Load(string filePath)
    {
        var path = Resources.Load(playerSaveData);

        // セーブデータのファイルがない
        if (path == null)
        {
            Debug.Log("セーブデータがありません");
            return null;
        }

        // ファイルを開く
        FileStream fs = new FileStream(path.ToString(), FileMode.Open, FileAccess.Read);
        StreamReader sr = new StreamReader(fs);

        // セーブデータを取得
        SaveData saveData = JsonUtility.FromJson<SaveData>(sr.ReadToEnd());

        // 中身が空ならnullを返す
        if (saveData == null) return null;

        return saveData;
    }
}