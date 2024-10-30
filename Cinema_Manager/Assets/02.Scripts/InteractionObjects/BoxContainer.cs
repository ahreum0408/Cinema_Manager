using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AyunDefine;

public class BoxContainer : MonoBehaviour, IIneractionable
{
    public Transform staffPoint;

    private Stack<ITakeable> _boxStack;
    private int _currentBoxCnt => _boxStack.Count;
    public bool IsStackMax => _currentBoxCnt >= _stackMaxCnt;

    [Header("Box")]
    [SerializeField] private PoolableType _poolObjType;
    [SerializeField] private Transform _spawnTrm; // 박스에 스폰될 때 위치
    [SerializeField] private Transform _moveTrm; // 트럭으로 이동할 때 갈 위치
    [Range(0, 5)][SerializeField] private float _spacingY;

    [Header("Effect")]
    [SerializeField] private Transform _smokeEffectSpawnTrm;
    [SerializeField] private ParticleSystem _smokeEffect;

    private bool _isEnterInteraction = false;
    private bool _isBoxGiving = false; // 플레이어가 박스 주고있는지
    private bool _isBoxTaking = false; // 박스 가지고 오고 있는지

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
        _playerController = PlayerManager.Instance.PlayerController;
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
        StartCoroutine(GiveBoxRoutine());
    }

    // 박스 넘겨주기
    private IEnumerator GiveBoxRoutine()
    {
        for (int i = 0; i < _takeBoxCnt; ++i)
        {
            yield return new WaitUntil(() => _currentBoxCnt > 0 && !_isBoxTaking && !_isBoxGiving);
            yield return new WaitForSeconds(0.1f);
            _isBoxTaking = true;

            ITakeable takeable = _boxStack.Pop();
            takeable.Take(_moveTrm, Vector3.zero, Vector3.zero);

            // 이펙트 실행
            GameObject effect = PoolManager.Instance.Pop(PoolableType.SmokeEffect.ToString(), _smokeEffectSpawnTrm);
            effect.GetComponent<ParticleSystem>().Play();

            yield return new WaitForSeconds(0.5f);

            GameObject go = (takeable as MonoBehaviour)?.gameObject;
            PoolManager.Instance.Push(_poolObjType.ToString(), go);

            _isBoxTaking = false;
        }
        _boxTruck.GoWithBox();
        // 이때 돈 받으면 될 듯 (택배비)
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
            if (_playerController.CanGiveTakeable(_poolObjType) && false == IsStackMax)
            {
                _isBoxGiving = true;
                ITakeable box = _playerController.OnGiveTakeable?.Invoke();
                TakeBox(box);
            }
            else
            {
                yield return new WaitForSeconds(0.5f);
                _isBoxGiving = false;
            }
            yield return new WaitForSeconds(0.15f);
        }
    }

    private void TakeBox(ITakeable box)
    {
        Vector3 boxPos = new Vector3(0, _spacingY * _boxStack.Count, 0);
        box.Take(_spawnTrm, boxPos, Vector3.zero);
        _boxStack.Push(box);
    }
}
