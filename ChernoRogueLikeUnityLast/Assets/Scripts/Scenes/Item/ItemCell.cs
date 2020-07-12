using System;
using UnityEngine;
using UnityEngine.UI;

public class ItemCell: MonoBehaviour
{
    // レクとトランスフォーム
    [SerializeField] RectTransform _rectTransform;
    public RectTransform RectTransform
    {
        get { return _rectTransform; }
        set { _rectTransform = value; }
    }

    // 名前用のテキスト
    [SerializeField] Text _nameText;
    public Text NameText
    {
        get { return _nameText; }
        set { _nameText = value; }
    }

    // サムネ用の画像
    [SerializeField] Image _sumbnailImage;
    public Image Sumbnail
    {
        get { return _sumbnailImage; }
        set { _sumbnailImage = value; }
    }

    // データが無い状態かどうか
    public bool IsEmpty { get; private set; }

    // UIの管理用オブジェクト
    [SerializeField] GameObject _contentsRoot;

    // 入力時の処理
    public Action<ItemData> OnClickAction;

    // アイテムの情報
    ItemData _itemData;

    /// <summary>
    /// UIの設定
    /// </summary>
    public void SetUIContents(ItemData data)
    {
        _itemData = data;

        // オブジェクトは必ず表示
        gameObject.SetActive(true);

        // アイテムあるかどうか
        IsEmpty = _itemData == null || _itemData.Id < 0;
        ShowItem(!IsEmpty);

        // データに沿って情報の設定
        NameText.text = _itemData.Name;
        Sumbnail.gameObject.SetActive(true);
    }

    /// <summary>
    /// アイテムの表示状態の設定
    /// </summary>
    public void ShowItem(bool show)
    {
        _contentsRoot.SetActive(show);
    }

    /// <summary>
    /// セルを選択した時の処理
    /// </summary>
    public void OnClickCell()
    {
        if (_itemData == null || OnClickAction == null)
        {
            return;
        }

        OnClickAction.SafeInvoke(_itemData);
    }
}
