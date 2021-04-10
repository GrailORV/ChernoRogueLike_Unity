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

		private Sprite[]	_terrainSprites	= null;
		private Sprite[]	_itemSprites	= null;
		private Sprite[]	_eventSprites	= null;

		private GameObject[,] _terrainObjects	= null;
		private GameObject[,] _itemObjects		= null;
		private GameObject[,] _eventObjects		= null;

		#if DEBUG
		[SerializeField]
		private bool _mapCreateTest	= false;
		#endif

		#region MonoBehaviour

		/// <summary>
		/// 
		/// </summary>
		private void Start()
		{
			#if DEBUG
			if( _mapCreateTest )
			{
				MAP_CREATE_DESC desc = DebugmapData.MapDesc;
				{
					Sprite[] sprites	= Resources.LoadAll<Sprite>( DebugmapData.TerrainSpriteFile );
					desc.terrainSprites	= new Sprite[(int)TerrainType.Count];
					for ( int i = 0; i < (int)TerrainType.Count; i++ )
					{
						string name = Enum.GetName(typeof(TerrainType), i);
						desc.terrainSprites[i] = System.Array.Find<Sprite>( sprites, ( sprite ) => sprite.name.Equals( name ) );
					}
				}
				{
					Sprite[] sprites = Resources.LoadAll<Sprite>( DebugmapData.ItemSpriteFile );
					desc.itemSprites = new Sprite[(int)ItemType.Count];
					for ( int i = 0; i < (int)ItemType.Count; i++ )
					{
						string name = Enum.GetName( typeof( ItemType ), i );
						desc.itemSprites[i] = System.Array.Find<Sprite>( sprites, ( sprite ) => sprite.name.Equals( name ) );
					}
				}
				{
					Sprite[] sprites = Resources.LoadAll<Sprite>( DebugmapData.EventSpriteFile );
					desc.eventSprites = new Sprite[(int)EventType.Count];
					for ( int i = 0; i < (int)EventType.Count; i++ )
					{
						string name = Enum.GetName( typeof( EventType ), i );
						desc.eventSprites[i] = System.Array.Find<Sprite>( sprites, ( sprite ) => sprite.name.Equals( name ) );
					}
				}

				StartUp( desc );
			}
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
		}

		#endregion

		#region public func

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

				// スプライト設定
				if( mapDesc.terrainSprites == null ) return false;
				_terrainSprites	= mapDesc.terrainSprites;
				_itemSprites	= mapDesc.itemSprites;
				_eventSprites	= mapDesc.eventSprites;

				// 固定マップの場合受け取ったデータで初期化
				if( mapDesc.isFixedMap )
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
							int i = mapDesc.itemData == null ? -1 : mapDesc.itemData[y,x];
							int e = mapDesc.eventData == null ? -1 : mapDesc.eventData[y,x];

							_mapData[y,x].terrain	= t;
							_mapData[y,x].item		= i;
							_mapData[y,x].eve		= e;
						}
					}
				}
				// ランダムマップ生成
				else
				{

				}

			}

			// マップオブジェクト生成
			{
				_terrainObjects	= new GameObject[_height, _width];
				_itemObjects	= new GameObject[_height, _width];
				_eventObjects	= new GameObject[_height, _width];

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
						var data = _mapData[y,x];

						// 地形オブジェクト生成
						{
							string objName = String.Format( "t_{0}{1}", y, x );
							GameObject terrainObj = new GameObject( objName );
							terrainObj.transform.position = new Vector3( x, -y, DungeonObjZOrder.TerrainObjZ );
							terrainObj.transform.parent = terrainRoot.transform;
							var terrainRenderer = terrainObj.AddComponent<SpriteRenderer>();
							terrainRenderer.sprite = _terrainSprites[data.terrain];

							_terrainObjects[y,x] = terrainObj;
						}

						// アイテムオブジェクト生成
						if( data.item >= 0 )
						{
							string objName = String.Format( "i_{0}{1}", y, x );
							GameObject obj = new GameObject( objName );
							obj.transform.position = new Vector3( x, -y, Dungeon.DungeonObjZOrder.ItemObjZ );
							obj.transform.parent = itemRoot.transform;
							var renderer = obj.AddComponent<SpriteRenderer>();
							renderer.sprite = _itemSprites[data.item];

							_itemObjects[y,x] = obj;
						}

						// イベントアイテムオブジェクト生成
						if( data.eve >= 0 )
						{
							string objName = String.Format( "e_{0}{1}", y, x );
							GameObject obj = new GameObject( objName );
							obj.transform.position = new Vector3( x, -y, Dungeon.DungeonObjZOrder.EventObjZ );
							obj.transform.parent = eventRoot.transform;
							var renderer = obj.AddComponent<SpriteRenderer>();
							renderer.sprite = _eventSprites[data.eve];

							_eventObjects[y,x] = obj;
						}
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

		#endregion

		#region private func

		#endregion
	}
}
