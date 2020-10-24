using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotItemWindow : ItemWindowBase
{
    // 壺アイテムのデータ
    ItemData _potItemData;

    // 最大ページ数
    public override int MaxPage
    {
        get { return ItemManager.MAX_PAGE_NUM_POT; }
    }

    public void SetUp(ItemData itemData)
    {
        _potItemData = itemData;

        // ページャーの初期化
        _pager.Init(MaxPage);

        // 初期化
        CurrentPageIndex = 0;

        // アイテム情報の更新
        SetData(_potItemData.PotItemDataList);

        // アイテム一覧ないなら作成
        if (_itemCellList == null || _itemCellList.Count != ItemManager.SHOW_ITEM_NUM)
        {
            CreateItemCell();
        }
        // 作成済みならUIの更新
        else
        {
            UpdateCurrentPageItemUI();
        }
    }

    /// <summary>
    /// 壺からアイテムを取り出す
    /// </summary>
    /// <param name="itemData"></param>
    public void RemovePotItem(ItemData itemData)
    {
        if(itemData == null)
        {
            return;
        }

        // 今は必ず取り出せる
        var itemManager = ItemManager.Instance;

        // アイテムがいっぱいなら取り出せない
        if (itemManager.CheckItemFull(itemManager.CurrentWindowType))
        {
            return;
        }

        // 壺から取り出す
        _potItemData.PotItemDataList.Remove(itemData);
        itemManager.AddItem(itemManager.CurrentWindowType, itemData);

        // UIの更新
        SetData(_potItemData.PotItemDataList);
        UpdateCurrentPageItemUI();
    }

    public override void OnUpdateItem()
    {
        Debug.LogError("壺アイテムの更新");
    }
}
