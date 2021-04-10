using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeon
{
	#if DEBUG

	/// <summary>
	/// デバッグマップデータ
	/// 生成確認用の仮データ、後でデバッグマップデータも読み込み確認用に正規のファイル形式で別に作る
	/// </summary>
	public class DebugmapData
	{
		static public readonly int Width = 10;		// 幅
		static public readonly int Height = 10;		// 高さ
		static public readonly int Story = 2;		// 階層

		/// <summary>
		/// 地形データ
		/// </summary>
		static public readonly int[,] TerrainData =
		{
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
			{ 0, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
			{ 0, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
			{ 0, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
			{ 0, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
			{ 0, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
			{ 0, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
			{ 0, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
			{ 0, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
			{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
		};

		/// <summary>
		/// アイテムデータ
		/// </summary>
		static public readonly int[,] ItemData =
		{
			{ -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
			{ -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
			{ -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
			{ -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
			{ -1, -1, -1, -1,  0, -1, -1, -1, -1, -1 },
			{ -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
			{ -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
			{ -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
			{ -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
			{ -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 }
		};

		/// <summary>
		/// イベントデータ
		/// </summary>
		static public readonly int[,] EventData =
		{
			{ -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
			{ -1,  0, -1, -1, -1, -1, -1, -1, -1, -1 },
			{ -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
			{ -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
			{ -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
			{ -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
			{ -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
			{ -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
			{ -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
			{ -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 }
		};

		static public readonly string TerrainSpriteFile	= "Textures/debug/debug_terrain_kari";
		static public readonly string ItemSpriteFile	= "Textures/debug/debug_item_kari";
		static public readonly string EventSpriteFile	= "Textures/debug/debug_event_kari";

		static public readonly MAP_CREATE_DESC MapDesc = new MAP_CREATE_DESC()
		{
			width			= Width,
			height			= Height,
			terrainSprites	= null,
			itemSprites		= null,
			eventSprites	= null,
			isFixedMap		= true,
			terrainData		= TerrainData,
			itemData		= ItemData,
			eventData		= EventData,
		};

		static public readonly MAP_INDEX_DESC MapIndexDesc = new MAP_INDEX_DESC()
		{
			sizeIndex	= 0,
			terrainSpriteIndex	= 0,
			itemSpriteIndex		= 0,
			eventSpriteIndex	= 0,

			terrainMapIndex	= 0,
			itemMapIndex	= 0,
			eventMapIndex	= 0,
		};

		static public readonly DUNGEON_DESC DungeonDesc = new DUNGEON_DESC()
		{
			story		= Story,
			mapDescs	= new MAP_INDEX_DESC[] {
				MapIndexDesc,
				MapIndexDesc
				},

			sizeTable				= new Vector2Int[] { new Vector2Int( Width, Height ) },
			terrainSpriteNameTable	= new string[] { TerrainSpriteFile },
			itemSpriteNameTable		= new string[] { ItemSpriteFile },
			eventSpriteNameTable	= new string[] { EventSpriteFile },

			terrainMapTable	= new int[][,] { TerrainData },
			itemMapTable	= new int[][,] { ItemData },
			eventMapTable	= new int[][,] { EventData },
		};
	}

	#endif
}