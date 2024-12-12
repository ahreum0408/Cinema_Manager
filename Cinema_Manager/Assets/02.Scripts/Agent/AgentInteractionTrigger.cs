using UnityEngine;
using static AyunDefine;

public class AgentInteractionTrigger : MonoBehaviour
{
    private AgentController _agentController;
    private StaffController _staffController;
    private IIneractionable _currentInteractionObject;

    private void OnTriggerEnter(Collider other)
    {
        if (_staffController != null)
        {
            if (other.GetComponentInParent<BuyChecker>() != null) return;

            if (other.GetComponentInParent<FoodContainer>() != null && _staffController.foodContainer != null)
            {
                if (_staffController.foodContainer.GetPoolObjType() !=
                other.GetComponentInParent<FoodContainer>().GetPoolObjType()) return;
            }
            if (other.GetComponentInParent<DisplayStand>() != null && _staffController.displayStand != null)
            {
                if (other.GetComponentInParent<DisplayStand>() != _staffController.displayStand) return;
            }
        }

        if (_currentInteractionObject != null && _currentInteractionObject.GameObject.activeSelf == true)
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
            if (other.GetComponentInParent<BuyChecker>() != null) return;

            if (other.GetComponentInParent<FoodContainer>() != null && _staffController.foodContainer != null)
            {
                if (_staffController.foodContainer.GetPoolObjType() !=
                other.GetComponentInParent<FoodContainer>().GetPoolObjType()) return;
            }
            if (other.GetComponentInParent<DisplayStand>() != null && _staffController.displayStand != null)
            {
                if (other.GetComponentInParent<DisplayStand>() != _staffController.displayStand) return;
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
        if (_staffController != null)
        {
            if (other.GetComponentInParent<BuyChecker>() != null) return;

            if (other.GetComponentInParent<FoodContainer>() != null && _staffController.foodContainer != null)
            {
                if (_staffController.foodContainer.GetPoolObjType() !=
                other.GetComponentInParent<FoodContainer>().GetPoolObjType()) return;
            }

            if (other.GetComponentInParent<DisplayStand>() != null && _staffController.displayStand != null)
            {
                if (other.GetComponentInParent<DisplayStand>() != _staffController.displayStand) return;
            }
        }

        if (_currentInteractionObject != null)
        {
            _currentInteractionObject.ExitInteraction(_agentController);
        }

        _currentInteractionObject = null;
    }

    private void Awake()
    {
        _agentController = GetComponent<AgentController>();
        _staffController = _agentController as StaffController;
    }
}
