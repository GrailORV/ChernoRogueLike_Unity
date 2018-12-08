using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class GraphicController : MonoBehaviour {

    [SerializeField] Toggle FullScreenToggle;

    public void OnClickFullScreenToggle()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }
}
