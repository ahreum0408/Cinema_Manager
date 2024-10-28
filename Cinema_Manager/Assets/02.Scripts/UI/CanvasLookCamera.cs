using UnityEngine;

public class CanvasLookCamera : MonoBehaviour
{
    private Camera _mainCam;

    private void Awake()
    {
        _mainCam = Camera.main;
    }

    private void Update()
    {

        transform.LookAt(transform.position + _mainCam.transform.rotation * Vector3.forward,
                         _mainCam.transform.rotation * Vector3.up);
    }
}
