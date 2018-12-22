using UnityEngine;
using UnityEngine.UI;

public class GraphicController : SingletonMonoBehaviour<GraphicController> {

    [SerializeField] Toggle FullScreenToggle;

    public void OnClickFullScreenToggle()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }
}
