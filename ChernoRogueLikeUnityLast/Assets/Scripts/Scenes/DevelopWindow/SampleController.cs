using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleController : MonoBehaviour
{
    [SerializeField] SampleWindow _sampleWindow;

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
