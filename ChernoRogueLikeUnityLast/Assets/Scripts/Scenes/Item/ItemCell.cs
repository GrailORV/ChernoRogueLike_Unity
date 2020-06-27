using UnityEngine;
using UnityEngine.UI;

public class ItemCell: MonoBehaviour
{
    // レクとトランスフォーム
    [SerializeField] RectTransform _rectTransform = null;
    public RectTransform RectTransform
    {
        get { return _rectTransform; }
        set { _rectTransform = value; }
    }

    // 名前用のテキスト
    [SerializeField] Text _nameText = null;
    public Text NameText
    {
        get { return _nameText; }
        set { _nameText = value; }
    }

    // サムネ用の画像
    [SerializeField] Image _sumbnailImage = null;
    public Image Sumbnail
    {
        get { return _sumbnailImage; }
        set { _sumbnailImage = value; }
    }

    bool _isEmpty = false;

    /// <summary>
    /// UIの設定
    /// </summary>
    public void SetUIContents(ItemData data)
    {
        if(data == null || data.Id < 0)
        {
            ShowEmpty();
            return;
        }

        _isEmpty = false;

        // TODO まだ名前のみ
        _nameText.text = data.Name;

        // 表示
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 「なし」表示の状態にする
    /// </summary>
    public void ShowEmpty()
    {
        _isEmpty = true;

        NameText.text = "なし";
        Sumbnail.gameObject.SetActive(false);
    }
}
