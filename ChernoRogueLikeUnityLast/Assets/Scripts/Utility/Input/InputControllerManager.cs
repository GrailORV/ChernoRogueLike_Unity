using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

using InputController;

public class InputControllerManager
{
    // ゲームパッド
    public enum GamePad
    {
        One, Two, Three, Four, Max, None
    }

    public struct InputBuffer
    {
        public Controller.Button button;
        public int count;
        public Action action;

        public InputBuffer(Controller.Button button, int count, Action action)
        {
            this.button = button;
            this.count = count;
            this.action = action;
        }

        public void Delete()
        {
            this.action = null;
        }
    }

    // 一時保存の入力情報を保存しておくフレーム数
    static readonly int BUFFER_HOLD_FRAME = 3;

    /// <summary>
    /// GamePadのひな形
    /// </summary>
    Dictionary<Controller.Button, int> _padButtonDict = new Dictionary<Controller.Button, int>()
    {
        {Controller.Button.Button_0 , 0  },
        {Controller.Button.Button_1 , 1  },
        {Controller.Button.Button_2 , 2  },
        {Controller.Button.Button_3 , 3  },
        {Controller.Button.Button_4 , 4  },
        {Controller.Button.Button_5 , 5  },
        {Controller.Button.Button_6 , 6  },
        {Controller.Button.Button_7 , 7  },
        {Controller.Button.Button_8 , 8  },
        {Controller.Button.Button_9 , 9  },
        {Controller.Button.Button_10, 10 },
        {Controller.Button.Button_11, 11 },
    };

    // コントローラーリスト
    List<Controller> _controllerList = new List<Controller>();

    // コントローラーのボタン用リスト
    List<Dictionary<Controller.Button, int>> _gamePadList = null;

    // 入力の一時保存用(バッファー)
    List<InputBuffer> _inputBuffList = new List<InputBuffer>();

    // 最後に更新したフレーム
    int _lastUpdateFrame = -1;

    // しきい値
    public double Threshold { get; private set; }

    /// <summary>
    /// unityのデフォルトのInputを対応させる
    /// </summary>
    public void SetDefaultButton()
    {
        // コントローラーの初期化
        _controllerList = new List<Controller>();

        // unity用の対応
        var controller = new Controller();

        // □
        controller.ButtonDict[Controller.Button.Button_0].AddKeyName("Square");

        // ×
        controller.ButtonDict[Controller.Button.Button_1].AddKeyName("Cross");

        // 〇
        controller.ButtonDict[Controller.Button.Button_2].AddKeyName("Circle");

        // △
        controller.ButtonDict[Controller.Button.Button_3].AddKeyName("Triangle");

        // L1
        controller.ButtonDict[Controller.Button.Button_4].AddKeyName("Left1");

        // R1
        controller.ButtonDict[Controller.Button.Button_5].AddKeyName("Right1");

        // L2
        controller.ButtonDict[Controller.Button.Button_6].AddKeyName("Left2");

        // R2
        controller.ButtonDict[Controller.Button.Button_7].AddKeyName("Right2");

        // share
        controller.ButtonDict[Controller.Button.Button_8].AddKeyName("Share");

        // Option
        controller.ButtonDict[Controller.Button.Button_9].AddKeyName("Option");

        // L3
        controller.ButtonDict[Controller.Button.Button_10].AddKeyName("Left3");

        // R3
        controller.ButtonDict[Controller.Button.Button_11].AddKeyName("Right3");

        // Lスティック
        controller.AxisDict[Controller.Axis.L_Horizontal].AddKeyName("LeftHorizontal");
        controller.AxisDict[Controller.Axis.L_Vertical].AddKeyName("LeftVertical");

        // Rスティック
        controller.AxisDict[Controller.Axis.R_Horizontal].AddKeyName("RightHorizontal");
        controller.AxisDict[Controller.Axis.R_Vertical].AddKeyName("RightVertical");

        // 十字キー
        controller.AxisDict[Controller.Axis.Cross_Horizontal].AddKeyName("CrossHorizontal");
        controller.AxisDict[Controller.Axis.Cross_Vertical].AddKeyName("CrossVertical");

        // コントローラーを追加する
        AddController(controller);
    }

    /// <summary>
    /// キーボード操作を行う
    /// </summary>
    public void SetForKeyboard()
    {
        // コントローラーの初期化
        _controllerList = new List<Controller>();

        // unity用の対応
        var controller = new Controller();

        // □
        controller.ButtonDict[Controller.Button.Button_0].AddKeyCode(KeyCode.R);

        // ×
        controller.ButtonDict[Controller.Button.Button_1].AddKeyCode(KeyCode.E);

        // 〇
        controller.ButtonDict[Controller.Button.Button_2].AddKeyCode(KeyCode.F);

        // △
        controller.ButtonDict[Controller.Button.Button_3].AddKeyCode(KeyCode.Q);

        // share
        controller.ButtonDict[Controller.Button.Button_8].AddKeyCode(KeyCode.P);

        // Option
        controller.ButtonDict[Controller.Button.Button_9].AddKeyCode(KeyCode.Escape);

        // Lスティック
        controller.AxisDict[Controller.Axis.L_Horizontal].AddKeyCode(KeyCode.D, KeyCode.A);
        controller.AxisDict[Controller.Axis.L_Vertical].AddKeyCode(KeyCode.W, KeyCode.S);

        // Rスティック
        controller.AxisDict[Controller.Axis.R_Horizontal].AddKeyCode(KeyCode.RightArrow, KeyCode.LeftArrow);
        controller.AxisDict[Controller.Axis.R_Vertical].AddKeyCode(KeyCode.UpArrow, KeyCode.DownArrow);

        // 十字キー
        controller.AxisDict[Controller.Axis.Cross_Horizontal].AddKeyCode(KeyCode.RightArrow, KeyCode.LeftArrow);
        controller.AxisDict[Controller.Axis.Cross_Vertical].AddKeyCode(KeyCode.UpArrow, KeyCode.DownArrow);

        // コントローラーを追加する
        AddController(controller);
    }

    /// <summary>
    /// 任意のキーの登録
    /// </summary>
    /// <param name="buttons"></param>
    /// <param name="buttonKeys"></param>
    /// <param name="axes"></param>
    /// <param name="axisKeys"></param>
    public void SetKeyCodes(List<Controller.Button> buttons, List<KeyCode> buttonKeys,
        List<Controller.Axis> axes, List<KeyCode[]> axisKeys)
    {
        if (_controllerList == null)
        {
            _controllerList = new List<Controller>();
        }

        // コントローラーが無かったら生成
        Controller controller;
        if(_controllerList.Count == 0)
        {
            controller = new Controller();
            AddController(controller);
        }

        // キーコードを設定
        controller = _controllerList[0];

        // button
        for (int i = 0; i < buttons.Count; i++)
        {
            controller.SetButtonKeyCode(buttons[i], buttonKeys[i]);
        }

        // axis
        for (var i = 0; i < axes.Count; i++)
        {
            controller.SetAxisKeyCodes(axes[i], axisKeys[i][0], axisKeys[i][1]);
        }
    }

    /// <summary>
    /// ゲームパッドのひな型の設定用
    /// </summary>
    /// <param name="button"></param>
    /// <param name="buttonNum"></param>
    public void SetDefaultPadButton(Controller.Button button, int buttonNum)
    {
        _padButtonDict[button] = buttonNum;
    }

    #region GamePad初期化用関数
    /// <summary>
    /// _padButtonDictを使ってGamePadを作成
    /// </summary>
    void InitializeGamePads()
    {
        // パッドの初期化
        _gamePadList = new List<Dictionary<Controller.Button, int>>();

        for (var i = 0; i < (int)GamePad.Max; i++)
        {
            var buttons = new Dictionary<Controller.Button, int>();
            for (var j = 0; i < _padButtonDict.Count; j++)
            {
                buttons.Add((Controller.Button)j, _padButtonDict[(Controller.Button)j]);
            }

            _gamePadList.Add(buttons);
        }

        SetGamePads();
    }

    /// <summary>
    /// 指定したコントローラーにボタンを設定(GamePad.Maxの場合はすべて)
    /// </summary>
    /// <param name="pad"></param>
    /// <param name="button"></param>
    /// <param name="buttonNum"></param>
    public void SetPadButton(GamePad pad, Controller.Button button, int buttonNum)
    {
        // パッドの初期化
        if (_gamePadList == null)
        {
            InitializeGamePads();
        }

        // 指定したパッドに設定
        if ((int)pad < (int)GamePad.Max)
        {
            _gamePadList[(int)pad][button] = buttonNum;
        }
        // Maxに設定すればすべてに適応
        else if ((int)pad == (int)GamePad.Max)
        {
            for (var i = 0; i < (int)GamePad.Max; i++)
            {
                _gamePadList[i][button] = buttonNum;
            }
        }
        SetGamePads();
    }

    /// <summary>
    /// ゲームパッド(Controllerクラス)を四つ設定する
    /// </summary>
    public void SetGamePads()
    {
        // ゲームパッドが一つもなかったら初期化
        if(_gamePadList == null)
        {
            InitializeGamePads();
        }

        _controllerList = new List<Controller>();

        for (var i = 0; i < (int)GamePad.Max; i++)
        {
            // コントローラーの作成
            Controller controller = new Controller();
            for (var j = 0; j < (int)Controller.Button.Max; j++)
            {
                // KeyCodeの350番代からJoyStickのキー
                // 20ずつ「1、2・・・」とコントローラーの番号が変わる
                controller.ButtonDict[(Controller.Button)j].AddKeyCode((KeyCode)(350 + (20 * i) + _gamePadList[i][(Controller.Button)j]));
            }

            // 軸の設定
            controller.AxisDict[Controller.Axis.R_Horizontal].AddKeyName("GamePad" + (i + 1) + "_R_Horizontal");
            controller.AxisDict[Controller.Axis.R_Vertical]  .AddKeyName("GamePad" + (i + 1) + "_R_Vertical");
            controller.AxisDict[Controller.Axis.L_Horizontal].AddKeyName("GamePad" + (i + 1) + "_L_Horizontal");
            controller.AxisDict[Controller.Axis.L_Vertical]  .AddKeyName("GamePad" + (i + 1) + "_L_Vertical");
            controller.AxisDict[Controller.Axis.Cross_Horizontal].AddKeyName("GamePad" + (i + 1) + "_Cross_Horizontal");
            controller.AxisDict[Controller.Axis.Cross_Vertical]  .AddKeyName("GamePad" + (i + 1) + "_Cross_Vertical");

            AddController(controller);
        }
    }
    #endregion

    /// <summary>
    /// コントローラーの追加
    /// </summary>
    /// <param name="controller"></param>
    public void AddController(Controller controller)
    {
        _controllerList.Add(controller);
    }

    /// <summary>
    /// Axisのしきい値の設定
    /// </summary>
    /// <param name="threshold"></param>
    /// <param name="pad"></param>
    public void SetThreshold(double threshold, GamePad pad = GamePad.One)
    {
        // 無効な値はreturn
        if(pad == GamePad.None)
        {
            return;
        }

        // Maxなら全部のコントローラーに一括で設定
        if(pad == GamePad.Max)
        {
            for (var i = 0; i < _controllerList.Count; i++)
            {
                // ControllerクラスのSetThresHold関数を呼ぶ
                _controllerList[i].SetThreshold(threshold);
            }
        }
        // それ以外は指定されたパッドに設定
        else
        {
            _controllerList[(int)pad].SetThreshold(threshold);
        }
    }

    /// <summary>
    /// 感度の設定
    /// </summary>
    /// <param name="sensitivity"></param>
    /// <param name="pad"></param>
    public void SetSensitivity(double sensitivity, GamePad pad = GamePad.One)
    {
        // 無効な値はreturn
        if (pad == GamePad.None)
        {
            return;
        }

        // Maxなら全部のコントローラーに一括で設定
        if (pad == GamePad.Max)
        {
            for (var i = 0; i < _controllerList.Count; i++)
            {
                // ControllerクラスのSetAxisSensitivity関数を呼ぶ
                _controllerList[i].SetAxisSensitivity(sensitivity);
            }
        }
        // それ以外は指定されたパッドに設定
        else
        {
            _controllerList[(int)pad].SetAxisSensitivity(sensitivity);
        }
    }

    /// <summary>
    /// 重力の設定
    /// </summary>
    /// <param name="gravity"></param>
    /// <param name="pad"></param>
    public void SetGravity(double gravity, GamePad pad = GamePad.One)
    {
        // 無効な値はreturn
        if (pad == GamePad.None)
        {
            return;
        }

        // Maxなら全部のコントローラーに一括で設定
        if (pad == GamePad.Max)
        {
            for (var i = 0; i < _controllerList.Count; i++)
            {
                // ControllerクラスのSetAxisGravity関数を呼ぶ
                _controllerList[i].SetAxisGravity(gravity);
            }
        }
        // それ以外は指定されたパッドに設定
        else
        {
            _controllerList[(int)pad].SetAxisGravity(gravity);
        }
    }

    /// <summary>
    /// 軸の反応しない範囲の設定
    /// </summary>
    /// <param name="dead"></param>
    /// <param name="pad"></param>
    public void SetDead(double dead, GamePad pad = GamePad.One)
    {
        // 無効な値はreturn
        if (pad == GamePad.None)
        {
            return;
        }

        // Maxなら全部のコントローラーに一括で設定
        if (pad == GamePad.Max)
        {
            for (var i = 0; i < _controllerList.Count; i++)
            {
                // ControllerクラスのSetAxisDead関数を呼ぶ
                _controllerList[i].SetAxisDead(dead);
            }
        }
        // それ以外は指定されたパッドに設定
        else
        {
            _controllerList[(int)pad].SetAxisDead(dead);
        }
    }

    /// <summary>
    /// 更新
    /// </summary>
    public void Update()
    {
        if (_lastUpdateFrame == Time.frameCount)
        {
            // 連続で呼ばれても一度だけ処理を行うようにする
            return;
        }

        _lastUpdateFrame = Time.frameCount;

        if (_controllerList == null || _controllerList.Count == 0)
        {
            SetForKeyboard();
        }

        // コントローラーの更新
        for (var i = 0; i < _controllerList.Count; i++)
        {
            _controllerList[i].Update();
        }

        // バッファーの更新
        UpdateInutBuffer();
    }

    #region Button関数

    /// <summary>
    /// ボタンが押された瞬間にtrue
    /// </summary>
    /// <param name="button"></param>
    /// <param name="pad"></param>
    /// <returns></returns>
    public bool GetButtonDown(Controller.Button button, GamePad pad = GamePad.One, Action action = null)
    {
        if(pad == GamePad.None)
        {
            return false;
        }
        
        // 全部のコントローラーを調べる
        if(pad == GamePad.Max)
        {
            bool down = false;
            foreach(var controller in _controllerList)
            {
                down = down || controller.GetButtonDown(button);
            }

            // 入力情報をバッファリング
            if(down)
            {
                AddBuffer(button, action: action);
            }

            return down;
        }
        // 指定のコントローラーを調べる
        else
        {
            bool down = _controllerList[(int)pad].GetButtonDown(button); ;

            // 入力情報をバッファリング
            if (down)
            {
                AddBuffer(button, action: action);
            }

            return down;
        }
    }

    /// <summary>
    /// どれかのボタンが押された瞬間にtrue
    /// </summary>
    /// <param name="pad"></param>
    /// <returns></returns>
    public bool GetAnyButtonDown(GamePad pad = GamePad.One)
    {
        if (pad == GamePad.None)
        {
            return false;
        }

        // 全部のコントローラーを調べる
        if (pad == GamePad.Max)
        {
            bool down = false;
            foreach (var controller in _controllerList)
            {
                down = down || controller.GetAnyButtonDown();
            }

            return down;
        }
        // 指定のコントローラーを調べる
        else
        {
            return _controllerList[(int)pad].GetAnyButtonDown();
        }
    }

    /// <summary>
    /// ボタンが押されている間true
    /// </summary>
    /// <param name="button"></param>
    /// <param name="pad"></param>
    /// <returns></returns>
    public bool GetButtonStay(Controller.Button button, GamePad pad = GamePad.One)
    {
        if (pad == GamePad.None)
        {
            return false;
        }

        // 全部のコントローラーを調べる
        if (pad == GamePad.Max)
        {
            bool stay = false;
            foreach (var controller in _controllerList)
            {
                stay = stay || controller.GetButtonStay(button);
            }

            return stay;
        }
        // 指定のコントローラーを調べる
        else
        {
            return _controllerList[(int)pad].GetButtonStay(button);
        }
    }

    /// <summary>
    /// どれかのボタンが押されている間true
    /// </summary>
    /// <param name="pad"></param>
    /// <returns></returns>
    public bool GetAnyButtonStay(GamePad pad = GamePad.One)
    {
        if (pad == GamePad.None)
        {
            return false;
        }

        // 全部のコントローラーを調べる
        if (pad == GamePad.Max)
        {
            bool stay = false;
            foreach (var controller in _controllerList)
            {
                stay = stay || controller.GetAnyButtonStay();
            }

            return stay;
        }
        // 指定のコントローラーを調べる
        else
        {
            return _controllerList[(int)pad].GetAnyButtonStay();
        }
    }

    /// <summary>
    /// ワンテンポ置いた連続押下処理
    /// </summary>
    /// <param name="button"></param>
    /// <param name="pad"></param>
    /// <returns></returns>
    public bool GetButtonRepeat(Controller.Button button, GamePad pad = GamePad.One)
    {
        if (pad == GamePad.None)
        {
            return false;
        }

        // 全部のコントローラーを調べる
        if (pad == GamePad.Max)
        {
            bool stay = false;
            foreach (var controller in _controllerList)
            {
                stay = stay || controller.GetButtonRepeat(button);
            }

            return stay;
        }
        // 指定のコントローラーを調べる
        else
        {
            return _controllerList[(int)pad].GetButtonRepeat(button);
        }
    }

    /// <summary>
    /// ボタンが離された瞬間にtrue
    /// </summary>
    /// <param name="button"></param>
    /// <param name="pad"></param>
    /// <returns></returns>
    public bool GetButtonUp(Controller.Button button, GamePad pad = GamePad.One)
    {
        if (pad == GamePad.None)
        {
            return false;
        }

        // 全部のコントローラーを調べる
        if (pad == GamePad.Max)
        {
            bool up = false;
            foreach (var controller in _controllerList)
            {
                up = up || controller.GetButtonUp(button);
            }

            return up;
        }
        // 指定のコントローラーを調べる
        else
        {
            return _controllerList[(int)pad].GetButtonUp(button);
        }
    }

    /// <summary>
    /// どれかのボタンが離された瞬間にtrue
    /// </summary>
    /// <param name="pad"></param>
    /// <returns></returns>
    public bool GetAnyButtonUp(GamePad pad = GamePad.One)
    {
        if (pad == GamePad.None)
        {
            return false;
        }

        // 全部のコントローラーを調べる
        if (pad == GamePad.Max)
        {
            bool up = false;
            foreach (var controller in _controllerList)
            {
                up = up || controller.GetAnyButtonUp();
            }

            return up;
        }
        // 指定のコントローラーを調べる
        else
        {
            return _controllerList[(int)pad].GetAnyButtonUp();
        }
    }

    /// <summary>
    /// ボタンの長押し時間の取得
    /// </summary>
    /// <param name="button"></param>
    /// <param name="pad"></param>
    /// <returns></returns>
    public float GetButtonStayTime(Controller.Button button, GamePad pad = GamePad.One)
    {
        // 取得できるコントローラーは一つだけ
        if (pad < GamePad.Max)
        {
            return _controllerList[(int)pad].GetButtonStayTime(button);
        }
        else
        {
            return 0;
        }
    }

    /// <summary>
    /// ボタン同時押し処理
    /// </summary>
    /// <param name="buttons"></param>
    /// <param name="pad"></param>
    /// <returns></returns>
    public bool GetSameButtonDown(List<Controller.Button> buttons, GamePad pad = GamePad.One, Action action = null)
    {
        if (pad == GamePad.None)
        {
            return false;
        }

        // 全部のコントローラーを調べる
        if (pad == GamePad.Max)
        {
            bool stay = false;
            for (int i = 0; i < _controllerList.Count; i++)
            {
                if((GamePad)i >= GamePad.Max)
                {
                    break;
                }

                if(!GetAnyButtonDown((GamePad)i))
                {
                    return false;
                }

                bool down = true;
                foreach (var button in buttons)
                {
                    down = down && (GetButtonDown(button, (GamePad)i, action) || _inputBuffList.Any(buff => buff.button == button));
                }
                stay = stay || down;

                // バッファを削除
                if (down)
                {
                    foreach (var button in buttons)
                    {
                        RemoveBuffer(button);
                    }
                }
            }

            return stay;
        }
        // 指定のコントローラーを調べる
        else
        {
            if (!GetAnyButtonDown())
            {
                return false;
            }

            bool down = true;
            foreach (var button in buttons)
            {
                down = (GetButtonDown(button, pad, action) || _inputBuffList.Any(buff => buff.button == button)) && down ;
            }

            // バッファを削除
            if (down)
            {
                foreach (var button in buttons)
                {
                    RemoveBuffer(button);
                }
            }
            return down;
        }
    }
    #endregion

    #region Axis関数

    /// <summary>
    /// 軸の方向が変わったときに 1 or -1 通常は0
    /// </summary>
    /// <param name="axis"></param>
    /// <param name="pad"></param>
    /// <returns></returns>
    public int GetAxisDown(Controller.Axis axis, GamePad pad = GamePad.One)
    {
        if(pad == GamePad.Max)
        {
            return 0;
        }

        // 全部のコントローラーを調べる
        if (pad == GamePad.Max)
        {
            int down = 0;
            for (var i = 0; i < _controllerList.Count; i++)
            {
                down += _controllerList[i].GetAxisDown(axis);
            }

            if(Mathf.Abs(down) > 1)
            {
                down = down / Mathf.Abs(down);
            }

            return down;
        }
        // 指定のコントローラーを調べる
        else
        {
            return _controllerList[(int)pad].GetAxisDown(axis);
        }
    }

    /// <summary>
    /// どれかの軸の方向が変わったときに 1 or -1 通常は0
    /// </summary>
    /// <param name="isHorizontal"></param>
    /// <param name="pad"></param>
    public int GetAnyAxisDown(bool isHorizontal, GamePad pad = GamePad.One)
    {
        if (pad == GamePad.None)
        {
            return 0;
        }

        // 全部のコントローラーを調べる
        if (pad == GamePad.Max)
        {
            int down = 0;
            for (var i = 0; i < _controllerList.Count; i++)
            {
                down += _controllerList[i].GetAnyAxisDown(isHorizontal);
            }

            if (Mathf.Abs(down) > 1)
            {
                down = down / Mathf.Abs(down);
            }

            return down;
        }
        // 指定のコントローラーを調べる
        else
        {
            return _controllerList[(int)pad].GetAnyAxisDown(isHorizontal);
        }
    }

    /// <summary>
    /// 軸が上がった時に 1 or -1
    /// </summary>
    /// <param name="axis"></param>
    /// <param name="pad"></param>
    /// <returns></returns>
    public int GetAxisUp(Controller.Axis axis, GamePad pad = GamePad.One)
    {
        if (pad == GamePad.None)
        {
            return 0;
        }

        // 全部のコントローラーを調べる
        if (pad == GamePad.Max)
        {
            int up = 0;
            for (var i = 0; i < _controllerList.Count; i++)
            {
                up = up + _controllerList[i].GetAxisUp(axis);
            }

            if (Mathf.Abs(up) > 1)
            {
                up = up / Mathf.Abs(up);
            }

            return up;
        }
        // 指定のコントローラーを調べる
        else
        {
            return _controllerList[(int)pad].GetAxisUp(axis);
        }
    }

    /// <summary>
    /// どれかの軸が上がった時に 1 or -1
    /// </summary>
    /// <param name="isHorizontal"></param>
    /// <param name="pad"></param>
    /// <returns></returns>
    public int GetAnyAxisUp(bool isHorizontal, GamePad pad = GamePad.One)
    {
        if (pad == GamePad.None)
        {
            return 0;
        }

        // 全部のコントローラーを調べる
        if (pad == GamePad.Max)
        {
            int up = 0;
            for (var i = 0; i < _controllerList.Count; i++)
            {
                up = up + _controllerList[i].GetAnyAxisUp(isHorizontal);
            }

            if (Mathf.Abs(up) > 1)
            {
                up = up / Mathf.Abs(up);
            }

            return up;
        }
        // 指定のコントローラーを調べる
        else
        {
            return _controllerList[(int)pad].GetAnyAxisUp(isHorizontal);
        }
    }

    /// <summary>
    /// 軸の状態を常に取得？
    /// </summary>
    /// <param name="axis"></param>
    /// <param name="pad"></param>
    /// <param name="sum"></param>
    /// <returns></returns>
    public float GetAxis(Controller.Axis axis, GamePad pad = GamePad.One, bool sum = false)
    {
        if (pad == GamePad.None)
        {
            return 0;
        }

        // 全部のコントローラーを調べる
        if (pad == GamePad.Max)
        {
            float value = 0;
            for (var i = 0; i < _controllerList.Count; i++)
            {
                value = value + _controllerList[i].GetAxis(axis);
            }

            if (!sum && Mathf.Abs(value) > 1)
            {
                value = value / Mathf.Abs(value);
            }

            return value;
        }
        // 指定のコントローラーを調べる
        else
        {
            return _controllerList[(int)pad].GetAxis(axis);
        }
    }

    /// <summary>
    /// どれかの軸の状態を常に取得？
    /// </summary>
    /// <param name="isHorizontal"></param>
    /// <param name="pad"></param>
    /// <param name="sum"></param>
    /// <returns></returns>
    public float GetAnyAxis(bool isHorizontal, GamePad pad = GamePad.One, bool sum = false)
    {
        if (pad == GamePad.None)
        {
            return 0;
        }

        // 全部のコントローラーを調べる
        if (pad == GamePad.Max)
        {
            float value = 0;
            for (var i = 0; i < _controllerList.Count; i++)
            {
                value = value + _controllerList[i].GetAnyAxis(isHorizontal);
            }

            if (!sum && Mathf.Abs(value) > 1)
            {
                value = value / Mathf.Abs(value);
            }

            return value;
        }
        // 指定のコントローラーを調べる
        else
        {
            return _controllerList[(int)pad].GetAnyAxis(isHorizontal);
        }
    }

    /// <summary>
    /// 一度押した後、間を置いて連続で押下される
    /// </summary>
    /// <param name="axis"></param>
    /// <param name="pad"></param>
    /// <param name="sum"></param>
    /// <returns></returns>
    public int GetAxisRepeat(Controller.Axis axis, GamePad pad = GamePad.One, bool sum = false)
    {
        if (pad == GamePad.None)
        {
            return 0;
        }

        // 全部のコントローラーを調べる
        if (pad == GamePad.Max)
        {
            int value = 0;
            for (var i = 0; i < _controllerList.Count; i++)
            {
                value = value + _controllerList[i].GetAxisRepeat(axis);
            }

            if (!sum && Mathf.Abs(value) > 1)
            {
                value = value / Mathf.Abs(value);
            }

            return value;
        }
        // 指定のコントローラーを調べる
        else
        {
            return _controllerList[(int)pad].GetAxisRepeat(axis);
        }
    }

    /// <summary>
    /// 軸が倒れた時に 1 or -1
    /// </summary>
    /// <param name="axis"></param>
    /// <param name="pad"></param>
    /// <param name="sum"></param>
    /// <returns></returns>
    public int GetAxisRow(Controller.Axis axis, GamePad pad = GamePad.One, bool sum = false)
    {
        if(pad == GamePad.None)
        {
            return 0;
        }

        // 全部のコントローラーを調べる
        if (pad == GamePad.Max)
        {
            int value = 0;
            for (var i = 0; i < _controllerList.Count; i++)
            {
                value = value + _controllerList[i].GetAxisRow(axis);
            }

            if (!sum && Mathf.Abs(value) > 1)
            {
                value = value / Mathf.Abs(value);
            }

            return value;
        }
        // 指定のコントローラーを調べる
        else
        {
            return _controllerList[(int)pad].GetAxisRow(axis);
        }
    }

    /// <summary>
    /// どれかの軸が倒れた時に 1 or -1
    /// </summary>
    /// <param name="isHorizontal"></param>
    /// <param name="pad"></param>
    /// <param name="sum"></param>
    /// <returns></returns>
    public int GetAnyAxisRow(bool isHorizontal, GamePad pad = GamePad.One, bool sum = false)
    {
        if (pad == GamePad.None)
        {
            return 0;
        }

        // 全部のコントローラーを調べる
        if (pad == GamePad.Max)
        {
            int value = 0;
            for (var i = 0; i < _controllerList.Count; i++)
            {
                value = value + _controllerList[i].GetAnyAxisRow(isHorizontal);
            }

            if (!sum && Mathf.Abs(value) > 1)
            {
                value = value / Mathf.Abs(value);
            }

            return value;
        }
        // 指定のコントローラーを調べる
        else
        {
            return _controllerList[(int)pad].GetAnyAxisRow(isHorizontal);
        }
    }

    /// <summary>
    /// 軸の倒している時間
    /// </summary>
    /// <param name="axis"></param>
    /// <param name="pad"></param>
    /// <returns></returns>
    public float GetAxisStayTime(Controller.Axis axis, GamePad pad = GamePad.One)
    {
        if (pad < GamePad.Max)
        {
            return _controllerList[(int)pad].GetAxisStayTime(axis);
        }
        else
            return 0;
    }
    #endregion

    #region バッファ

    /// <summary>
    /// バッファリストに入力情報を追加
    /// </summary>
    /// <param name="button"></param>
    void AddBuffer(Controller.Button button, int count = 0, Action action = null)
    {
        // 同一フレーム内に入力したボタンと同じものがある場合は追加しない
        if (_inputBuffList.Any(buff => buff.button == button && buff.count == count))
        {
            return;
        }

        _inputBuffList.Add(new InputBuffer(button, count, action));
    }

    /// <summary>
    /// バッファリストに指定のものを削除
    /// </summary>
    /// <param name="button"></param>
    /// <param name="count"></param>
    void RemoveBuffer(Controller.Button button, int count = -1)
    {
        // カウントに指定があればそれを削除
        if(count >= 0)
        {
            var buffer = _inputBuffList.FirstOrDefault(buff => buff.button == button && buff.count == count);
            buffer.Delete();
            _inputBuffList.Remove(buffer);
        }

        // 特に指定されていなければ古いものから削除
        for (int i = BUFFER_HOLD_FRAME - 1; i >= 0; i--)
        {
            var buffer = _inputBuffList.FirstOrDefault(buff => buff.button == button && buff.count == i);
            buffer.Delete();
            var result = _inputBuffList.Remove(buffer);

            if(result)
            {
                break;
            }
        }
    }

    /// <summary>
    /// バッファリストの更新
    /// </summary>
    /// <param name="buff"></param>
    void UpdateInutBuffer()
    {
        if (_inputBuffList.Count > 0)
        {
            for (int i = _inputBuffList.Count - 1; i >= 0; i--)
            {
                var buff = _inputBuffList[i];
                buff.count++;
                _inputBuffList[i] = buff;

                if (_inputBuffList[i].count > BUFFER_HOLD_FRAME)
                {
                    Debug.Log("バッファ削除 button = " + _inputBuffList[i].button);
                    if(_inputBuffList[i].action != null)
                    {
                        _inputBuffList[i].action.Invoke();
                        _inputBuffList[i].Delete();
                    }

                    _inputBuffList.Remove(_inputBuffList[i]);
                }
            }
        }
    }
    #endregion
}
