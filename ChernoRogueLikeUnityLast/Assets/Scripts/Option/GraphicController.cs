using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 画面設定の管理クラス
/// </summary>
public class GraphicController : SingletonMonoBehaviour<GraphicController>
{
    /// <summary>
    /// 全画面にするかどうかのチェックボックス
    /// </summary>
    [SerializeField] Toggle FullScreenToggle;


    /// <summary>
    /// Unity Awake
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
    }

    /// <summary>
    /// 画面の設定変更
    /// </summary>
    public void OnClickFullScreenToggle()
    {
        // フルスクリーンの切り替え
        Screen.fullScreen = !Screen.fullScreen;
    }
}
