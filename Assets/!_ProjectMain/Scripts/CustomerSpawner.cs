using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    public int customerLimit;
    public int currCustomerCount;
    public float maxDelayTime;
    public float currDelayTime;
    public GameObject customerPrefab;

    public Transform spawnPoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (currDelayTime <= 0)
        {
            // create new customer
            if (currCustomerCount < customerLimit)
            {
                Instantiate(customerPrefab, spawnPoint.position, spawnPoint.rotation);
                currCustomerCount++;
                currDelayTime = maxDelayTime;
            }
        }

        currDelayTime -= 1 * Time.deltaTime;
    }
}