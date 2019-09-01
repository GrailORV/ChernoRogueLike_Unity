using UnityEngine;

/// <summary>
/// オプションの管理クラス
/// </summary>
public class OptionController : SingletonMonoBehaviour<OptionController> {

    /// <summary>
    /// オプション画面
    /// </summary>
    [SerializeField]
    private GameObject option = null;


    /// <summary>
    /// Unity Awake
    /// </summary>
    override protected void Awake()
    {
        base.Awake();
    }

    /// <summary>
    /// Unity Update
    /// </summary>
    private void Update()
    {
        // オプションがあるなら処理
        if(this.option != null)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                // オプションを表示
                if (!this.option.activeSelf)
                    OpenOption();
                // オプションを非表示
                else
                    CloseOption();
            }
        }
    }

    /// <summary>
    /// オプションを開く
    /// </summary>
    public void OpenOption()
    {
        // 中身がnullなら処理しない
        if (this.option == null) return;

        // オプションを表示
        this.option.SetActive(true);
    }

    /// <summary>
    /// オプションを閉じる
    /// </summary>
    public void CloseOption()
    {
        // 変更していて確定前だったら確認する
        // 変更しない場合変更前の状態にすべて戻す


        // 中身がnullなら処理しない
        if (this.option == null) return;

        // オプションを非表示
        this.option.SetActive(false);
    }
}