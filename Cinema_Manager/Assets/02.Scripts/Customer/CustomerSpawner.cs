using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject customerPrefab;
    [SerializeField] private float spawnCoolTime;

    private float spawnTime;

    private void Start()
    {
        Instantiate(customerPrefab, transform.position, Quaternion.identity);
        spawnTime = Time.time;
    }

    private void Update()
    {
        if(spawnCoolTime < Time.time - spawnTime)
        {
            Instantiate(customerPrefab, transform.position, Quaternion.identity);
            spawnTime = Time.time;
        }
    }
}
