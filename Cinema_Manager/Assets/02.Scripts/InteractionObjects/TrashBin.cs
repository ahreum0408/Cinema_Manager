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
        while (_isEnterInteraction)
        {
            if (_playerController.CanGiveTakeable(_poolObjType))
            {
                ITakeable trash = _playerController.OnGiveTakeable?.Invoke();
                TakeTrash(trash);
            }
            yield return new WaitForSeconds(0.15f);
        }
    }

    private void TakeTrash(ITakeable trash)
    {
        trash.Take(_trashContainerTrm, Vector3.zero, Vector3.zero);
    }
}
