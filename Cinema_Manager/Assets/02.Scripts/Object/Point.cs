using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    public Transform holder;

    public bool IsUsing; //손님이 사용 중?
    public bool IsDirty; //이 자리가 더러운가?

    private Stack<ITakeable> _foodStack;

    private bool _isEnterInteraction = false;
    private int _currentFoodCnt;

    private Customer _currentCustomer;

    private bool _isStart;

    private void Awake()
    {
        _foodStack = new Stack<ITakeable>();
        holder = GetComponentInChildren<Transform>();
    }

    private void Start()
    {
        _currentFoodCnt = 0;
        _isStart = true;
    }

    public void TakeFood(Customer customer, float spacingY)
    {
        StartCoroutine(TakeFoodRoutine(customer, spacingY));
    }

    private IEnumerator TakeFoodRoutine(Customer customer, float spacingY)
    {
        while (_isEnterInteraction)
        {
            _currentFoodCnt++;
            ITakeable food = customer.OnGiveFood?.Invoke();

            Vector3 foodPos = new Vector3
                (customer.currentChair.holder.position.x,
                customer.currentChair.holder.position.y + _currentFoodCnt * spacingY,
                customer.currentChair.holder.position.z);

            food.Take(customer.transform, foodPos, Vector3.zero);
            _foodStack.Push(food);

            yield return new WaitForSeconds(0.15f);
        }
    }

    public void EatFood()
    {
        _foodStack.Pop();
    }

    public void ChangeUsingState(bool isUse)
    {
        IsUsing = isUse;
    }

    public void ChangeDirtyState(bool isDirty)
    {
        IsDirty = isDirty; 
    }
}
