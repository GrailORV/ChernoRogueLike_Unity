using UnityEngine;
using UnityEngine.UI;

public class GraphicController : SingletonMonoBehaviour<GraphicController> {

    [SerializeField] Toggle FullScreenToggle;


    /// <summary>
    /// 画面の設定変更
    /// </summary>
    public void OnClickFullScreenToggle()
    {
        // フルスクリーンの切り替え
        Screen.fullScreen = !Screen.fullScreen;
    }
}
