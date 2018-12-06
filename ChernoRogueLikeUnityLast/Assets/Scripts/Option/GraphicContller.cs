using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class GraphicContller : MonoBehaviour {

    [SerializeField] Toggle FullScreenToggle;

    private static Vector2 minSize = new Vector2(100.0f, 100.0f);
    private static Rect position = new Rect(0.0f, 0.0f, 640.0f, 480.0f);

    public void OnClickFullScreenToggle()
    {
        EditorWindow gameView = GetGameView();

        if(FullScreenToggle.isOn)
        {
            gameView.Close();

            var width = Screen.currentResolution.width;
            var height = Screen.currentResolution.height;
            var offset = 17.0f;

            gameView = GetGameView();
            gameView.minSize = new Vector2(width, height + offset);
            gameView.position = new Rect(0, -offset, width, height + offset);
        }

        else
        {
            gameView.minSize = minSize;
            gameView.position = position;
            gameView.Close();
        }
    }

    private static EditorWindow GetGameView()
    {
        // ウィンドウがない場合生成
        return EditorWindow.GetWindow(System.Type.GetType("UnityEditor.GameView,UnityEditor"));
    }
}
