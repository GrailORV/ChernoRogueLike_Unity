﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioController : SingletonMonoBehaviour<AudioController> {

    //ボリューム保存用のkeyとデフォルト値
    private const string BGM_VOLUME_KEY = "BGM_VOLUME_KEY";
    private const string SE_VOLUME_KEY = "SE_VOLUME_KEY";
    private const float BGM_VOLUME_DEFULT = 0.6f;
    private const float SE_VOLUME_DEFULT = 0.6f;

    //BGMがフェードするのにかかる時間
    public const float BGM_FADE_SPEED_RATE_HIGH = 0.9f;
    public const float BGM_FADE_SPEED_RATE_LOW = 0.3f;
    private float _bgmFadeSpeedRate = BGM_FADE_SPEED_RATE_HIGH;

    //BGMをフェードアウト中か
    private bool _isFadeOut = false;

    //次流すBGM名、SE名
    private string _nextBGMName;
    private string _nextSEName;

    //BGM用、SE用に分けてオーディオソースを持つ
    [SerializeField]
    private AudioSource AttachBGMSource, AttachSESource;

    // 全部のAudioを所持
    private Dictionary<string, AudioClip> _bgmDic, _seDic;

    // 変更確定用のボタン
    [SerializeField]
    private Button _applyButton, _cancelButton;

    [SerializeField]
    private Slider _BGMSlider, _SESlider;

    private float _beforeBGM, _beforeSE;

    // Start関数より先に起こす処理
    private void Awake()
    {
        if (this != Instance)
        {
            Destroy(this);
            return;
        }

        // シーンを移動してもこのゲームオブジェクトは消えないように
        DontDestroyOnLoad(gameObject);

        // リソースフォルダにある「BGM」「SE」のファイルを全部読み込む
        _bgmDic = new Dictionary<string, AudioClip>();
        _seDic = new Dictionary<string, AudioClip>();

        object[] bgmList = Resources.LoadAll("Audio/BGM");
        object[] seList = Resources.LoadAll("Audio/SE");

        // bgmListの中から名前と同じやつを_bgmDicに入れる
        foreach (AudioClip bgm in bgmList)
        {
            _bgmDic[bgm.name] = bgm;
        }

        // seListの中から名前と同じやつを_seDicに入れる
        foreach (AudioClip se in seList)
        {
            _seDic[se.name] = se;
        }
    }

    // Use this for initialization
    void Start()
    {
        _beforeBGM = PlayerPrefs.GetFloat(BGM_VOLUME_KEY, BGM_VOLUME_DEFULT);
        _beforeSE = PlayerPrefs.GetFloat(SE_VOLUME_KEY, SE_VOLUME_DEFULT);

        // 音量をセーブされている所から値を設定
        AttachBGMSource.volume = _beforeBGM;
        AttachSESource.volume = _beforeSE;

        _BGMSlider.value = _beforeBGM;
        _SESlider.value = _beforeSE;
    }

    //======================================================================================
    // BGM
    //======================================================================================
    /// <summary>
    /// 指定したファイル名をBGMを流す。ただしすでに流れている場合は前の曲をフェードアウトさせてから。
    /// 第二引数のfadeSpeedRateに指定した割合でフェードアウトするスピードが変わる
    /// </summary>
    public void PlayBGM(string bgmName, float fadeSpeedRate = BGM_FADE_SPEED_RATE_HIGH)
    {
        if (!_bgmDic.ContainsKey(bgmName))
        {
            Debug.LogWarning(bgmName + "という名前のBGMはありません");
            return;
        }

        // 現在のBGMが流れていない時はそのまま流す
        if (!AttachBGMSource.isPlaying)
        {
            _nextBGMName = "";
            AttachBGMSource.clip = _bgmDic[bgmName] as AudioClip;
            AttachBGMSource.Play();
        }
        // 違うBGMが流れている場合は、流れているBGMをフェードアウトさせてから次を流す。同じBGMが流れているならスルー
        else if (AttachBGMSource.clip.name != bgmName)
        {
            _nextBGMName = bgmName;
            FadeOutBGM(fadeSpeedRate);
        }
    }
    /// <summary>
    /// 現在流れている曲をフェードアウトさせる
    /// fadeSpeedRateに指定した割合でフェードアウトするスピードが変わる
    /// </summary>
    public void FadeOutBGM(float fadeSpeedRate = BGM_FADE_SPEED_RATE_LOW)
    {
        _bgmFadeSpeedRate = fadeSpeedRate;
        _isFadeOut = true;
    }

    //======================================================================================
    // SE
    //======================================================================================

    public void PlaySE(string seName, float delay = 0.0f)
    {
        if (!_seDic.ContainsKey(seName))
        {
            Debug.LogWarning(seName + "という名前のSEはありません");
            return;
        }
        _nextSEName = seName;
        Invoke("DelayPlaySE", delay);
    }

    private void DelayPlaySE()
    {
        // ここでSEを鳴らす
        AttachSESource.PlayOneShot(_seDic[_nextSEName] as AudioClip);
    }


    //======================================================================================
    // 音量変更
    //======================================================================================

    public void ChangeBGMVolume(float BGMVolume)
    {
        AttachBGMSource.volume = BGMVolume;
    }

    public void ChangeSEVolume(float SEVolume)
    {
        AttachSESource.volume = SEVolume;
    }

    //======================================================================================
    // 設定確定 or キャンセル
    //======================================================================================
    public void OnClickApply()
    {
        PlayerPrefs.SetFloat(BGM_VOLUME_KEY, AttachBGMSource.volume);
        PlayerPrefs.SetFloat(SE_VOLUME_KEY, AttachSESource.volume);
    }

    public void OnClickChancel()
    {
        AttachBGMSource.volume = _beforeBGM;
        AttachSESource.volume = _beforeSE;

        _BGMSlider.value = _beforeBGM;
        _SESlider.value = _beforeSE;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isFadeOut)
        {
            return;
        }

        // 徐々にボリュームを下げていき、ボリュームが0になったらボリュームを戻し次の曲を流す
        AttachBGMSource.volume -= Time.deltaTime * _bgmFadeSpeedRate;
        if (AttachBGMSource.volume <= 0)
        {
            AttachBGMSource.Stop();
            AttachBGMSource.volume = PlayerPrefs.GetFloat(BGM_VOLUME_KEY, BGM_VOLUME_DEFULT);
            _isFadeOut = false;

            if (!string.IsNullOrEmpty(_nextBGMName))
            {
                PlayBGM(_nextBGMName);
            }
        }
    }
}
