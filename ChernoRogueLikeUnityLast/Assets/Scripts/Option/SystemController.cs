using UnityEngine;
using UnityEngine.UI;

public class SystemController : SingletonMonoBehaviour<SystemController> {

    // ゲームの速度と文字送りの速度のkeyとデフォルト値
    private const string GAME_SPEED_KEY = "GAME_SPEED_KEY";
    private const string MESSAGE_SPEED_KEY = "MESSAGE_SPEED_KEY";

    private const float GAME_SPEED_DEFULT = 0.5f;
    private const float MESSAGE_SPEED_DEFULT = 0.5f;

    // スライダー
    [SerializeField] private Slider gameSpeedSlider;
    [SerializeField] private Slider messageSpeedSlider;

    // Use this for initialization
    void Start () {

        // PlayerPrefsにセーブされている値を取得し、入れる(今は仮でスライダーに代入)
        gameSpeedSlider.value = PlayerPrefs.GetFloat(GAME_SPEED_KEY, GAME_SPEED_DEFULT);
        messageSpeedSlider.value = PlayerPrefs.GetFloat(MESSAGE_SPEED_KEY, MESSAGE_SPEED_DEFULT);
	}


    /// <summary>
    /// ゲームスピード変更
    /// </summary>
    /// <param name="speed"></param>
    public void ChangeGameSpeed(float speed)
    {
        // ここでゲームスピードに変更を加える

    }

    /// <summary>
    /// 文字送り速度変更
    /// </summary>
    /// <param name="speed"></param>
    public void ChangeMesssageSpeed(float speed)
    {
        // ここで文字送り速度に変更を加える

    }

    /// <summary>
    /// 変更確定
    /// </summary>
    public void FixConfiguration()
    {
        PlayerPrefs.SetFloat(GAME_SPEED_KEY, gameSpeedSlider.value);
        PlayerPrefs.SetFloat(MESSAGE_SPEED_KEY, messageSpeedSlider.value);
    }
}