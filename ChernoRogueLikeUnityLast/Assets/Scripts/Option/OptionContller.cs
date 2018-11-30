using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionContller : MonoBehaviour {

    // 音量調節スライダー
    [SerializeField]
    Slider _bgmSlider, _seSlider;

    // フルスクリーンかどうかのチェックボックス
    [SerializeField]
    Toggle _windowToggle;

    // 直接音量を調節するためのInputfield
    [SerializeField]
    InputField _bgmInputfield, _seInputfield;

    // InputFieldの値を入れるものと初期値
    int _bgmVolume = 100;
    int _seVolume = 100;

    // 変更前の音量
    int _beforeBGMVolume, _beforeSEVolume;

    private void Start()
    {
        _bgmSlider.value = _bgmVolume;
        _seSlider.value = _seVolume;
    }

    private void Update()
    {

    }

    public void BGMApply()
    {
        Int32.TryParse(_bgmInputfield.text, out _bgmVolume);
        _bgmSlider.value = _bgmVolume;
    }

    public void SEApply()
    {
        Int32.TryParse(_seInputfield.text, out _seVolume);
        _seSlider.value = _seVolume;
    }
}
