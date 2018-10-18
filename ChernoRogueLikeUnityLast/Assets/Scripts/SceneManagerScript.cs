using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    [HideInInspector]
    public List<string> sceneNameList;

    public static SceneManagerScript script = null;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void CreateSceneManager()
    {
        GameObject prefab = Resources.Load("Prefabs/SceneManager") as GameObject;
        var obj = Instantiate(prefab);
        DontDestroyOnLoad(obj);
        script = obj.GetComponent<SceneManagerScript>();
    }

    public void SetScene(int index)
    {
        SceneManager.LoadScene(sceneNameList[index]);
    }
}
