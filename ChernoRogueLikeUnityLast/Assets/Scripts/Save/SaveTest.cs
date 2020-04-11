using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveTest : MonoBehaviour
{
    /// <summary>
    /// 読み込みデータ
    /// </summary>
    [SerializeField]
    private Text text = null;

    /// <summary>
    /// 保存したいデータ
    /// </summary>
    [SerializeField]
    private InputField inputFirld = null;

    /// <summary>
    /// 保存データ
    /// </summary>
    private SaveDataBase.SaveData saveData = null;

    private void Awake()
    {
        this.saveData = new SaveDataBase.SaveData();
    }

    /// <summary>
    /// セーブボタンイベント
    /// </summary>
    public void OnClickSaveButton()
    {
        this.saveDataEncode();
        SaveManager.Save();
    }

    /// <summary>
    /// ロードボタンイベント
    /// </summary>
    public void OnClickLoadButton()
    {
        var data = SaveManager.Load();
        if (data == null)
        {
            Debug.Log(data);
            return;
        }
        Debug.Log("データ読み込み成功");
        Debug.Log(data.playerName);
        Debug.Log(data.playerLevel);
    }

    private void saveDataEncode()
    {
        this.saveData.playerName = this.inputFirld.text;
    }

    public void changeInputText()
    {
        if(this.inputFirld.text == "")
        {
            this.text.text = "入力してください";
            return;
        }
        this.text.text = this.inputFirld.text;
    }
}
