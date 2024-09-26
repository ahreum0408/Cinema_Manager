using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject customerPrefab;
    [SerializeField] private int maxCustomer;
    [SerializeField] private float spawnCoolTime;

    private int currentCustomer = 0;
    private float spawnTime;

    private void Start()
    {
        currentCustomer++;
        Instantiate(customerPrefab, transform.position, Quaternion.identity);
        spawnTime = Time.time;
    }

    private void Update()
    {
        if(spawnCoolTime < Time.time - spawnTime && currentCustomer <= maxCustomer)
        {
            currentCustomer++;
            Instantiate(customerPrefab, transform.position, Quaternion.identity);
            spawnTime = Time.time;
        }
    }
}
