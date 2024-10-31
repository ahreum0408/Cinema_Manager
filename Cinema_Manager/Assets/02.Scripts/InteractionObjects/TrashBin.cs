using System.Collections;
using UnityEngine;
using static AyunDefine;

public class TrashBin : MonoBehaviour, IIneractionable
{
    public Transform staffPoint;

    [SerializeField] private Transform _trashContainerTrm; // 쓰레기가 이동해야할 위치

    private bool _isEnterInteraction = false;

    private NotifyImageComponent _notifyImageComponent;

    private void Awake()
    {
        _notifyImageComponent = GetComponentInChildren<NotifyImageComponent>();
    }

    public void EnterInteraction(AgentController agent)
    {
        _isEnterInteraction = true;
        StartCoroutine(TakeTrashRoutine(agent));
        _notifyImageComponent.SetNotifySensorImage(1.1f);
    }

    public void ExitInteraction(AgentController agent)
    {
        _isEnterInteraction = false;
        StopCoroutine(TakeTrashRoutine(agent));
        _notifyImageComponent.SetNotifySensorImage(1f);
    }

    private IEnumerator TakeTrashRoutine(AgentController agent)
    {
        bool isStacked = agent.GetAgentComponent<AgentStackComponent>().IsStacked;

        if (isStacked)
            SoundManager.Instance.Play(AudioClips.Trashcan, 1, null, false);

        while (_isEnterInteraction)
        {
            if (isStacked)
            {
                ITakeable takeable = agent.OnGiveTakeable?.Invoke();
                takeable.Take(_trashContainerTrm, Vector3.zero, Vector3.zero);
            }
            yield return new WaitForSeconds(0.15f);
        }
    }
}
