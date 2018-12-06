using System;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(MapManager))]
public class MapManagerEditor : Editor
{
    MapManager mm = null;
    bool[] _isOpen;

    private void OnEnable()
    {
        mm = target as MapManager;


        _isOpen = new bool[(int)MapManager.EMapCategory.MAX];
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (mm._WallTextures == null)
        {
            mm._FloorTextures = new Texture[(int)MapManager.EMapCategory.MAX];
            mm._WallTextures = new Texture[(int)MapManager.EMapCategory.MAX];
            mm._WaterTextures = new Texture[(int)MapManager.EMapCategory.MAX];
        }
        if (mm._WallTextures.Length != (int)MapManager.EMapCategory.MAX)
        {
            Array.Resize(ref mm._FloorTextures, (int)MapManager.EMapCategory.MAX);
            Array.Resize(ref mm._WallTextures, (int)MapManager.EMapCategory.MAX);
            Array.Resize(ref mm._WaterTextures, (int)MapManager.EMapCategory.MAX);
        }

        for (uint i = 0; i < _isOpen.Length; i++)
        {
            MapManager.EMapCategory category = (MapManager.EMapCategory)Enum.ToObject(typeof(MapManager.EMapCategory), i);
            bool isOpen = EditorGUILayout.Foldout(_isOpen[i], category.ToString());

            if (_isOpen[i] != isOpen)
            {
                _isOpen[i] = isOpen;
            }

            if (isOpen)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Floor Tex");
                mm._FloorTextures[i] = EditorGUILayout.ObjectField(mm._FloorTextures[i], typeof(Texture), false, GUILayout.Width(64), GUILayout.Height(64)) as Texture;
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Wall Tex");
                mm._WallTextures[i] = EditorGUILayout.ObjectField(mm._WallTextures[i], typeof(Texture), false, GUILayout.Width(64), GUILayout.Height(64)) as Texture;
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Water Tex");
                mm._WaterTextures[i] = EditorGUILayout.ObjectField(mm._WaterTextures[i], typeof(Texture), false, GUILayout.Width(64), GUILayout.Height(64)) as Texture;
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}
