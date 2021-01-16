using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDescriptionWindow : WindowBase
{
    // アイコン
    [SerializeField] Image _iconImage;
    
    // アイテム名
    [SerializeField] Text _nameText;

    // 説明
    [SerializeField] Text _descriptionText;

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="itemData"></param>
    public void SetUp(ItemData itemData)
    {
        // 名前の設定
        _nameText.text = itemData.Name;

        // 説明の設定
        _descriptionText.text = itemData.Description;

        // アイコン
        //_iconImage.sprite = 
    }
}
