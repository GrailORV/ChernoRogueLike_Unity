﻿using UnityEngine;
 
public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {                
                instance = (T)FindObjectOfType(typeof(T));
 
                if (instance == null)
                {
                    Debug.LogError(typeof(T) + "がないからつくるで");
 
                    var obj = new GameObject(typeof(T).ToString());
                    obj.AddComponent(typeof(T));
 
                    DontDestroyOnLoad(obj);
                }
            }
 
            return instance;
        }
    }
}