using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeTest : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            Fade.FadeStart(Fade.FadeMode.FM_FadeIn, 1.0f);
        }

        if(Input.GetKeyDown(KeyCode.Backspace))
        {
            Fade.FadeStart(Fade.FadeMode.FM_FadeOut, 1.0f, test);
        }
    }

    public void test()
    {
        SceneManagerScript.script.SetScene(0);
    }
}
