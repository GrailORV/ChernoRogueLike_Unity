using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class unty : MonoBehaviour {

    [SerializeField] GameObject Logo;
    [SerializeField] float fadeTime, IntercalTime;

	// Use this for initialization
	void Start () {
        LogoFade();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LogoFade()
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(Logo.GetComponent<CanvasGroup>().DOFade(1f, fadeTime))
            .AppendInterval(IntercalTime)
            .Append(Logo.GetComponent<CanvasGroup>().DOFade(0f, fadeTime))
            .OnComplete(() =>
            {
                Fade.FadeStart(Fade.FadeMode.FM_FadeOut, 1f, SceneChange);
            });
    }

    public void SceneChange()
    {
        SceneManager.LoadScene("Title");
    }
}