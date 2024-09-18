using BehaviorDesigner.Runtime;
using UnityEngine;
using UnityEngine.AI;

public enum CustomerState
{
    Line, 
    Buy, 
    WaitSeat, 
    Seat, 
    End
}

public class Customer : MonoBehaviour
{
    [Header("Customer Type")]
    public bool isSeat; // 식탁을 사용하는 손님인가?
    public bool isBad; // 진상 손님인가?

    private CustomerState currentState;

    public NavMeshAgent Agent { get; private set; }


    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        currentState = CustomerState.Line;
    }

    private void Start()
    {
        int rand = Random.Range(0, 3);
        if (rand > 0)
            isSeat = false;
        else
            isSeat = true;
    }

    public void ChangeState(CustomerState changeState)
    {
        currentState = changeState;
    }
}

public class SharedCustomer : SharedVariable<Customer>
{
    public static implicit operator SharedCustomer(Customer value)
    {
        return new SharedCustomer { Value = value };
    }
}
