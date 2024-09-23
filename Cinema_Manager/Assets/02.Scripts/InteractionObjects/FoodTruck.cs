using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using ObjectPooling;

public class FoodTruck : MonoBehaviour
{
    [Header("Food")]
    [SerializeField] private ObjectPool.PoolObjectType _poolObjType;

    [Header("Truck Move Setting")]
    [SerializeField] private Transform _visualTrm;
    [SerializeField] private Transform _startTrm;
    [SerializeField] private Transform _endTrm;
    [SerializeField] private float _moveTime = 5;

    private FoodBox _foodBox;

    private void Awake()
    {
        _foodBox = GetComponentInParent<FoodBox>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            GoTakeFood();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            BringBackFood();
        }
    }

    // 음식 가지러 가기
    public void GoTakeFood()
    {
        TruckMove(_startTrm.position);
    }

    // 음식 가져옴
    public void BringBackFood()
    {
        PoolableMono food = PoolManager.Instance.Pop(_poolObjType);
        // foodBox한테 food넘겨주기 (함수)
        TruckMove(_endTrm.position);
    }

    private void TruckMove(Vector3 targetPos)
    {
        if (_visualTrm.position == targetPos) return;

        _visualTrm.DOMove(targetPos, _moveTime).SetEase(Ease.InBack);
    }
}
