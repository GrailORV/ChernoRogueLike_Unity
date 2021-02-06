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
		TYPE_MAX
	}

	/// <summary>
	/// マップイベント列挙子
	/// </summary>
	public enum EventType
	{
		None = 0,
		Stair
	}
}
