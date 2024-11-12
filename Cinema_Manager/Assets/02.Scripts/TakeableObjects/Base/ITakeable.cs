using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AyunDefine;

public interface ITakeable
{
    public void Take(Transform parentTransform, Vector3 takePosition, Vector3 takeRotation, float playTime = 0.4f);
    public void ResetPositionAndRotation();
}
