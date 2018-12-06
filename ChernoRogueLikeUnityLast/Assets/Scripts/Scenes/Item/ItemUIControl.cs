using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUIControl : MonoBehaviour
{
    // 表示するアイテムの最大数
    const int MAX_ITEM = 10;

    // アイテムウィンドウ
    [SerializeField] GameObject _itemWindowObj = null;

    // アイテムをまとめるオブジェクト
    [SerializeField] GameObject _contentsObj = null;

    // アイテムのベースプレハブ
    [SerializeField] GameObject _itemBasePrefab = null;

    // アイテムの情報のリスト
    List<ItemData> _itemDataList = new List<ItemData>();

    // アイテムのUI操作用のリスト
    List<ItemDataControl> _itemControlList = new List<ItemDataControl>();

    void Start()
    {
        // 初期化
        Init();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            if (_itemDataList != null)
            {
                _itemDataList[0].Name += "(使用済み)";
                ItemDataModifier.WriteItemData(_itemDataList);

                _itemControlList[0].SetUIContents(_itemDataList[0]);
            }
        }
    }

    /// <summary>
    /// 初期化
    /// </summary>
    void Init()
    {
        // ウィンドウの非表示
        _itemWindowObj.SetActive(false);

        // アイテムの生成
        CreateItemObject();
    }

    /// <summary>
    /// 表示ボタンを押したときの処理
    /// </summary>
    public void OnShowButtonClick()
    {
        // アイテムウィンドウの表示・非表示
        _itemWindowObj.SetActive(!_itemWindowObj.activeSelf);
    }
    
    /// <summary>
    /// ウィンドウにアイテムを生成
    /// </summary>
    public void CreateItemObject()
    {
        // データを取得
        _itemDataList = ItemDataModifier.ReadItemData();

        for (int i = 0; i < MAX_ITEM; i++)
        {
            if(i >= _itemDataList.Count)
            {
                return;
            }

            // 生成
            var item = Instantiate(_itemBasePrefab, _contentsObj.transform);

            // 情報を設定
            var control = item.GetComponent<ItemDataControl>();
            control.SetUIContents(_itemDataList[i]);

            // リストの追加
            _itemControlList.Add(control);
        }
    }
}
