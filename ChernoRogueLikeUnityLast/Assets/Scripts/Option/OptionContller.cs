using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionContller : SinglTonMonoBehaviour<OptionContller> {

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private void InitOption()
    {
        var optionController = Resources.Load("Prefabs/OptionController") as GameObject;
        Instantiate(optionController);
        GameObject.DontDestroyOnLoad(optionController);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {

        }
    }
}