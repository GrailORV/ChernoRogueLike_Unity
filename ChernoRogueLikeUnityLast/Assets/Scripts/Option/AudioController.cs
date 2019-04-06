using System.Collections.Generic;
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

    // 音量変更スライダー
    [SerializeField] private Slider _BGMSlider = null;
    [SerializeField] private Slider _SESlider = null;

    // デフォルトのBGMとSEの音量
    private float _beforeBGM = 0.0f;
    private float _beforeSE = 0.0f;
    
    // 音量の変更タイプ
    public enum ChangeType
    {
        BGM,
        SE
    };

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
        // 各ボリュームの初期化
        _beforeBGM = PlayerPrefs.GetFloat(BGM_VOLUME_KEY, BGM_VOLUME_DEFULT);
        _beforeSE = PlayerPrefs.GetFloat(SE_VOLUME_KEY, SE_VOLUME_DEFULT);

        // 音量をセーブされている所から値を設定
        AttachBGMSource.volume = _beforeBGM;
        AttachSESource.volume = _beforeSE;

        // スライダーの値の初期化
        _BGMSlider.value = _beforeBGM;
        _SESlider.value = _beforeSE;
    }

    /// <summary>
    /// BGMの再生
    /// ほかのBGMが流れている場合はフェードアウトさせてから再生
    /// </summary>
    /// <param name="bgmName">流したいBGMの名前</param>
    /// <param name="fadeSpeedRate">前の曲をフェードアウトさせる長さ</param>
    public void PlayBGM(string bgmName, float fadeSpeedRate = BGM_FADE_SPEED_RATE_HIGH)
    {
        // BGMがない場合はログを表示しスルー
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

    /// <summary>
    /// SEを再生
    /// </summary>
    /// <param name="seName">流したいSEの名前</param>
    /// <param name="delay">遅らせて再生したい場合は任意の値を入力（defaultは0）</param>
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

    /// <summary>
    /// 指定されたSEを再生
    /// </summary>
    private void DelayPlaySE()
    {
        // ここでSEを鳴らす
        AttachSESource.PlayOneShot(_seDic[_nextSEName] as AudioClip);
    }

    /// <summary>
    /// 音量調節
    /// </summary>
    /// <param name="volume">変更したい音量</param>
    /// <param name="changeType">BGM or SE</param>
    public void ChangeVolume(float volume ,ChangeType changeType)
    {
        switch(changeType)
        {
            case ChangeType.BGM:
                AttachBGMSource.volume = volume;
                break;

            case ChangeType.SE:
                AttachSESource.volume = volume;
                break;
        }
    }

    /// <summary>
    /// Applyボタンを押すと変更が保存される
    /// </summary>
    public void OnClickApply()
    {
        // PlayerPrefsに値を保存
        PlayerPrefs.SetFloat(BGM_VOLUME_KEY, AttachBGMSource.volume);
        PlayerPrefs.SetFloat(SE_VOLUME_KEY, AttachSESource.volume);
    }

    /// <summary>
    /// Chancelボタンを押すと変更前の値に修正される
    /// </summary>
    public void OnClickChancel()
    {
        // 実行時の値を代入
        AttachBGMSource.volume = _beforeBGM;
        AttachSESource.volume = _beforeSE;

        _BGMSlider.value = _beforeBGM;
        _SESlider.value = _beforeSE;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isFadeOut) return;
        
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
