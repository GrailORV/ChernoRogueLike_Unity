using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// プレイヤークラス
/// </summary>
public class PlayerController : Pawn
{
	private static readonly string[] BodySpriteNames = {
		"back",
		"front",
		"right",
		"left"
	};

	private Sprite[] _bodySprites;

	/// <summary>
	/// 
	/// </summary>
	/// <returns></returns>
	static public PlayerController CreatePlayer( string spritePath )
	{
		// 本体オブジェクト作成
		GameObject root = new GameObject( "Player", typeof(PlayerController) );
		PlayerController result = root.GetComponent<PlayerController>();
		result.Position = Vector2Int.zero;

		{ // 描画用オブジェクト作成
			GameObject bodyObj = new GameObject( "BodyRenderer", typeof(SpriteRenderer) );
			bodyObj.transform.parent = root.transform;
			bodyObj.transform.localPosition = new Vector3( 0.0f, 0.0f, Dungeon.DungeonObjZOrder.PawnObjZ );

			SpriteRenderer renderer = bodyObj.GetComponent<SpriteRenderer>();
			Sprite[] sprites = Resources.LoadAll<Sprite>( spritePath );
			result._bodySprites = new Sprite[(int)Direction.MAX];
			for( int i = 0; i < (int)Direction.MAX; i++ )
			{
				result._bodySprites[i] = System.Array.Find( sprites, (s) => s.name.Equals( BodySpriteNames[i] ) );
			}
			renderer.sprite = result._bodySprites[(int)Direction.South];
		}

		{ // 影描画用オブジェクト
			GameObject shadowObj = new GameObject( "ShadowRenderer", typeof(SpriteRenderer) );
			shadowObj.transform.parent = root.transform;
			shadowObj.transform.localPosition = new Vector3( 0.0f, 0.0f, Dungeon.DungeonObjZOrder.ShadowObjZ );

			result.LoadShadowSprite();

			SpriteRenderer renderer = shadowObj.GetComponent<SpriteRenderer>();
			renderer.sprite = result._shadowSprite;
		}

		return result;
	}
}
