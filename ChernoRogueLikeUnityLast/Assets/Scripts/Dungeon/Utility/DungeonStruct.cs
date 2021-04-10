using UnityEngine;

namespace Dungeon
{
	/// <summary>
	/// ダンジョンマップ１マスに含まれるデータ
	/// </summary>
	public struct DungeonCellData
	{
		public int terrain;		// 地形データ
		public int item;		// アイテムデータ
		public int eve;			// イベントデータ
		public Pawn Chara;		// UE4のPawnクラスのようなもの、nullじゃないとき何らかのキャラがそのマスにいる
	}

	/// <summary>
	/// ダンジョン初期化用記述子
	/// </summary>
	public struct DUNGEON_DESC
	{

		public int			story;		// 階数
		public MAP_INDEX_DESC[]	mapDescs;	// マップ生成記述子

		public Vector2Int[]	sizeTable;				// マップサイズテーブル
		public string[]		terrainSpriteNameTable;	// 地形スプライト名テーブル
		public string[]		itemSpriteNameTable;	// アイテムスプライト名テーブル
		public string[]		eventSpriteNameTable;	// イベントスプライト名テーブル

		public int[][,]		terrainMapTable;	// 固定地形データテーブル
		public int[][,]		itemMapTable;		// 固定アイテムデータテーブル
		public int[][,]		eventMapTable;		// 固定イベントデータテーブル
	}

	/// <summary>
	/// マップデータのインデックス構造体
	/// </summary>
	public struct MAP_INDEX_DESC
	{
		public int sizeIndex;
		public int terrainSpriteIndex;
		public int itemSpriteIndex;
		public int eventSpriteIndex;

		// 固定マップ用（-1の時ランダム）
		public int terrainMapIndex;
		public int itemMapIndex;
		public int eventMapIndex;
	}

	/// <summary>
	/// マップ初期化用記述子
	/// </summary>
	public struct MAP_CREATE_DESC
	{
		// 必須
		public int		width;			// 幅
		public int		height;			// 高さ
		public Sprite[]	terrainSprites;	// 地形データスプライト
		public Sprite[]	itemSprites;	// アイテムスプライト
		public Sprite[]	eventSprites;	// イベントスプライト
		public bool		isFixedMap;		// 固定マップか？

		// 固定マップ用変数
		public int[,] terrainData;	// 地形データ
		public int[,] itemData;		// アイテムデータ
		public int[,] eventData;	// イベントデータ

		// ランダムマップ用変数
	}
}
