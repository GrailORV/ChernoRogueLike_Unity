using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// WindowManagerクラスで作成できるウィンドウのタイプ<Enum>とパス<string>を作成するエディタ拡張
/// </summary>
public class WindowManagerEditor : MonoBehaviour
{
    // コマンド名
    const string COMMAND_NAME = "Tools/CreateWindowData";

    // 検索するPrefabのディレクトリPath
    const string PREFAB_DIRECTORY_PATH = "Assets/Resources/Prefab";

    // 生成するcsファイルのPath(作成するファイル名含む)
    const string CREATE_SCRIPT_PATH = "Assets/Scripts/Manager/Window/WindowData.cs";

    // ファイル名(拡張子あり)
    readonly static string FILENAME = Path.GetFileName(CREATE_SCRIPT_PATH);

    // ファイル名(拡張子なし)
    readonly static string FILENAME_WITHOUT_EXTENSION = Path.GetFileNameWithoutExtension(CREATE_SCRIPT_PATH);

    // 無効な文字を管理する配列
    static readonly string[] INVALUD_CHARS =
    {
        " ", "!", "\"", "#", "$",
        "%", "&", "\'", "(", ")",
        "-", "=", "^",  "~", "\\",
        "|", "[", "{",  "@", "`",
        "]", "}", ":",  "*", ";",
        "+", "/", "?",  ".", ">",
        ",", "<"
    };

    /// <summary>
    /// ウィンドウのタイプ<Enum>とパス<string>を作成する（メニューから選択可能）
    /// </summary>
    [MenuItem(COMMAND_NAME)]
    static void CreateWindowTypeAndPath()
    {
        if(!CanCreate())
        {
            Debug.LogError("作成できません");
            return;
        }

        // 作成
        CreateScript();
        EditorUtility.DisplayDialog(FILENAME, "作成が完了しました", "OK");
    }

    /// <summary>
    /// スクリプト作成
    /// </summary>
    public static void CreateScript()
    {
        var builder = new StringBuilder();

        // コメントアウト部分
        builder.AppendLine("using System.Collections.Generic;").AppendLine();
        builder.AppendLine("/// (summary)");
        builder.AppendLine("/// ウィンドウのタイプ<Enum>とパス<string>を管理するクラス");
        builder.AppendLine("/// (summary)");

        // クラス名
        builder.AppendFormat("public static class {0}", FILENAME_WITHOUT_EXTENSION).AppendLine();

        // クラスの作成
        builder.AppendLine("{");
        CreateClass(ref builder);
        builder.AppendLine("}");

        // csファイルの作成
        var directoryName = Path.GetDirectoryName(CREATE_SCRIPT_PATH);
        if (!Directory.Exists(directoryName))
        {
            // pathにファイルがなければ作成
            Directory.CreateDirectory(directoryName);
        }
        File.WriteAllText(CREATE_SCRIPT_PATH, builder.ToString(), Encoding.UTF8);
        AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);
    }

    /// <summary>
    /// クラス作成
    /// </summary>
    public static void CreateClass(ref StringBuilder builder)
    {
        // key = 名前 value = パス
        Dictionary<string, string> dict = new Dictionary<string, string>();

        // 指定されたディレクトリ内にあるPrefabをすべて取得する
        // まずはPrefabのオブジェクトのGUIDを検索(複数系)
        var guids = AssetDatabase.FindAssets("t:Prefab");

        if(guids.Length == 0)
        {
            throw new FileNotFoundException("Prefabが見つかりませんでした");
        }

        // GUIDをPathに変換しそこからWindowBaseコンポーネントを使用しているオブジェクトを取得する
        foreach(var guid in guids)
        {
            // pathに変換
            var path = AssetDatabase.GUIDToAssetPath(guid);

            // pathからGameObjectのアセットを取得
            var obj = AssetDatabase.LoadAssetAtPath<WindowBase>(path);

            // WindowBaseをアタッチされたオブジェクトでないなら次へ
            if(obj == null)
            {
                continue;
            }

            // オブジェクトの名前とPathを取得
            // pathはResources以降を取得
            // Pathと名前のどちらも拡張子は必要なし
            path = path.Replace(Path.GetExtension(path), "");
            dict.Add(RemoveInvalidChars(obj.name), path.Replace("Assets/Resources/", ""));
        }

        // enum作成
        builder.AppendLine("\tpublic enum WindowType");
        builder.AppendLine("\t{");
        builder.AppendLine(("\t\t" + "None,")).AppendLine();

        foreach (var key in dict.Keys)
        {
            builder.AppendLine(("\t\t" + key + ","));
        }
        builder.AppendLine("\t}").AppendLine();

        // Dictionaryのpath一覧を作成
        // key = 名前 value = パス
        builder.AppendLine("\tpublic readonly static Dictionary<WindowType, string> WindowPathDict = new Dictionary<WindowType, string>()");
        builder.AppendLine("\t{");
        int count = 0;
        foreach (var data in dict)
        {
            builder.AppendLine(("\t\t{ WindowType." + data.Key + " , \"" + data.Value + "\" },"));
            count++;
        }
        builder.AppendLine("\t};");
    }

    /// <summary>
    /// 作成可能かどうか
    /// </summary>
    /// <returns></returns>
    [MenuItem(COMMAND_NAME, true)]
    public static bool CanCreate()
    {
        return !EditorApplication.isPlaying && !Application.isPlaying && !EditorApplication.isCompiling;
    }

    /// <summary>
    /// 無効な文字を削除します
    /// </summary>
    public static string RemoveInvalidChars(string str)
    {
        Array.ForEach(INVALUD_CHARS, c => str = str.Replace(c, string.Empty));
        return str;
    }
}
