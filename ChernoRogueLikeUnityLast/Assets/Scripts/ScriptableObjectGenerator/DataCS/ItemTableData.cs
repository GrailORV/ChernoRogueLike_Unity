using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTableData : ScriptableObject
{
    /// <summary> データリスト </summary>
    public List<Data> dataList = new List<Data>();

    [System.Serializable]
    public class Data
    {
        public int id;
        public int type;
        public string name;
    }
}
