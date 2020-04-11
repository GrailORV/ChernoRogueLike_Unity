using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// セーブデータの管理クラス
/// </summary>
public class SaveManager : SaveDataBase
{
    /// <summary>
    /// データをJsonに変換、暗号化して書き込み
    /// </summary>
    public static void Save()
    {
        SaveData saveData = new SaveData();
        var json = JsonUtility.ToJson(saveData);
        json = Encryption.EncryptString(json);
        File.WriteAllText(Application.persistentDataPath + "/" + playerSaveData, json);
    }

    /// <summary>
    /// 読み込んで複合化
    /// </summary>
    /// <param name="filePath">読み込みたいファイルのパス</param>
    /// <returns></returns>
    public static SaveData Load()
    {
        var data = File.ReadAllText(Application.persistentDataPath + "/" + playerSaveData);
        data = Encryption.DecryptString(data);
        Debug.Log(data);
        var saveData = JsonUtility.FromJson<SaveData>(data);

        // 中身が空ならnullを返す
        // TODO : 将来的にはnullではなくエラーメッセージを返したい
        if (saveData == null)
        {
            return null;
        }

        return saveData;
    }
}