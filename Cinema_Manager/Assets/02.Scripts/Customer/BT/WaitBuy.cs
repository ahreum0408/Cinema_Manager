using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitBuy : Conditional
{
    public override TaskStatus OnUpdate()
    {
        return TaskStatus.Success;
    }
}
