using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleController : MonoBehaviour
{
    SampleWindow _sampleWindow;

    private void Start()
    {
        _sampleWindow = WindowManager.Instance.CreateWindow<SampleWindow>(WindowData.WindowType.SampleWindowPanel);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            _sampleWindow.Open();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            _sampleWindow.Close();
        }
    }
}
