using System.Collections;
using UnityEngine;
using static AyunDefine;
public class TrashBin : MonoBehaviour, IIneractionable
{
    [SerializeField] private Transform _trashContainerTrm; // 쓰레기가 이동해야할 위치
    private PoolableType _poolObjType = PoolableType.Trash;

    private bool _isEnterInteraction = false;

    private PlayerController _playerController;
    private NotifyImageComponent _notifyImageComponent;

    private void Awake()
    {
        // 플레이어 나중에 싱글톤으로 만들기
        _playerController = FindObjectOfType<PlayerController>();
        _notifyImageComponent = GetComponentInChildren<NotifyImageComponent>();
    }

    public void EnterInteraction()
    {
        _isEnterInteraction = true;
        _notifyImageComponent.SetNotifySensorImage(1.1f);
        StartCoroutine(TakeTrashRoutine());
    }

    public void ExitInteraction()
    {
        _isEnterInteraction = false;
        _notifyImageComponent.SetNotifySensorImage(1f);
    }

    private IEnumerator TakeTrashRoutine()
    {
        // 걍 들어올 떄 마다 할거면 if문 없애주면됨
        if (_playerController.IsStacked)
            SoundManager.Instance.Play(AudioClips.Trashcan);

        while (_isEnterInteraction)
        {
            if (_playerController.IsStacked)
            {
                ITakeable takeable = _playerController.OnGiveTakeable?.Invoke();
                takeable.Take(_trashContainerTrm, Vector3.zero, Vector3.zero);
            }
            yield return new WaitForSeconds(0.15f);
        }
    }
}
