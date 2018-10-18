using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(SceneManager))]
public class SceneListEditor : Editor
{
    SceneManager sceneManager = null;

    bool _isOpen = false;

    void OnEnable()
    {
        sceneManager = (SceneManager)target;
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        bool isOpen = EditorGUILayout.Foldout(_isOpen, "SceneList");
        if (_isOpen != isOpen)
        {
            _isOpen = isOpen;
        }


        if (isOpen)
        {
            for (int i = 0; i < sceneManager.sceneNameList.Count; )
            {
                var oldScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(sceneManager.sceneNameList[i]);
                EditorGUILayout.BeginHorizontal();
                var newScene = EditorGUILayout.ObjectField("Scene", oldScene, typeof(SceneAsset), false) as SceneAsset;
                var newPath = AssetDatabase.GetAssetPath(newScene);
                sceneManager.sceneNameList[i] = newPath;
                if(GUILayout.Button("-",GUILayout.Width(32)))
                {
                    sceneManager.sceneNameList.RemoveAt(i);
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
                sceneManager.sceneNameList.Add(null);
            }
            EditorGUILayout.EndHorizontal();
        }
    }

}
