using UnityEngine;

/// <summary>
/// オプション画面のテスト用クラス
/// </summary>
public class OptionTest : MonoBehaviour
{
    /// <summary>
    /// Unity Update
    /// </summary>
	void Update () {

        // スペースキーでBGM再生
		if(Input.GetKeyDown(KeyCode.Space))
        {
            AudioController.Instance.PlayBGM(AudioData.BGM_TEST);
        }

        // AキーでSE再生
        else if(Input.GetKeyDown(KeyCode.A))
        {
            AudioController.Instance.PlaySE(AudioData.SE_TEST);
        }
	}
}
