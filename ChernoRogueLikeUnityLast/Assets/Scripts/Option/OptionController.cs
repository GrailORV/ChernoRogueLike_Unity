using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionController : SingletonMonoBehaviour<OptionController> {

    static GameObject _option;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        _option = Resources.Load("Prefabs/OptionMenu") as GameObject;
        Instantiate(_option);
        GameObject.DontDestroyOnLoad(_option);
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            _option.SetActive(!_option.activeSelf);
        }
    }
}