using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 倉庫のアイテム一覧用画面
/// </summary>
public class InventoryWindow : ItemWindowBase
{
    public override void SetUp()
    {
        ItemManager.Instance.CurrentWindowType = ItemManager.ItemWindowType.Inventory;

        // ページャーの初期化
        _pager.Init(MaxPage);

        // 初期化
        CurrentPageIndex = 0;

        // アイテム情報の更新
        SetData(GetDataList());

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