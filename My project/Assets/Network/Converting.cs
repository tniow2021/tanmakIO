using UnityEngine;
public static class Converting
{
    public static UserTransform ToUserTransForm(Transform t)
    {
        return new UserTransform(t.position.x,t.position.y);
    }
    public static UserTransform ToUserTransForm(Vector3 v3)
    {
        return new UserTransform(v3.x, v3.y);
    }
    public static Vector3 ToVector3(UserTransform ut)
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
