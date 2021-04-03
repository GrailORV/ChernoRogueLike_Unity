using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DramaTableData : ScriptableObject
{
    /// <summary> データリスト </summary>
    public List<Data> dataList = new List<Data>();

    [System.Serializable]
    public class Data
    {
        public int code;
        public int index;
        public string text;
        public float base_speed;
        public int effect;
    }
}
