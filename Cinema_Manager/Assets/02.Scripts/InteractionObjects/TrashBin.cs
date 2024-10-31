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

    public void EnterInteraction(Collider collider)
    {
        _isEnterInteraction = true;
        StartCoroutine(TakeTrashRoutine(collider));
        _notifyImageComponent.SetNotifySensorImage(1.1f);
    }

    public void ExitInteraction(Collider collider)
    {
        _isEnterInteraction = false;
        StopCoroutine(TakeTrashRoutine(collider));
        _notifyImageComponent.SetNotifySensorImage(1f);
    }

    private IEnumerator TakeTrashRoutine(Collider collider)
    {
        SoundManager.Instance.Play(AudioClips.Trashcan, 1, null, false);

        while (_isEnterInteraction)
        {
            // Player
            if (collider.TryGetComponent(out PlayerController player))
            {
                if (player.IsStacked)
                {
                    ITakeable takeable = player.OnGiveTakeable?.Invoke();
                    takeable.Take(_trashContainerTrm, Vector3.zero, Vector3.zero);
                }
            }
            // Staff
            else if (collider.TryGetComponent(out StaffController staff))
            {
                if (staff.IsStacked)
                {
                    ITakeable takeable = staff.OnGiveTakeable?.Invoke();
                    takeable.Take(_trashContainerTrm, Vector3.zero, Vector3.zero);
                }
            }
            yield return new WaitForSeconds(0.15f);
        }
    }
}
