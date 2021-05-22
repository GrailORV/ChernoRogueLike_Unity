using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NavigationLayer))]
[CanEditMultipleObjects]
public class NavigationLayerEditor : Editor
{
    // 入力モード(表示用)
    readonly string[] InputModeNameArray =
    {
        "単一押し",     // single
        "複数押し",     // multi
    };

    // 入力タイプ(表示用)
    readonly string[] InputTypeNameArray =
    {
        "押している間",   // Press
        "押したとき",     // Down
        "離したとき",     // Up
        "連打",           // Repeat
    };

    // キータイプ(表示用)
    readonly string[] InputKeyTypeNameArray =
    {
        "○",
        "×",
        "□",
        "△",
        "L1",
        "L2",
        "L3",
        "R1",
        "R2",
        "R3",
        "Lスティック",
        "Rスティック",
        "十字キー",
    };

    // NavigationLayerクラスのコマンドデータ
    List<NavigationLayer.KeyCommand> commandList;
    //SerializedProperty commandList;

    // KeyCommand毎の折り畳み表示フラグ
    List<bool> isCommandListOpen;

    bool isOpenCommandList = false;
    int commandListCount = 0;

    private void Awake()
    {
        if(commandList == null)
        {
            commandList = (target as NavigationLayer).GetCommandList();
        }

        if (isCommandListOpen == null)
        {
            isCommandListOpen = new List<bool>();
        }

        for (int i = 0; i < commandList.Count; i++)
        {
            if (i >= isCommandListOpen.Count)
            {
                isCommandListOpen.Add(false);
            }
            else
            {
                isCommandListOpen[i] = false;
            }
        }

        commandListCount = commandList.Count;
    }

    public override void OnInspectorGUI()
    {
        // コマンドリスト全体の折り畳み
        if (isOpenCommandList = EditorGUILayout.Foldout(isOpenCommandList, "コマンドリスト", true))
        {
            EditorGUI.indentLevel++;

            //commandListCount = Mathf.Max(0, EditorGUILayout.IntField("コマンド数", commandListCount));

            // すべて展開する or すべて閉じる のボタン追加
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();

                //すべて開くボタン
                if (GUILayout.Button("すべて展開"))
                {
                    for (int i = 0; i < isCommandListOpen.Count; i++)
                    {
                        isCommandListOpen[i] = true;
                    }
                }
                if (GUILayout.Button("すべて閉じる"))
                {
                    for (int i = 0; i < isCommandListOpen.Count; i++)
                    {
                        isCommandListOpen[i] = false;
                    }
                }
            }

            if (GUILayout.Button("コマンドを追加"))
            {
                commandList.Add(new NavigationLayer.KeyCommand());
                isCommandListOpen.Add(true);
            }

            // リストの要素数の調整
            //while (commandListCount != commandList.Count)
            //{
            //    // 実際のデータ数がコマンド数のカウントより少ない時は追加する
            //    if (commandList.Count < commandListCount)
            //    {
            //        commandList.Add(new NavigationLayer.KeyCommand());
            //        isCommandListOpen.Add(true);
            //    }
            //    // 逆に多い時は削除
            //    else if (commandList.Count > commandListCount)
            //    {
            //        var index = Mathf.Max(0, commandList.Count - 1);
            //        commandList.RemoveAt(index);
            //        isCommandListOpen.RemoveAt(index);
            //    }
            //}

            // リストの名前用
            var strBuilder = new StringBuilder();

            for (int i = 0; i < commandList.Count; i++)
            {
                GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));

                var command = commandList[i];

                // 削除ボタン
                using (new GUILayout.HorizontalScope())
                {
                    strBuilder.Clear();

                    // 入力するボタン名を表示
                    strBuilder.Append(i.ToString());
                    strBuilder.Append("【");                    
                    strBuilder.Append(GetKeyName(command));
                    strBuilder.Append("】");

                    // コマンド毎の折り畳み
                    isCommandListOpen[i] = EditorGUILayout.Foldout(isCommandListOpen[i], strBuilder.ToString(), true);

                    if (GUILayout.Button("削除", GUILayout.Width(40)))
                    {
                        commandList.RemoveAt(i);
                        isCommandListOpen.RemoveAt(i);
                        break;
                    }
                }

                // 折り畳みを開いたときの表示
                if (isCommandListOpen[i])
                {
                    EditorGUI.indentLevel++;

                    // モード
                    command.mode = (NavigationManager.InputMode)EditorGUILayout.Popup("モード", (int)command.mode, InputModeNameArray);

                    // タイプ
                    command.type = (NavigationManager.InputType)EditorGUILayout.Popup("タイプ", (int)command.type, InputTypeNameArray);

                    // キーはモードによって表示を変える
                    if (command.mode == NavigationManager.InputMode.Single)
                    {   
                        // キー
                        command.key = (NavigationManager.InputKey)EditorGUILayout.Popup("キー", (int)command.key, InputKeyTypeNameArray);
                    }
                    else if (command.mode == NavigationManager.InputMode.Multi)
                    {
                        // リストの表示をする
                        for()
                    }


                    EditorGUI.indentLevel--;
                }
            }

            EditorGUI.indentLevel--;
        }

        //base.OnInspectorGUI();
    }

    /// <summary>
    /// 同時押し用のキーのエディタUIを表示
    /// </summary>
    void OnGUICommandKeys(List<NavigationManager.InputKey> keys)
    {
        foreach (var key in keys)
        {

        }
    }

    /// <summary>
    /// コマンドからキー名を取得
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    string GetKeyName(NavigationLayer.KeyCommand command)
    {
        if (command == null)
        {
            return "command none";
        }

        if (command.mode == NavigationManager.InputMode.Single)
        {
            return InputKeyTypeNameArray[(int)command.key];
        }

        if (command.mode == NavigationManager.InputMode.Multi)
        {
            var keys = command.keys; 
            var strs = new string[keys.Count];

            for (int i = 0; i < keys.Count; i++)
            {
                strs[i] = InputKeyTypeNameArray[(int)keys[i]];
            }

            return string.Join(",", strs);
        }

        return "InputModeが設定されてません。";
    }
}
