using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionController : SingletonMonoBehaviour<OptionController> {

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        var obj = Resources.Load("Prefabs/OptionMenu") as GameObject;
        Instantiate(obj);
    }

    private void Awake()
    {
        if (this != Instance)
        {
            Destroy(this);
            return;
        }
    }

    private void Start()
    {

    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            CloseOption();
        }
    }

    /// <summary>
    /// オプションを閉じる
    /// </summary>
    public void CloseOption()
    {
        // 変更していて確定前だったら確認する
        // 変更しない場合変更前の状態にすべて戻す


        // オプションを非表示にする
        foreach (Transform child in this.gameObject.transform)
        {
            child.gameObject.SetActive(!child.gameObject.activeSelf);
        }
    }
}