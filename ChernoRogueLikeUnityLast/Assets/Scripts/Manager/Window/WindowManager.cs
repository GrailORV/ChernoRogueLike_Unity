using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// WindowBaseを管理するクラス。
/// WindowBaseを継承しているプレハブの作成や削除・インスタンスの取得などを
/// このクラスで行えるようにする。これで、参照させる負担やまとめて処理などを
/// 行えるようにする（すべてのウィンドウを閉じるなど→実装するかはわからんが…）
/// </summary>
[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(RectTransform))]
public class WindowManager : SingletonMonoBehaviour<WindowManager>
{
    // Root用のキャンバスオブジェクトのPath
    static readonly string WINDOW_ROOT_CANVAS_PATH = "Prefabs/Manager/Window/RootCanvas";

    // ウィンドウのキャッシュ用のディクショナリ
    // key:WindowData.WindowType → ウィンドウのタイプ
    // value:WindowBase →　WindowBaseクラス
    Dictionary<WindowData.WindowType, WindowBase> _windowDict = new Dictionary<WindowData.WindowType, WindowBase>();

    // Root用のキャンバス
    Canvas _rootCanvas;

    protected override void Awake()
    {
        base.Awake();

        // シーン遷移時オブジェクトは削除されない
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// ウィンドウの作成
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="type"></param>
    /// <returns></returns>
    public T CreateWindow<T>(WindowData.WindowType type) where T : WindowBase
    {
        Debug.LogFormat("<color=blue>ウィンドウ作成！\ninstance = {0} type = {1}</color>", typeof(T), type);
        T instance = null;

        // キャッシュにあるかどうか確認
        if(_windowDict.ContainsKey(type))
        {
            // あるならキャッシュから渡すだけ
            return _windowDict[type] as T;
        }

        // ディクショナリにあるか確認
        if(WindowData.WindowPathDict.ContainsKey(type))
        {
            // 親となるキャンバスが無ければ生成
            if (_rootCanvas == null)
            {
                CreateRootCanvas();
            }

            // ウィンドウの生成
            var obj = Resources.Load(WindowData.WindowPathDict[type]);
            var window = Instantiate(obj, _rootCanvas.transform) as GameObject;
            window.name = obj.name;

            // インスタンスの取得
            instance = window.GetComponent<T>();
            if(instance == null)
            {
                Debug.LogError( typeof(T) + "のインスタンスを取得できませんでした");
                return null;
            }

            // 削除時のコールバックを設定
            instance.OnDestroyAction = () => RemoveCache(type);

            // キャッシュに保存
            _windowDict.Add(type, instance);
        }
        else
        {
            Debug.LogError(type + "のウィンドウデータがWindowData.WindowPathDictにありません");
        }

        return instance;
    }

    /// <summary>
    /// ウィンドウの作成後表示
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="type"></param>
    public T CreateAndOpenWindow<T>(WindowData.WindowType type) where T : WindowBase
    {
        var window = CreateWindow<T>(type);
        if(window != null)
        {
            window.Open();
        }

        return window;
    }

    /// <summary>
    /// ウィンドウの取得
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="type"></param>
    /// <returns></returns>
    public T GetWindow<T>(WindowData.WindowType type) where T : WindowBase
    {
        Debug.LogFormat("<color=blue>ウィンドウの取得！\ninstance = {0} type = {1}</color>", typeof(T), type);
        T instance = null;

        // キャッシュに保存されていないか確認
        if(_windowDict.ContainsKey(type))
        {
            // あれば取得
            instance = _windowDict[type] as T;
        }

        return instance;
    }

    /// <summary>
    /// ウィンドウタイプの取得
    /// </summary>
    /// <param name="window"></param>
    /// <returns></returns>
    public WindowData.WindowType GetWindowType(WindowBase window)
    {
        if (_windowDict.ContainsValue(window))
        {
            return _windowDict.FirstOrDefault(w => w.Value == window).Key;
        }

        return WindowData.WindowType.None;
    }

    /// <summary>
    /// キャッシュの削除
    /// </summary>
    /// <param name="type"></param>
    public void RemoveCache(WindowData.WindowType type)
    {
        if(_windowDict.ContainsKey(type))
        {
            _windowDict.Remove(type);
        }
    }

    /// <summary>
    /// キャンバスの作成
    /// </summary>
    void CreateRootCanvas()
    {
        var prefab = Resources.Load(WINDOW_ROOT_CANVAS_PATH);
        var canvasObject = Instantiate(prefab) as GameObject;
        canvasObject.name = prefab.name;

        _rootCanvas = canvasObject.GetComponent<Canvas>();
    }
}
