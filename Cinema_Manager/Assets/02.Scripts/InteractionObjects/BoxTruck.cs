using DG.Tweening;
using System;
using UnityEngine;
using static AyunDefine;

public class BoxTruck : MonoBehaviour
{
    public Action OnTruckArrival;

    [Header("Truck Move Setting")]
    [SerializeField] private Transform _visualTrm;
    [SerializeField] private Transform _startTrm;
    [SerializeField] private Transform _endTrm;
    public Transform EndTrm =>_endTrm;
    [SerializeField] private float _moveTime = 3;

    private bool _isWithBox = false;

    #region 나중에 업그레이드로 빼야할 것들
    private float _truckBringTime = 8f; // 음식 가져오는데 걸리는 시간
    // 이 시간은 _moveTime * 2보다 커야함
    #endregion
    private float _currentBringTime = 0;

    private BoxContainer _boxContainer;

    private void Awake()
    {
        _boxContainer = GetComponentInParent<BoxContainer>();
    }

    private void Start()
    {
        _boxContainer.OnScaleSettingEndEvent += HandleOnScaleSettingEndEvent;

        _isWithBox = false;
        _visualTrm.gameObject.SetActive(false);
    }

    private void HandleOnScaleSettingEndEvent()
    {
        _visualTrm.DOKill();

        _currentBringTime = _truckBringTime;
        _isWithBox = true;

        _visualTrm.position = _startTrm.position;
        _visualTrm.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (_isWithBox)
        {
            _currentBringTime += Time.deltaTime;
            if (_currentBringTime >= _truckBringTime - _moveTime) // 이거 시간 재는거 확인해보기
            {
                _currentBringTime = 0;
                _isWithBox = false;
                GoGetBox();
            }
        }
    }

    // 박스 가지고 (처리하러) 가기
    public void GoWithBox()
    {
        // Sound
        SoundManager.Instance.Play(AudioClips.TruckStart, true, 1, _visualTrm);
        SoundManager.Instance.Play(AudioClips.TruckHorn, true, 1, _visualTrm);

        _isWithBox = true;
        TruckMove(_startTrm.position, false);
    }

    // 박스 가지러 가기
    public void GoGetBox()
    {
        TruckMove(_endTrm.position, true);
    }

    private void TruckMove(Vector3 targetPos, bool isBringFood)
    {
        _visualTrm.DOMove(targetPos, _moveTime)
            .SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                if (isBringFood)
                {
                    // Sound
                    SoundManager.Instance.Play(AudioClips.TruckStop, true, 1, _visualTrm);

                    OnTruckArrival?.Invoke();
                }
            });
    }
}
