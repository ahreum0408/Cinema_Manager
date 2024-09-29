using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AyunDefine;

public class PlayerInteractionTrigger : MonoBehaviour
{
    private IIneractionable _currentInteractionObject;

    private void OnTriggerEnter(Collider other)
    {
        if (_currentInteractionObject != null)
        {
            return;
        }

        if (other.CompareTag(ObjectTagString.InteractionableTag))
        {
            if (other.transform.parent.TryGetComponent(out IIneractionable interactionObject))
            {
                _currentInteractionObject = interactionObject;
                _currentInteractionObject.EnterInteraction();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_currentInteractionObject == null)
        {
            if (other.CompareTag(ObjectTagString.InteractionableTag))
            {
                if (other.transform.parent.TryGetComponent(out IIneractionable interactionObject))
                {
                    _currentInteractionObject = interactionObject;
                    _currentInteractionObject.EnterInteraction();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_currentInteractionObject != null)
        {
            _currentInteractionObject.ExitInteraction();
        }

        _currentInteractionObject = null;
    }
}
