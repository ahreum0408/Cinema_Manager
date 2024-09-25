using BehaviorDesigner.Runtime.ObjectDrawers;
using ObjectPooling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodBox : MonoBehaviour, IIneractionable
{
    private Stack<ITakeable> _foodStack;

    [Header("Food")]
    [SerializeField] private Transform _spawnTrm;
    [SerializeField] private ObjectPool.PoolObjectType _poolObjType;
    [Range(0, 5)] [SerializeField] private float _spacingX;
    [Range(0, 5)] [SerializeField] private float _spacingZ; // 밑으로 내려가야 하기 때문에 음수로 바꿔 사용
    [Range(0, 5)] [SerializeField] private float _spacingY;

    private bool _isEnterInteraction = false;

    private int _currentFoodCnt => _foodStack.Count;

    private PlayerController _playerController;
    private FoodTruck _foodTruck;

    private void Awake()
    {
        // 플레이어 나중에 싱글톤으로 만들기
        _playerController = FindObjectOfType<PlayerController>();
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
        StartCoroutine(FillingFoodRoutine());
    }

    private IEnumerator FillingFoodRoutine()
    {
        for (int i = 0; i < 4; ++i)
        {
            PoolableMono food = PoolManager.Instance.Pop(_poolObjType);
            _foodStack.Push(food.GetComponent<ITakeable>());

            int posInGroup = (_currentFoodCnt - 1) % 4; // 0, 1, 2, 3 순서로 반복

            float x = _spacingX * (posInGroup % 2 == 1 ? 1 : 0);
            float z = _spacingZ * (posInGroup < 2 ? 0 : -1);
            float y = _spacingY * ((_currentFoodCnt - 1) / 4); // 4개 마다 위로

            Vector3 localPos = new Vector3(x, y, z);
            Vector3 spawnPos = _spawnTrm.TransformPoint(localPos); // 로컬 좌표를 월드 좌표로 변환

            food.transform.position = spawnPos;

            yield return new WaitForSeconds(0.3f);
        }
    }

    public void EnterInteraction()
    {
        _isEnterInteraction = true;
        //notifyImageComponent.SetNotifySensorImage(1.1f);
        StartCoroutine(GetFoodRoutine());
    }   

    public void ExitInteraction()
    {
        _isEnterInteraction = false;
        //notifyImageComponent.SetNotifySensorImage(1.0f);
    }

    private IEnumerator GetFoodRoutine()
    {
        while (_isEnterInteraction)
        {
            if (_currentFoodCnt > 0 ) // && 스택이 다 차지는 않았는지
            {
                Debug.Log("ㅏㅑ");

                // 가장 위에있는 음식 주기
                _playerController.OnTakeFood?.Invoke(_foodStack.Pop());
                yield return new WaitForSeconds(0.15f);
            }
            yield return null;
        }
    }
}
