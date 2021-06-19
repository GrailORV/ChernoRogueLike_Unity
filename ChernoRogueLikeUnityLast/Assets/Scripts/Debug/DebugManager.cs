#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// デバッグ機能
/// </summary>
public class DebugManager : SingletonMonoBehaviour<DebugManager>
{
	// Use this for initialization
	void Start ()
	{
		DontDestroyOnLoad(this.gameObject);
	}
}
#endif	// !UNITY_EDITOR || DEVELOPMENT_BUILD