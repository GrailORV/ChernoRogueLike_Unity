using System.Collections.Generic;

/// (summary)
/// ウィンドウのタイプ<Enum>とパス<string>を管理するクラス
/// (summary)
public static class WindowData
{
	public enum WindowType
	{
		InventoryWindow,
		ItemWindow,
		PotItemWindow,
		SampleWindowPanel,
	}

	public readonly static Dictionary<WindowType, string> WindowPathDict = new Dictionary<WindowType, string>()
	{
		{ WindowType.InventoryWindow , "Prefabs/Scene/Item/InventoryWindow" },
		{ WindowType.ItemWindow , "Prefabs/Scene/Item/ItemWindow" },
		{ WindowType.PotItemWindow , "Prefabs/Scene/Item/PotItemWindow" },
		{ WindowType.SampleWindowPanel , "Prefabs/Scene/SampleWindowManager/SampleWindow(Panel)" },
	};
}
