using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeon
{
	public class DungeonUtility
	{
		public static Vector3 transDungeonPosToWorldPos( Vector2Int pos, float z = 0.0f )
		{
			return new Vector3(pos.x, -pos.y, z);
		}
	}
}
