using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public enum FadeMode
    {
        FM_None,
        FM_FadeIn,
        FM_FadeOut,
        FM_FadeEnd,
        FM_MAX
    }

    public static Fade script = null;
    public static FadeMode CurrentMode = FadeMode.FM_None;

    private static UnityEvent fadeEndEvent = new UnityEvent();
    private static float fadeTimer = 0;
    private static float fadeRate = 0;
    private static Material mat;

    // Use this for initialization
    void Start()
    {
        script = this;
        CurrentMode = FadeMode.FM_None;
        fadeEndEvent.RemoveAllListeners();
        fadeTimer = 0;
        fadeRate = 0;
        mat = GetComponent<Image>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentMode == FadeMode.FM_None)
        {
            return;
        }

        if (CurrentMode == FadeMode.FM_FadeEnd)
        {
            fadeEndEvent.Invoke();
            fadeEndEvent.RemoveAllListeners();
            CurrentMode = FadeMode.FM_None;
            return;
        }

        fadeTimer -= Time.deltaTime;
        float shadeTime = mat.GetFloat("_FadeTime") + fadeRate * Time.deltaTime;
        mat.SetFloat("_FadeTime", shadeTime);

        if (fadeTimer <= 0)
        {
            if (CurrentMode == FadeMode.FM_FadeIn)
            {
                mat.SetFloat("_FadeTime", 0.0f);
            }
            else
            {
                mat.SetFloat("_FadeTime", 1.0f);
            }

            CurrentMode = FadeMode.FM_FadeEnd;
            fadeTimer = 0;
            fadeRate = 0;
        }
    }

    static public void FadeStart(FadeMode mode, float time, UnityAction action = null)
    {
        if (script == null)
        {
            return;
        }

        if (CurrentMode != FadeMode.FM_None)
        {
            return;
        }

        if (mode != FadeMode.FM_FadeIn && mode != FadeMode.FM_FadeOut)
        {
            return;
        }

        if (time < Mathf.Epsilon)
        {
            time = Mathf.Epsilon;
        }

        CurrentMode = mode;
        fadeTimer = time;

        if (action != null)
        {
            fadeEndEvent.AddListener(action);
        }

        if (mode == FadeMode.FM_FadeIn)
        {
            mat.SetFloat("_FadeTime", 1.0f);
            fadeRate = -1f / time;
        }
        else
        {
            mat.SetFloat("_FadeTime", 0.0f);
            fadeRate = 1f / time;
        }

    }
}
