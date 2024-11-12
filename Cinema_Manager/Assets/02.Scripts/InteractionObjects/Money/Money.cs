using System.Collections;
using UnityEngine;

public class Money : TakeableBase
{
    protected override void OnEnable()
    {
        base.OnEnable();
        transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.Euler(0, 0, 0));
    }

    public void JumpToPositionAndDestory(Vector3 position)
    {
        StartCoroutine(JumpRoutine(position));
    }

    private IEnumerator JumpRoutine(Vector3 position)
    {
        _objectMovement.JumpToPosition(position, 0.2f, Space.World);
        yield return new WaitForSeconds(0.2f);
        PoolManager.Instance.Push(transform.name, gameObject);
    }
}
