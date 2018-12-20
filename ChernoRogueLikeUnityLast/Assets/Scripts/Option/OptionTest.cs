using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
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
