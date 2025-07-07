using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorSwitcher
{

    //UnityEngine.Vector2 to System.Numerics.Vector2
    List<UnityEngine.Vector2> vectorList2 = new List<UnityEngine.Vector2>();

    public static List<System.Numerics.Vector2> FromUnityList(List<UnityEngine.Vector2> unityVector)
    {
        //System.Numerics.Vector2 to UnityEngine.Vector2
        List<System.Numerics.Vector2> vectorList1 = new List<System.Numerics.Vector2>();
        foreach (var v in unityVector)
        {
            vectorList1.Add(new System.Numerics.Vector2(v.x, v.y));
        }
        return vectorList1;
    }
    public List<UnityEngine.Vector2> FromNumericsList(List<System.Numerics.Vector2> unityVector)
    {
        //UnityEngine.Vector2 to System.Numerics.Vector2
        List<UnityEngine.Vector2> vectorList2 = new List<UnityEngine.Vector2>();
        foreach (var v in unityVector)
        {
            vectorList2.Add(new UnityEngine.Vector2(v.X, v.Y));
        }
        return vectorList2;
    }
}
