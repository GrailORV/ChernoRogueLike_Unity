using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using System.IO;
using System.Text;

/// <summary>
/// ScriptableObjectを作成してくれる便利なウィンドウを表示させる
/// 
/// このエディターで作成するファイルは 2 つ
/// 基本となるデータクラス
/// ScriptableObjectにデータを入れてくれるクラス
/// </summary>
public class ScriptableObjectGenerater : EditorWindow
{
    // 実際に使用するScriptableObjectの生成場所
    readonly string MASTERDATA_PATH_FORMAT = "Assets/MasterData/{0}.asset";

    // データクラス・管理クラスのファイルパス
    readonly string DATAFILE_PATH_FORMAT = "Assets/Editor/ScriptableObjectGenerator/DataCS/{0}Data.cs";
    readonly string OPERATOR_PATH_FORMAT = "Assets/Editor/ScriptableObjectGenerator/CsvOperator/{0}Operator.cs";

    // テンプレート用テキストファイルのパス
    readonly string DETAFILE_TEMPLATE_PATH = "Assets/Editor/ScriptableObjectGenerator/Template/ScriptableObjectTemplate.txt";
    readonly string OPERATOR_TEMPLATE_PATH = "Assets/Editor/ScriptableObjectGenerator/Template/OperatorTemplate.txt";

    // Keyの列を表す記号
    readonly string CSV_KEY_SYMBOL = "##";

    // 読み込みたくない行を表す記号
    readonly string CSV_IGNORE_KEY = "//";

    public string masterDataPath = string.Empty;

    Vector2 windowScrollPos = Vector2.zero;
    Vector2 valuesScrollPos = Vector2.zero;
    List<ColumnNameData> columnNameDataList = new List<ColumnNameData>();

    #region メニュー
    /// <summary>
    /// 返り値がtrueの時のみ "Assets/ScriptableObject Generater" をメニューから選択できるようにチェック
    /// </summary>
    /// <returns></returns>
    [MenuItem("Assets/ScriptableObject Generater", validate = true)]
    private static bool ImportCsvValidation()
    {
        //return true;
        // 選択しているものの中でアセットのオブジェクトのみ取得
        var assets = Selection.GetFiltered(typeof(Object), SelectionMode.Assets);

        // 何も選択していない or 複数選択している場合は無効
        if (assets.Length != 1)
        {
            return false;
        }

        // 選択したアセットのパスを取得し、
        // 拡張子を確認してCsvファイル以外の場合は無効
        var path = AssetDatabase.GetAssetPath(assets[0]);
        return Path.GetExtension(path) == ".csv";
    }

    /// <summary>
    /// ScriptableObject Generaterのウィンドウを作成
    /// </summary>
    [MenuItem("Assets/ScriptableObject Generater")]
    private static void ShowWindow()
    {
        // インスタンス生成
        var window = GetWindow<ScriptableObjectGenerater>();

        // カラム名取得
        window.masterDataPath = AssetDatabase.GetAssetPath(Selection.activeInstanceID);
        window.GetColumNameDataList(window.masterDataPath);

        window.Show();
    }

    [MenuItem("Assets/GetPath")]
    private static void GetPath()
    {
        Debug.Log("GetAssetPath = " + AssetDatabase.GetAssetPath(Selection.activeInstanceID));
        Debug.Log("FullPath = " + Path.GetFullPath(AssetDatabase.GetAssetPath(Selection.activeInstanceID)));
    }
    #endregion

    #region デザイン

    /// <summary>
    /// 表示時(表示中も)に呼ばれる処理
    /// 見た目を司る
    /// </summary>
    private void OnGUI()
    {
        // ウィンドウ全体をスクロール
        windowScrollPos = EditorGUILayout.BeginScrollView(windowScrollPos, GUILayout.ExpandWidth(true));

        // ファイル名
        var filePath = masterDataPath;
        EditorGUILayout.LabelField("ファイル名");
        EditorGUILayout.BeginHorizontal(GUI.skin.box);
        EditorGUILayout.LabelField(filePath);
        EditorGUILayout.EndHorizontal();

        // 空白
        GUILayout.Space(10);

        // 変数名 型の二つセットを表示
        EditorGUILayout.LabelField("変数一覧");
        if (columnNameDataList == null || columnNameDataList.Count == 0)
        {
            if(GUILayout.Button("再読み込み"))
            {
                GetColumNameDataList(filePath);
            }

            EditorGUILayout.EndScrollView();
            return;
        }

        valuesScrollPos = EditorGUILayout.BeginScrollView(valuesScrollPos, GUI.skin.box, GUILayout.MinHeight(100), GUILayout.MaxHeight(155));
        {
            // 変数の表示
            for (int i = 0; i < columnNameDataList.Count; i++)
            {
                EditorGUILayout.BeginHorizontal(GUI.skin.box);
                EditorGUILayout.LabelField(columnNameDataList[i].columName);
                columnNameDataList[i].type = (Type)EditorGUILayout.EnumPopup(columnNameDataList[i].type, GUILayout.MaxWidth(100));
                EditorGUILayout.EndHorizontal();

                if (i + 1 != columnNameDataList.Count)
                {
                    GUILayout.Space(5);
                }
            }
        }
        EditorGUILayout.EndScrollView();

        // 空白
        GUILayout.Space(10);

        // ScriptableObjectの生成場所を表示
        var scriptableObjectPath = new StringBuilder();
        scriptableObjectPath.AppendFormat(MASTERDATA_PATH_FORMAT, Path.GetFileNameWithoutExtension(filePath));
        EditorGUILayout.LabelField("ScriptableObjectの作成場所");
        EditorGUILayout.BeginHorizontal(GUI.skin.box);
        EditorGUILayout.LabelField(scriptableObjectPath.ToString());
        EditorGUILayout.EndHorizontal();

        // 空白
        GUILayout.Space(10);

        // ScriptableObjectのデータクラスの生成場所を表示
        var dataFilePath = new StringBuilder();
        dataFilePath.AppendFormat(DATAFILE_PATH_FORMAT, Path.GetFileNameWithoutExtension(filePath));
        EditorGUILayout.LabelField("データクラスの作成場所");
        EditorGUILayout.BeginHorizontal(GUI.skin.box);
        EditorGUILayout.LabelField(dataFilePath.ToString());
        EditorGUILayout.EndHorizontal();

        // 空白
        GUILayout.Space(10);

        // ScriptableObjectの管理者の生成場所を表示        
        var operatorPath = new StringBuilder();
        operatorPath.AppendFormat(OPERATOR_PATH_FORMAT, Path.GetFileNameWithoutExtension(filePath));
        EditorGUILayout.LabelField("ScriptableObjectの管理者の作成場所");
        EditorGUILayout.BeginHorizontal(GUI.skin.box);
        EditorGUILayout.LabelField(operatorPath.ToString());
        EditorGUILayout.EndHorizontal();

        // 空白
        GUILayout.Space(10);

        // 作成ボタンを表示
        if(GUILayout.Button("作成",GUILayout.Height(30)))
        {
            // ScriptableObject と 管理用スクリプトの作成
            ExportScriptableObjectData(dataFilePath.ToString());
            ExportDataOperator(operatorPath.ToString(), filePath);
        }

        // ウィンドウ全体をスクロールの終了
        EditorGUILayout.EndScrollView();
    }

    #endregion

    /// <summary> 型の種類 </summary>
    enum Type
    {
        Int = 0,
        Uint,
        Float,
        Double,
        String,
    }

    /// <summary> カラム名の情報 </summary>
    private class ColumnNameData
    {
        public string columName; // カラム名
        public Type type;        // 型の種類
    }

    /// <summary>
    /// カラム名の情報一覧を取得
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private void GetColumNameDataList(string path)
    {
        // パスをもとにcsvファイルを読み込む
        string csvtext = string.Empty;

        try
        {
            csvtext = File.ReadAllText(path);
        }
        catch (System.Exception)
        {
            Debug.LogError("csvファイルが読み込めませんでした。\npath = " + path);
            columnNameDataList = null;
            return;
        }

        // 初期化
        var stringReader = new StringReader(csvtext);

        // 最後の行までループするが、最大カラム名+3行まで読む
        int readCount = 0;
        while (stringReader.Peek() > -1)
        {
            // 一行ずつ取り出す
            var line = stringReader.ReadLine();

            // 改行コード削除
            line = line.Replace("\n", "").Replace("¥r", "");

            // CSV_KEY_SYMBOL が入っている行がスタート位置なのでそこまで検索
            if (columnNameDataList.Count == 0)
            {
                if (!line.Contains(CSV_KEY_SYMBOL))
                {
                    continue;
                }
                else
                {
                    // カラム名の設定
                    // 始めに##を削除
                    line = line.Replace(CSV_KEY_SYMBOL, "");

                    // カンマ区切りで分割
                    var names = line.Split(',');

                    // ColumNameDataのリストに値を追加していく
                    foreach (var name in names)
                    {
                        // typeは次の行から読み込むが、一応Stringにしておく
                        var data = new ColumnNameData();
                        data.columName = name;
                        data.type = Type.String;

                        columnNameDataList.Add(data);
                    }

                    // 次から最大3行調べて、カラム名にあった型を調べる
                    readCount = 0;
                }
            }

            // コメントアウト行ならその行は読み込まない
            if (line.Contains(CSV_IGNORE_KEY))
            {
                continue;
            }

            // カンマ区切りで分割
            var cells = line.Split(',');
            if (cells.Length == 0)
            {
                continue;
            }

            // ここから値をみるよー
            // 数字に変換できるならint or float型、出来なければstring型として判断する
            for (int i = 0; i < columnNameDataList.Count; i++)
            {
                if (cells.Length <= i)
                {
                    break;
                }

                var nameData = columnNameDataList[i];
                var cell = cells[i];

                // string型以外と判断したなら確認しない
                if (nameData.type != Type.String)
                {
                    continue;
                }

                // int型
                {
                    int value;
                    if (int.TryParse(cell, out value))
                    {
                        nameData.type = Type.Int;
                        continue;
                    }
                }
                // float型
                {
                    float value;
                    if (float.TryParse(cell, out value))
                    {
                        nameData.type = Type.Float;
                        continue;
                    }
                }
            }

            // カウントが3なら終了
            readCount++;
            if (readCount == 3)
            {
                break;
            }
        }

        if (columnNameDataList.Count == 0)
        {
            Debug.LogWarning("カラム名を一つも取得できませんでした");
        }
    }

    /// <summary>
    /// ScriptableObject用のデータファイル(.cs)を生成するよ
    /// </summary>
    /// <param name="exportPath"></param>
    private void ExportScriptableObjectData(string exportPath)
    {
        // template用のテキストファイルを読み込む
        string templateText = string.Empty;

        try
        {
            templateText = File.ReadAllText(DETAFILE_TEMPLATE_PATH);
        }
        catch (System.Exception)
        {
            Debug.LogError("ScriptableObjectのtemplateファイルが読み込めませんでした。\npath = " + DETAFILE_TEMPLATE_PATH);
            return;
        }

        // 中に入れるデータの準備をする
        var dataBuilder = new StringBuilder();
        var tab = "        ";
        for (var i = 0; i < columnNameDataList.Count; i++)
        {
            var data = columnNameDataList[i];

            // 型に合わせて変数を用意する
            switch (data.type)
            {
                case Type.Int:
                    dataBuilder.AppendFormat("{0}public int {1};", tab, data.columName);
                    break;

                case Type.Uint:
                    dataBuilder.AppendFormat("{0}public uint {1};", tab, data.columName);
                    break;

                case Type.Float:
                    dataBuilder.AppendFormat("{0}public float {1};", tab, data.columName);
                    break;

                case Type.Double:
                    dataBuilder.AppendFormat("{0}public double {1};", tab, data.columName);
                    break;

                case Type.String:
                    dataBuilder.AppendFormat("{0}public string {1};", tab, data.columName);
                    break;
            }

            // 改行
            if (i + 1 < columnNameDataList.Count)
            {
                dataBuilder.AppendLine();
            }
        }

        // テンプレートに合わせてテンプレートの中身にテキストを入力
        {
            // クラス名( = ファイル名)
            templateText = templateText.Replace("$CLASS_NAME$", Path.GetFileNameWithoutExtension(exportPath));

            // インナークラスのデータ
            templateText = templateText.Replace("$DATA_VALUE$", dataBuilder.ToString());
        }

        // 編集したテキストを.csファイルにする
        // 既にファイルが作成済みなら上書き保存
        // なければ新規作成を自動でしてくれる(はず)
        var exportCSFilePath = new StringBuilder();
        exportCSFilePath.Append(exportPath);
        Directory.CreateDirectory(Path.GetDirectoryName(exportCSFilePath.ToString()));
        File.WriteAllText(exportCSFilePath.ToString(), templateText, Encoding.UTF8);

        //　Unity側でアセットの更新
        AssetDatabase.ImportAsset(exportCSFilePath.ToString());
    }

    /// <summary>
    /// ScriptableObjectを管理する.csファイルを生成
    /// </summary>
    /// <param name="exportPath"></param>
    private void ExportDataOperator(string exportPath, string csvFilePath)
    {
        // テンプレート用のテキストファイルを読み込む
        string templateText = string.Empty;

        try
        {
            templateText = File.ReadAllText(OPERATOR_TEMPLATE_PATH);
        }
        catch (System.Exception)
        {
            Debug.LogError("Operatorのtemplateファイルが読み込めませんでした。\npath = " + OPERATOR_TEMPLATE_PATH);
            return;
        }

        // 中に入れるデータの準備をする
        var dataBuilder = new StringBuilder();

        for (var i = 0; i < columnNameDataList.Count; i++)
        {
            var data = columnNameDataList[i];
            var format = string.Empty;

            // 型にキャストするようにする
            switch (data.type)
            {
                case Type.Int:
                    format = "                data.{0} = int.Parse(cells[{1}]);";
                    break;

                case Type.Uint:
                    format = "                data.{0} = uint.Parse(cells[{1}]);";
                    break;

                case Type.Float:
                    format = "                data.{0} = float.Parse(cells[{1}]);";
                    break;

                case Type.Double:
                    format = "                data.{0} = double.Parse(cells[{1}]);";
                    break;

                case Type.String:
                    format = "                data.{0} = cells[{1}];";
                    break;
                default:
                    break;
            }

            dataBuilder.AppendFormat(format, data.columName, i);
            dataBuilder.AppendLine();
        }

        // テンプレートに合わせてテンプレートの中身にテキストを入力
        {
            var tempBuilder = new StringBuilder();
            // クラス名( = ファイル名)
            templateText = templateText.Replace("$CLASS_NAME$", Path.GetFileNameWithoutExtension(exportPath));

            // データクラスのクラス名
            tempBuilder.AppendFormat(DATAFILE_PATH_FORMAT, Path.GetFileNameWithoutExtension(csvFilePath));
            templateText = templateText.Replace("$DATA_CLASS_NAME$", Path.GetFileNameWithoutExtension(tempBuilder.ToString()));

            // csvファイルのpath
            templateText = templateText.Replace("$CSV_FILE_PATH$", csvFilePath);

            // ScriptableObjectのPath
            tempBuilder.Clear();
            tempBuilder.AppendFormat(MASTERDATA_PATH_FORMAT, Path.GetFileNameWithoutExtension(csvFilePath));
            templateText = templateText.Replace("$SCRIPTABLE_OBJECT_EXPORT_PATH$", tempBuilder.ToString());

            // データ挿入部分
            templateText = templateText.Replace("$EXPORT_DATA$", dataBuilder.ToString());
        }

        // 編集したテキストを.csファイルにする
        // 既にファイルが作成済みなら上書き保存
        // なければ新規作成を自動でしてくれる(はず)
        var exportCSFilePath = new StringBuilder();
        exportCSFilePath.Append(exportPath);
        Directory.CreateDirectory(Path.GetDirectoryName(exportCSFilePath.ToString()));
        File.WriteAllText(exportCSFilePath.ToString(), templateText, Encoding.UTF8);

        //　Unity側でアセットの更新
        AssetDatabase.ImportAsset(exportCSFilePath.ToString());
    }
}
