using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITakeable
{
    public void Take(Transform parentTransform, Vector3 takePosition, Vector3 takeRotation);
    public void ResetPositionAndRotation();
}
