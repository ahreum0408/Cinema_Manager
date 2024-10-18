using BehaviorDesigner.Runtime.ObjectDrawers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static AyunDefine;

public class FoodContainer : MonoBehaviour, IIneractionable
{
    private Stack<ITakeable> _foodStack;
    private int _currentFoodCnt => _foodStack.Count;
    public bool IsStackMax => _currentFoodCnt >= _stackMaxCnt;

    [Header("Food")]
    [SerializeField] private Transform _spawnTrm;
    [SerializeField] private PoolableType _poolObjType;
    [SerializeField] private bool _isFood;
    [Range(0, 5)] [SerializeField] private float _spacingX;
    [Range(0, 5)] [SerializeField] private float _spacingZ; // 밑으로 내려가야 하기 때문에 음수로 바꿔 사용
    [Range(0, 5)] [SerializeField] private float _spacingY;

    private bool _isFoodFilling = false;
    private bool _isEnterInteraction = false;

    private PlayerController _playerController;
    private NotifyImageComponent _notifyImageComponent;
    private FoodTruck _foodTruck;

    #region 나중에 업그레이드로 빼야할 것들
    private int _stackMaxCnt = 8; // 스택에 쌓이는 음식 개수
    private int _spawnFoodCnt = 4; // 스폰되는 음식 개수
    #endregion

    private void Awake()
    {
        // 플레이어 나중에 싱글톤으로 만들기
        _playerController = FindObjectOfType<PlayerController>();
        _notifyImageComponent = GetComponentInChildren<NotifyImageComponent>();
        _foodTruck = GetComponentInChildren<FoodTruck>();
        _foodStack = new Stack<ITakeable>();
    }

    #region Action
    private void OnEnable()
    {
        _foodTruck.OnBringFood += HandleBringFood;
    }

    private void OnDisable()
    {
        _foodTruck.OnBringFood -= HandleBringFood;
    }
    #endregion

    public void HandleBringFood()
    {
        _isFoodFilling = true;
        StartCoroutine(FillingFoodRoutine());
    }

    private IEnumerator FillingFoodRoutine()
    {
        for (int i = 0; i < _spawnFoodCnt; ++i)
        {
            yield return new WaitUntil(() => false == IsStackMax);

            int posInGroup = _currentFoodCnt % 4; // 0, 1, 2, 3 순서로 반복

            float x = _spacingX * (posInGroup % 2 == 1 ? 1 : 0);
            float z = _spacingZ * (posInGroup < 2 ? 0 : -1);
            float y = _spawnTrm.position.y + (_spacingY * (_currentFoodCnt / 4)); // 4개 마다 위로

            Vector3 localPos = new Vector3(x, y, z);
            // 음료가 아니라면 90도 돌려서 배치
            Quaternion quaternion = _isFood == true? Quaternion.Euler(-90, 0, 0) : Quaternion.Euler(0, 0, 0);

            GameObject food = PoolManager.Instance.Pop(_poolObjType.ToString(), _spawnTrm, localPos, quaternion);
            _foodStack.Push(food.GetComponent<ITakeable>());

            yield return new WaitForSeconds(0.25f);
        }
        _isFoodFilling = false;
        _foodTruck.GoTakeFood();
    }

    public void EnterInteraction()
    {
        _isEnterInteraction = true;
        _notifyImageComponent.SetNotifySensorImage(1.1f);
        StartCoroutine(GetFoodRoutine());
    }   

    public void ExitInteraction()
    {
        _isEnterInteraction = false;
        StopCoroutine(GetFoodRoutine());
        _notifyImageComponent.SetNotifySensorImage(1.0f);
    }

    private IEnumerator GetFoodRoutine()
    {
        while (_isEnterInteraction)
        {
            if (_currentFoodCnt > 0)
            {
                ITakeable takeable = _foodStack.Peek();

                if (_playerController.CanTakeFood(_poolObjType))
                {
                    _playerController.OnTakeTakeable?.Invoke(_foodStack.Pop(), _poolObjType, _spacingY, _isFood);
                    yield return new WaitForSeconds(0.15f);
                }
            }
            yield return null;
        }
    }
}
