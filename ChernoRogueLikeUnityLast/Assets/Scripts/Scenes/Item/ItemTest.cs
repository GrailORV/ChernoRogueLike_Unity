using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTest : MonoBehaviour
{
	// Use this for initialization
	void Start ()
    {
        var window = WindowManager.Instance.CreateAndOpenWindow<ItemWindow>(WindowData.WindowType.ItemWindow);
        window.SetUp();
        return;
	}
}
