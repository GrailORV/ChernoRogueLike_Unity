using System.Collections.Generic;
using UnityEngine;

using InputController;

/// <summary>
/// ボタンの状態を管理するクラス
/// 一つのインスタンスにつき、一つのボタンに対応する仕組みなので
/// コントローラーで使用するボタン分インスタンスを用意しよう
/// </summary>
public class ButtonState
{
    // UnityのInputManagerでのキーの名前 <例>Fire1
    List<string> _keyNameList = new List<string>();

    // キーボードのKeyCode
    List<KeyCode> _keyCodeList = new List<KeyCode>();

    // 前フレームの状態
    bool _prePress = false;

    // 現在の状態
    bool _nowPress = true;


    // ボタンの種類
    public Controller.Button Button { get; private set; }

    // 初期化したかどうか
    public bool IsInitialized { get; private set; }

    // 有効かどうか
    public bool Enabled { get; private set; }

    // 押している時間
    public float StayTime { get; private set; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="button"></param>
    public ButtonState(Controller.Button button)
    {
        Button = button;
        Enabled = true;
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="button"></param>
    /// <param name="name"></param>
    public ButtonState(Controller.Button button, string name)
    {
        Button = button;
        _keyNameList.Add(name);
        Enabled = true;
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="button"></param>
    /// <param name="key"></param>
    public ButtonState(Controller.Button button, KeyCode key)
    {
        Button = button;
        _keyCodeList.Add(key);
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
    public void AddKeyCode(KeyCode keycode)
    {
        _keyCodeList.Add(keycode);
        Enabled = true;
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    public void Update()
    {
        if(!Enabled)
        {
            return;
        }

        int nameCount = 0;
        int codeCount = 0;
        
        try
        {
            _prePress = _nowPress;

            // _keyNameListに登録されているキーが有効かどうか確認
            bool namePress = false;
            foreach (string name in _keyNameList)
            {
                // キーが押されているかどうかチェック
                namePress = Input.GetButton(name);
                if(namePress)
                {
                    break;
                }

                nameCount++;
            }

            // _keyCodeListに登録されているキーが有効かどうか確認
            bool keyCodePress = false;
            foreach (KeyCode key in _keyCodeList)
            {
                // キーが押されているかチェック
                keyCodePress = Input.GetKey(key);
                if (keyCodePress)
                {
                    break;
                }

                codeCount++;
            }

            // 登録しているキーがどれかでも押されればTrue
            _nowPress = (namePress || keyCodePress);

            // 前フレーム押している＆現在押している時は時間を追加
            if (_prePress && _nowPress)
            {
                StayTime += Time.deltaTime;
            }
            // 前フレーム押していない＆現在も押していない時は時間をリセット
            else if (!_prePress && !_nowPress)
            {
                StayTime = 0;
            }
        }
        catch
        {
            // ボタンを使えなくする
            Enabled = false;
            if (_keyNameList.Count != 0)
            {
                Debug.LogWarningFormat("キー名が使えません？ KeyName = [{0}]", _keyNameList[nameCount]);
            }
            if (_keyCodeList.Count != 0)
            {
                Debug.LogWarningFormat("キーコードが使えません？ KeyCode = [{0}]", _keyCodeList[codeCount]);
            }
        }
    }

    /// <summary>
    /// ボタンを押した直後
    /// </summary>
    /// <returns></returns>
    public bool GetButtonDown()
    {
        return (!_prePress && _nowPress);
    }

    /// <summary>
    /// ボタンを押している間
    /// </summary>
    /// <returns></returns>
    public bool GetButtonStay()
    {
        return _nowPress;
    }

    /// <summary>
    /// 一度押した後、間を置いて連続で押下される
    /// </summary>
    /// <returns></returns>
    public bool GetButtonRepeat()
    {
        double offset   = 0.5;
        double interval = 0.1;

        // 押した直後は一度trueを返す
        if (GetButtonDown())
        {
            return true;
        }
        // 押下中は間をあけて連続で押せるように調整
        else if(GetButtonStay())
        {
            if (StayTime < offset)
            {
                // 間をあけるようにする
                return false;
            }
            else if ((int)(StayTime - offset - Time.deltaTime) / interval != (int)((StayTime - offset) / interval))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// ボタンを離したとき
    /// </summary>
    /// <returns></returns>
    public bool GetButtonUp()
    {
        return (_prePress && !_nowPress);
    }
}
