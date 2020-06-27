using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ItemWindow : WindowBase
{
    /*
    // アイテムのページャー
    [SerializeField] PagerControl _pager = null;

    // カーソルのプレハブ
    [SerializeField] GameObject _cursorPrefab = null;

    // アイテムのベースプレハブ
    [SerializeField] GameObject _itemBasePrefab = null;

    // アイテムをまとめるオブジェクト
    [SerializeField] GameObject _contentsObj = null;

    // アイテムの画面
    [SerializeField] GameObject _itemWindow = null;

    // アイテムの情報のリストのディクショナリ
    // key:ページ value:アイテムリスト
    Dictionary<int, List<ItemData>> _itemDataDict = new Dictionary<int, List<ItemData>>();

    // アイテムのセルのリスト
    List<ItemCell> _itemCellList = new List<ItemCell>();
    
    // カーソルのImage
    Image _cursorImage = null;

    // 現在のページ数
    int _currentPageNum = 0;

    // 現在の選択しているインデックス
    int _currentSelectIndex = 0;

    // 現在のページ数
    public int CurrentPageNum
    {
        get { return _currentPageNum; }
        set
        {
            _currentPageNum = value;

            // 値が範囲外にいかないようにする
            if (_currentPageNum >= ItemManager.MAX_PAGE)
            {
                _currentPageNum = 0;
            }
            else if (_currentPageNum < 0)
            {
                _currentPageNum = ItemManager.MAX_PAGE - 1;
            }

            // ページャーの切り替え
            _pager.DisplayIndex = _currentPageNum;

            // ページ切り替え時にカーソルを0にする
            CurrentSelectIndex = 0;

            // UIの更新
            ChangePage(_currentPageNum);
        }
    }

    // 現在の選択しているインデックス
    public int CurrentSelectIndex
    {
        get { return _currentSelectIndex; }
        set
        {
            // カーソルの調整
            int difference = value - _currentSelectIndex;
            _currentSelectIndex = AdjustCursorNum(value, (int)Mathf.Clamp(difference, -1f, 1f));

            // UIの更新
            ChangeCursor(_currentSelectIndex);
        }
    }

    public override void Open()
    {
        base.Open();
        Init();
    }

    /// <summary>
    /// 初期化処理
    /// </summary>
    public void Init()
    {
        // ページャーの初期化
        _pager.Init(ItemManager.MAX_PAGE);

        // アイテムの生成
        CreateItemObject();

        // 初期化
        CurrentSelectIndex = 0;
        CurrentPageNum = 0;
    }

    /// <summary>
    /// ウィンドウにアイテムを生成
    /// </summary>
    public void CreateItemObject()
    {
        // 中身をリセットする
        _itemDataDict.Clear();
        if (_contentsObj.transform.childCount > 0)
        {
            foreach (Transform child in _contentsObj.transform)
            {
                Destroy(child.gameObject);
            }
        }

        // データを取得
        var itemDataList = ItemManager.Instance.GetItemData(0);

        // データを格納
        SetData(itemDataList);

        // アイテムのUIを生成
        for (int i = 0; i < ItemManager.MAX_SHOW_ITEM; i++)
        {
            // 生成
            var item = Instantiate(_itemBasePrefab, _contentsObj.transform);
            item.name = _itemBasePrefab.name + i;

            // 情報を設定
            var control = item.GetComponent<ItemCell>();

            // 一ページ目のUIを設定
            control.SetUIContents(_itemDataDict[0][i]);

            // リストの追加
            _itemCellList.Add(control);
        }
    }

    /// <summary>
    /// アイテムのUI更新
    /// </summary>
    /// <param name="dataList"></param>
    public void UpdateItemUI(List<ItemData> dataList)
    {
        for (int i = 0; i < ItemManager.MAX_SHOW_ITEM; i++)
        {
            _itemCellList[i].SetUIContents(dataList[i]);
        }
    }

    /// <summary>
    /// アイテムの追加
    /// </summary>
    /// <param name="id"></param>
    public bool AddItem(int id)
    {
        // アイテムマネージャーで追加
        var result = ItemManager.Instance.AddPlayerItem(id);

        // 追加出来たら更新
        if (result == true)
        {
            // データの更新
            SetData(ItemManager.Instance.GetItemData(0));

            // UIの更新
            UpdateItemUI(_itemDataDict[CurrentPageNum]);
        }

        return result;
    }

    /// <summary>
    /// アイテムの削除
    /// </summary>
    /// <returns></returns>
    public bool DeleteItem(int index, int pageNum)
    {
        // 引数が有効か確認
        if (index < 0 || index >= ItemManager.MAX_SHOW_ITEM)
        {
            Debug.Log("<color=red>indexが無効です</color>");
            return false;
        }
        if (pageNum < 0 || pageNum >= ItemManager.MAX_PAGE)
        {
            Debug.Log("<color=red>pageが無効です</color>");
            return false;
        }

        // アイテムマネージャーで削除
        var result = ItemManager.Instance.DeletePlayerItem(_itemDataDict[pageNum][index]);

        // 削除出来たら更新
        if (result == true)
        {
            // データの更新
            SetData(ItemManager.Instance.GetItemData(0));

            // UIの更新
            UpdateItemUI(_itemDataDict[CurrentPageNum]);

            // カーソルを更新
            CurrentSelectIndex = index;
            if (!_itemCellList[index].gameObject.activeSelf)
            {
                CurrentSelectIndex--;
            }
        }

        return result;
    }

    /// <summary>
    /// ページの変更
    /// </summary>
    /// <param name="pageNum"></param>
    public void ChangePage(int pageNum)
    {
        // ページ数の更新
        if (CurrentPageNum != pageNum)
        {
            CurrentPageNum = pageNum;
            return;
        }

        // UIの更新を行う
        UpdateItemUI(_itemDataDict[pageNum]);
    }

    /// <summary>
    /// アイテムがいっぱいかどうか
    /// </summary>
    /// <returns></returns>
    public bool CheckItemFull()
    {
        // いっぱいならtrue
        if (GetItemNum() >= ItemManager.MAX_SHOW_ITEM * ItemManager.MAX_PAGE)
        {
            return true;
        }
        // いっぱいじゃないならfalse
        else
        {
            Debug.Log("アイテムの数 " + GetItemNum() + " / " + ItemManager.MAX_SHOW_ITEM * ItemManager.MAX_PAGE);
            return false;
        }
    }

    /// <summary>
    /// 所持しているアイテムの数を取得
    /// </summary>
    /// <returns></returns>
    public int GetItemNum()
    {
        int count = 0;

        for (int i = 0; i < ItemManager.MAX_PAGE; i++)
        {
            // _itemDataDict[i]があるかどうか確認。なかったら次のページを確認
            if (!_itemDataDict.ContainsKey(i))
            {
                continue;
            }

            // 中身がnullではないアイテムの数を数える
            var itemList = _itemDataDict[i];
            for (int j = 0; j < itemList.Count; j++)
            {
                if (itemList[j] != null)
                {
                    count++;
                }
            }
        }

        return count;
    }

    /// <summary>
    /// アイテムウィンドウの表示
    /// </summary>
    public void ShowItemWindow()
    {
        // アイテムウィンドウの表示
        _itemWindow.gameObject.SetActive(true);

        // カーソルとページの初期化
        CurrentPageNum = 0;
        CurrentSelectIndex = 0;
    }

    /// <summary>
    /// アイテムウィンドウの非表示
    /// </summary>
    public void HideItemWindow()
    {
        // アイテムウィンドウの非表示
        _itemWindow.gameObject.SetActive(false);
    }

    /// <summary>
    /// カーソルが置けるかどうかの確認
    /// </summary>
    /// <param name="index"></param>
    /// <param name="difference"></param>
    public int AdjustCursorNum(int index, int difference)
    {
        for (int i = 0; i < _itemCellList.Count; i++)
        {
            // 値が範囲外にいかないようにする
            if (index >= ItemManager.MAX_SHOW_ITEM)
            {
                index = 0;
            }
            else if (index < 0)
            {
                index = ItemManager.MAX_SHOW_ITEM - 1;
            }

            if (!_itemCellList[index].gameObject.activeSelf)
            {
                index = index - difference;
            }
            else
            {
                break;
            }
        }

        if (index >= ItemManager.MAX_SHOW_ITEM || index < 0)
        {
            index = 0;
        }

        return index;
    }

    /// <summary>
    /// カーソルの変更
    /// </summary>
    /// <param name="index"></param>
    public void ChangeCursor(int index)
    {
        // カーソルの更新
        if (CurrentSelectIndex != index)
        {
            CurrentSelectIndex = index;
            return;
        }

        // カーソルが無かったら生成
        if (_cursorImage == null)
        {
            var cursorObj = Instantiate(_cursorPrefab, _itemCellList[0].transform) as GameObject;
            cursorObj.name = _cursorPrefab.name;
            _cursorImage = cursorObj.GetComponent<Image>();
        }

        // カーソルの位置を変更させる
        _cursorImage.rectTransform.SetParent(_itemCellList[index].RectTransform);
        _cursorImage.rectTransform.anchoredPosition = Vector3.zero;
    }

    /// <summary>
    /// アイテムの並び替え
    /// </summary>
    public void SortItemData()
    {
        // アイテムデータを一つのリストにする
        List<ItemData> itemDatalist = GetDataList();

        // 一つにまとめたリストをソート
        itemDatalist = itemDatalist.OrderBy(x => x.Id).ToList();

        // ソートしたデータを_itemDataDictに戻す
        for (int i = 0; i < ItemManager.MAX_PAGE; i++)
        {
            var dataList = new List<ItemData>();

            for (int j = 0; j < ItemManager.MAX_SHOW_ITEM; j++)
            {
                // 取得したデータの中身がなくてもリストは用意する
                if ((j + ItemManager.MAX_SHOW_ITEM * i) < itemDatalist.Count)
                {
                    dataList.Add(itemDatalist[j + (ItemManager.MAX_SHOW_ITEM * i)]);
                }
                else
                {
                    dataList.Add(null);
                    continue;
                }
            }

            // ディクショナリに差し替え
            _itemDataDict[i] = dataList;
        }

        // アイテムウィンドウが非表示の時はUIの更新を行わない
        if (!_itemWindow.gameObject.activeSelf)
        {
            // UIの更新
            UpdateItemUI(_itemDataDict[CurrentPageNum]);

            // カーソルの初期化
            CurrentSelectIndex = 0;
        }
    }

    /// <summary>
    /// ぺージごとに分けているデータを取得
    /// </summary>
    /// <returns></returns>
    List<ItemData> GetDataList()
    {
        // アイテムデータを一つのリストにする
        List<ItemData> itemDatalist = new List<ItemData>();
        for (int page = 0; page < ItemManager.MAX_PAGE; page++)
        {
            for (int select = 0; select < ItemManager.MAX_SHOW_ITEM; select++)
            {
                try
                {
                    // データがあるものだけリストに追加
                    if (_itemDataDict[page][select] != null)
                    {
                        itemDatalist.Add(_itemDataDict[page][select]);
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogError("リスト化失敗？：" + e);
                    return null;
                }
            }
        }

        // 一つのリスト化にしたものを返す
        return itemDatalist;
    }

    /// <summary>
    /// 一つにまとめたデータをページごとに分ける
    /// </summary>
    /// <param name="datas"></param>
    void SetData(List<ItemData> datas)
    {
        if (datas == null)
        {
            Debug.LogError("データがありません");
            return;
        }
        if (datas.Count == ItemManager.MAX_SHOW_ITEM * ItemManager.MAX_PAGE)
        {
            Debug.LogError("データが足りません");
            return;
        }

        // ソートしたデータを_itemDataDictに戻す
        for (int i = 0; i < ItemManager.MAX_PAGE; i++)
        {
            var dataList = new List<ItemData>();

            for (int j = 0; j < ItemManager.MAX_SHOW_ITEM; j++)
            {
                // 取得したデータの中身がなくてもリストは用意する
                if ((j + ItemManager.MAX_SHOW_ITEM * i) < datas.Count)
                {
                    dataList.Add(datas[j + (ItemManager.MAX_SHOW_ITEM * i)]);
                }
                else
                {
                    dataList.Add(null);
                    continue;
                }
            }

            // ディクショナリに差し替え
            _itemDataDict[i] = dataList;
        }
    }

    // Update is called once per frame
    void Update ()
    {
        // キー入力
        KeyControl();
    }

    /// <summary>
    /// キー入力
    /// </summary>
    void KeyControl()
    {
        // TODO 汎用の入力クラスが出来たらそっちのほうが良き


        // 左右キーでページの切り替え
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // 右
            CurrentPageNum++;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // 左
            CurrentPageNum--;
        }

        // 上下キーでカーソルの切り替え
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            // 上
            CurrentSelectIndex--;
        }
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            // 下
            CurrentSelectIndex++;
        }

        // TODO デバッグ
        if(Input.GetKeyDown(KeyCode.A))
        {
            // ソートを行う
            SortItemData();
        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            // 追加を行う
            AddItem(0);
        }
        if(Input.GetKeyDown(KeyCode.D))
        {
            // 削除を行う
            DeleteItem(CurrentSelectIndex, CurrentPageNum);
        }
    }
    */
}
