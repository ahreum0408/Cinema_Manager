using UnityEngine;

public class Food : MonoBehaviour, ITakeable
{
    private Rigidbody _rigid;
    private ObjectMovement _objectMovement;
    private Vector3 _originScale;
    private Vector3 _currentScale;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
        _objectMovement = GetComponent<ObjectMovement>();
    }

    private void OnEnable()
    {
        _originScale = transform.localScale;
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
