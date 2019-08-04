using System.Collections.Generic;

/// (summary)
/// ウィンドウのタイプ<Enum>とパス<string>を管理するクラス
/// (summary)
public static class WindowData
{
	public enum WindowType
	{
		SampleWindowPanel,
	}

	public readonly static Dictionary<int, string> windowPathDict = new Dictionary<int, string>()
	{
		{ 0 , "Prefabs/Scene/SampleWindowManager/SampleWindow(Panel)" },
	};
}
