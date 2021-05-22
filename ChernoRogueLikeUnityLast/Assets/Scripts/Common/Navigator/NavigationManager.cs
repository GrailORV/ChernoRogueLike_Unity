using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NavigationManager : SingletonMonoBehaviour<NavigationManager>
{
    // 入力モード
    public enum InputMode
    {
        Single, // 単一押し
        Multi,  // 同時押し
    }

    // 入力タイプ
    public enum InputType
    {
        Press,  // 押している間
        Down,   // 押した直後
        Up,     // 離した直後
        Repeat, // ピ、、ピピピピピピ
        Same,   // 同時押し
    }

    // 入力キー
    public enum InputKey
    {
        Circle,     // 〇
        Cross,      // ×
        Square,     // □
        Triangle,   // △

        Left1,      // L1
        Left2,      // L2
        Left3,      // L3(ボタン)

        Right1,     // R1
        Right2,     // R2
        Right3,     // R3(ボタン)

        LeftStick,  // Lスティック
        RightStick, // Rスティック

        Select,     // 十字キー
    }

    // 入力方向
    public enum InputDirection
    {
        Up,     // ↑
        Down,   // ↓
        Left,   // ←
        Right,  // →
    }

    // ナビゲーションのレイヤーリスト
    [SerializeField] List<NavigationLayer> _layerList = new List<NavigationLayer>();

    // 通常のレイヤーリストより優先されるレイヤーリスト
    [SerializeField] List<NavigationLayer> _highLayerList = new List<NavigationLayer>();

    // インプットマネージャー
    public InputControllerManager _inputManager;

    // 現在フォーカス中のレイヤー
    NavigationLayer _currentLayer;
    public NavigationLayer CurrentLayer
    {
        get { return _currentLayer; }
        private set
        {
            _currentLayer = value;

            if(value != null)
            {
                _currentLayer.OnLayerActive();
            }
        }
    }

    /// <summary>キー入力が可能か </summary>
    public bool IsKeyInput { get; set; }

    /// <summary>
    /// 絶対にキー入力をさせないかどうか
    /// trueで入力不可 false で入力可
    /// IsKeyInputに関係なく操作不可にする
    /// </summary>
    public bool IsNeverInput { get; set; }

    protected override void Awake()
    {
        base.Awake();

        // シーン遷移時オブジェクトは削除されない
        DontDestroyOnLoad(gameObject);

        // キーの初期化
        _inputManager = new InputControllerManager();

        // キーの割り当て
        _inputManager.SetDefaultButtonAndKeyboard();
    }

    void Update()
    {
        // フォーカス中のレイヤーがない
        if(CurrentLayer == null)
        {
            return;
        }

        // 絶対に押せなくされている
        if(IsNeverInput)
        {
            return;
        }

        // キー入力が不可能
        if(!IsKeyInput)
        {
            return;
        }

        // InputControllerManagerの更新(絶対必要)
        _inputManager.Update();

        // 入力の更新
        CurrentLayer.KeyUpdate(_inputManager);
    }

    /// <summary>
    /// レイヤーの追加
    /// </summary>
    /// <param name="layer"></param>
    /// <param name="上位のレイヤーとして設定するか"></param>
    public void PushLayer(NavigationLayer layer, bool isHighLayer = false)
    {
        // レイヤーがない
        if(layer == null)
        {
            return;
        }

        // フォーカス中のレイヤーと同じ
        if(CurrentLayer == layer)
        {
            return;
        }

        // 登録済みなら削除
        if (_highLayerList.Contains(layer))
        {
            _highLayerList.Remove(layer);
        }
        if (_layerList.Contains(layer))
        {
            _layerList.Remove(layer);
        }

        // レイヤーの設定
        if (isHighLayer)
        {
            _highLayerList.Add(layer);
        }
        else
        {
            _layerList.Add(layer);
        }

        // CurrentLayerの更新
        UpdateCurrentLayer();
    }

    /// <summary>
    /// レイヤーの削除
    /// </summary>
    /// <param name="layer"></param>
    public void RemoveLayer(NavigationLayer layer)
    {
        // レイヤーがない
        if (layer == null)
        {
            return;
        }

        bool result = false;

        // 登録済みなら削除
        if (_highLayerList.Contains(layer))
        {
            _highLayerList.Remove(layer);
            result = true;
        }
        if (_layerList.Contains(layer))
        {
            _layerList.Remove(layer);
            result = true;
        }

        // 削除できれば更新をかける
        if(result)
        {
            UpdateCurrentLayer();
        }
    }

    /// <summary>
    /// フォーカス中のレイヤーの設定
    /// </summary>
    void UpdateCurrentLayer()
    {
        // 上位のレイヤーがあるのならそれを優先
        if (_highLayerList.Count > 0)
        {
            CurrentLayer = _highLayerList.Last();
        }
        // 通常のレイヤーリストから設定
        else if (_layerList.Count > 0)
        {
            CurrentLayer = _layerList.Last();
        }
        else
        {
            CurrentLayer = null;
        }
    }
}