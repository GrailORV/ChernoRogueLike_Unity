using System.Collections.Generic;
using UnityEngine;

using InputController;

/// <summary>
/// ボタンの軸の状態を管理するクラス
/// 一つのインスタンスにつき、一つのボタンに対応する仕組みなので
/// コントローラーで使用するボタン分インスタンスを用意しよう
/// </summary>
public class AxisState
{
    // UnityのInputManagerでのキーの名前 <例>Fire1
    List<string> _keyNameList = new List<string>();

    // キーボードのKeyCode(ポジティブ)
    List<KeyCode> _positiveKeyCodeList = new List<KeyCode>();
    
    // キーボードのKeyCode(ネガティブ)
    List<KeyCode> _negativeKeyCodeList = new List<KeyCode>();

    // 前フレームの状態
    float _preState;

    // 現在の状態
    float _nowState;


    // 軸の種類
    public Controller.Axis Axis { get; private set; }

    // 軸のしきい値
    double _threshold = 0;
    public double Threshold { get { return _threshold; } set { _threshold = value; } }

    // 感度
    double _seneitivity = 2;
    public double Sensitivity { get { return _seneitivity; } set { Sensitivity = value; } }

    // 重力
    double _gravity = 10;
    public double Gravity { get { return _gravity; } set { _gravity = value; } }

    // 無反応
    double _dead = 0.1;
    public double Dead { get { return _dead; } set { _dead = value; } }

    // 有効かどうか
    public bool Enabled { get; private set; }

    // 倒している時間
    public float StayTime { get; private set; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="button"></param>
    public AxisState(Controller.Axis axis)
    {
        Axis = axis;
        Enabled = true;
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="button"></param>
    public AxisState(Controller.Axis axis, string name)
    {
        Axis = axis;
        _keyNameList.Add(name);
        Enabled = true;
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="button"></param>
    public AxisState(Controller.Axis axis, KeyCode positive, KeyCode negative)
    {
        Axis = axis;
        _positiveKeyCodeList.Add(positive);
        _negativeKeyCodeList.Add(negative);
        Enabled = true;
    }

    /// <summary>
    /// キー(名前)の追加
    /// </summary>
    /// <param name="name"></param>
    public void AddKeyName(string name)
    {
        _keyNameList.Add(name);
        Enabled = true;
    }

    /// <summary>
    /// キー(KeyCode)の追加
    /// </summary>
    /// <param name="keycode"></param>
    public void AddKeyCode(KeyCode positive, KeyCode negative)
    {
        _positiveKeyCodeList.Add(positive);
        _negativeKeyCodeList.Add(negative);
        Enabled = true;
    }

    public void Update()
    {
        if (!Enabled)
        {
            return;
        }

        int nameCount = 0;
        int codeCount = 0;

        try
        {
            _preState = _nowState;
            _nowState = 0;

            // _keyNameListに登録されているキーが有効かどうか確認
            foreach (string name in _keyNameList)
            {
                var axis = Input.GetAxis(name);

                // キーが押されているかチェック
                if (Mathf.Abs(_nowState) < Mathf.Abs(axis))
                {
                    _nowState = axis;
                }

                nameCount++;
            }

            // キー入力で登録したスティック操作のチェック
            for (int i = 0; i < _positiveKeyCodeList.Count; i++)
            {
                var axis = _preState;

                // ボタン(多分ArrowKeyやWASD)をそもそも押しているかどうか
                var key = (Input.GetKey(_positiveKeyCodeList[i]) ? 1 : 0) + (Input.GetKey(_negativeKeyCodeList[i]) ? -1 : 0);

                // なにも押していないor逆方向へ押した場合
                if (key == 0 || _preState * key < 0)
                {
                    if(Gravity > 0)
                    {
                        axis += -(float)(_preState * Gravity * Time.deltaTime);

                        // 押してすぐは反応させないって処理？(不明)
                        if(axis * _preState < 0)
                        {
                            axis = 0;
                        }
                        if (Mathf.Abs(axis) <= Dead)
                        {
                            axis = 0;
                        }
                    }
                    else
                    {
                        axis = 0;
                    }

                    // 押してすぐには反応させない
                    if(Mathf.Abs(axis) <= Dead)
                    {
                        axis = 0;
                    }

                    _nowState += axis;
                }
                // それ以外
                else
                {
                    if(Sensitivity > 0)
                    {
                        axis += (float)(key * Sensitivity * Time.deltaTime);
                        if(Mathf.Abs(axis) > 1)
                        {
                            // -1 ～ 1の範囲にする
                            axis = key;
                        }
                    }
                    else
                    {
                        axis = key;
                    }
                }

                // キーが押されているかチェック
                if (Mathf.Abs(_nowState) < Mathf.Abs(axis))
                {
                    _nowState = axis;
                }
                codeCount++;
            }

            int now = GetAxisRow();

            // 前フレーム押している＆現在押している時は時間を追加
            if (_preState != 0 && _preState == now)
            {
                StayTime += Time.deltaTime;
            }
            // 前フレーム押していない＆現在も押していない時は時間をリセット
            else if (_preState == 0 && now == 0)
            {
                StayTime = 0;
            }
        }
        catch
        {
            // ボタンを使えなくする
            Enabled = false;
            if(_keyNameList.Count != 0)
            {
                Debug.LogWarningFormat("キー名が使えません？ KeyName = [{0}]", _keyNameList[nameCount]);
            }
            if(_positiveKeyCodeList.Count != 0 && _negativeKeyCodeList.Count != 0)
            {
                Debug.LogWarningFormat("キーコードが使えません？ PositiveKeyCode = [{0}] : NegativeKeyCoden = [{1}]", _positiveKeyCodeList[codeCount], _negativeKeyCodeList[codeCount]);
            }
        }
    }

    /// <summary>
    /// スティックを倒した直後
    /// </summary>
    /// <returns></returns>
    public int GetAxisDown()
    {
        // 前フレームの状態
        int pre = 0;
        if(_preState > Threshold)
        {
            pre = 1;
        }
        else if (_preState < -Threshold)
        {
            pre = -1;
        }

        // 今の状態
        int now = 0;
        if (_nowState > Threshold)
        {
            now = 1;
        }
        else if (_nowState < -Threshold)
        {
            now = -1;
        }

        // 全フレームは押しておらず、今は押しているのか
        return (pre == now) ? 0 : now;
    }

    /// <summary>
    /// スティック倒している間
    /// </summary>
    /// <returns></returns>
    public float GetAxis()
    {
        return _nowState;
    }

    /// <summary>
    /// スティックを起こしたとき、前フレームの状態を変えす？
    /// </summary>
    /// <returns></returns>
    public int GetAxisUp()
    {
        // 前フレームの状態
        int pre = 0;
        if (_preState > Threshold)
        {
            pre = 1;
        }
        else if (_preState < -Threshold)
        {
            pre = -1;
        }

        // 今の状態
        int now = 0;
        if (_nowState > Threshold)
        {
            now = 1;
        }
        else if (_nowState < -Threshold)
        {
            now = -1;
        }

        // 現在が
        return (now == 0) ? pre : 0;
    }

    /// <summary>
    /// 一度押した後、間を置いて連続で押下される
    /// </summary>
    /// <returns></returns>
    public int GetAxisRepeat()
    {
        double offset = 0.2;
        double interval = 0.1;

        // 前フレームの状態
        int pre = 0;
        if (_preState > Threshold)
        {
            pre = 1;
        }
        else if (_preState < -Threshold)
        {
            pre = -1;
        }

        // 今の状態
        int now = 0;
        if (_nowState > Threshold)
        {
            now = 1;
        }
        else if (_nowState < -Threshold)
        {
            now = -1;
        }

        // 全フレームと違う方向に倒したか
        if(pre != now)
        {
            return now;
        }
        else
        {
            if (StayTime < offset)
            {
                return 0;
            }
            else if ((int)((StayTime - offset - Time.deltaTime) / interval) != (int)((StayTime - offset) / interval))
            {
                return now;
            }
            else
            {
                return 0;
            }
        }
    }

    /// <summary>
    /// 閾値込みのスティック
    /// </summary>
    /// <returns></returns>
    public int GetAxisRow()
    {
        int n = _nowState > Threshold ? 1 : (_nowState < -Threshold ? -1 : 0);

        return n;
    }
}
