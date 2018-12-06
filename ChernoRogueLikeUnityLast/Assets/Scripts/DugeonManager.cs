using UnityEngine;

public class DugeonManager : MonoBehaviour
{
    [SerializeField]
    GameObject _PlayerPawnPrefab;

    [SerializeField]
    GameObject _MapManagerPrefab;

    PlayerController _PlayerController;
    MapManager _MapManager;
    bool _IsInitialize;

    // Use this for initialization
    void Start()
    {
        _IsInitialize = true;

        if (_MapManagerPrefab == null)
        {
            return;
        }

        _MapManager = Instantiate(_MapManagerPrefab).GetComponent<MapManager>();

        if (_PlayerPawnPrefab == null)
        {
            return;
        }

        _PlayerController = Instantiate(_PlayerPawnPrefab).GetComponent<PlayerController>();

        _IsInitialize = false;

    }

    // Update is called once per frame
    void Update()
    {
        if(!_IsInitialize)
        {
            while (true)
            {
                byte w = (byte)Random.Range(0, (int)_MapManager._Width);
                byte d = (byte)Random.Range(0, (int)_MapManager._Height);

                if (_MapManager._FloorMapData[w, d] != 0)
                {
                    continue;
                }

                if (_MapManager._RoomMapData[w, d] == 0)
                {
                    continue;
                }

                _PlayerController.SetPosition(w, d, true);
                break;
            }
        }

        _IsInitialize = true;
    }
}
