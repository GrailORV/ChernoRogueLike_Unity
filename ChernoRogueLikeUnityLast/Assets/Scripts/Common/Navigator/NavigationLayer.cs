using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InputController;

/// <summary>
/// NavigationManagerのほうで管理するクラス
/// 基本的に操作がある画面にはこのコンポーネントを用意する
/// </summary>
public class NavigationLayer : MonoBehaviour
{
    [Serializable]
    public class KeyCommand
    {
        public NavigationManager.InputType type;
        public NavigationManager.InputKey key;
        public List<NavigationManager.InputKey> keys;
        public GameObject target;
        public string methodName;
    }

    /// <summary>
    /// ナビゲーターが切り替わるときに呼ばれるコールバック(current,old)
    /// </summary>
    public Action<Navigator, Navigator> OnChangeCurrentNavigator = null;


    // 登録用コマンド一覧
    [SerializeField] List<KeyCommand> _commandList = new List<KeyCommand>();

    // ナビゲーター一覧
    [SerializeField] List<Navigator> _navigatorList = new List<Navigator>();

    // _defaultNavigatorIndexを自動で更新するかどうか
    [Header("defaultNavigatorIndexを自動で更新するかどうか")]
    [SerializeField] bool _isUpdateDefaultIndex = true;

    // CurrentNavigatorの初期化時に設定される_navigatorListのインデックス
    int _defaultNavigatorIndex = -1;

    // フォーカス中のナビゲーター
    Navigator _currentNavigator;

    /// <summary>
    /// _defaultNavigatorIndexを自動で更新するかどうか
    /// </summary>
    public bool IsUpdateDefaultIndex
    {
        get { return _isUpdateDefaultIndex; }
        set { _isUpdateDefaultIndex = value; }
    }

    /// <summary>
    /// フォーカス中のナビゲーター
    /// </summary>
    public Navigator CurrentNavigator
    {
        get { return _currentNavigator; }
        set
        {
            if (_currentNavigator == value)
            {
                // 変わらないなら何もしない
                return;
            }

            // Current更新
            var old = _currentNavigator;
            _currentNavigator = value;

            // Navigator更新
            if (old != null)
            {
                old.OnHover(false);
            }
            if (_currentNavigator != null)
            {
                old.OnHover(true);
            }

            // defaultIndexの更新
            if(_isUpdateDefaultIndex)
            {
                SetDefaultIndex(GetCurrentIndex());
            }

            // 切り替わり時のコールバックを呼ぶ
            OnChangeCurrentNavigator.SafeInvoke(_currentNavigator, old);
        }
    }

    /// <summary>
    /// レイヤーが有効になった時に呼ばれる処理
    /// </summary>
    public void OnLayerActive()
    {
        Debug.Log("<color=yellow>Layer on Active name = " + name + "</color>");

        // Currentの初期化
        if(_navigatorList.Count > 0)
        {
            var index = _defaultNavigatorIndex;

            // デフォルト用のindexが適切ではない場合は0にする
            if (index < 0 || index > _navigatorList.Count)
            {
                index =  0;
            }

            // CurrentNavigatoの設定
            SetCurrentNavigatorFromIndex(index);
        }
    }

    /// <summary>
    /// レイヤーが無効になった時に呼ばれる処理
    /// </summary>
    public void OnLayerDeactive()
    {

    }

    #region Input

    /// <summary>
    /// 入力操作のUpdate
    /// </summary>
    public void KeyUpdate(InputControllerManager inputManager)
    {
        // 入力のチェックを行う
        foreach (var command in _commandList)
        {
            // 同時押し
            if(command.type == NavigationManager.InputType.Same)
            {
                CheckSameButtonAction(command, inputManager);
            }
            // command.keyがボタン系ならボタン操作の確認
            else if (IsInputTypeButton(command.key))
            {
                CheckButtonAction(command, inputManager);
            }
            // そうでないなら方向キーの操作の確認
            else
            {
                CheckAxisAction(command, inputManager);
            }
        }

        // ナビゲーターの更新
        UpdateNavigatAction(inputManager);
    }

    /// <summary>
    /// 同時押しのボタン入力の確認
    /// </summary>
    /// <param name="command"></param>
    /// <param name="inputManager"></param>
    void CheckSameButtonAction(KeyCommand command, InputControllerManager inputManager)
    {
        var keys = new List<Controller.Button>();
        foreach (var key in command.keys)
        {
            keys.Add(GetButtonKey(key));
        }

        // 同時押しは入力パターンをDown(押下時)限定で確認する
        var result = inputManager.GetSameButtonDown(keys);

        // 結果がtrueならcommandに設定されたアクションを実行
        if (result)
        {
            ActionSendMessage(command.target, command.methodName);
        }
    }

    /// <summary>
    /// ボタン入力の確認
    /// </summary>
    /// <param name="command"></param>
    /// <param name="inputManager"></param>
    void CheckButtonAction(KeyCommand command, InputControllerManager inputManager)
    {
        var result = false;
        var key = GetButtonKey(command.key);

        // 意図していない入力タイプなら処理を行わない
        if(key == Controller.Button.Max)
        {
            return;
        }

        // チェック
        switch (command.type)
        {
            case NavigationManager.InputType.Press:
                result = inputManager.GetButtonStay(key);
                break;

            case NavigationManager.InputType.Down:
                // Downだけは入力時の処理をコールバックで呼ぶようにする
                inputManager.GetButtonDown(key, action: () => ActionSendMessage(command.target, command.methodName));
                return;

            case NavigationManager.InputType.Up:
                result = inputManager.GetButtonUp(key);
                break;

            case NavigationManager.InputType.Repeat:
                result = inputManager.GetButtonRepeat(key);
                break;
        }

        // 結果がtrueならcommandに設定されたアクションを実行
        if (result)
        {
            ActionSendMessage(command.target, command.methodName);
        }
    }

    /// <summary>
    /// 方向キー入力の確認
    /// </summary>
    /// <param name="command"></param>
    /// <param name="inputManager"></param>
    void CheckAxisAction(KeyCommand command, InputControllerManager inputManager)
    {
        var isDirection = true;
        var value = Vector2.zero;
        var axes = GetAxisKeys(command.key);

        // 意図していない入力タイプなら処理を行わない
        if (axes[0] == Controller.Axis.Max || axes[1] == Controller.Axis.Max)
        {
            return;
        }

        // チェック
        switch (command.type)
        {
            case NavigationManager.InputType.Press:
                value.x = inputManager.GetAxis(axes[0]);
                value.y = inputManager.GetAxis(axes[1]);
                isDirection = false;
                break;
            case NavigationManager.InputType.Down:
                value.x = inputManager.GetAxisDown(axes[0]);
                value.y = inputManager.GetAxisDown(axes[1]);
                break;
            case NavigationManager.InputType.Up:
                value.x = inputManager.GetAxisUp(axes[0]);
                value.y = inputManager.GetAxisUp(axes[1]);
                break;
            case NavigationManager.InputType.Repeat:
                value.x = inputManager.GetAxisRepeat(axes[0]);
                value.y = inputManager.GetAxisRepeat(axes[1]);
                break;
        }

        // 入力されていなければcommandに設定されたアクションを実行しない
        if (value.sqrMagnitude == 0)
        {
            return;
        }

        // 上下左右の方向を渡す
        if(isDirection)
        {
            var direction = GetInputDirection(value);
            ActionSendMessage(command.target, command.methodName, direction);
        }
        // スティックの倒した値を渡す
        else
        {
            ActionSendMessage(command.target, command.methodName, value);
        }
    }

    /// <summary>
    /// ナビゲーターの移動用更新
    /// </summary>
    /// <param name="inputManager"></param>
    void UpdateNavigatAction(InputControllerManager inputManager)
    {
        if(CurrentNavigator == null)
        {
            return;
        }

        var value = Vector2.zero;

        // 十字の入力を取得する
        value.x = inputManager.GetAxisDown(Controller.Axis.Cross_Horizontal);
        value.y = inputManager.GetAxisDown(Controller.Axis.Cross_Vertical);

        // 何も入力されていなければ終了
        if (value.sqrMagnitude == 0)
        {
            return;
        }

        // 上下左右のどれかに変換する
        var direction = GetInputDirection(value);
        Debug.Log(direction);
        // ナビゲーターの移動
        SelectMoveNavigator(direction);
    }

    /// <summary>
    /// チェック入りのSendMessage(引数なし)
    /// </summary>
    void ActionSendMessage(GameObject target, string methodName)
    {
        ActionSendMessage(target, methodName, null, false);
    }

    /// <summary>
    /// チェック入りのSendMessage(引数あり)
    /// </summary>
    void ActionSendMessage(GameObject target, string methodName, object value)
    {
        ActionSendMessage(target, methodName, value, true);
    }

    /// <summary>
    /// チェック入りのSendMessage
    /// </summary>
    /// <param name="target"></param>
    /// <param name="methodName"></param>
    /// <param name="value"></param>
    void ActionSendMessage(GameObject target, string methodName, object value, bool isArg)
    {
        if(target == null)
        {
            Debug.LogError("SendMessageError : 送信先のターゲットがありません。");
            return;
        }

        if(string.IsNullOrEmpty(methodName))
        {
            Debug.LogError("SendMessageError : 関数名が設定されていません。");
            return;
        }

        // 引数が必要なら値を渡す
        if(isArg)
        {
            target.SendMessage(methodName, value);
        }
        else
        {
            target.SendMessage(methodName);
        }
    }

    /// <summary>
    /// InputKeyがボタンなら true 、スティックなら false を返す
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    bool IsInputTypeButton(NavigationManager.InputKey key)
    {
        // スティックor十字キーなら false を返す
        if (key == NavigationManager.InputKey.LeftStick ||
            key == NavigationManager.InputKey.RightStick ||
            key == NavigationManager.InputKey.Select)
        {
            return false;
        }

        // ↑以外ならtrue
        return true;
    }

    /// <summary>
    /// InputControllerManagerのボタンを取得する
    /// スティックなどで該当しない場合はController.Button.Maxを返す
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    Controller.Button GetButtonKey(NavigationManager.InputKey key)
    {
        var conttollerButton = Controller.Button.Max;

        switch (key)
        {
            case NavigationManager.InputKey.Circle:
                conttollerButton = Controller.Button.Button_2;
                break;
            case NavigationManager.InputKey.Cross:
                conttollerButton = Controller.Button.Button_1;
                break;
            case NavigationManager.InputKey.Square:
                conttollerButton = Controller.Button.Button_0;
                break;
            case NavigationManager.InputKey.Triangle:
                conttollerButton = Controller.Button.Button_3;
                break;
            case NavigationManager.InputKey.Left1:
                conttollerButton = Controller.Button.Button_4;
                break;
            case NavigationManager.InputKey.Left2:
                conttollerButton = Controller.Button.Button_6;
                break;
            case NavigationManager.InputKey.Left3:
                conttollerButton = Controller.Button.Button_10;
                break;
            case NavigationManager.InputKey.Right1:
                conttollerButton = Controller.Button.Button_5;
                break;
            case NavigationManager.InputKey.Right2:
                conttollerButton = Controller.Button.Button_7;
                break;
            case NavigationManager.InputKey.Right3:
                conttollerButton = Controller.Button.Button_11;
                break;
            default:
                conttollerButton = Controller.Button.Max;
                break;
        }

        return conttollerButton;
    }

    /// <summary>
    /// InputControllerManagerの軸を取得する(配列)
    /// x = Horizontal 、　y = Vertical
    /// ボタンなどで該当しない場合はController.Input.Maxを返す
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    Controller.Axis[] GetAxisKeys(NavigationManager.InputKey key)
    {
        Controller.Axis[] controllerAxes =
        {
            Controller.Axis.Max,
            Controller.Axis.Max
        };

        switch (key)
        {
            case NavigationManager.InputKey.LeftStick:
                controllerAxes[0] = Controller.Axis.L_Horizontal;
                controllerAxes[1] = Controller.Axis.L_Vertical;
                break;
            case NavigationManager.InputKey.RightStick:
                controllerAxes[0] = Controller.Axis.R_Horizontal;
                controllerAxes[1] = Controller.Axis.R_Vertical;
                break;
            case NavigationManager.InputKey.Select:
                controllerAxes[0] = Controller.Axis.Cross_Horizontal;
                controllerAxes[1] = Controller.Axis.Cross_Vertical;
                break;
        }

        return controllerAxes;
    }

    /// <summary>
    /// Vector2の値から上下左右の該当する方向を返す
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public NavigationManager.InputDirection GetInputDirection(Vector2 value)
    {
        var direction = NavigationManager.InputDirection.Up;

        // 横方向かどうか
        if (Mathf.Abs(value.x) > Mathf.Abs(value.y))
        {
            // 右(xが０より上)かどうか
            if (0 < value.x)
            {
                direction = NavigationManager.InputDirection.Right;
            }
            // それ以外は左
            else
            {
                direction = NavigationManager.InputDirection.Left;
            }
        }
        else
        {
            // 上(yが０より上)かどうか
            if (0 < value.y)
            {
                direction = NavigationManager.InputDirection.Up;
            }
            // それ以外は↓
            else
            {
                direction = NavigationManager.InputDirection.Down;
            }
        }

        return direction;
    }

    #endregion

    #region Navigation

    /// <summary>
    /// ナビゲーターの追加
    /// </summary>
    /// <param name="navigator"></param>
    public void AddNavigator(Navigator navigator)
    {
        if (navigator == null)
        {
            return;
        }

        _navigatorList.Add(navigator);
    }

    /// <summary>
    /// ナビゲーターの削除
    /// </summary>
    /// <param name="navigator"></param>
    public void RemoveNavigator(Navigator navigator)
    {
        if (navigator == null)
        {
            return;
        }

        _navigatorList.Remove(navigator);
    }

    /// <summary>
    /// ナビゲーターの削除(index)
    /// </summary>
    /// <param name="index"></param>
    public void RemoveNavigatorAt(int index)
    {
        if (_navigatorList.Count == 0)
        {
            return;
        }

        _navigatorList.RemoveAt(index);
    }

    /// <summary>
    /// ナビゲーターの全削除
    /// </summary>
    public void RemoveNavigatorAll()
    {
        _navigatorList.Clear();
    }

    /// <summary>
    /// Navigatorの移動先を設定(縦方向限定)
    /// </summary>
    public void SetVerticalNavigtor()
    {
        SetSelectNavigator(true, false);
    }

    /// <summary>
    /// Navigatorの移動先を設定(横方向限定)
    /// </summary>
    public void SetHorizontalNavigtor()
    {
        SetSelectNavigator(false, true);
    }

    /// <summary>
    /// Navigatorの移動先を設定(全方向)
    /// </summary>
    public void SetSelectNavigator()
    {
        SetSelectNavigator(true, true);
    }

    /// <summary>
    /// Navigatorの移動先を設定する
    /// </summary>
    private void SetSelectNavigator(bool isVertical, bool isHorizontal)
    {
        if(_navigatorList.Count == 0 || isVertical == isHorizontal == false)
        {
            return;
        }

        // 距離を調べて最適なものを設定する
        for (var i = 0; i < _navigatorList.Count; i++)
        {
            var baseNavi = _navigatorList[i];
            Navigator up    = null;
            Navigator down  = null;
            Navigator left  = null;
            Navigator right = null;

            for(var j = 0; j < _navigatorList.Count; j++)
            {
                if(baseNavi == _navigatorList[j])
                {
                    // 設定元と同じものは確認しない
                    continue;
                }

                var navi = _navigatorList[j];

                // 縦方向の設定
                if(isVertical)
                {
                    // 上方向を調べる
                    // baseNaviより上にあるかどうか
                    if (navi.transform.localPosition.y > baseNavi.transform.localPosition.y)
                    {
                        // upがnullなら暫定で設定
                        if (up == null)
                        {
                            up = navi;
                            continue;
                        }
                        // 今のupよりもbaseNaviに近いなら設定(yが小さい方)
                        else if (navi.transform.localPosition.y < up.transform.localPosition.y)
                        {
                            up = navi;
                            continue;
                        }
                    }
                    // 下方向を調べる
                    // baseNaviより下にあるかどうか
                    else if (navi.transform.localPosition.y < baseNavi.transform.localPosition.y)
                    {
                        // downがnullなら暫定で設定
                        if (down == null)
                        {
                            down = navi;
                            continue;
                        }
                        // 今のdownよりもbaseNaviに近いなら設定(yが大きい方)
                        else if (navi.transform.localPosition.y > down.transform.localPosition.y)
                        {
                            up = navi;
                            continue;
                        }
                    }
                }

                // 横方向の設定
                if(isHorizontal)
                {
                    // 左方向を調べる
                    // baseNaviより左にあるかどうか
                    if (navi.transform.localPosition.x < baseNavi.transform.localPosition.x)
                    {
                        // leftがnullなら暫定で設定
                        if (left == null)
                        {
                            left = navi;
                        }
                        // 今のleftよりもbaseNaviに近いなら設定(xが大きい方)
                        else if (navi.transform.localPosition.x > left.transform.localPosition.x)
                        {
                            left = navi;
                        }
                    }
                    // 右方向を調べる
                    // baseNaviより右にあるかどうか
                    else if (navi.transform.localPosition.x > baseNavi.transform.localPosition.x)
                    {
                        // rightがnullなら暫定で設定
                        if (right == null)
                        {
                            right = navi;
                        }
                        // 今のrightよりもbaseNaviに近いなら設定(xが小さい方)
                        if (navi.transform.localPosition.x < right.transform.localPosition.x)
                        {
                            right = navi;
                        }
                    }
                }
            }

            // 上下左右を調べ終わったら移動先を設定
            baseNavi.SetAllNavigator(up, down, left, right);
        }
    }

    /// <summary>
    /// ナビゲーターの移動
    /// </summary>
    public void SelectMoveNavigator(NavigationManager.InputDirection direction)
    {
        if(CurrentNavigator == null)
        {
            return;
        }

        // 指定の方向のナビゲーターがあるか
        var movedNavi = CurrentNavigator.GetDirectionNavigator(direction);
        if (movedNavi == null)
        {
            return;
        }

        // 指定の方向に合わせてナビゲーターを移動
        CurrentNavigator = movedNavi;
    }

    /// <summary>
    /// IndexからCurrentNavigatorの設定
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public bool SetCurrentNavigatorFromIndex(int index)
    {
        var result = false;

        if(_navigatorList.Count == 0)
        {
            return result;
        }

        // CurrentNavigatorの設定
        if(index < _navigatorList.Count)
        {
            CurrentNavigator = _navigatorList[index];
            result = true;
        }

        return result;
    }

    #endregion

    #region Index

    /// <summary>
    /// 指定したNavigatorのIndexを取得
    /// </summary>
    /// <param name="navigator"></param>
    /// <returns></returns>
    public int GetNavigatorIndex(Navigator navigator)
    {
        // _navigatorListやnavigatorがない場合は-1
        if (_navigatorList.Count == 0 || navigator == null)
        {
            return -1;
        }

        // 指定したnavigatorが何番目かを返す
        // ない場合は -1 を返す
        return _navigatorList.IndexOf(navigator);
    }

    /// <summary>
    /// CurrentNavigatorのIndexの取得
    /// </summary>
    public int GetCurrentIndex()
    {
        // Currentに設定されたナビゲーターが_navigatorListの何番目かを取得
        return GetNavigatorIndex(CurrentNavigator);
    }

    /// <summary>
    /// デフォルトIndexの取得
    /// </summary>
    /// <returns></returns>
    public int GetDefaultIndex()
    {
        return _defaultNavigatorIndex;
    }

    /// <summary>
    /// デフォルトインデックスの設定
    /// </summary>
    /// <param name="index"></param>
    /// <param name="isUpdateNavigator">設定した値でナビゲーターをフォーカスするかどうか</param>
    public void SetDefaultIndex(int index, bool isUpdateNavigator = false)
    {
        // インデックスを設定
        _defaultNavigatorIndex = index;

        if(!isUpdateNavigator || _navigatorList.Count == 0)
        {
            return;
        }

        // CurrentNavigatorの更新
        SetCurrentNavigatorFromIndex(_defaultNavigatorIndex);
    }

    #endregion
}
