using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AyunDefine;

public class AgentInteractionTrigger : MonoBehaviour
{
    private AgentController _agentController;
    private IIneractionable _currentInteractionObject;
    private StaffController _staffController;

    private void Awake()
    {
        _agentController = GetComponent<AgentController>();
        _staffController = GetComponent<StaffController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enter");
        Debug.Log(_currentInteractionObject);

        if (_staffController != null)
        {
            if (other.GetComponentInParent<FoodContainer>() != null && _staffController.foodContainer != null)
            {
                if (_staffController.foodContainer.GetPoolObjType() !=
                other.GetComponentInParent<FoodContainer>().GetPoolObjType()) return;
            }
        }

        if (_currentInteractionObject != null && _currentInteractionObject.GameObject.active == true)
            return;

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
        if (_staffController != null)
        {
            if (other.GetComponentInParent<FoodContainer>() != null && _staffController.foodContainer != null)
            {
                if (_staffController.foodContainer.GetPoolObjType() !=
                other.GetComponentInParent<FoodContainer>().GetPoolObjType()) return;
            }
        }

        if (_currentInteractionObject == null && other.CompareTag(ObjectTagString.InteractionableTag))
        {
            if (other.transform.parent.TryGetComponent(out IIneractionable interactionObject))
            {
                _currentInteractionObject = interactionObject;
                _currentInteractionObject.EnterInteraction(_agentController);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Exit");
        Debug.Log(_currentInteractionObject);

        if (_staffController != null)
        {
            if (other.GetComponentInParent<FoodContainer>() != null && _staffController.foodContainer != null)
            {
                if (_staffController.foodContainer.GetPoolObjType() !=
                other.GetComponentInParent<FoodContainer>().GetPoolObjType()) return;
            }
        }

        if (_currentInteractionObject != null)
        {
            _currentInteractionObject.ExitInteraction(_agentController);
        }

        _currentInteractionObject = null;
    }
}
