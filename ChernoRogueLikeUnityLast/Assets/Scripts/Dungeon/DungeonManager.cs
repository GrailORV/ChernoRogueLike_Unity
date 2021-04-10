using System;
using UnityEngine;

namespace Dungeon
{
	/// <summary>
	/// ダンジョンマネージャー
	/// </summary>
	public class DungeonManager : SingletonMonoBehaviour<DungeonManager>
	{
		private enum DungeonState
		{
			NONE,
			MAP_CREATE_BEGIN,
			MAP_CREATE,
			MAP_CREATE_END
		}

		private int _story = 0;
		private int _floor = 0;
		private MAP_INDEX_DESC[] _mapDescs = null;
		private Vector2Int[] _sizeTable;
		private Sprite[][] _terrainSpritesTable;
		private Sprite[][] _itemSpritesTable;
		private Sprite[][] _eventSpritesTable;
		private int[][,] _terrainMapTable;
		private int[][,] _itemMapTable;
		private int[][,] _eventMapTable;

		private bool			_changeDungeon = false;
		private DUNGEON_DESC	_nextDungeonDesc;
		private int				_nextFloor = -1;

		private MapManager		_mapManager = null;
		private DungeonState	_dungeonState = DungeonState.NONE;

		#if DEBUG
		[SerializeField]
		private bool _dungeonTest	= false;
		#endif

		#region MonoBehaviour Func

		/// <summary>
		/// MonoBehaviour Start
		/// </summary>
		private void Start()
		{
			// マップマネージャーの取得
			_mapManager = MapManager.Instance;
			_mapManager.transform.parent = this.transform;

			_dungeonState = DungeonState.NONE;

			#if DEBUG
			if( _dungeonTest )
			{
				ChangeDungeon( DebugmapData.DungeonDesc );
			}
			#endif

		}

		/// <summary>
		/// MonoBehaviour Update
		/// </summary>
		private void Update()
		{
			switch( _dungeonState )
			{
			case DungeonState.MAP_CREATE_BEGIN:	// マップ生成開始まで待機
				if( !IsTransition() )
				{
					// マップ生成開始
					_dungeonState = DungeonState.MAP_CREATE;
				}
				break;
			case DungeonState.MAP_CREATE:       // マップ生成
				{
					CreateMap();
				}
				break;
			case DungeonState.MAP_CREATE_END:	// マップ生成完了から操作可能まで待機
				if( !IsTransition() )
				{
					// 操作開始
					_dungeonState = DungeonState.NONE;
				}
				break;
			default:
				break;
			}
		}

		#endregion

		#region public Func

		/// <summary>
		/// ダンジョンの変更
		/// </summary>
		/// <param name="desc">次のダンジョン情報</param>
		/// <returns>設定の成否</returns>
		public bool ChangeDungeon( DUNGEON_DESC desc )
		{
			// 不適切な設定があった場合失敗
			if( desc.story <= 0 )
			{
				Debug.LogErrorFormat( "DungeonManager.ChangeDungeon() : 不適切な階層数{0}が指定されました。", desc.story );
				return false;
			}
			if( desc.mapDescs == null || desc.mapDescs.Length < desc.story )
			{
				Debug.LogErrorFormat( "DungeonManager.ChangeDungeon() : マップデータのインデックス配列がnull、もしくは要素数が不足しています。" );
				return false;
			}

			// 次のダンジョン情報の設定
			_nextDungeonDesc = desc;
			_nextFloor = 0;
			_changeDungeon = true;

			_dungeonState = DungeonState.MAP_CREATE_BEGIN;

			return true;
		}

		/// <summary>
		/// フロアの変更
		/// </summary>
		/// <param name="floor">次の階層</param>
		/// <returns>設定の成否</returns>
		public bool ChangeFloor( int floor )
		{
			if( floor <= 0 || _story < floor )
			{
				Debug.LogErrorFormat( "DungeonManager.ChangeFloor() : 不適切な階{0}が指定されました。", floor );
				return false;
			}

			// 次の階層を設定
			_nextFloor = floor;

			// ステートをマップ生成開始待機に変更
			_dungeonState = DungeonState.MAP_CREATE_BEGIN;

			return true;
		}

		#endregion

		#region private Func

		/// <summary>
		/// 遷移中か？
		/// </summary>
		/// <returns></returns>
		private bool IsTransition()
		{
			return false;
		}

		/// <summary>
		/// マップ生成
		/// </summary>
		/// <returns></returns>
		private bool CreateMap()
		{
			// ダンジョン情報の設定
			if( _changeDungeon )
			{

				// 地形スプライトロード
				{
					int tblCnt = _nextDungeonDesc.terrainSpriteNameTable.Length;
					int typeCnt = (int)TerrainType.Count;
					Sprite[][] spriteTbl = new Sprite[tblCnt][];
					for( int i = 0; i < tblCnt; i++ )
					{
						Sprite[] sprites = Resources.LoadAll<Sprite>( _nextDungeonDesc.terrainSpriteNameTable[i] );
						if( sprites == null || sprites.Length < typeCnt ) return false;
						spriteTbl[i] = new Sprite[typeCnt];
						for( int t = 0; t < typeCnt; t++ )
						{
							string name = Enum.GetName( typeof(TerrainType), t );
							spriteTbl[i][t] = System.Array.Find<Sprite>( sprites, (s) => s.name.Equals( name ) );
						}
					}
					_terrainSpritesTable = spriteTbl;
				}
				// アイテムスプライトロード
				{
					int tblCnt = _nextDungeonDesc.itemSpriteNameTable.Length;
					int typeCnt = (int)ItemType.Count;
					Sprite[][] spriteTbl = new Sprite[tblCnt][];
					for ( int i = 0; i < tblCnt; i++ )
					{
						Sprite[] sprites = Resources.LoadAll<Sprite>( _nextDungeonDesc.itemSpriteNameTable[i] );
						if ( sprites == null || sprites.Length < typeCnt ) return false;
						spriteTbl[i] = new Sprite[typeCnt];
						for ( int t = 0; t < typeCnt; t++ )
						{
							string name = Enum.GetName( typeof( ItemType ), t );
							spriteTbl[i][t] = System.Array.Find<Sprite>( sprites, ( s ) => s.name.Equals( name ) );
						}
					}
					_itemSpritesTable = spriteTbl;
				}
				// イベントスプライトロード
				{
					int tblCnt = _nextDungeonDesc.eventSpriteNameTable.Length;
					int typeCnt = (int)EventType.Count;
					Sprite[][] spriteTbl = new Sprite[tblCnt][];
					for( int i = 0; i < tblCnt; i++ )
					{
						Sprite[] sprites = Resources.LoadAll<Sprite>( _nextDungeonDesc.eventSpriteNameTable[i] );
						if( sprites == null || sprites.Length < typeCnt ) return false;
						spriteTbl[i] = new Sprite[typeCnt];
						for( int t = 0; t < typeCnt; t++ )
						{
							string name = Enum.GetName( typeof(EventType), t );
							spriteTbl[i][t] = System.Array.Find<Sprite>( sprites, (s) => s.name.Equals( name ) );
						}
					}
					_eventSpritesTable = spriteTbl;
				}

				_story = _nextDungeonDesc.story;
				_mapDescs = _nextDungeonDesc.mapDescs;

				// テーブル設定
				_sizeTable = _nextDungeonDesc.sizeTable;
				_terrainMapTable = _nextDungeonDesc.terrainMapTable;
				_itemMapTable = _nextDungeonDesc.itemMapTable;
				_eventMapTable = _nextDungeonDesc.eventMapTable;

				_changeDungeon = false;
			}

			// マップ生成記述子設定
			MAP_CREATE_DESC createDesc = new MAP_CREATE_DESC();
			{
				MAP_INDEX_DESC indices = _mapDescs[_nextFloor];

				// サイズ設定
				Vector2Int size = _sizeTable[indices.sizeIndex];
				createDesc.width = size.x;
				createDesc.height = size.y;

				// スプライト設定
				createDesc.terrainSprites	= _terrainSpritesTable[indices.terrainSpriteIndex];
				createDesc.itemSprites		= _itemSpritesTable[indices.itemSpriteIndex];
				createDesc.eventSprites		= _eventSpritesTable[indices.eventSpriteIndex];

				// 固定マップか？
				if( indices.terrainMapIndex > -1 )
				{
					createDesc.isFixedMap = true;
					createDesc.terrainData = _terrainMapTable[indices.terrainMapIndex];
					if( indices.itemMapIndex > -1 )
						createDesc.itemData = _itemMapTable[indices.itemMapIndex];
					if( indices.eventMapIndex > -1 )
						createDesc.eventData = _eventMapTable[indices.eventMapIndex];
				}
				else
				{
					createDesc.isFixedMap = false;
				}
			}

			// マップ生成
			if( !_mapManager.StartUp( createDesc ) ) return false;

			// 階設定
			_floor = _nextFloor;
			_nextFloor = -1;

			_dungeonState = DungeonState.MAP_CREATE_END;

			return true;
		}

		#endregion

	}
}
