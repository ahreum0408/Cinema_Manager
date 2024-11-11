using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static AyunDefine;

public class Point : MonoBehaviour, IIneractionable
{
    [HideInInspector] public GameObject GameObject => gameObject;
    public Transform holder;

    public bool IsUsing; //손님이 사용 중?
    public bool IsDirty; //이 자리가 더러운가?

    public GameObject trash;

    public PoolableType currentFoodType;

    public Stack<ITakeable> foodStack;
    private int _currentFoodCnt => foodStack.Count;

    private void Awake()
    {
        foodStack = new Stack<ITakeable>();
    }

    private void Start()
    {
        trash = null;
    }

    public void TakeFood(Customer customer, float spacingY, bool isFood)
    {
        currentFoodType = customer.StackCompo.CurrentHoldType;
        StartCoroutine(TakeFoodRoutine(customer, spacingY, isFood));
    }

    private IEnumerator TakeFoodRoutine(Customer customer, float spacingY, bool isFood)
    {
        ITakeable food = customer.OnGiveTakeable?.Invoke();
        
        Vector3 foodPos = Vector3.zero;
        foodPos.z = spacingY * _currentFoodCnt;
        Vector3 rotation = isFood == false ? new Vector3(-90, 0, 0) : Vector3.zero;

        food.Take(holder.transform, foodPos, rotation);
        foodStack.Push(food);
        
        yield return new WaitForSeconds(1f);
    }

    public void EatFood()
    {
        if(foodStack != null)
        {
            PoolManager.Instance.Push(currentFoodType.ToString(), holder.GetChild(holder.childCount -1).gameObject);

            foodStack.Pop();
        }
    }

    public void ChangeUsingState(bool isUse) => IsUsing = isUse;

    public void ChangeDirtyState(bool isDirty) => IsDirty = isDirty;

    public void EnterInteraction(AgentController agent) {}

    public void ExitInteraction(AgentController agent) {}
}
