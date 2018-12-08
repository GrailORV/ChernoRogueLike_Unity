using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemController : MonoBehaviour {

    // ゲームの速度と文字送りの速度のkeyとデフォルト値
    private const string GAME_SPEED_KEY = "GAME_SPEED_KEY";
    private const string MESSAGE_SPEED_KEY = "MESSAGE_SPEED_KEY";

    private const float GAME_SPEED_DEFULT = 0.5f;
    private const float MESSAGE_SPEED_DEFULT = 0.5f;

    [SerializeField]
    private Slider[] _slider;

    // Use this for initialization
    void Start () {

        // PlayerPrefsにセーブされている値を取得し、入れる(今は仮でスライダーに代入)
        _slider[0].value = PlayerPrefs.GetFloat(GAME_SPEED_KEY, GAME_SPEED_DEFULT);
        _slider[1].value = PlayerPrefs.GetFloat(MESSAGE_SPEED_KEY, MESSAGE_SPEED_DEFULT);
	}


    //======================================================================================
    // 音量変更
    //======================================================================================
    public void ChangeGameSpeed(float speed)
    {
        // ここでゲームスピードに変更を加える


        PlayerPrefs.SetFloat(GAME_SPEED_KEY, speed);
    }

    public void ChangeMesssageSpeed(float speed)
    {
        // ここでゲームスピードに変更を加える


        PlayerPrefs.SetFloat(MESSAGE_SPEED_KEY, speed);
    }
}
