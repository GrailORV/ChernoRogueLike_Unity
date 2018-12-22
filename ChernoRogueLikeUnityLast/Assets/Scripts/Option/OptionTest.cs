using UnityEngine;

public class OptionTest : MonoBehaviour {


	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space))
        {
            AudioController.Instance.PlayBGM(AudioData.BGM_TEST);
        }

        else if(Input.GetKeyDown(KeyCode.A))
        {
            AudioController.Instance.PlaySE(AudioData.SE_TEST);
        }
	}
}
