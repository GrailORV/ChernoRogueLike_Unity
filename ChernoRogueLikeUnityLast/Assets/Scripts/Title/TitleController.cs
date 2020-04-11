using UnityEngine;
using DG.Tweening;

namespace TITLE
{
    /// <summary>
    /// タイトル管理クラス
    /// </summary>
    public class TitleController : MonoBehaviour
    {
        /// <summary>
        /// タイトルロゴオブジェクト
        /// </summary>
        [SerializeField]
        private GameObject[] titleLogo = new GameObject[3];

        /// <summary>
        /// タイトルロゴの移動先トランスフォーム
        /// </summary>
        [SerializeField]
        private Transform[] logoPos = new Transform[3];

        /// <summary>
        /// タイトルロゴの移動にかかる時間
        /// </summary>
        [SerializeField]
        private float[] logoMoveTime = new float[3];

        /// <summary>
        /// タイトルロゴの親オブジェクト
        /// </summary>
        [SerializeField]
        private GameObject titleLogoParent = null;

        /// <summary>
        /// タイトルロゴの待機時間
        /// </summary>
        [SerializeField]
        private float logoWaitTime = 0.5f;

        /// <summary>
        /// タイトルロゴの上昇値
        /// </summary>
        [SerializeField]
        private float titleLogoPosY = 0.0f;

        /// <summary>
        /// タイトルロゴが上に到達するまでの時間
        /// </summary>
        [SerializeField]
        private float titleLogoMoveTime = 0.5f;

        /// <summary>
        /// モードセレクト用パネル
        /// </summary>
        [SerializeField]
        private GameObject selectPanel = null;

        /// <summary>
        /// セレクトパネルが表示されるまでの時間
        /// </summary>
        [SerializeField]
        private float selectFadeTime = 0.5f;

        /// <summary>
        /// Unity Awake
        /// </summary>
        private void Awake()
        {
            Fade.FadeStart(Fade.FadeMode.FM_FadeIn, 1f);
        }

        /// <summary>
        /// Unity Start
        /// </summary>
        void Start()
        {
            var seq = DOTween.Sequence();

            // タイトルロゴを移動先オブジェクトのposまで移動
            seq.Append(titleLogo[0].transform.DOMove(logoPos[0].position, logoMoveTime[0]))
            .Append(titleLogo[1].transform.DOMove(logoPos[1].position, logoMoveTime[1]))
            .Append(titleLogo[2].transform.DOMove(logoPos[2].position, logoMoveTime[2]))
            .AppendInterval(logoWaitTime)
            .Append(titleLogoParent.transform.DOLocalMoveY(titleLogoPosY, titleLogoMoveTime))
            .OnComplete(() =>
            {
                selectPanel.GetComponent<CanvasGroup>().DOFade(1f, selectFadeTime);
            });
        }
    }

}
