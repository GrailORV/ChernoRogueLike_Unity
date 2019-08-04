using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUIControl : MonoBehaviour
{
    // 表示するアイテムの最大数
    const int MAX_ITEM = 10;

    // アイテムのページの最大数
    const int MAX_PAGE = 4;

    // アイテムウィンドウ
    [SerializeField] GameObject _itemWindowObj = null;

    // アイテムをまとめるオブジェクト
    [SerializeField] GameObject _contentsObj = null;

    // アイテムのベースプレハブ
    [SerializeField] GameObject _itemBasePrefab = null;

    // アイテムのページャー
    [SerializeField] PagerControl _pager = null;

    // アイテムの情報のリストのディクショナリ　key:ページ value:アイテムリスト
    Dictionary<int, List<ItemData>> _itemDataDict = new Dictionary<int, List<ItemData>>();

    // アイテムのUI操作用のリスト
    List<ItemDataControl> _itemControlList = new List<ItemDataControl>();

    // 現在のページ数
    int _currentPageNum = 0;

    /// <summary>
    /// 現在のページ数
    /// </summary>
    public int CurrentPageNum
    {
        get { return _currentPageNum; }
        set
        {
            // 値が範囲外にいかないようにする
            _currentPageNum = Mathf.Clamp(value, 0, MAX_PAGE - 1);
            Debug.Log(_currentPageNum);
            // UIの更新
            UpdateItemUI(_itemDataDict[_currentPageNum]);
        }
    }

    void Start()
    {
        // 初期化
        Init();
    }

    void Update()
    {
        if(_itemWindowObj.activeSelf)
        {
            if(Input.GetKeyDown(KeyCode.RightArrow))
            {
                CurrentPageNum++;
                _pager.DisplayIndex++;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                CurrentPageNum--;
                _pager.DisplayIndex--;
            }
        }
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public void Init()
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
        var allItemDataList = ItemDataModifier.ReadItemData();

        // ページャーの初期化
        _pager.Init(MAX_PAGE);

        // アイテムデータをページごとに格納
        for (int i = 0; i < MAX_PAGE; i++)
        {
            var dataList = new List<ItemData>();

            for (int j = 0; j < MAX_ITEM; j++)
            {
                // 取得したデータの中身がなくてもリストは用意する
                if ((j + MAX_ITEM * i) < allItemDataList.Count)
                {
                    dataList.Add(allItemDataList[j + (MAX_ITEM * i)]);
                }
                else
                {
                    dataList.Add(null);
                    continue;
                }
            }
            
            // ディクショナリに追加
            _itemDataDict.Add(i, dataList);
        }

        // アイテムのUIを生成
        for (int i = 0; i < MAX_ITEM; i++)
        {           
            // 生成
            var item = Instantiate(_itemBasePrefab, _contentsObj.transform);
            item.name = _itemBasePrefab.name + i;

            // 情報を設定
            var control = item.GetComponent<ItemDataControl>();

            // 一ページ目のUIを設定
            control.SetUIContents(_itemDataDict[0][i]);

            // リストの追加
            _itemControlList.Add(control);
        }
    }

    /// <summary>
    /// アイテムのUI更新
    /// </summary>
    /// <param name="dataList"></param>
    public void UpdateItemUI(List<ItemData> dataList)
    {
        for (int i = 0; i < MAX_ITEM; i++)
        {
            _itemControlList[i].SetUIContents(dataList[i]);
        }
    }
}
