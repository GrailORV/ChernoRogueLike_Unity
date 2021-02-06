#define MAP_CREATE_TEST

using System;
using System.IO;
using UnityEngine;

namespace Dungeon
{
	/// <summary>
	/// マップ生成マネージャー
	/// </summary>
	public class MapManager : SingletonMonoBehaviour<MapManager>
	{
		/// <summary>初期化済みか</summary>
		private bool	_bInitialized = false;
		/// <summary>マップサイズ</summary>
		private int		_width = 0, _height = 0;
		/// <summary>マップデータ</summary>
		private DungeonCellData[,]	_mapData = null;

		private Sprite[]	terrainSprites = null;

		private GameObject[,] terrainObjects	= null;
		private GameObject[,] itemObjects		= null;
		private GameObject[,] eventObjects		= null;

		/// <summary>地形スプライト読み込み用名前</summary>
		private readonly string[] TerrainSpriteNames =
		{
			"wall",
			"floor",
			"water"
		};

		/// <summary>
		/// 
		/// </summary>
		private void Start()
		{
			#if MAP_CREATE_TEST
			MAP_CREATE_DESC desc = new MAP_CREATE_DESC();
			desc.width			= DebugmapData.Width;
			desc.height			= DebugmapData.Height;
			desc.terrainData	= DebugmapData.TerrainData;
			desc.itemData		= null;
			desc.eventData		= null;
			desc.terrainSpriteFileName = DebugmapData.TerrainSpriteFile;

			StartUp( desc );
			#endif
		}

		/// <summary>
		/// 更新処理
		/// </summary>
		private void Update()
		{
			if( !_bInitialized )
			{
				return;
			}

			// 更新処理
			// ここでスタックした各種更新処理を呼び出す（予定）
		}

		/// <summary>
		/// 各種初期化処理
		/// </summary>
		/// <param name="mapDesc">マップ初期化記述子</param>
		/// <returns></returns>
		public bool StartUp( MAP_CREATE_DESC mapDesc )
		{
			if( _bInitialized ) return false;

			// データ設定
			{
				// マップサイズ設定
				_width = mapDesc.width;
				_height = mapDesc.height;
				_mapData = new DungeonCellData[_height, _width];

				// スプライトのロード
				Sprite[] terrainSpriteBuf = Resources.LoadAll<Sprite>( mapDesc.terrainSpriteFileName );
				terrainSprites = new Sprite[(int)TerrainType.TYPE_MAX];
				for( int i = 0; i < (int)TerrainType.TYPE_MAX; i++ )
				{
					terrainSprites[i] = System.Array.Find<Sprite>( terrainSpriteBuf, (sprite) => sprite.name.Equals(TerrainSpriteNames[i]) );
				}

				// 固定マップの場合受け取ったデータで初期化
				//if( mapDesc.)			← ここで固定マップ化の判定
				{
					// 少なくとも地形データがないと初期化できない
					if( mapDesc.terrainData == null ) return false;

					// 指定のサイズ未満であれば抜ける
					if( mapDesc.terrainData.GetLength(0) < _height || mapDesc.terrainData.GetLength(1) < _width ) return false;
					if( mapDesc.itemData != null && ( mapDesc.itemData.GetLength(0) < _height || mapDesc.itemData.GetLength(1) < _width ) ) return false;
					if( mapDesc.eventData != null && ( mapDesc.eventData.GetLength(0) < _height || mapDesc.eventData.GetLength(1) < _width ) ) return false;

					for( int y = 0; y < _height; y++ )
					{
						for( int x = 0; x < _width; x++ )
						{
							int t = mapDesc.terrainData[y,x];
							int i = mapDesc.itemData == null ? 0 : mapDesc.itemData[y,x];
							int e = mapDesc.eventData == null ? 0 : mapDesc.eventData[y,x];

							_mapData[y,x].terrain	= t;
							_mapData[y,x].item		= i;
							_mapData[y,x].eve		= e;
						}
					}
				}

			}

			// マップオブジェクト生成
			{
				terrainObjects	= new GameObject[_height, _width];
				itemObjects		= new GameObject[_height, _width];
				eventObjects	= new GameObject[_height, _width];

				// ルートオブジェクト生成
				GameObject terrainRoot = new GameObject( "TerrainRoot" );
				terrainRoot.transform.parent = this.transform;
				GameObject itemRoot = new GameObject( "ItemRoot" );
				itemRoot.transform.parent = this.transform;
				GameObject eventRoot = new GameObject( "EventRoot" );
				eventRoot.transform.parent = this.transform;

				for( int y = 0; y < _height; y++ )
				{
					for( int x = 0; x < _width; x++ )
					{
						// 地形オブジェクト生成
						string terrainName = String.Format( "t_{0}{1}", y, x );
						GameObject terrainObj = new GameObject( terrainName );
						terrainObj.transform.position = new Vector3( x, -y, 0 );
						terrainObj.transform.parent = terrainRoot.transform;
						var terrainRenderer = terrainObj.AddComponent<SpriteRenderer>();
						terrainRenderer.sprite = terrainSprites[mapDesc.terrainData[y,x]];

						terrainObjects[y,x] = terrainObj;
					}
				}
			}

			// 初期化完了
			_bInitialized = true;

			return true;
		}

		/// <summary>
		/// マップクリア
		/// </summary>
		public void Clear()
		{
			if( !_bInitialized ) return;

			// 各種パラメータ初期化
			_width = _height	= 0;
			_mapData			= null;
			_bInitialized		= false;

		}
	}
}
