using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour, IIneractionable
{
    [HideInInspector] public GameObject GameObject => gameObject;

    [SerializeField] private float _openAngle = 110f;
    [SerializeField] private float _rotationSpeed = 200f;
    private Quaternion _closedRotation;
    private Quaternion _openRotation;

    private bool _isOpen = false;

    private Transform _visualTrm;
    private Coroutine _currentRotationRoutine;

    private void Awake()
    {
        _visualTrm = transform.Find("Visual").GetComponent<Transform>();
        _closedRotation = _visualTrm.rotation;
    }

    public void EnterInteraction(AgentController agent)
    {
        Vector3 directionToAgent = (agent.transform.position - transform.position).normalized;
        directionToAgent.y = 0;

        float dotProduct = Vector3.Dot(transform.forward, directionToAgent);
        float targetYAngle = dotProduct > 0 ? -_openAngle : _openAngle;
        _openRotation = Quaternion.Euler(0, targetYAngle, 0) * _closedRotation;

        StartRotation(_openRotation, true);
    }

    public void ExitInteraction(AgentController agent)
    {
        StartRotation(_closedRotation, false);
    }

    private void StartRotation(Quaternion targetRotation, bool isOpen)
    {

        if (_currentRotationRoutine != null)
        {
            if (_isOpen && isOpen) return;
            StopCoroutine(_currentRotationRoutine);
        }
        _isOpen = isOpen;
        _currentRotationRoutine = StartCoroutine(RotateDoorRoutine(targetRotation));
    }

    private IEnumerator RotateDoorRoutine(Quaternion targetRotation)
    {
        while (Quaternion.Angle(_visualTrm.rotation, targetRotation) > 0.01f)
        {
            _visualTrm.rotation = Quaternion.RotateTowards(_visualTrm.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
            yield return null;
        }
        _visualTrm.rotation = targetRotation;
    }
}
