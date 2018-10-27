using UnityEngine;
using UnityEngine.UI;

public class ItemUIControl : MonoBehaviour
{
    // アイテムウィンドウ
    [SerializeField] GameObject _itemWindowObj = null;

    // 表示用
    [SerializeField] Button _showButton = null;

    /// <summary>
    /// 表示ボタンを押したときの処理
    /// </summary>
    public void OnShowButtonClick()
    {
        // アイテムウィンドウの表示・非表示
        _itemWindowObj.SetActive(!_itemWindowObj.activeSelf);
    }
}
