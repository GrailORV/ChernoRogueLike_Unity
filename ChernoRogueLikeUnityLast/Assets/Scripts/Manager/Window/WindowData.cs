using System.Collections.Generic;

/// (summary)
/// ウィンドウのタイプ<Enum>とパス<string>を管理するクラス
/// (summary)
public static class WindowData
{
	public enum WindowType
	{
		None,

		DramaWindow,
		InventoryWindow,
		ItemDescriptionWindow,
		ItemMenuWindow,
		ItemWindow,
		PotItemWindow,
		SampleWindowPanel,
	}

	public readonly static Dictionary<WindowType, string> WindowPathDict = new Dictionary<WindowType, string>()
	{
		{ WindowType.DramaWindow , "Prefabs/Scene/Drama/DramaWindow" },
		{ WindowType.InventoryWindow , "Prefabs/Scene/Item/InventoryWindow" },
		{ WindowType.ItemDescriptionWindow , "Prefabs/Scene/Item/ItemDescriptionWindow" },
		{ WindowType.ItemMenuWindow , "Prefabs/Scene/Item/ItemMenuWindow" },
		{ WindowType.ItemWindow , "Prefabs/Scene/Item/ItemWindow" },
		{ WindowType.PotItemWindow , "Prefabs/Scene/Item/PotItemWindow" },
		{ WindowType.SampleWindowPanel , "Prefabs/Scene/SampleWindowManager/SampleWindow(Panel)" },
	};
}
