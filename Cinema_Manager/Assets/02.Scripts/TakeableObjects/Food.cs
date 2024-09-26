using ObjectPooling;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEngine;

public class Food : PoolableMono, ITakeable
{
    private Rigidbody _rigid;
    private ObjectMovement _objectMovement;
    private Vector3 _originScale;
    private Vector3 _currentScale;

    public override void Reset()
    {
        _originScale = transform.localScale;
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
        _currentScale = transform.localScale;

        Vector3 pos = new Vector3(takePosition.x,
                                  takePosition.y * (_currentScale.y / _originScale.y),
                                  takePosition.z);
        Debug.Log((_currentScale.y / _originScale.y));
        _objectMovement.JumpToPosition(pos);
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
