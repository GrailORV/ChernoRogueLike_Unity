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

    /// <summary>
    /// UIの設定
    /// </summary>
    public void SetUIContents(ItemData data)
    {
        if(data == null)
        {
            gameObject.SetActive(false);
            return;
        }
        if (data.Id < 0)
        {
            gameObject.SetActive(false);
            return;
        }

        // TODO まだ名前のみ
        _nameText.text = data.Name;

        // 表示
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 削除されたとき呼ばれる
    /// </summary>
    void OnDestroy()
    {
        // 保持していた変数の初期化
        _rectTransform = null;
        _nameText = null;
        _sumbnailImage.sprite = null;
        _sumbnailImage = null;
    }
}
