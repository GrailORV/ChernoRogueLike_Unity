﻿using UnityEngine;
 
public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance != null)
            {
                // instanceが生成済みなら返して終わり
                return instance;
            }

            // 同じクラスを持つオブジェクトがないか調べる
            var objects = FindObjectsOfType<T>();
            if(objects.Length > 0)
            {
                instance = objects[0];

                // 2つ以上同じクラスのオブジェクトがあるなら削除する
                for(int i = 1; i < objects.Length; i++)
                {
                    DestroyImmediate(objects[i].gameObject);
                }

                return instance;
            }

            if (instance == null)
            {
                Debug.Log("<color=yellow>" + typeof(T) + "がないからつくるで</color>");

                var obj = new GameObject(typeof(T).ToString());
                instance = obj.AddComponent(typeof(T)) as T;
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (FindObjectsOfType<T>().Length > 1)
        {
            DestroyImmediate(this);
        }
    }

    /// <summary>
    /// 削除時に呼ばれる処理
    /// </summary>
    void OnDestroy()
    {
        // インスタンスの初期化
        instance = null;
    }
}