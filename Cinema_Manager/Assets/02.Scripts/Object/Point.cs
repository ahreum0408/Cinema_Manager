using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    [SerializeField] private Transform holder;

    public bool IsUsing; //손님이 사용 중?
    public bool IsDirty; //이 자리가 더러운가?

    private Stack<ITakeable> _foodStack;
    private int _currentFoodCnt;

    private Customer _currentCustomer;

    private bool _isStart;

    private void Awake()
    {
        _foodStack = new Stack<ITakeable>();
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
        Debug.Log("Take Food On Table");
        _currentFoodCnt++;
        ITakeable food = customer.OnGiveFood?.Invoke();
        
        Vector3 foodPos = Vector3.zero;
        foodPos.z += spacingY * _currentFoodCnt;

        food.Take(holder.transform, foodPos, Vector3.zero);
        _foodStack.Push(food);
        
        yield return new WaitForSeconds(1f);
    }

    public void EatFood()
    {
        if(_foodStack != null)
        {
            // 음식 먹으면 줄어들기
        }
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
