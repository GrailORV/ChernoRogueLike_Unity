using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamplePagerComtrol : MonoBehaviour
{
    [SerializeField] PagerControl _pagerControl = null;

	// Use this for initialization
	void Start ()
    {
        _pagerControl.Init(5);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _pagerControl.DisplayIndex--;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _pagerControl.DisplayIndex++;
        }
    }
}
