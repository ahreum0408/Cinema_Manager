using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static AyunDefine;

public class Table : MonoBehaviour
{
    public List<Point> points;

    private void Awake()
    {
        points = GetComponentsInChildren<Point>().ToList();
    }

    public Point CanSeatChair()
    {
        foreach (var chair in points)
        {
            if (!chair.IsUsing && !chair.IsDirty)
            {
                return chair;
            }
        }
        return null;
    }
}
