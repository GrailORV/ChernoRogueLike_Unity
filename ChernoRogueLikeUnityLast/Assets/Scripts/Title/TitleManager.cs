using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace TITLE
{
    public class TitleManager : MonoBehaviour
    {

        // タイトルロゴのobject
        [SerializeField] GameObject[] m_TitleLogo = new GameObject[3];

        // タイトルロゴの最終到達点座標
        [SerializeField] static readonly Vector3[] m_LogoMovePos = new Vector3[3];

        // タイトルロゴが移動し終わる時間
        [SerializeField] static readonly float[] m_LogoMoveTime = new float[3];

        // ロゴ全体を縦に動かす
        [SerializeField, Space(10)] readonly float m_LogoPos_Y = 0.0f;

        // ロゴを縦に動かすのにかかる時間
        [SerializeField] static readonly float m_LogoTime_Y = 0.0f;

        // ロゴを固定して縦に動かすまでの待機時間
        [SerializeField] static readonly float m_TitleWaitTime = 0.0f;

        // 
        [SerializeField, Space(25)] GameObject SelectPanel;
        [SerializeField] float m_SelectFadeTime;

        private void Awake()
        {
            Fade.FadeStart(Fade.FadeMode.FM_FadeIn, 1f);
        }

        // Use this for initialization
        void Start()
        {

            Sequence seq = DOTween.Sequence();
            seq.OnStart(() =>
            {

            })
            .Append(m_TitleLogo[0].transform.DOMove(m_LogoMovePos[0], m_LogoMoveTime[0]))
            .Append(m_TitleLogo[1].transform.DOMove(m_LogoMovePos[1], m_LogoMoveTime[1]))
            .Append(m_TitleLogo[2].transform.DOMove(m_LogoMovePos[2], m_LogoMoveTime[2]))
            .AppendInterval(m_TitleWaitTime)
            .Append(m_TitleLogo[3].transform.DOLocalMoveY(m_LogoPos_Y, m_LogoTime_Y))
            .OnComplete(() =>
            {
                SelectPanel.GetComponent<CanvasGroup>().DOFade(1f, m_SelectFadeTime);
            });
        }
    }

}
