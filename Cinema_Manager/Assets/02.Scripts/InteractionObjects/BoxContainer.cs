using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AyunDefine;

public class BoxContainer : MonoBehaviour, IIneractionable, IOpenTarget
{
    [HideInInspector] public GameObject GameObject => gameObject;
    [SerializeField] private TargetType _targetType;

    public Transform staffPoint;

    private Stack<ITakeable> _boxStack;
    private int _currentBoxCnt => _boxStack.Count;
    public bool IsStackMax => _currentBoxCnt >= _stackMaxCnt;

    private bool _isOpen;
    public bool IsOpen { get => _isOpen; set => _isOpen = value; }
    public TargetType Type { get => _targetType; set => _targetType = value; }

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

    private NotifyImageComponent _notifyImageComponent;
    private BoxTruck _boxTruck;
    private MoneyDummy _moneyDummy;

    #region 나중에 업그레이드로 빼야할 것들
    private int _stackMaxCnt = 8; // 스택에 쌓이는 음식 개수
    private int _takeBoxCnt = 4; // 가져가는 박스의 개수
    #endregion

    private void Awake()
    {
        _notifyImageComponent = GetComponentInChildren<NotifyImageComponent>();
        _boxTruck = GetComponentInChildren<BoxTruck>();
        _moneyDummy = transform.GetComponentInChildren<MoneyDummy>();
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

            // Effect
            GameObject effect = PoolManager.Instance.Pop(PoolableType.SmokeEffect.ToString(), _smokeEffectSpawnTrm);
            effect.GetComponent<ParticleSystem>().Play();

            // Sound
            SoundManager.Instance.Play(AudioClips.BoxPacking, true, 1, transform);

            // Money
            _moneyDummy.AddMoneyObject(4);

            yield return new WaitForSeconds(0.5f);

            GameObject go = (takeable as MonoBehaviour)?.gameObject;
            PoolManager.Instance.Push(_poolObjType.ToString(), go);

            _isBoxTaking = false;
        }
        _boxTruck.GoWithBox();
        // 이때 돈 받으면 될 듯 (택배비)
    }

    public void EnterInteraction(AgentController agent)
    {
        _isEnterInteraction = true;
        _notifyImageComponent.SetNotifySensorImage(1.1f);
        StartCoroutine(StackBoxRoutine(agent));
    }

    public void ExitInteraction(AgentController agent)
    {
        _isEnterInteraction = false;
        StopCoroutine(StackBoxRoutine(agent));
        _notifyImageComponent.SetNotifySensorImage(1.0f);
    }

    private IEnumerator StackBoxRoutine(AgentController agent)
    {
        while (_isEnterInteraction)
        {
            if (agent.CanGiveTakeable(_poolObjType) && false == IsStackMax)
            {
                _isBoxGiving = true;
                ITakeable box = agent.OnGiveTakeable?.Invoke();
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

    public void ActiveObj(bool active , bool on = false) {
        _isOpen = active;
        gameObject.SetActive(active);
        LevelEvents.ChangeBoxTruckActiveEvent?.Invoke(this, active);
    }

    public void ScaleSetting()
    {
        float time = 0.5f;
        Vector3 originScale = transform.localScale;
        transform.localScale = Vector3.zero;
        transform.DOScale(originScale, time).SetEase(Ease.OutBack);
    }
}
