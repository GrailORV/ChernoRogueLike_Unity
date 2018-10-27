using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TitleManager : MonoBehaviour {

    [SerializeField] GameObject[] m_TitleLogo;
    [SerializeField] Vector3[] m_LogoMovePos;
    [SerializeField] float[] m_LogoMoveTime;
    [SerializeField, Space(10)] float m_LogoPos_Y;
    [SerializeField] float m_TitleWaitTime;

    [SerializeField, Space(25)] GameObject SelectPanel;
    [SerializeField] float m_SelectFadeTime;

    private void Awake()
    {
        Fade.FadeStart(Fade.FadeMode.FM_FadeIn, 1f);
    }

    // Use this for initialization
    void Start () {

        Sequence seq = DOTween.Sequence()
            .OnStart(() =>
            {

            })
            .Append(m_TitleLogo[0].transform.DOMove(m_LogoMovePos[0], m_LogoMoveTime[0]))
            .Append(m_TitleLogo[1].transform.DOMove(m_LogoMovePos[1], m_LogoMoveTime[1]))
            .Append(m_TitleLogo[2].transform.DOMove(m_LogoMovePos[2], m_LogoMoveTime[2]))
            .AppendInterval(m_TitleWaitTime)
            .Append(m_TitleLogo[3].transform.DOLocalMoveY(m_LogoPos_Y, m_LogoMoveTime[3]))
            .OnComplete(() =>
            {
                SelectPanel.GetComponent<CanvasGroup>().DOFade(1f, m_SelectFadeTime);
            });
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
