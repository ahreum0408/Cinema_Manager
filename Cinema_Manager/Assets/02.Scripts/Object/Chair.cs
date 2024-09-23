using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chair : MonoBehaviour
{
    public bool IsUsing; //손님이 사용 중?
    public bool IsDirty; //이 자리가 더러운가?

    public void ChangeUsingState(bool isUse)
    {
        IsUsing = isUse;
    }

    public void ChangeDirtyState(bool isDirty)
    {
        IsDirty = isDirty; 
    }
}
