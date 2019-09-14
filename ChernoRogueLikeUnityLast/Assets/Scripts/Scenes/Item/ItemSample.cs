using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSample : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        WindowManager.Instance.CreateAndOpenWindow<ItemWindow>(WindowData.WindowType.ItemWindow);
    }
}
