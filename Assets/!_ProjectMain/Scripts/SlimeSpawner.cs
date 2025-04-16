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
    public GameObject shelveGroup;
    public static List<Transform> shelveLocations;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shelveLocations = new List<Transform>();
        foreach (Transform child in shelveGroup.transform)
        {
            shelveLocations.Add(child);
        }
        
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
        return shelveLocations[Random.Range(0, shelveLocations.Count)];
    }
    
    public static Transform GetShelfLocation(Transform shelf)
    {
        return shelf.GetComponent<ItemCounter>().destinations[Random.Range(0, shelf.GetComponent<ItemCounter>().destinations.Count)];
    }
}
