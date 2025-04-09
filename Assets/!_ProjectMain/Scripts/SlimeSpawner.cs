using System.Collections.Generic;
using UnityEngine;

public class SlimeSpawner : MonoBehaviour
{
    public int slimeLimit;
    public static int currSlimeCount;
    public GameObject slimePrefab;
    public Transform spawnPoint;
    public float maxDelayTime;
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
    
    public static Transform GetShelveLocation()
    {
        return shelveLocations[Random.Range(0, shelveLocations.Count)];
    }
}
