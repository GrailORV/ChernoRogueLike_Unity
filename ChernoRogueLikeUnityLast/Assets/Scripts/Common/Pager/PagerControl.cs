using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PagerControl : MonoBehaviour
{
    // アイコンのdefaultサイズ
    const float ICON_DEFAULT_SIZE = 30f;

    // アイコンのスケール
    const float ICON_DEFAULT_SCALE = 1f;
    const float ICON_ACTIVE_SCALE  = 1.5f;

    // アイコンの width & Height
    [SerializeField] Vector2 _iconSize = new Vector2(ICON_DEFAULT_SIZE, ICON_DEFAULT_SIZE);

    // アイコンのリスト
    [SerializeField] List<Sprite> _iconSpriteList = new List<Sprite>();

    // ページャーのImageリスト
    List<Image> _pagerList = new List<Image>();

    // 表示する画像のインデックス
    int displayIndex = 0;
    public int DisplayIndex
    {
        get { return displayIndex; }
        set
        {
            // 値がはみ出ないように調整
            displayIndex = value;
            if (displayIndex >= _pagerList.Count)
            {
                var count    = _pagerList.Count;
                displayIndex = --count;
            }
            if (displayIndex < 0)
            {
                displayIndex = 0;
            }

            // UIの更新
            IconUIUpdate();
        }
    }

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="pagerNum">ページャーの数</param>
    /// <param name="startPos">デフォルトの位置(0～)</param>
    public void Init(int pagerNum, int startPos = 0)
    {
        if(_iconSpriteList.Count == 0)
        {
            Debug.Log("<color=yellow>ページャーのアイコン画像が一つも設定されていません</color>");
            return;
        }

        // 子オブジェクトを削除
        if(transform.childCount > 0)
        {
            foreach(Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }

        _pagerList.Clear();

        // ページャーを作成
        for (int i = 0; i < pagerNum; i++)
        {
            var obj = new GameObject("pager_" + i);
            var icon = obj.AddComponent<Image>();

            // 親の設定
            icon.transform.SetParent(transform);

            // 画像の設定
            icon.sprite = _iconSpriteList[i % _iconSpriteList.Count];

            // アスペクト比固定
            icon.preserveAspect = true;

            // サイズの設定
            icon.rectTransform.sizeDelta = _iconSize;

            // リストに追加
            _pagerList.Add(icon);
        }

        // 初期位置の調整
        DisplayIndex = startPos;
    }

    /// <summary>
    /// アイコンのUIの更新
    /// </summary>
    void IconUIUpdate()
    {
        if(_pagerList.Count == 0)
        {
            Debug.Log("<color=yellow>ページャーがないのでUIの更新が出来ませんでした</color>");
            return;
        }

        for (int i = 0; i < _pagerList.Count; i++)
        {
            var icon = _pagerList[i];

            // アクティブの場合
            if(i == DisplayIndex)
            {
                // サイズを大きく、色を通常に
                icon.rectTransform.localScale = new Vector2(ICON_ACTIVE_SCALE, ICON_ACTIVE_SCALE);
                icon.color = Color.white;
            }
            // 非アクティブの時
            else
            {
                // サイズを通常に、色を暗く
                icon.rectTransform.localScale = new Vector2(ICON_DEFAULT_SCALE, ICON_DEFAULT_SCALE);
                icon.color = Color.gray;
            }
        }
    }

    /// <summary>
    /// 表示しているページャーの数を取得
    /// </summary>
    /// <returns></returns>
    public int GetCount()
    {
        return _pagerList == null ? 0 : _pagerList.Count;
    }
}
