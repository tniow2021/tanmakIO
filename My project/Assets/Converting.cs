using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public static class Converting
{
    public static TypeBuff.UserTransform ToUserTransForm(Vector3 v3)
    {
        return new TypeBuff.UserTransform(v3.x,v3.y);
    }
    public static Vector3 ToVector3(in TypeBuff.UserTransform ut)
    {
        return new Vector3(ut.x, ut.y, 0);
    }

    public static string TestByteArrayPrint(byte[]b)
    {
        string str = "";
        foreach(byte b2 in b)
        {
            str += b2.ToString()+".";
        }
        return str;
    }
}
