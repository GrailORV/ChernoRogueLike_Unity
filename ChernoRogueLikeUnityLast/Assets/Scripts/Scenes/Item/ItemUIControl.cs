using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ItemUIControl : MonoBehaviour
{
    // アイテムウィンドウのコントロールクラス
    [SerializeField] ItemWindowControl _itemWindowControl = null;

    // カーソルのプレハブ
    [SerializeField] GameObject _cursorPrefab = null;

    // アイテムをまとめるオブジェクト
    [SerializeField] GameObject _contentsObj = null;

    // アイテムのベースプレハブ
    [SerializeField] GameObject _itemBasePrefab = null;

    // アイテムのマスタデータ(ディクショナリ)
    // key:アイテムのID value:アイテムデータ
    Dictionary<int, ItemData> _mstItemData = new Dictionary<int, ItemData>();

    // アイテムの情報のリストのディクショナリ
    // key:ページ value:アイテムリスト
    Dictionary<int, List<ItemData>> _itemDataDict = new Dictionary<int, List<ItemData>>();

    // アイテムのUI操作用のリスト
    List<ItemDataControl> _itemControlList = new List<ItemDataControl>();

    // カーソルのImage
    Image _cursorImage = null;

    /// <summary>
    /// 現在のページ数
    /// </summary>
    public int CurrentPageNum
    {
        get { return _itemWindowControl.CurrentPageNum; }
        set { _itemWindowControl.CurrentPageNum = value; }
    }

    /// <summary>
    /// 現在の選択しているインデックス
    /// </summary>
    public int CurrentSelectIndex
    {
        get { return _itemWindowControl.CurrentSelectIndex; }
        set { _itemWindowControl.CurrentSelectIndex = value; }
    }

    void Awake()
    {
        // マスタデータの取得
        _mstItemData = ItemDataModifier.GetMstItemData();

        if(_mstItemData == null)
        {
            Debug.LogWarning("アイテムのマスタデータがないですyo!");
        }
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public void Init()
    {
        // アイテムの生成
        CreateItemObject();

        // ウィンドウの初期化
        _itemWindowControl.Init(this);
        _itemWindowControl.gameObject.SetActive(false);
    }

    /// <summary>
    /// 表示ボタンを押したときの処理
    /// </summary>
    public void OnShowButtonClick()
    {
        // アイテムウィンドウの表示・非表示
        if(_itemWindowControl.gameObject.activeSelf)
        {
            HideItemWindow();
        }
        else
        {
            ShowItemWindow();
        }
    }

    /// <summary>
    /// アイテムウィンドウの表示
    /// </summary>
    public void ShowItemWindow()
    {
        // アイテムウィンドウの表示
        _itemWindowControl.gameObject.SetActive(true);

        // カーソルとページの初期化
        CurrentPageNum     = 0;
        CurrentSelectIndex = 0;
    }

    /// <summary>
    /// アイテムウィンドウの非表示
    /// </summary>
    public void HideItemWindow()
    {
        // アイテムウィンドウの非表示
        _itemWindowControl.gameObject.SetActive(false);
    }

    /// <summary>
    /// ウィンドウにアイテムを生成
    /// </summary>
    public void CreateItemObject()
    {
        // 中身をリセットする
        if (_contentsObj.transform.childCount > 0)
        {
            foreach (Transform child in _contentsObj.transform)
            {
                Destroy(child.gameObject);
            }
        }

        // データを取得
        var allItemDataList = ItemDataModifier.ReadItemData();

        // アイテムデータをページごとに格納
        for (int i = 0; i < ItemManager.MAX_PAGE; i++)
        {
            var dataList = new List<ItemData>();

            for (int j = 0; j < ItemManager.MAX_ITEM; j++)
            {
                // 取得したデータの中身がなくてもリストは用意する
                if ((j + ItemManager.MAX_ITEM * i) < allItemDataList.Count)
                {
                    dataList.Add(allItemDataList[j + (ItemManager.MAX_ITEM * i)]);
                }
                else
                {
                    // ない場合はnull代入
                    dataList.Add(null);
                    continue;
                }
            }
            
            // ディクショナリに追加
            _itemDataDict.Add(i, dataList);
        }

        // アイテムのUIを生成
        for (int i = 0; i < ItemManager.MAX_ITEM; i++)
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
        for (int i = 0; i < ItemManager.MAX_ITEM; i++)
        {
            _itemControlList[i].SetUIContents(dataList[i]);
        }
    }

    /// <summary>
    /// ページの変更
    /// </summary>
    /// <param name="pageNum"></param>
    public void ChangePage(int pageNum)
    {
        // ページ数の更新
        if(CurrentPageNum != pageNum)
        {
            CurrentPageNum = pageNum;
            return;
        }
        
        // UIの更新を行う
        UpdateItemUI(_itemDataDict[pageNum]);
    }

    /// <summary>
    /// カーソルが置けるかどうかの確認
    /// </summary>
    /// <param name="index"></param>
    /// <param name="difference"></param>
    public int AdjustCursorNum(int index ,int difference)
    {
        for(int i = 0; i < _itemControlList.Count; i++)       
        {
            // 値が範囲外にいかないようにする
            if (index >= ItemManager.MAX_ITEM)
            {
                index = 0;
            }
            else if (index < 0)
            {
                index = ItemManager.MAX_ITEM - 1;
            }

            if (!_itemControlList[index].gameObject.activeSelf)
            {
                index = index - difference;
            } 
            else
            {
                break;
            }
        }

        if(index >= ItemManager.MAX_ITEM || index < 0)
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
        if(CurrentSelectIndex != index)
        {
            CurrentSelectIndex = index;
            return;
        }

        // カーソルが無かったら生成
        if(_cursorImage == null)
        {
            var cursorObj = Instantiate(_cursorPrefab, _itemControlList[0].transform) as GameObject;
            cursorObj.name = _cursorPrefab.name;
            _cursorImage = cursorObj.GetComponent<Image>();
        }

        // カーソルの位置を変更させる
        _cursorImage.rectTransform.SetParent(_itemControlList[index].RectTransform);
        _cursorImage.rectTransform.anchoredPosition = Vector3.zero;
    }

    /// <summary>
    /// アイテムの追加
    /// </summary>
    /// <param name="id"></param>
    public bool AddItem(int id)
    {
        bool isSuccess = false;

        // マスタデータがあるか確認
        if(_mstItemData == null)
        {
            Debug.Log("<color=red>アイテムのマスタデータがないので追加できません</color>");
            return isSuccess;
        }
        
        // idが存在するか確認
        if(!_mstItemData.ContainsKey(id))
        {
            Debug.Log("<color=red>ID = " + id + "のデータはマスタデータに存在しないです</color>");
            return isSuccess;
        }

        // アイテムがいっぱいかどうか
        if (CheckItemFull())
        {
            Debug.Log("<color=red>アイテムがいっぱいです</color>");
            return isSuccess;
        }

        // 空いているところにアイテムデータを追加する
        for (int i = 0; i < ItemManager.MAX_PAGE; i++)
        {
            // ページが存在するか確認
            if(!_itemDataDict.ContainsKey(i))
            {
                continue;
            }

            var itemList = _itemDataDict[i];
            for (int j = 0; j < ItemManager.MAX_ITEM; j++)
            {
                // 空きがあるならデータを追加する
                if (itemList[j].Id < 0)
                {
                    // マスタデータの情報を取得
                    itemList[j] = _mstItemData[id];

                    // ディクショナリに保存
                    _itemDataDict[i] = itemList;
                    isSuccess = true;
                    break;
                }
            }
        }

        // UIの更新
        UpdateItemUI(_itemDataDict[CurrentPageNum]);
        return isSuccess;
    }

    /// <summary>
    /// アイテムの削除
    /// </summary>
    /// <returns></returns>
    public bool DeleteItem(int index, int pageNum)
    {
        bool isSuccess = false;

        // 引数が有効か確認
        if(index < 0 || index >= ItemManager.MAX_ITEM)
        {
            Debug.Log("<color=red>indexが無効です</color>");
            return isSuccess;
        }
        if (pageNum < 0 || pageNum >= ItemManager.MAX_PAGE)
        {
            Debug.Log("<color=red>pageが無効です</color>");
            return isSuccess;
        }
        // ページが存在するか確認
        if (!_itemDataDict.ContainsKey(pageNum))
        {
            Debug.Log("<color=red>pageが存在しないです page=" + pageNum + "</color>");
            return isSuccess;
        }
        // 指定したインデックスのアイテムがあるか確認
        if(_itemDataDict[pageNum][index] == null)
        {
            Debug.Log("<color=red>削除したいアイテムがありません [" + pageNum + "][" + index + "]</color>");
            return isSuccess;
        }

        // 削除
        var dataList = GetDataList();
        var deleteIndex = pageNum * ItemManager.MAX_ITEM + index;
        dataList.RemoveAt(deleteIndex);

        // 削除した分空のデータを追加
        dataList.Add(null);

        // データを整理        
        SetData(dataList);
        isSuccess = true;

        // UIの更新
        UpdateItemUI(_itemDataDict[CurrentPageNum]);

        // カーソルを更新
        CurrentSelectIndex = index;
        if (!_itemControlList[index].gameObject.activeSelf)
        {
            CurrentSelectIndex--;
        }
        return isSuccess;
    }

    /// <summary>
    /// アイテムがいっぱいかどうか
    /// </summary>
    /// <returns></returns>
    public bool CheckItemFull()
    {
        // いっぱいならtrue
        if(GetItemNum() >= ItemManager.MAX_ITEM * ItemManager.MAX_PAGE)
        {
            return true;
        }
        // いっぱいじゃないならfalse
        else
        {
            Debug.Log("アイテムの数 " + GetItemNum() + " / " + ItemManager.MAX_ITEM * ItemManager.MAX_PAGE);
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

        for(int i = 0; i < ItemManager.MAX_PAGE; i++)
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
                if(itemList[j] != null)
                {
                    count++;
                }
            }
        }

        return count;
    }
    
    /// <summary>
    /// アイテムの並び替え
    /// </summary>
    public void SortItemData()
    {
        // アイテムウィンドウが非表示の時は処理を行わない
        if(!_itemWindowControl.gameObject.activeSelf)
        {
            return;
        }

        // アイテムデータを一つのリストにする
        List<ItemData> itemDatalist = GetDataList();

        // 一つにまとめたリストをソート
        itemDatalist = itemDatalist.OrderBy(x => x.Id).ToList();
        
        // ソートしたデータを_itemDataDictに戻す
        for (int i = 0; i < ItemManager.MAX_PAGE; i++)
        {
            var dataList = new List<ItemData>();

            for (int j = 0; j < ItemManager.MAX_ITEM; j++)
            {
                // 取得したデータの中身がなくてもリストは用意する
                if ((j + ItemManager.MAX_ITEM * i) < itemDatalist.Count)
                {
                    dataList.Add(itemDatalist[j + (ItemManager.MAX_ITEM * i)]);
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

        // UIの更新
        UpdateItemUI(_itemDataDict[CurrentPageNum]);

        // カーソルの初期化
        CurrentSelectIndex = 0;
    }

    /// <summary>
    /// パージごとに分けているデータを取得
    /// </summary>
    /// <returns></returns>
    List<ItemData> GetDataList()
    {
        // アイテムデータを一つのリストにする
        List<ItemData> itemDatalist = new List<ItemData>();
        for (int page = 0; page < ItemManager.MAX_PAGE; page++)
        {
            for (int select = 0; select < ItemManager.MAX_ITEM; select++)
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
        if(datas.Count == ItemManager.MAX_ITEM * ItemManager.MAX_PAGE)
        {
            Debug.LogError("データが足りません");
            return;
        }

        // ソートしたデータを_itemDataDictに戻す
        for (int i = 0; i < ItemManager.MAX_PAGE; i++)
        {
            var dataList = new List<ItemData>();

            for (int j = 0; j < ItemManager.MAX_ITEM; j++)
            {
                // 取得したデータの中身がなくてもリストは用意する
                if ((j + ItemManager.MAX_ITEM * i) < datas.Count)
                {
                    dataList.Add(datas[j + (ItemManager.MAX_ITEM * i)]);
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
}
