using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Table : MonoBehaviour
{
    public List<Chair> chairs;

    private void Awake()
    {
        chairs = GetComponentsInChildren<Chair>().ToList();
    }

    public Chair CanSeatChair()
    {
        foreach (var chair in chairs)
        {
            if (!chair.IsUsing && !chair.IsDirty)
            {
                return chair;
            }
        }
        return null;
    }
}
