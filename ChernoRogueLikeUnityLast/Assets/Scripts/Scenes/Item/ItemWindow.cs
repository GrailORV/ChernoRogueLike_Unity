using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 所持するアイテム一覧用画面
/// </summary>
public class ItemWindow : ItemWindowBase
{
    // 最大ページ数
    public override int MaxPage
    {
        get { return ItemManager.MAX_PAGE_NUM_ITEM; }
    }

    public override void SetUp()
    {
        _windowType = ItemManager.ItemWindowType.Item;
        ItemManager.Instance.CurrentWindowType = _windowType;

        // ページャーの初期化
        _pager.Init(MaxPage);

        // 初期化
        CurrentPageIndex = 0;

        // アイテム情報の更新
        SetData(ItemManager.Instance.GetPlayerItemList(_windowType));
        ItemManager.Instance.AddItem(_windowType, new List<int>() { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2 });

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
}
