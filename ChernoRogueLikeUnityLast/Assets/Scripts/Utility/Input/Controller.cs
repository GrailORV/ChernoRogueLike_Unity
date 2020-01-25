using System.Collections.Generic;
using UnityEngine;

namespace InputController
{
    public class Controller
    {
        #region ENUM

        // ボタン(コメントはPS4基準)
        public enum Button
        {
            Button_0,   // □ボタン
            Button_1,   // ×ボタン
            Button_2,   // 〇ボタン
            Button_3,   // △ボタン

            Button_4,   // L1ボタン
            Button_5,   // R1ボタン

            Button_6,   // L2ボタン
            Button_7,   // R2ボタン

            Button_8,   // Shareボタン
            Button_9,   // Optionボタン

            Button_10,  // L3ボタン(スティック押し込み)
            Button_11,  // R3ボタン(スティック押し込み)

            Button_12,  // PSボタン押し込み
            Button_13,  // パッドボタン押し込み

            Max,
        }

        // スティック（十字キー含む）
        public enum Axis
        {
            L_Horizontal,       // Lスティック 左右
            L_Vertical,         // Lスティック 上下

            R_Horizontal,       // Rスティック 左右
            R_Vertical,         // Rスティック 上下

            Cross_Horizontal,   // 十字キー    左右
            Cross_Vertical,     // 十字キー    上下

            Max,
        }

        #endregion

        /// <summary> AxisのDownやUp、Rowなどの閾値 </summary>
        public double Threshold { get; private set; }

        // プロパティ
        public Dictionary<Button, ButtonState> ButtonDict { get; set; }
        public Dictionary<Axis, AxisState> AxisDict { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Controller()
        {
            // ボタンの初期化
            ButtonDict = new Dictionary<Button, ButtonState>();
            for (var i = 0; i < (int)Button.Max; i++)
            {
                ButtonDict.Add((Button)i, new ButtonState((Button)i));
            }

            // 軸の初期化
            AxisDict = new Dictionary<Axis, AxisState>();
            for (var i = 0; i < (int)Axis.Max; i++)
            {
                AxisDict.Add((Axis)i, new AxisState((Axis)i));
            }

            // しきい値の設定
            SetThreshold(Threshold);
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        public void Update()
        {
            // ボタンの更新
            foreach (var button in ButtonDict.Values)
            {
                // ボタンが有効か
                if (button != null && button.Enabled)
                {
                    button.Update();
                }
            }

            // 軸の更新
            foreach (var axis in AxisDict.Values)
            {
                // 軸が有効か
                if (axis != null && axis.Enabled)
                {
                    axis.Update();
                }
            }
        }

        /// <summary>
        /// 閾値の設定
        /// </summary>
        /// <param name="threshold"></param>
        public void SetThreshold(double threshold)
        {
            foreach (var key in AxisDict.Keys)
            {
                AxisDict[key].Threshold = threshold;
            }

            Threshold = threshold;
        }

        /// <summary>
        /// ボタンに対応するキーコードの追加
        /// </summary>
        /// <param name="button"></param>
        /// <param name="key"></param>
        public void SetButtonKeyCode(Button button, KeyCode key)
        {
            ButtonDict[button].AddKeyCode(key);
        }

        /// <summary>
        /// ボタンに対応する名前の追加
        /// </summary>
        /// <param name="button"></param>
        /// <param name="name"></param>
        public void SetButtonName(Button button, string name)
        {
            ButtonDict[button].AddKeyName(name);
        }

        /// <summary>
        /// 軸に対応するキーコードの追加
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="keyCode"></param>
        /// <param name="negative"></param>
        public void SetAxisKeyCodes(Axis axis, KeyCode positive, KeyCode negative)
        {
            AxisDict[axis].AddKeyCode(positive, negative);
        }

        /// <summary>
        /// 軸に対応する名前の追加
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="name"></param>
        public void SetAxisName(Axis axis, string name)
        {
            AxisDict[axis].AddKeyName(name);
        }

        /// <summary>
        /// 軸の感度の設定
        /// </summary>
        /// <param name="sensitivity"></param>
        public void SetAxisSensitivity(double sensitivity)
        {
            for (var i = 0; i < (int)Axis.Max; i++)
            {
                AxisDict[(Axis)i].Sensitivity = sensitivity;
            }
        }

        /// <summary>
        /// 軸の重さの設定
        /// </summary>
        /// <param name="gravity"></param>
        public void SetAxisGravity(double gravity)
        {
            for (var i = 0; i < (int)Axis.Max; i++)
            {
                AxisDict[(Axis)i].Gravity = gravity;
            }
        }

        /// <summary>
        /// 軸の反応しない範囲の設定
        /// </summary>
        /// <param name="dead"></param>
        public void SetAxisDead(double dead)
        {
            if (dead >= 1)
            {
                return;
            }

            for (var i = 0; i < (int)Axis.Max; i++)
            {
                AxisDict[(Axis)i].Dead = dead;
            }
        }

        #region Button関数

        /// <summary>
        /// ボタン押下の取得
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public bool GetButtonDown(Button button)
        {
            return ButtonDict[button].GetButtonDown();
        }

        /// <summary>
        /// どれかのボタン押下
        /// </summary>
        /// <returns></returns>
        public bool GetAnyButtonDown()
        {
            bool down = false;

            for (int i = 0; i < (int)Button.Max; i++)
            {
                down = down || GetButtonDown((Button)i);
            }

            return down;
        }

        /// <summary>
        /// ボタンを押している間
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public bool GetButtonStay(Button button)
        {
            return ButtonDict[button].GetButtonStay();
        }

        /// <summary>
        /// どれかのボタンを押している間
        /// </summary>
        /// <returns></returns>
        public bool GetAnyButtonStay()
        {
            bool stay = false;

            for (var i = 0; i < (int)Button.Max; i++)
            {
                stay = stay || GetButtonStay((Button)i);
            }

            return stay;
        }

        /// <summary>
        /// ワンテンポ置いた連打
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public bool GetButtonRepeat(Button button)
        {
            return ButtonDict[button].GetButtonRepeat();
        }

        /// <summary>
        /// どれかのワンテンポ置いた連打
        /// </summary>
        /// <returns></returns>
        public bool GetAnyButtonRepeat()
        {
            bool stay = false;

            for (int i = 0; i < (int)Button.Max; i++)
            {
                stay = stay || GetButtonRepeat((Button)i);
            }

            return stay;
        }

        /// <summary>
        /// ボタンを離したとき
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public bool GetButtonUp(Button button)
        {
            return ButtonDict[button].GetButtonUp();
        }

        /// <summary>
        /// どれかのボタンを離したとき
        /// </summary>
        /// <returns></returns>
        public bool GetAnyButtonUp()
        {
            bool up = false;

            for (int i = 0; i < (int)Button.Max; i++)
            {
                up = up || GetButtonUp((Button)i);
            }

            return up;
        }

        /// <summary>
        /// 長押し時間の取得
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public float GetButtonStayTime(Button button)
        {
            if (button == Button.Max)
            {
                return 0f;
            }

            return ButtonDict[button].StayTime;
        }
        #endregion

        #region Axis関数

        /// <summary>
        /// スティックを倒す
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public int GetAxisDown(Axis axis)
        {
            return AxisDict[axis].GetAxisDown();
        }

        /// <summary>
        /// どれかのスティックを倒す
        /// </summary>
        /// <param name="isHorizontal"></param>
        /// <returns></returns>
        public int GetAnyAxisDown(bool isHorizontal)
        {
            int value = 0;
            if (isHorizontal)
            {
                value += GetAxisDown(Axis.R_Horizontal);
                value += GetAxisDown(Axis.L_Horizontal);
                value += GetAxisDown(Axis.Cross_Horizontal);
            }
            else
            {
                value += GetAxisDown(Axis.R_Vertical);
                value += GetAxisDown(Axis.L_Vertical);
                value += GetAxisDown(Axis.Cross_Vertical);
            }

            // 値を1か-1以上にさせない
            if (Mathf.Abs(value) > 1)
            {
                value = value / Mathf.Abs(value);
            }

            return value;
        }

        /// <summary>
        /// スティックを離したとき
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public int GetAxisUp(Axis axis)
        {
            return AxisDict[axis].GetAxisUp();
        }

        public int GetAnyAxisUp(bool isHorizontal)
        {
            int value = 0;
            if (isHorizontal)
            {
                value += GetAxisUp(Axis.R_Horizontal);
                value += GetAxisUp(Axis.L_Horizontal);
                value += GetAxisUp(Axis.Cross_Horizontal);
            }
            else
            {
                value += GetAxisUp(Axis.R_Vertical);
                value += GetAxisUp(Axis.L_Vertical);
                value += GetAxisUp(Axis.Cross_Vertical);
            }

            // 値を1か-1以上にさせない
            if (Mathf.Abs(value) > 1)
            {
                value = value / Mathf.Abs(value);
            }

            return value;
        }

        /// <summary>
        /// スティックを倒している間
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public float GetAxis(Axis axis)
        {
            return AxisDict[axis].GetAxis();
        }

        /// <summary>
        /// どれかのスティックを倒している間
        /// </summary>
        /// <param name="isHorizontal"></param>
        /// <returns></returns>
        public float GetAnyAxis(bool isHorizontal)
        {
            float value = 0;
            if (isHorizontal)
            {
                value += GetAxis(Axis.R_Horizontal);
                value += GetAxis(Axis.L_Horizontal);
                value += GetAxis(Axis.Cross_Horizontal);
            }
            else
            {
                value += GetAxis(Axis.R_Vertical);
                value += GetAxis(Axis.L_Vertical);
                value += GetAxis(Axis.Cross_Vertical);
            }

            // 値を1か-1以上にさせない
            if (Mathf.Abs(value) > 1)
            {
                value = value / Mathf.Abs(value);
            }

            return value;
        }

        /// <summary>
        /// 一度押した後、間を置いて連続で押下される
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public int GetAxisRepeat(Axis axis)
        {
            return AxisDict[axis].GetAxisRepeat();
        }

        /// <summary>
        /// スティックを閾値込みで倒したか
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public int GetAxisRow(Axis axis)
        {
            return AxisDict[axis].GetAxisRow();
        }

        /// <summary>
        /// どれかのスティックを閾値込みで倒したか
        /// </summary>
        /// <param name="isHorizontal"></param>
        /// <returns></returns>
        public int GetAnyAxisRow(bool isHorizontal)
        {
            int value = 0;
            if (isHorizontal)
            {
                value += GetAxisRow(Axis.R_Horizontal);
                value += GetAxisRow(Axis.L_Horizontal);
                value += GetAxisRow(Axis.Cross_Horizontal);
            }
            else
            {
                value += GetAxisRow(Axis.R_Vertical);
                value += GetAxisRow(Axis.L_Vertical);
                value += GetAxisRow(Axis.Cross_Vertical);
            }

            // 値を1か-1以上にさせない
            if (Mathf.Abs(value) > 1)
            {
                value = value / Mathf.Abs(value);
            }

            return value;
        }

        /// <summary>
        /// スティックの倒した時間
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        public float GetAxisStayTime(Axis axis)
        {
            if (axis == Axis.Max)
            {
                return 0;
            }

            return AxisDict[axis].StayTime;
        }

        #endregion
    }
}