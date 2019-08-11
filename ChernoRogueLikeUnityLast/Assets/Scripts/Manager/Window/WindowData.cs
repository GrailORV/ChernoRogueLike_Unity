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

	public readonly static Dictionary<WindowType, string> WindowPathDict = new Dictionary<WindowType, string>()
	{
		{ WindowType.SampleWindowPanel , "Prefabs/Scene/SampleWindowManager/SampleWindow(Panel)" },
	};
}
