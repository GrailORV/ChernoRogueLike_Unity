using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ItemManager : SingletonMonoBehaviour<ItemManager>
{
    // アイテムコントローラーのプレハブのパス
    const string ITEM_PREFAB_PATH = "Prefabs/Scene/Item/ItemCanvas";

    // アイテムのコントローラークラス
    ItemUIControl _itemControl = null;

    /// <summary>
    /// Instance生成時に呼ばれる処理
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        Debug.Log("ItemManager OnAwake");

        // アイテムの操作に必要なオブジェクトを生成
        var prefab = Resources.Load(ITEM_PREFAB_PATH) as GameObject;
        var itemCanvas = Instantiate(prefab, transform) as GameObject;
        itemCanvas.name = prefab.name;
        _itemControl = itemCanvas.GetComponent<ItemUIControl>();

        // アイテムクラスの初期化
        _itemControl.Init();
    }
}
