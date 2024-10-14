using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AyunDefine;

public class BoxContainer : MonoBehaviour, IIneractionable
{
    private Stack<ITakeable> _boxStack;
    private int _currentBoxCnt => _boxStack.Count;
    public bool IsStackMax => _currentBoxCnt >= _stackMaxCnt;

    [Header("Box")]
    [SerializeField] private Transform _spawnTrm;
    [SerializeField] private PoolableType _poolObjType;
    [Range(0, 5)][SerializeField] private float _spacingY;

    private bool _isEnterInteraction = false;
    private bool _isBoxTaking = false;

    private PlayerController _playerController;
    private NotifyImageComponent _notifyImageComponent;
    private BoxTruck _boxTruck;

    #region 나중에 업그레이드로 빼야할 것들
    private int _stackMaxCnt = 8; // 스택에 쌓이는 음식 개수
    private int _takeBoxCnt = 4; // 가져가는 박스의 개수
    #endregion

    private void Awake()
    {
        // 플레이어 나중에 싱글톤으로 만들기
        _playerController = FindObjectOfType<PlayerController>();
        _notifyImageComponent = GetComponentInChildren<NotifyImageComponent>();
        _boxTruck = GetComponentInChildren<BoxTruck>();
        _boxStack = new Stack<ITakeable>();
    }

    #region Action
    private void OnEnable()
    {
        _boxTruck.OnTruckArrival += HandleTruckArrival;
    }

    private void OnDisable()
    {
        _boxTruck.OnTruckArrival -= HandleTruckArrival;
    }
    #endregion

    public void HandleTruckArrival()
    {
        _isBoxTaking = true;
        StartCoroutine(GiveBoxRoutine());
    }

    // 박스 넘겨주기
    private IEnumerator GiveBoxRoutine()
    {
        for (int i = 0; i < _takeBoxCnt; ++i)
        {
            while (false == _boxStack.Count <= 0 && _isBoxTaking == true)
            {
                // StackMaxCnt가 안 넘을 때 까지 대기
                yield return null;
            }

            ITakeable takeable = _boxStack.Peek();
            takeable.Take(_boxTruck.transform, _boxTruck.EndTrm.position, Vector3.zero);
            yield return new WaitForSeconds(1f);
            GameObject go = (takeable as MonoBehaviour)?.gameObject;
            PoolManager.Instance.Push(_poolObjType.ToString(), go);
        }
        _isBoxTaking = false;
        _boxTruck.GoWithBox();
    }

    public void EnterInteraction()
    {
        _isEnterInteraction = true;
        _notifyImageComponent.SetNotifySensorImage(1.1f);
        StartCoroutine(StackBoxRoutine());
    }

    public void ExitInteraction()
    {
        _isEnterInteraction = false;
        StopCoroutine(StackBoxRoutine());
        _notifyImageComponent.SetNotifySensorImage(1.0f);
    }

    private IEnumerator StackBoxRoutine()
    {
        while (_isEnterInteraction)
        {
            if (_playerController.CanGiveFood(_poolObjType) && false == IsStackMax)
            {
                ITakeable food = _playerController.OnGiveFood?.Invoke();
                TakeBox(food);
            }
            yield return new WaitForSeconds(0.15f);
        }
    }

    private void TakeBox(ITakeable food)
    {
        Vector3 foodPos = new Vector3(0,
                                      _spawnTrm.position.y + (_spacingY * _boxStack.Count),
                                      0);
        food.Take(_spawnTrm, foodPos, Vector3.zero);
        _boxStack.Push(food);
    }
}
