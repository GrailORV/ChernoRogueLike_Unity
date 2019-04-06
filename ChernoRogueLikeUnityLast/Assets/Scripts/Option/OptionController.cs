using UnityEngine;

public class OptionController : SingletonMonoBehaviour<OptionController> {

    private void Update()
    {
        // Escapeキーでオプションを閉じる
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            CloseOption();
        }
    }

    /// <summary>
    /// オプションを閉じる
    /// </summary>
    public void CloseOption()
    {
        // 変更していて確定前だったら確認する
        // 変更しない場合変更前の状態にすべて戻す


        // オプションを非表示にする
        foreach (Transform child in this.gameObject.transform)
        {
            child.gameObject.SetActive(!child.gameObject.activeSelf);
        }
    }
}