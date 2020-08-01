using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMenuWindow : WindowBase
{
    /// <summary>
    /// メニューの種類
    /// </summary>
    public enum MenuType
    {
        None = 0,

        Equip,          // 装備する
        Open,           // 開く
        Throw,          // 投げる
        Put,            // 置く
        Explanation,    // 説明
        Back,           // 戻る
    }

    /// <summary>
    /// メニューの情報
    /// </summary>
    [Serializable]
    public class MenuData
    {
        public MenuType type = MenuType.None;
        public GameObject obj;
        public Navigator navigator;
    }

    // サブメニューの情報リスト
    [SerializeField] List<MenuData> _menuList = new List<MenuData>();

    // 操作するアイテムの情報
    ItemData _itemData = null;

    /// <summary>
    /// 初期化
    /// </summary>
    public void SetUp(ItemData data)
    {
        _itemData = data;

        // アイテムの種類で表示する項目を調整

        // ナビゲーションの設定
        SetUpNavigator();
    }

    /// <summary>
    /// ナビゲーターの設定
    /// </summary>
    public void SetUpNavigator()
    {
        navigationLayer.RemoveNavigatorAll();

        foreach (var menu in _menuList)
        {
            if (menu.obj.activeSelf)
            {
                navigationLayer.AddNavigator(menu.navigator);
            }
        }

        navigationLayer.SetVerticalNavigtor();
        navigationLayer.SetCurrentNavigatorFromIndex(0);
    }

    #region メニューの入力処理

    /// <summary>
    /// 「装備する」ボタン入力時
    /// </summary>
    public void OnClickEquipButton()
    {
        Debug.Log("装備するよ");
    }

    /// <summary>
    /// 「開く」ボタン入力時
    /// </summary>
    public void OnClickOpenButton()
    {
        Debug.Log("開くよ");

        if(_itemData.Type == ItemData.ItemType.Pot)
        {
            var potItemWindow = WindowManager.Instance.CreateAndOpenWindow<PotItemWindow>(WindowData.WindowType.PotItemWindow);
            potItemWindow.SetUp();
        }
    }

    /// <summary>
    /// 「投げる」ボタン入力時
    /// </summary>
    public void OnClickThrowButton()
    {
        Debug.Log("投げるよ");
    }

    /// <summary>
    /// 「置く」ボタン入力時
    /// </summary>
    public void OnClickPutButton()
    {
        Debug.Log("置くよ");
    }

    /// <summary>
    /// 「説明」ボタン入力時
    /// </summary>
    public void OnClickExplanationButton()
    {
        Debug.Log("説明するよ");
    }

    /// <summary>
    /// 「戻る」ボタン入力時
    /// </summary>
    public void OnClickBackButton()
    {
        Debug.Log("戻るよ");
        Close();
    }

    #endregion
}
