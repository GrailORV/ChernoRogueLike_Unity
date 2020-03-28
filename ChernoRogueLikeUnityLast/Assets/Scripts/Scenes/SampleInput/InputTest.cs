using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InputController;

public class InputTest : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        //var inputmanager = navigationmanager.instance._inputmanager;

        //更新(必ず必要)
        //inputmanager.update();

        //if (inputmanager.getanybuttondown())
        //{
        //    debug.log("どれかのボタン押下した");
        //}

        //if (inputmanager.getaxisdown(controller.axis.r_horizontal) != 0)
        //{
        //    debug.log("r_horizontal downd");
        //}

        //if (inputmanager.getaxisrow(controller.axis.r_vertical) != 0)
        //{
        //    debug.log("r_vertical = " + inputmanager.getaxis(controller.axis.r_vertical));
        //}

        //if (inputmanager.getaxisrow(controller.axis.l_vertical) != 0)
        //{
        //    debug.log("l_vertical = " + inputmanager.getaxis(controller.axis.l_vertical));
        //}
    }

    [SerializeField] NavigationLayer _navigationLayer;

    private void Start()
    {
        NavigationManager.Instance.PushLayer(_navigationLayer);

        // 入力の許可
        NavigationManager.Instance.IsKeyInput = true;

        _navigationLayer.SetSelectNavigator();
    }

    public void OnClickCircle()
    {
        Debug.LogError("〇");
    }

    public void OnClickCross()
    {
        Debug.LogError("×");
    }

    public void OnClickSquare()
    {
        Debug.LogError("□");
    }

    public void OnClickTraiangle()
    {
        Debug.LogError("△");
    }

    public void OnClickLeft1()
    {
        Debug.LogError("L1");
    }

    public void OnClickLeft2()
    {
        Debug.LogError("L2");
    }

    public void OnClickLeft3()
    {
        Debug.LogError("L3");
    }

    public void OnClickRight1()
    {
        Debug.LogError("R1");
    }

    public void OnClickRight2()
    {
        Debug.LogError("R2");
    }

    public void OnClickRight3()
    {
        Debug.LogError("R3");
    }

    public void OnLeftStickDown(NavigationManager.InputDirection direction)
    {
        Debug.LogError("Left:" + direction);
    }

    public void OnRightStickDown(NavigationManager.InputDirection direction)
    {
        Debug.LogError("Right:" + direction);
    }

    public void OnSameButtonDown()
    {
        Debug.LogError("〇と×同時押し");
    }
}
