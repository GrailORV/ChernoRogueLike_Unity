using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    MapPosition _pos = new MapPosition();
    Transform _bodyTrans;

    [SerializeField, Range(float.Epsilon, 10f)]
    float _MoveDuration = 0.1f;

    bool _EnableMove = false;

    [HideInInspector]
    public MapManager _MapManager;


    // Use this for initialization
    void Start()
    {
        _MapManager = FindObjectOfType<MapManager>();
        _EnableMove = true;
        _bodyTrans = transform.Find("Body");
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }


    private void Move()
    {
        if (!_EnableMove)
        {
            return;
        }

        if (_MapManager == null)
        {
            return;
        }

        int h = (int)Input.GetAxisRaw("Horizontal");
        int v = (int)Input.GetAxisRaw("Vertical");

        MapPosition newPos;

        if (!Judge(h, v, out newPos))
        {
            return;
        }

        SetPosition(newPos, false);

    }

    private bool Judge(int diffX, int diffY, out MapPosition nextPos)
    {
        // Initialize Next Position
        nextPos.x = 0;
        nextPos.y = 0;

        // Movement Value Check
        if (diffX == 0 && diffY == 0)
        {
            return false;
        }

        // Look At Input Direction
        Vector3 currentPos = _MapManager._FloorObjects[_pos.x, _pos.y].transform.position;
        _bodyTrans.DOLookAt(new Vector3(currentPos.x + diffX, _bodyTrans.position.y, currentPos.z + diffY), _MoveDuration / 3f);

        // Map Size Check
        int xBuff = _pos.x + diffX;
        if (xBuff < 0)
        {
            return false;
        }
        if (xBuff >= _MapManager._Width)
        {
            return false;
        }

        int yBuff = _pos.y - diffY;
        if (yBuff < 0)
        {
            return false;
        }
        if (yBuff >= _MapManager._Height)
        {
            return false;
        }

        // Movement Root Check
        if (_MapManager._FloorMapData[xBuff, _pos.y] == (byte)MapManager.EFloorData.WALL)
        {
            return false;
        }

        if (_MapManager._FloorMapData[_pos.x, yBuff] == (byte)MapManager.EFloorData.WALL)
        {
            return false;
        }

        // Goal Position Check
        if (_MapManager._FloorMapData[xBuff, yBuff] != (byte)MapManager.EFloorData.FLOOR)
        {
            return false;
        }

        // return Suceeded
        nextPos.x = (byte)xBuff;
        nextPos.y = (byte)yBuff;

        return true;
    }

    public void SetPosition(MapPosition pos, bool warp)
    {
        _pos = pos;

        if (warp)
        {
            transform.position = _MapManager._FloorObjects[_pos.x, _pos.y].transform.position;
            _bodyTrans.rotation = Quaternion.LookRotation(Vector3.back);
        }
        else
        {
            _EnableMove = false;
            Vector3 newPos = _MapManager._FloorObjects[_pos.x, _pos.y].transform.position;

            Sequence seq = DOTween.Sequence();
            seq.Append(transform.DOMove(newPos, _MoveDuration))
                .OnComplete(() =>
                {
                    _EnableMove = true;
                });
        }
    }

    public void SetPosition(byte x, byte y, bool warp)
    {
        SetPosition(new MapPosition(x, y), warp);
    }
}
