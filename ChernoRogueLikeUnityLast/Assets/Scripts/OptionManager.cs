using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour {

    [SerializeField]
    Text optionLabel;

    [RuntimeInitializeOnLoadMethod]
    private static void OptionCreate()
    {
        var obj = Resources.Load("Prefabs/Option");
        Instantiate(obj);
        GameObject.DontDestroyOnLoad(obj);
    }
}