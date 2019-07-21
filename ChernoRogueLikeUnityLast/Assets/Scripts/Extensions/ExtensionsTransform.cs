using UnityEngine;

//================================================================================
// class ExtensionsTransform
//================================================================================
public static class ExtensionsTransform
{
    //--------------------------------------------------------------------------------
    // set world position
    //--------------------------------------------------------------------------------
    public static void  SetPos( this Transform transform, float x, float y, float z )
    {
        transform.position = new Vector3(x,y,z);
    }
    public static void  SetPosX( this Transform transform, float x )
    {
        var pos = transform.position;
        pos.x = x;
        transform.position = pos;
    }
    public static void  SetPosY( this Transform transform, float y )
    {
        var pos = transform.position;
        pos.y = y;
        transform.position = pos;
    }
    public static void  SetPosZ( this Transform transform, float z )
    {
        var pos = transform.position;
        pos.z = z;
        transform.position = pos;
    }
    //--------------------------------------------------------------------------------
    // add world position
    //--------------------------------------------------------------------------------
    public static void  AddPos( this Transform transform, float x, float y, float z )
    {
        var pos = transform.position;
        pos.x += x;
        pos.y += y;
        pos.z += z;
        transform.position = pos;
    }
    public static void  AddPosX( this Transform transform, float x )
    {
        var pos = transform.position;
        pos.x += x;
        transform.position = pos;
    }
    public static void  AddPosY( this Transform transform, float y )
    {
        var pos = transform.position;
        pos.y += y;
        transform.position = pos;
    }
    public static void  AddPosZ( this Transform transform, float z )
    {
        var pos = transform.position;
        pos.z += z;
        transform.position = pos;
    }

    //--------------------------------------------------------------------------------
    // set local position
    //--------------------------------------------------------------------------------
    public static void  SetLocalPos( this Transform transform, float x, float y, float z )
    {
        transform.localPosition = new Vector3(x,y,z);
    }
    public static void  SetLocalPosX( this Transform transform, float x )
    {
        var lpos = transform.localPosition;
        lpos.x = x;
        transform.localPosition = lpos;
    }
    public static void  SetLocalPosY( this Transform transform, float y )
    {
        var lpos = transform.localPosition;
        lpos.y = y;
        transform.localPosition = lpos;
    }
    public static void  SetLocalPosZ( this Transform transform, float z )
    {
        var lpos = transform.localPosition;
        lpos.z = z;
        transform.localPosition = lpos;
    }
    //--------------------------------------------------------------------------------
    // add local position
    //--------------------------------------------------------------------------------
    public static void  AddLocalPos( this Transform transform, float x, float y, float z )
    {
        var lpos = transform.localPosition;
        lpos.x += x;
        lpos.y += y;
        lpos.z += z;
        transform.localPosition = lpos;
    }
    public static void  AddLocalPosX( this Transform transform, float x )
    {
        var lpos = transform.localPosition;
        lpos.x += x;
        transform.localPosition = lpos;
    }
    public static void  AddLocalPosY( this Transform transform, float y )
    {
        var lpos = transform.localPosition;
        lpos.y += y;
        transform.localPosition = lpos;
    }
    public static void  AddLocalPosZ( this Transform transform, float z )
    {
        var lpos = transform.localPosition;
        lpos.z += z;
        transform.localPosition = lpos;
    }

    //--------------------------------------------------------------------------------
    // set local scale
    //--------------------------------------------------------------------------------
    public static void  SetLocalScale( this Transform transform, float x, float y, float z )
    {
        transform.localScale = new Vector3(x,y,z);
    }
    public static void  SetLocalScaleX( this Transform transform, float x )
    {
        var scale = transform.localScale;
        scale.x = x;
        transform.localScale = scale;
    }
    public static void  SetLocalScaleY( this Transform transform, float y )
    {
        var scale = transform.localScale;
        scale.y = y;
        transform.localScale = scale;
    }
    public static void  SetLocalScaleZ( this Transform transform, float z )
    {
        var scale = transform.localScale;
        scale.z = z;
        transform.localScale = scale;
    }
    //--------------------------------------------------------------------------------
    // add local scale
    //--------------------------------------------------------------------------------
    public static void  AddLocalScale( this Transform transform, float x, float y, float z )
    {
        var scale = transform.localScale;
        scale.x += x;
        scale.y += y;
        scale.z += z;
        transform.localScale = scale;
    }
    public static void  AddLocalScaleX( this Transform transform, float x )
    {
        var scale = transform.localScale;
        scale.x += x;
        transform.localScale = scale;
    }
    public static void  AddLocalScaleY( this Transform transform, float y )
    {
        var scale = transform.localScale;
        scale.y += y;
        transform.localScale = scale;
    }
    public static void  AddLocalScaleZ( this Transform transform, float z )
    {
        var scale = transform.localScale;
        scale.z += z;
        transform.localScale = scale;
    }

    //--------------------------------------------------------------------------------
    // set euler angle
    //--------------------------------------------------------------------------------
    public static void  SetEulerAngles( this Transform transform, float x, float y, float z )
    {
        transform.eulerAngles = new Vector3(x,y,z);
    }
    public static void  SetEulerAnglesX( this Transform transform, float x )
    {
        var angle = transform.eulerAngles;
        angle.x = x;
        transform.eulerAngles = angle;
    }
    public static void  SetEulerAnglesY( this Transform transform, float y )
    {
        var angle = transform.eulerAngles;
        angle.y = y;
        transform.eulerAngles = angle;
    }
    public static void  SetEulerAnglesZ( this Transform transform, float z )
    {
        var angle = transform.eulerAngles;
        angle.z = z;
        transform.eulerAngles = angle;
    }
    //--------------------------------------------------------------------------------
    // add euler angle
    //--------------------------------------------------------------------------------
    public static void  AddEulerAngle( this Transform transform, float x, float y, float z )
    {
        var angle = transform.eulerAngles;
        angle.x += x;
        angle.y += y;
        angle.z += z;
        transform.eulerAngles = angle;
    }
    public static void  AddEulerAngleX( this Transform transform, float x )
    {
        var angle = transform.eulerAngles;
        angle.x += x;
        transform.eulerAngles = angle;
    }
    public static void  AddEulerAngleY( this Transform transform, float y )
    {
        var angle = transform.eulerAngles;
        angle.y += y;
        transform.eulerAngles = angle;
    }
    public static void  AddEulerAngleZ( this Transform transform, float z )
    {
        var angle = transform.eulerAngles;
        angle.z += z;
        transform.eulerAngles = angle;
    }

    //--------------------------------------------------------------------------------
    // set local euler angle
    //--------------------------------------------------------------------------------
    public static void  SetLocalEulerAngles( this Transform transform, float x, float y, float z )
    {
        transform.localEulerAngles = new Vector3(x,y,z);
    }
    public static void  SetLocalEulerAnglesX( this Transform transform, float x )
    {
        var angle = transform.localEulerAngles;
        angle.x = x;
        transform.localEulerAngles = angle;
    }
    public static void  SetLocalEulerAnglesY( this Transform transform, float y )
    {
        var angle = transform.localEulerAngles;
        angle.y = y;
        transform.localEulerAngles = angle;
    }
    public static void  SetLocalEulerAnglesZ( this Transform transform, float z )
    {
        var angle = transform.localEulerAngles;
        angle.z = z;
        transform.localEulerAngles = angle;
    }
    //--------------------------------------------------------------------------------
    // add local euler angle
    //--------------------------------------------------------------------------------
    public static void  AddLocalEulerAngle( this Transform transform, float x, float y, float z )
    {
        var angle = transform.localEulerAngles;
        angle.x += x;
        angle.y += y;
        angle.z += z;
        transform.localEulerAngles = angle;
    }
    public static void  AddLocalEulerAngleX( this Transform transform, float x )
    {
        var angle = transform.localEulerAngles;
        angle.x += x;
        transform.localEulerAngles = angle;
    }
    public static void  AddLocalEulerAngleY( this Transform transform, float y )
    {
        var angle = transform.localEulerAngles;
        angle.y += y;
        transform.localEulerAngles = angle;
    }
    public static void  AddLocalEulerAngleZ( this Transform transform, float z )
    {
        var angle = transform.localEulerAngles;
        angle.z += z;
        transform.localEulerAngles = angle;
    }
}