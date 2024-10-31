using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AyunDefine;

public class AgentInteractionTrigger : MonoBehaviour
{
    private AgentController _agentController;
    private IIneractionable _currentInteractionObject;

    private void Awake()
    {
        _agentController = GetComponent<AgentController>();
    }

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
                _currentInteractionObject.EnterInteraction(_agentController);
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
                    _currentInteractionObject.EnterInteraction(_agentController);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_currentInteractionObject != null)
        {
            _currentInteractionObject.ExitInteraction(_agentController);
        }

        _currentInteractionObject = null;
    }
}
