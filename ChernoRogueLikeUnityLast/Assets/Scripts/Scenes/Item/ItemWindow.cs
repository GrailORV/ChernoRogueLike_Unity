using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 所持するアイテム一覧用画面
/// </summary>
public class ItemWindow : ItemWindowBase
{
    public override void SetUp()
    {
        // ページャーの初期化
        _pager.Init(MaxPage);

        // 初期化
        CurrentPageIndex = 0;

        // アイテム情報の更新
        SetData(GetDataList());

        ItemTableHelper.Load();
        AddItem(new List<int>() { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2 });

        // アイテム一覧ないなら作成
        if (_itemCellList == null || _itemCellList.Count != SHOW_ITEM_NUM)
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
