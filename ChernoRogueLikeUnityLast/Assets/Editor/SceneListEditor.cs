using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(SceneManagerScript))]
public class SceneListEditor : Editor
{
    SceneManagerScript sms = null;
    bool _isOpen = false;

    void OnEnable()
    {
        sms = target as SceneManagerScript;
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        

        if (sms.sceneNameList==null)
        {
            sms.sceneNameList = new List<string>();
        }

        bool isOpen = EditorGUILayout.Foldout(_isOpen, "SceneList");
        if (_isOpen != isOpen)
        {
            _isOpen = isOpen;
        }


        if (isOpen)
        {
            for (int i = 0; i < sms.sceneNameList.Count; )
            {
                var oldScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(sms.sceneNameList[i]);
                EditorGUILayout.BeginHorizontal();
                var newScene = EditorGUILayout.ObjectField("Scene", oldScene, typeof(SceneAsset), false) as SceneAsset;
                var newPath = AssetDatabase.GetAssetPath(newScene);
                sms.sceneNameList[i] = newPath;
                if(GUILayout.Button("-",GUILayout.Width(32)))
                {
                    sms.sceneNameList.RemoveAt(i);
                    continue;
                }
                EditorGUILayout.EndHorizontal();
                i++;
            }

            EditorGUILayout.LabelField("");
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("+", GUILayout.Width(32)))
            {
                sms.sceneNameList.Add(null);
            }
            EditorGUILayout.EndHorizontal();
        }
    }

}
