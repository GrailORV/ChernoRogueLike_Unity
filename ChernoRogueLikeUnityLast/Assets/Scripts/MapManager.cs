using System;
using System.IO;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public enum EMapCategory
    {
        CAVE = 0,
        MAX
    }

    public enum EFloorData
    {
        FLOOR = 0,
        WALL,
        WATER,
        MAX
    }

    [SerializeField]
    private TextAsset _MapTextData;

    private string _MapName;
    private EMapCategory _MapCategory;

    [HideInInspector]
    public uint _Width, _Height;

    [HideInInspector]
    public Texture[] _WallTextures = new Texture[(int)EMapCategory.MAX];
    [HideInInspector]
    public Texture[] _FloorTextures = new Texture[(int)EMapCategory.MAX];
    [HideInInspector]
    public Texture[] _WaterTextures = new Texture[(int)EMapCategory.MAX];

    [HideInInspector]
    public byte[,] _FloorMapData;
    [HideInInspector]
    public byte[,] _RoomMapData;
    [HideInInspector]
    public byte[,] _ObjMapData;

    [HideInInspector]
    public GameObject[,] _FloorObjects;

    void Start()
    {
        // Load Text Data As String
        StringReader reader = new StringReader(_MapTextData.text);
        string lineBuff;

        #region MapName

        lineBuff = reader.ReadLine();
        _MapName = lineBuff.Split(',')[1];

        #endregion

        #region Category

        lineBuff = reader.ReadLine();
        int category = int.Parse(lineBuff.Split(',')[1]);
        _MapCategory = (EMapCategory)Enum.ToObject(typeof(EMapCategory), category);

        #endregion

        #region MapSize

        lineBuff = reader.ReadLine();
        _Width = uint.Parse(lineBuff.Split(',')[1]);
        _Height = uint.Parse(lineBuff.Split(',')[3]);

        _FloorMapData = new byte[_Width, _Height];
        _RoomMapData = new byte[_Width, _Height];
        _ObjMapData = new byte[_Width, _Height];
        _FloorObjects = new GameObject[_Width, _Height];

        #endregion

        reader.ReadLine();
        reader.ReadLine();

        #region Import Map Floor Data & Create Map Floor

        GameObject floorPrefab = Resources.Load("Prefabs/Floor") as GameObject;
        GameObject wallPrefab = Resources.Load("Prefabs/Wall") as GameObject;
        GameObject waterPrefab = Resources.Load("Prefabs/Water") as GameObject;

        for (uint v = 0; v < _Height; v++)
        {
            lineBuff = reader.ReadLine();
            var values = Array.ConvertAll(lineBuff.Split(','), int.Parse);
            for (uint h = 0; h < _Width; h++)
            {
                Vector3 pos = new Vector3(h, 0, -v);
                GameObject obj;
                Material mat;
                _FloorMapData[h, v] = (byte)values[h];
                switch ((EFloorData)values[h])
                {
                    case EFloorData.FLOOR:
                        obj = Instantiate(floorPrefab, pos, Quaternion.Euler(0, 180, 0));
                        mat = obj.GetComponentInChildren<Renderer>().material;
                        mat.SetTexture("_MainTex", _FloorTextures[category]);
                        obj.transform.parent = transform;
                        _FloorObjects[h, v] = obj;
                        break;
                    case EFloorData.WALL:
                        obj = Instantiate(wallPrefab, pos, Quaternion.identity);
                        mat = obj.GetComponentInChildren<Renderer>().material;
                        mat.SetTexture("_MainTex", _WallTextures[category]);
                        obj.transform.parent = transform;
                        _FloorObjects[h, v] = obj;
                        break;
                    case EFloorData.WATER:
                        obj = Instantiate(waterPrefab, pos, Quaternion.identity);
                        mat = obj.GetComponentInChildren<Renderer>().material;
                        mat.SetTexture("_MainTex", _WaterTextures[category]);
                        obj.transform.parent = transform;
                        _FloorObjects[h, v] = obj;
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion

        reader.ReadLine();

        #region RoomNum

        lineBuff = reader.ReadLine();

        #endregion

        reader.ReadLine();

        #region Import Room Data

        for (uint v = 0; v < _Height; v++)
        {
            lineBuff = reader.ReadLine();
            var values = Array.ConvertAll(lineBuff.Split(','), byte.Parse);
            for (uint h = 0; h < _Width; h++)
            {
                _RoomMapData[h, v] = values[h];
            }
        }

        #endregion

        #region Import Map Object Data

        for (uint v = 0; v < _Height; v++)
        {
            lineBuff = reader.ReadLine();
            // var values = Array.ConvertAll(lineBuff.Split(','), byte.Parse);
            for (uint h = 0; h < _Width; h++)
            {
                _ObjMapData[h, v] = 0;
            }
        }

        #endregion
    }

    private void OnGUI()
    {

        GUI.Label(new Rect(20, 20, 100, 20), _MapName);
        GUI.Label(new Rect(20, 40, 100, 20), _MapCategory.ToString());
        GUI.Label(new Rect(20, 60, 100, 20), "Width : " + _Width.ToString());
        GUI.Label(new Rect(20, 80, 100, 20), "Height : " + _Height.ToString());
        GUI.Label(new Rect(20, 100, 100, 20), _WallTextures.Length.ToString());

    }

}
