using UnityEngine;

public class MapManager : MonoBehaviour
{
    enum MapCategory
    {
        MC_Grass,
        MC_Cave
    }

    [SerializeField]
    private MapCategory mCat;

    [SerializeField]
    private GameObject WallPrefab;

    void Start()
    {
        //if(!WallPrefab)
        //{
        //    return;
        //}

        //for (int x = 0; x < 100; x++)
        //{
        //    for (int z = 0; z < 300; z++)
        //    {
        //        Instantiate(WallPrefab, new Vector3(x, 0, z), Quaternion.identity);
        //    }
        //}
    }

    // Update is called once per frame
    void Update()
    {

    }
}
