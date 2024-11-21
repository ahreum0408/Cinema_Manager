using UnityEngine;
using static AyunDefine;

public class TakeableBase : MonoBehaviour, ITakeable
{
    [SerializeField] private PoolableType _poolType;
    public PoolableType PoolType => _poolType;

    private Rigidbody _rigid;
    protected ObjectMovement _objectMovement;

    private Vector3 _originScale;
    private Vector3 _currentScale;

    protected virtual void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
        _objectMovement = GetComponent<ObjectMovement>();
    }

    protected virtual void OnEnable()
    {
        _originScale = transform.localScale;

        _rigid.useGravity = true;
        _rigid.isKinematic = true;

        ResetPositionAndRotation();
    }

    public void Take(Transform parentTransform, Vector3 takePosition, Vector3 takeRotation, float playTime = 0.2f)
    {
        PhysicsSetting(true);
        transform.SetParent(parentTransform);
        transform.localRotation = Quaternion.Euler(takeRotation);
        _currentScale = transform.localScale;

        Vector3 pos = new Vector3(takePosition.x * (_currentScale.x / _originScale.x),
                                  takePosition.y * (_currentScale.y / _originScale.y),
                                  takePosition.z * (_currentScale.z / _originScale.z));
        _objectMovement.JumpToPosition(pos, playTime);
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
