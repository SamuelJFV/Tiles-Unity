using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGridLayout
{
    public string name;
    float Sqrt3 = Mathf.Sqrt(3);

    public HexGridLayout(string _name = "hexGridLayout")
    {
        name = _name;
    }

    public Vector3 SnapToGrid(Vector3 point)
    {
        point.x = Mathf.Floor(point.x*Sqrt3+Sqrt3/2.0f);
        point.y = 1.501f;
        point.z = Mathf.Floor(point.z);
        return point;
    }
}
