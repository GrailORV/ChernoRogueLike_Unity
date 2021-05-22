using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DramaWindow : WindowBase
{
    /// <summary>
    /// ドラマの状態
    /// </summary>
    public enum DramaState
    {
        None,       // ドラマを再生していない
        Play,       // 再生中
        WaitNext,   // テキストが最後まで表示されて次のテキストを表示or終了を待っている
        //MoveNext,   // 次のテキストへ移行させる
    }

    // タグの開始と終了のどちらも取得するための正規表現(string.Format用)
    const string REGEX_FORMAT_TAG_BASE = @"</?({0})(=.*?)?>";

    // タグの開始と終了のどちらも取得するための正規表現
    const string REGEX_TAG_BASE = @"<[^>]+>";

    readonly List<DramaTagData> tagDataList = new List<DramaTagData>()
    {
        new DramaTagData(DramaTagData.TagType.Speed,"speed","sp","speed|sp"),
    };

    /// <summary>
    /// 文字送りのスピード 1秒で何文字表示するか
    /// </summary>
    [SerializeField] float BASE_TEXT_SPEED = 4f;

    // 本文
    [SerializeField] TextMeshProUGUI mainText;

    // ドラマデータのキャッシュ
    Dictionary<int, Dictionary<int, DramaData>> dramaDataCache = new Dictionary<int, Dictionary<int, DramaData>>();

    /// <summary>
    /// ドラマの状態
    /// </summary>
    public DramaState CurrentState { get; private set; }

    /// <summary>
    /// ドラマが再生中かどうか
    /// </summary>
    public bool IsPlaying { get { return CurrentState >= DramaState.Play; } }

    /// <summary>
    /// 現在のドラマコード
    /// </summary>
    public int CurrentCode { get; private set; }

    /// <summary>
    /// 現在のドラマインデックス
    /// </summary>
    public int CurrentIndex { get; private set; }


    /// <summary>
    /// 一文づつのデータ
    /// </summary>
    public class DramaData
    {
        public int index;
        public string text;
        public string parsedText;
        public float baseSpeed;
        public List<float> SpeedDatas;

        public int effectId;
        public List<int> choice;
    }

    protected override void Awake()
    {
        base.Awake();

        // ドラマ用マスタデータの読み込み
        DramaTableHelper.Load();
    }

    public override void OnEnable()
    {
        base.OnEnable();

        // Debug
        PlayDrama(1);
    }

    /// <summary>
    /// ドラマの再生
    /// </summary>
    /// <param name="code">ドラマ毎のコード</param>
    /// <param name="index">ドラマの会話の開始位置</param>
    public void PlayDrama(int code, int index = 0)
    {
        // 再生中なら何もしない
        if(IsPlaying)
        {
            Debug.LogErrorFormat("ドラマが再生中です\n【再生中: code = {0} index = {1}】【要求:code = {2} index = {3}】", CurrentCode, CurrentIndex, code, index);
            return;
        }

        var dramaDatas = GetDramaData(code);

        if(dramaDatas == null)
        {
            Debug.LogErrorFormat("ドラマのデータが取得できませんでした。マスタデータが最新か確認してください。 code = {0}", code);
            return;
        }

        // 再生
        CurrentState = DramaState.Play;
        CurrentCode  = code;
        CurrentIndex = index;

        StartCoroutine(PlayDrama(dramaDatas));
    }

    /// <summary>
    /// ドラマの再生(非同期)
    /// </summary>
    /// <param name="dramaDatas"></param>
    /// <returns></returns>
    IEnumerator PlayDrama(Dictionary<int, DramaData> dramaDatas)
    {
        Debug.LogError("<color=yellow>code:" + CurrentCode + "ドラマ開始</color>");

        // indexを探してそこから回していく
        while (dramaDatas.ContainsKey(CurrentIndex))
        {
            // テキストの表示開始
            yield return ShowText(dramaDatas[CurrentIndex]);

            // 最後までテキストが表示されたら待機の状態に移行
            CurrentState = DramaState.WaitNext;

            // 入力待機中
            // CurrentState == DramaState.Play になって抜ける想定
            while (CurrentState == DramaState.WaitNext)
            {
                yield return null;
            }

            // 次のテキストへ
            CurrentIndex++;
        }

        // ドラマ終了
        CurrentState = DramaState.None;
        Debug.LogError("<color=yellow>code:" + CurrentCode + "ドラマ終了</color>");

        PlayDrama(1);
    }

    /// <summary>
    /// テキストの表示(非同期)
    /// </summary>
    /// <param name="dramaData"></param>
    /// <returns></returns>
    IEnumerator ShowText(DramaData dramaData)
    {
        // indexの更新
        CurrentIndex = dramaData.index;

        // テキストを設定
        mainText.text = dramaData.parsedText;
        mainText.ForceMeshUpdate();
        mainText.maxVisibleCharacters = 0;

        var length = mainText.GetParsedText().Length;
        var count = 0;

        var speed = 1 / BASE_TEXT_SPEED;

        while (count < length)
        {
            // テキスト表示中に入力されたら最後まで一気に表示させる
            if (CurrentState == DramaState.WaitNext)
            {
                count = length;
                mainText.maxVisibleCharacters = count;
                continue;
            }

            speed = 1 / dramaData.SpeedDatas[count];
            yield return new WaitForSeconds(speed);

            count++;
            mainText.maxVisibleCharacters = count;
        }
    }

    /// <summary>
    /// ドラマデータの取得
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    Dictionary<int, DramaData> GetDramaData(int code)
    {
        // キャッシュにあるか確認
        if (dramaDataCache.ContainsKey(code))
        {
            return dramaDataCache[code];
        }

        // ないならマスターデータから取得
        var dataDict = DramaTableHelper.GetDramaData(code);

        if (dataDict == null)
        {
            return null;
        }

        // 型が違うので変換(DramaTableData.Data → DramaData)
        var dramas = new Dictionary<int, DramaData>();

        foreach (var data in dataDict.Values)
        {
            var drama = new DramaData();
            drama.index      = data.index;
            drama.text       = data.text;
            drama.baseSpeed  = data.base_speed;
            drama.SpeedDatas = new List<float>();
            drama.effectId   = data.effect;

            // テキストの解析
            ParseText(drama);

            dramas.Add(drama.index, drama);
        }

        // キャッシュに保存
        dramaDataCache.Add(code, dramas);

        return dramas;
    }

    #region TextAnalytics

    /// <summary>
    /// テキストを解析する
    /// </summary>
    /// <param name="data"></param>
    void ParseText(DramaData data)
    {
        // TextMeshProで使用できるタグの反映後のテキストを取得する
        mainText.SetText(data.text);
        mainText.ForceMeshUpdate();

        var text = mainText.GetParsedText();

        // 文字送りの速度情報
        var sendTextSpeedList = new List<float>() { data.baseSpeed };

        var count = 0;

        while (count < text.Length)
        {
            var str = text.Substring(count, 1);

            // タグかどうか確認
            if (str == "<")
            {
                var tagRegex = new Regex(REGEX_TAG_BASE);

                if (!tagRegex.IsMatch(text, count))
                {
                    // 速度データの設定
                    data.SpeedDatas.Add(sendTextSpeedList.Last());

                    count++;
                    continue;
                }

                // タグ情報の取得
                var match = tagRegex.Match(text, count);
                var tagData = GetTagData(match.Value);

                if (tagData == null)
                {
                    // 速度データの設定
                    data.SpeedDatas.Add(sendTextSpeedList.Last());

                    count++;
                    continue;
                }

                // 終了タグかどうかの確認
                var isEndTag = match.Value.Contains("</");

                // タグ一覧からタグが取得できればタグによって処理を行う
                switch (tagData.tagType)
                {
                    case DramaTagData.TagType.Speed:
                        SetSpeedData(sendTextSpeedList, match.Value, isEndTag);
                        break;
                    default:
                        break;
                }

                // タグの分カウントをスキップする
                count += match.Value.Length;
                continue;
            }

            // 速度データの設定
            data.SpeedDatas.Add(sendTextSpeedList.Last());

            count++;
        }

        // テキストの中からMeshPro用テキスト以外のタグを削除
        data.parsedText = DeleteDramaTag(data.text);

        // 改行コードの修正
        data.parsedText = data.parsedText.Replace("\\n","\n");
    }

    /// <summary>
    /// テキストからドラマタグを削除する
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    string DeleteDramaTag(string text)
    {
        for (int i = 0; i < (int)DramaTagData.TagType.All; i++)
        {
            var type = (DramaTagData.TagType)i;

            if (type == DramaTagData.TagType.All)
            {
                continue;
            }

            var tagData = GetTagData(type);

            if (tagData == null)
            {
                continue;
            }

            // タグを削除
            var pattern = string.Format(REGEX_FORMAT_TAG_BASE, tagData.allTag);

            Debug.Log("pattern:" + pattern);

            foreach (Match match in Regex.Matches(text, pattern))
            {
                Debug.Log("value:" + match.Value);
            }

            text = Regex.Replace(text, pattern, string.Empty);
        }

        return text;
    }

    /// <summary>
    /// タグデータの取得
    /// </summary>
    /// <param name="tagType"></param>
    /// <returns></returns>
    DramaTagData GetTagData(DramaTagData.TagType tagType)
    {
        return tagDataList.Find(data => data.tagType == tagType);
    }

    /// <summary>
    /// タグデータの取得
    /// </summary>
    /// <param name="tagType"></param>
    /// <returns></returns>
    DramaTagData GetTagData(string tag)
    {
        return tagDataList.Find(data => Regex.IsMatch(tag,string.Format(REGEX_FORMAT_TAG_BASE, data.allTag)));
    }

    /// <summary>
    /// 速度データの設定
    /// </summary>
    /// <param name="text"></param>
    void SetSpeedData(List<float> speedList, string tagText, bool isEndTag)
    {
        // 終了タグなら速度を一つ前に戻す
        if (isEndTag && speedList.Count > 1)
        {
            speedList.RemoveAt(speedList.Count - 1);
            return;
        }

        // テキストから値を取得
        var match = Regex.Match(tagText, string.Format(@"(?<=<({0})=).*(?=>)",GetTagData(DramaTagData.TagType.Speed).allTag));

        var speed = 0f;
        if (match.Success && float.TryParse(match.Value, out speed))
        {
            if (speed > 0)
            {
                speedList.Add(speed);
            }
        }
    }
    #endregion

    #region Input

    /// <summary>
    /// 決定ボタン押下
    /// </summary>
    public void OnPressDramaEnter()
    {
        // 再生中の場合はテキストを一気に表示
        if (CurrentState == DramaState.Play)
        {
            CurrentState = DramaState.WaitNext;
        }
        // テキスト表示が終わっているばあいは次を再生
        else if (CurrentState == DramaState.WaitNext)
        {
            CurrentState = DramaState.Play;
        }

        Debug.Log("入力した！ Current State = " + CurrentState);
    }

    #endregion
}
