using System.Collections.Generic;

/// <summary>
/// セーブデータの基底クラス
/// </summary>
public class SaveDataBase
{
    /// <summary>
    /// セーブデータの保存先パス
    /// </summary>
    public const string playerSaveData = "PlayerData.json";

    /// <summary>
    /// セーブデータ本体
    /// </summary>
    public class SaveData
    {
        /// <summary>
        /// 名前
        /// </summary>
        public string playerName = "";

        /// <summary>
        /// プレイ時間
        /// </summary>
        public string playTime = "00:00:00";

        /// <summary>
        /// プレイヤーのレベル
        /// </summary>
        public int playerLevel = 1;

        /// <summary>
        /// 所持アイテム一覧
        /// </summary>
        /// string: アイテムID
        /// List: 所持数
        public List<Dictionary<int, int>> haveItemList = null;

        /// <summary>
        /// 倉庫アイテム一覧
        /// </summary>
        /// string: アイテムID
        /// List: 所持数
        public List<Dictionary<int, int>> warehouseItemList = null;

        /// <summary>
        /// 所持金
        /// </summary>
        public int money = 0;


        /* 中断セーブデータ情報 */

        /// <summary>
        /// 現在潜っているダンジョン情報
        /// </summary>
        /// int: ダンジョンID
        /// int: 現在の階層
        public Dictionary<int, int> dungeonData = null;
    }
}