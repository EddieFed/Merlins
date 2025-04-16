using System.Collections.Generic;
using UnityEngine;

public class SlimeSpawner : MonoBehaviour
{
    public int slimeLimit;
    public static int currSlimeCount;
    public GameObject slimePrefab;
    public Transform spawnPoint;
    public float maxDelayTime;
    public float minDelayTime;
    public float randomDelay;
    public float currDelayTime;
    private static List<GameObject> _shelfLocations;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _shelfLocations = new List<GameObject>();
        GameObject.FindGameObjectsWithTag("Shelf", _shelfLocations);
        
        randomDelay = Random.Range(minDelayTime, maxDelayTime);
        currDelayTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        currDelayTime += Time.deltaTime;
        if (currDelayTime >= randomDelay && currSlimeCount < slimeLimit)
        {
            Instantiate(slimePrefab, spawnPoint.position, spawnPoint.rotation);
            currSlimeCount++;
            randomDelay = Random.Range(minDelayTime, maxDelayTime);
            currDelayTime = 0;
        }
    }
    
    public static Transform GetShelf()
    {
        return _shelfLocations[Random.Range(0, _shelfLocations.Count)].transform;
    }
    
    public static Transform GetShelfLocation(Transform shelf)
    {
        return shelf.GetComponent<ItemCounter>().destinations[Random.Range(0, shelf.GetComponent<ItemCounter>().destinations.Count)];
    }
}
