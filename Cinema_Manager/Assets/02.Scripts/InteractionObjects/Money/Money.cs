using ObjectPooling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Money : PoolableMono
{
    private ObjectMovement _objectMovement;

    private void Awake()
    {
        _objectMovement = GetComponent<ObjectMovement>();
    }

    public override void Reset()
    {
        transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.Euler(0, 0, 0));
    }

    public void JumpToPositionAndDestory(Vector3 position)
    {
        StartCoroutine(JumpRoutine(position));
    }

    private IEnumerator JumpRoutine(Vector3 position)
    {
        // 해당 오브젝트는 누군가에게 자식으로 들어가지 않고
        // 항상 PoolManager 자식으로 있기 때문에 World 좌표계 이용

        _objectMovement.JumpToPosition(position, Space.World);
        yield return new WaitForSeconds(_objectMovement.AnimationDuration);
        PoolManager.Instance.Push(this);
    }
}
