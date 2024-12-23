using System.Collections.Generic;
using UnityEngine;
using static AyunDefine;

public class CustomerSpawnManager : MonoSingleton<CustomerSpawnManager>
{
    [Header("Customer Mat")]
    [SerializeField] private List<Material> _skinMat = new List<Material>();
    [SerializeField] private List<Material> _shirtMat = new List<Material>();
    [SerializeField] private List<Material> _pantMat = new List<Material>();

    [Header("Customers")]
    [SerializeField] private Customer basicCustomer;
    [SerializeField] private Customer callCustomer;
    [SerializeField] private Customer sleepCustomer;
    [SerializeField] private Customer parcelCustomer;

    [SerializeField] private int maxCustomer = 3;
    private int currentCustomer = 0;

    [SerializeField] private float spawnCoolTime = 3f;
    private float spawnTime;

    private bool isParcel = false;

    public int FoodTypeSum = 1;

    private void Update()
    {
        if (spawnCoolTime < Time.time - spawnTime && currentCustomer < maxCustomer)
        {
            if (ObjectManager.Instance.CanUseDisplayStand())
                SpawnRandomCustomer();
        }
    }

    private void SpawnRandomCustomer()
    {
        spawnCoolTime = Random.Range(1.5f, 3f);

        int rand = Random.Range(0, 100);
        Customer selectedCustomer = basicCustomer;

        if (rand < 79)
            selectedCustomer = basicCustomer;
        else if (rand < 94)
        {
            if (isParcel && ObjectManager.Instance.parcelService.BoxStackCheck())
                selectedCustomer = parcelCustomer;
            else
                selectedCustomer = basicCustomer;
        }
        else if (rand < 97)
            selectedCustomer = callCustomer;
        else if (rand < 100)
            selectedCustomer = sleepCustomer;

        GameObject customer = PoolManager.Instance.Pop
            (selectedCustomer.CurrentCustomerType.ToString() + "Customer",
            transform.position, Quaternion.identity);

        RandomCustomerSet(customer, CustomerSetType.skin, _skinMat);
        RandomCustomerSet(customer, CustomerSetType.tshirt, _shirtMat);
        RandomCustomerSet(customer, CustomerSetType.shorts, _pantMat);

        currentCustomer++;
        spawnTime = Time.time;
    }

    private void RandomCustomerSet(GameObject customer, CustomerSetType type, List<Material> matList)
    {
        if (type == CustomerSetType.skin)
        {
            int rand = Random.Range(0, matList.Count);

            SkinnedMeshRenderer renderer = customer.transform.Find("Visual")
                .transform.Find("Male_body")
                .GetComponent<SkinnedMeshRenderer>();
            renderer.material = matList[rand];

            renderer = customer.transform.Find("Visual")
                .transform.Find("Male_hair")
                .GetComponent<SkinnedMeshRenderer>();
            renderer.material = matList[rand];

        }
        else
        {
            SkinnedMeshRenderer renderer = customer.transform.Find("Visual")
                .transform.Find("Male_" + type.ToString())
                .GetComponent<SkinnedMeshRenderer>();
            renderer.material = matList[Random.Range(0, matList.Count)];
        }
    }

    // 택배가 해금되면 true 하기
    public void SetIsParcel(bool parcel)
    {
        isParcel = parcel;
    }

    // display 1개가 해금되면 max 2명씩 늘어나게하기
    public void SetMaxCustomer(PoolableType foodType)
    {
        switch (foodType)
        {
            case PoolableType.TriangleKimbap:
            case PoolableType.CupRamen:
            case PoolableType.Snack:
            case PoolableType.Juice:
            case PoolableType.Coffee:
            case PoolableType.Bread:
                maxCustomer += 3;
                break;
            case PoolableType.Coke:
            case PoolableType.Beer:
            case PoolableType.Soju:
                maxCustomer += 2;
                break;
            default:
                Debug.LogWarning("올바르지 못한 형식");
                break;
        }
    }

    // 음식 종류가 증가할 때 마다(스탠드 갯수 X, 음식 종류)
    public void SetFoodTypeSum()
    {
        FoodTypeSum++;
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
