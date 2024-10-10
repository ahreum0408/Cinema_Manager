using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeableBase : MonoBehaviour, ITakeable
{
    private Rigidbody _rigid;
    private Animator _animator;
    private ObjectMovement _objectMovement;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
        _animator = transform.Find("Visual").GetComponent<Animator>();
        _objectMovement = GetComponent<ObjectMovement>();
    }

    private void OnEnable()
    {
        _rigid.useGravity = true;
        _rigid.isKinematic = true;

        ResetPositionAndRotation();
    }

    public void Take(Transform parentTransform, Vector3 takePosition, Vector3 takeRotation)
    {
        PhysicsSetting(true);
        transform.SetParent(parentTransform);
        transform.localRotation = Quaternion.Euler(takeRotation);
        _objectMovement.JumpToPosition(takePosition);
    }

    private void PhysicsSetting(bool isOn)
    {
        _rigid.isKinematic = isOn;
        _rigid.useGravity = !isOn;
    }

    public void ResetPositionAndRotation()
    {
        transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.Euler(Vector3.zero));
    }
}
