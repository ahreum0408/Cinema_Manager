using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static AyunDefine;

public class Point : MonoBehaviour
{
    public Transform holder;

    public bool IsUsing; //손님이 사용 중?
    public bool IsDirty; //이 자리가 더러운가?

    public GameObject trash;

    public PoolableType currentFoodType;

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
        currentFoodType = customer.StackCompo.CurrentHoldType;
        StartCoroutine(TakeFoodRoutine(customer, spacingY));
    }

    private IEnumerator TakeFoodRoutine(Customer customer, float spacingY)
    {
        _currentFoodCnt++;
        ITakeable food = customer.OnGiveTakeable?.Invoke();
        
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
            _currentFoodCnt--;

            ITakeable food = _foodStack.Peek();
            PoolManager.Instance.Push(currentFoodType.ToString(), 
                holder.GetChild(holder.childCount -1).GetComponent<GameObject>());

            _foodStack.Pop();
            RemoveTrash();
        }
    }

    public void RemoveTrash()
    {
        PoolManager.Instance.Push(PoolableType.Trash.ToString(), trash);
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
