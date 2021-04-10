using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeon
{
	/// <summary>
	/// マップ地形列挙子
	/// </summary>
	public enum TerrainType
	{
		Wall = 0,
		Floor,
		Water,

		Count
	}

	/// <summary>
	/// マップアイテム列挙子
	/// </summary>
	public enum ItemType
	{
		None = -1,
		debug_item_0,

		Count
	}

	/// <summary>
	/// マップイベント列挙子
	/// </summary>
	public enum EventType
	{
		None = -1,
		Stair,

		Count
	}
}
