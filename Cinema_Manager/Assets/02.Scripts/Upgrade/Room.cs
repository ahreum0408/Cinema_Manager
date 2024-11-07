using UnityEngine;

public class Room : MonoBehaviour, IOpenTarget {
    [SerializeField] private GameObject _openTarget;

    private bool _isOpen;
    public bool IsOpen { get => _isOpen; set => _isOpen = value; }

    public void ActiveObj(bool active, bool on = false) {
        _isOpen = active;
        gameObject.SetActive(!active);
        _openTarget.gameObject.SetActive(active);
        LevelEvents.ChangeRoomActiveEvent?.Invoke(this, active);
    }
}
