using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class ItemMenuWindow : WindowBase
{
    /// <summary>
    /// メニューの種類
    /// </summary>
    public enum MenuType
    {
        None = 0,

        Take,           // 取り出す
        Equip,          // 装備する
        Eat,            // 食べる
        Open,           // 開く
        Set,            // 入れる
        Throw,          // 投げる
        Put,            // 置く
        Description,    // 説明
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

    // 壺用アイテム画面のメニューかどうか
    [SerializeField] bool _isPotMenuWindow;

    // サブメニューの情報リスト
    [SerializeField] List<MenuData> _menuList = new List<MenuData>();

    // 対応しているアイテムウィンドウ
    ItemWindowBase _itemWindow;

    // 操作するアイテムの情報
    ItemData _itemData;

    /// <summary>
    /// 初期化
    /// </summary>
    public void SetUp(ItemWindowBase window,ItemData data)
    {
        _itemData = data;
        _itemWindow = window;

        // アイテムの種類で表示する項目を調整
        SetDisplayMenu();

        // ナビゲーションの設定
        SetUpNavigator();
    }

    /// <summary>
    /// 表示されるメニューの設定
    /// </summary>
    void SetDisplayMenu()
    {
        if(_itemData == null)
        {
            return;
        }

        // すべてのメニューを非表示
        HideAllMenu();

        var types = new List<MenuType>();

        // アイテムの種類によって表示させるメニューを変える
        switch (_itemData.Type)
        {
            // 武器＆防具：装備する
            case ItemData.ItemType.Wepon:
            case ItemData.ItemType.armor:
                types.Add(MenuType.Equip);
                break;

            // 食べ物：食べる
            case ItemData.ItemType.Food:
                types.Add(MenuType.Eat);
                break;

            // 壺：開く・入れる
            case ItemData.ItemType.Pot:
                types.Add(MenuType.Open);
                types.Add(MenuType.Set);
                break;
        }

        // 投げる・置く・説明・戻る　は必ず入れる
        types.Add(MenuType.Throw);
        types.Add(MenuType.Put);
        types.Add(MenuType.Description);
        types.Add(MenuType.Back);

        // 壺の中身を表示しているなら「取り出す」を表示する
        if (_isPotMenuWindow)
        {
            types.Add(MenuType.Take);
        }

        // 表示
        ShowMenu(types);
    }

    /// <summary>
    /// メニューの表示
    /// </summary>
    /// <param name="types"></param>
    public void ShowMenu(List<MenuType> types)
    {
        // 引数で指定したメニューを取り出す
        var showMenus = _menuList.Where(menu => types.Contains(menu.type));

        foreach (var menu in showMenus)
        {
            menu.obj.SetActive(true);
        }
    }

    /// <summary>
    /// すべてのメニューを非表示
    /// </summary>
    public void HideAllMenu()
    {
        foreach(var menu in _menuList)
        {
            menu.obj.SetActive(false);
        }
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
    /// 「取り出す」ボタン入力時
    /// </summary>
    public void OnClickTakeButton()
    {
        Debug.Log("取り出すよ");

        if (_itemWindow is PotItemWindow)
        {
            var potItemWindow = _itemWindow as PotItemWindow;
            potItemWindow.RemovePotItem(_itemData);
        }

        Close();
    }

    /// <summary>
    /// 「装備する」ボタン入力時
    /// </summary>
    public void OnClickEquipButton()
    {
        Debug.Log("装備するよ");
    }

    /// <summary>
    /// 「食べる」ボタン入力時
    /// </summary>
    public void OnClickEatButton()
    {
        Debug.Log("食べるよ");
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
            potItemWindow.SetUp(_itemData);

            Close();
        }
    }

    /// <summary>
    /// 「入れる」ボタン入力時
    /// </summary>
    public void OnClickSetButton()
    {
        Debug.Log("入れるよ");

        if (_itemData.Type == ItemData.ItemType.Pot)
        {
            if (_itemWindow != null)
            {
                _itemWindow.OnSelectSetPotItem();
            }

            Close();
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
    public void OnClickDescriptionButton()
    {
        Debug.Log("説明するよ");

        var descriptionWindow = WindowManager.Instance.CreateAndOpenWindow<ItemDescriptionWindow>(WindowData.WindowType.ItemDescriptionWindow);
        descriptionWindow.SetUp(_itemData);
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