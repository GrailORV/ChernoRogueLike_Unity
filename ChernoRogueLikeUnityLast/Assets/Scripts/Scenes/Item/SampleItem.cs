using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleItem : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        // 確認用のテスト
        if (ItemManager.Instance == null)
        {
            Debug.Log("ItemManager = null");
        }
        else
        {
            Debug.Log("ItemManager = not null");
        }
    }
}
