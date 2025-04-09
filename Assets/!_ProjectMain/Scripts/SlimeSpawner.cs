using UnityEngine;

public class SlimeSpawner : MonoBehaviour
{
    public int slimeLimit;
    public static int currSlimeCount;
    public GameObject slimePrefab;
    public Transform spawnPoint;
    public float maxDelayTime;
    public float currDelayTime;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currSlimeCount < slimeLimit)
        {
            Instantiate(slimePrefab, spawnPoint.position, spawnPoint.rotation);
            currSlimeCount++;
            currDelayTime = maxDelayTime;
        }
    }
}
