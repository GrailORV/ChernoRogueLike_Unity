using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionController : SinglTonMonoBehaviour<OptionController> {

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private void InitOption()
    {
        var optionController = Resources.Load("Prefabs/OptionMenu") as GameObject;
        Instantiate(optionController);
        GameObject.DontDestroyOnLoad(optionController);
    }

    //private void Awake()
    //{
    //    if(this != Instance)
    //    {
    //        Destroy(this);
    //        return;
    //    }

    //    DontDestroyOnLoad(this.gameObject);
    //}

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {

        }
    }
}