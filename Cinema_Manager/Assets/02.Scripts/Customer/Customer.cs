using BehaviorDesigner.Runtime;
using UnityEngine;
using UnityEngine.AI;

public class Customer : MonoBehaviour
{
    [Header("Customer Type")]
    public bool isBuy = false; // 구매 완료? 물건 다 받았냐
    public bool isSeat; // 식탁을 사용하는 손님인가?
    public bool isBad; // 진상 손님인가?

    [Header("Buy Type")]
    // 나중에 물건 타입도 추가?
    [SerializeField] private int maxBuySum = 3;
    public int wantBuy; // 원하는 수량
    public int currentBuy; // 현재 받은 수량

    public Vector3 startPos;

    public NavMeshAgent Agent { get; private set; }
    public Counter Counter {  get; private set; }
    public Table Table { get; private set; }

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        Counter = FindObjectOfType<Counter>();
        Table = FindObjectOfType<Table>();
    }

    private void Start()
    {
        startPos = transform.position;
        int rand = Random.Range(0, 3);
        if (rand > 0)
            isSeat = false;
        else
            isSeat = true;
        SelectBuySum();
    }

    public Chair CanSeatChair()
    {

        foreach(var c in Table.chairs)
        {
            if(!c.IsUsing && !c.IsDirty)
            {
                c.IsUsing = true;
                c.IsDirty = true;
                return c;
            }
        }
        return null;
    }

    private void SelectBuySum()
    {
        wantBuy = Random.Range(1, maxBuySum + 1);
    }
}

public class SharedCustomer : SharedVariable<Customer>
{
    public static implicit operator SharedCustomer(Customer value)
    {
        return new SharedCustomer { Value = value };
    }
}
