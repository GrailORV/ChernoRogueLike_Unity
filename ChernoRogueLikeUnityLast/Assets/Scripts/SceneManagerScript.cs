using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : SingletonMonoBehaviour<SceneManagerScript>
{
    [HideInInspector]
    public List<string> sceneNameList;

    public static SceneManagerScript script = null;

    public void SetScene(int index)
    {
        SceneManager.LoadScene(sceneNameList[index]);
    }
}
