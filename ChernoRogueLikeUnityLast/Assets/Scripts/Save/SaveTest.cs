using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveTest : MonoBehaviour
{
    /// <summary>
    /// セーブボタン
    /// </summary>
    [SerializeField]
    private GameObject saveButton = null;
    /// <summary>
    /// ロードボタン
    /// </summary>
    [SerializeField]
    private GameObject loadButton = null;

    /// <summary>
    /// セーブボタンイベント
    /// </summary>
    public void OnClickSaveButton()
    {
        SaveManager.Save(SaveData.playerSaveData, null, SaveDataTypeData.SaveDataType.PlayerData);
    }

    /// <summary>
    /// ロードボタンイベント
    /// </summary>
    public void OnClickLoadButton()
    {
        SaveManager.Load(SaveData.playerSaveData);
    }

    /// <summary>
    /// Unity Update
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SaveData.name = "omnk";
            SaveData.playerLavel = 1919;
        }
    }
}
