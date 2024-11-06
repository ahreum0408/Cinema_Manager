using UnityEngine;

public class CustomerSpawnManager : MonoSingleton<CustomerSpawnManager>
{
    [SerializeField] private Customer basicCustomer;
    [SerializeField] private Customer callCustomer;
    [SerializeField] private Customer sleepCustomer;
    [SerializeField] private Customer parcelCustomer;

    [SerializeField] private int maxCustomer = 3;
    private int currentCustomer = 0;

    [SerializeField] private float spawnCoolTime = 3f;
    private float spawnTime;

    private bool isParcel = false;

    private void Update()
    {
        if (spawnCoolTime < Time.time - spawnTime && currentCustomer < maxCustomer)
        {
            if(ObjectManager.Instance.CanUseDisplayStand())
                SpawnRandomCustomer();
        }
    }

    private void SpawnRandomCustomer()
    {
        int rand = Random.Range(0, 100);
        Customer selectedCustomer;

        if (rand < 60)
            selectedCustomer = basicCustomer;
        else if (rand < 65)
            selectedCustomer = callCustomer;
        else if (rand < 70)
            selectedCustomer = sleepCustomer;
        else
        {
            if (isParcel)
                selectedCustomer = parcelCustomer;
            else
                selectedCustomer = basicCustomer;
        }

        PoolManager.Instance.Pop(selectedCustomer.CurrentCustomerType.ToString() + "Customer", 
            transform.position, Quaternion.identity);

        currentCustomer++;
        spawnTime = Time.time;
    }

    // 택배가 해금되면 true 하기
    private void SetIsParcel(bool parcel)
    {
        isParcel = parcel;
    }

    // display 1개가 해금되면 max 3명씩 늘어나게하기
    public void SetMaxCustomer()
    {
        maxCustomer += 2;
    }

    public void MinusCustomer()
    {
        currentCustomer--;
    }

    public void SetSpawnCoolTime(float spawnCool)
    {
        spawnCoolTime = spawnCool;
    }
}
