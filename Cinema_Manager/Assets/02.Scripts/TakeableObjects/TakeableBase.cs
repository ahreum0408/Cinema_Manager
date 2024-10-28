using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AyunDefine;

public class TakeableBase : MonoBehaviour, ITakeable
{
    [SerializeField] private PoolableType _poolType;

    private Rigidbody _rigid;
    //private Animator _animator;
    private ObjectMovement _objectMovement;

    private Vector3 _originScale;
    private Vector3 _currentScale;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
        //_animator = transform.Find("Visual").GetComponent<Animator>();
        _objectMovement = GetComponent<ObjectMovement>();
    }

    private void OnEnable()
    {
        _originScale = transform.localScale;

        _rigid.useGravity = true;
        _rigid.isKinematic = true;

        ResetPositionAndRotation();
    }

    public void Take(Transform parentTransform, Vector3 takePosition, Vector3 takeRotation)
    {
        PhysicsSetting(true);
        transform.SetParent(parentTransform);
        transform.localRotation = Quaternion.Euler(takeRotation);
        _currentScale = transform.localScale;

        Vector3 pos = new Vector3(takePosition.x * (_currentScale.x / _originScale.x),
                                  takePosition.y * (_currentScale.y / _originScale.y),
                                  takePosition.z * (_currentScale.z / _originScale.z));
        _objectMovement.JumpToPosition(pos);
    }

    protected void PhysicsSetting(bool isOn)
    {
        _rigid.isKinematic = isOn;
        _rigid.useGravity = !isOn;
    }

    public void ResetPositionAndRotation()
    {
        transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.Euler(Vector3.zero));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Container"))
        {
            PoolManager.Instance.Push(_poolType.ToString(), gameObject);
        }
    }
}
