using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// セーブデータの管理クラス
/// </summary>
public class SaveManager : SingletonMonoBehaviour<SaveManager>
{
    /// <summary>
    /// セーブデータ
    /// </summary>
    public class SaveData
    {
        public int playerLavel = 0;     // プレイヤーレベル
        public string name = "";        // 名前
        public List<int> ItemDatas = new List<int>();   // 所持アイテムのリスト
    }

    private SaveData saveDatas = new SaveData();

    [HideInInspector]
    public string playerSavePath = "SaveData/PlayerData";

    /// <summary>
    /// データをJsonに変換して書き込み
    /// </summary>
    /// <param name="filePath">保存先のパス</param>
    /// <param name="saveData">セーブしたいデータ</param>
    public void Save(string filePath,SaveData saveData)
    {
        var path = Resources.Load(playerSavePath);

        FileStream fs = new FileStream(path.ToString(), FileMode.Create, FileAccess.Write);
        StreamWriter sw = new StreamWriter(fs);

        var newSaveData = saveData;

        sw.WriteLine(JsonUtility.ToJson(newSaveData));
    }

    /// <summary>
    /// データロード
    /// </summary>
    /// <param name="filePath">読み込みたいファイルのパス</param>
    /// <returns></returns>
    public SaveData Load(string filePath)
    {
        var path = Resources.Load(playerSavePath);

        // セーブデータのファイルがない
        if (!File.Exists(path.ToString()))
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

    /// <summary>
    /// Unity Updata
    /// </summary>
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            saveDatas.name = "omnk";
            saveDatas.playerLavel = 10;
        }
    }


    public void LoadButton()
    {
        Load(playerSavePath);
    }


    public void SaveButton()
    {
        Save(playerSavePath, saveDatas);
    }
}
