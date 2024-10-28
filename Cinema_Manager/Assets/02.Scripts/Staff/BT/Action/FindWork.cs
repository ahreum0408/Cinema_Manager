using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindWork : Action
{
    public SharedStaff staff;

    //public override TaskStatus OnUpdate()
    //{

    //}

    private void CheckDisplayStand()
    {
        
    }

    private void CheckCounter()
    {
        if(ObjectManager.Instance.counter.lineList.Count > 0)
        {

        }
    }
}
