using System;

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
		//public Pawn Chara;	// UE4のPawnクラスのようなもの、nullじゃないとき何らかのキャラがそのマスにいる
	}

	/// <summary>
	/// マップ初期化用記述子
	/// </summary>
	public struct MAP_CREATE_DESC
	{
		public int width;			// 幅
		public int height;			// 高さ
		public int[,] terrainData;	// 地形データ
		public int[,] itemData;		// アイテムデータ
		public int[,] eventData;	// イベントデータ

		public string terrainSpriteFileName;	// 地形データスプライトファイル名
	}
}
