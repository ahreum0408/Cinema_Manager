using System.Collections;
using UnityEngine;

public class Money : MonoBehaviour
{
    private ObjectMovement _objectMovement;

    private void Awake()
    {
        _objectMovement = GetComponent<ObjectMovement>();
    }

    private void OnEnable()
    {
        transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.Euler(0, 0, 0));
    }

    public void JumpToPositionAndDestory(Vector3 position)
    {
        StartCoroutine(JumpRoutine(position));
    }

    private IEnumerator JumpRoutine(Vector3 position)
    {
        _objectMovement.JumpToPosition(position, Space.World);
        yield return new WaitForSeconds(_objectMovement.AnimationDuration);
        PoolManager.Instance.Push(transform.name, gameObject);
    }
}
