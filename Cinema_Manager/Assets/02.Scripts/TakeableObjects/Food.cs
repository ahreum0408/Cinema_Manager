using ObjectPooling;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;

public class Food : PoolableMono, ITakeable
{
    private Rigidbody _rigid;
    private ObjectMovement _objectMovement;

    public override void Reset()
    {

    }

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();

        _objectMovement = GetComponent<ObjectMovement>();
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
