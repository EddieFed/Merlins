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
    private static List<GameObject> _shelfLocations;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _shelfLocations = new List<GameObject>();
        GameObject.FindGameObjectsWithTag("Shelf", _shelfLocations);

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
    
    public static GameObject GetShelveLocation()
    {
        return _shelfLocations[Random.Range(0, _shelfLocations.Count)];
    }
}
