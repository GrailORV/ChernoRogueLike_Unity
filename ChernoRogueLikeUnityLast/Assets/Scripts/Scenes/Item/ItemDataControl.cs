using UnityEngine;
using UnityEngine.UI;

public class ItemDataControl: MonoBehaviour
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

    /// <summary>
    /// UIの設定
    /// </summary>
    public void SetUIContents(ItemData data)
    {
        // TODO まだ名前のみ
        _nameText.text = data.Name;
    }
}
