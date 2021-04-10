using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャラクター基底クラス
/// </summary>
public class Pawn : MonoBehaviour
{
	protected enum Direction
	{
		North = 0,
		South,
		East,
		West,
		MAX
	}

	/// <summary>マップ上の位置</summary>
	public Vector2Int Position {
		get{ return _position; }
		set{ _position = value; transform.position = Dungeon.DungeonUtility.transDungeonPosToWorldPos( value ); }
		}

	/// <summary>影用スプライト</summary>
	protected Sprite _shadowSprite;

	/// <summary>デフォルト影テクスチャ</summary>
	static protected string DefaultShadowPath = "Textures/shadow";

	protected Vector2Int	_position;
	protected Direction		_direction;

	/// <summary>
	/// 影スプライト読み込み
	/// </summary>
	/// <param name="shadowPath">nullもしくは空欄の場合デフォルトの影テクスチャが読み込まれる。</param>
	/// <returns></returns>
	protected bool LoadShadowSprite( string shadowPath = null )
	{
		if( string.IsNullOrEmpty(shadowPath) )
		{
			shadowPath = DefaultShadowPath;
		}

		_shadowSprite = Resources.Load<Sprite>( shadowPath );
		if( _shadowSprite == null )
		{
			Debug.LogFormat( "{0} : 影テクスチャ読み込み失敗。", shadowPath );
			return false;
		}

		return true;
	}

}
