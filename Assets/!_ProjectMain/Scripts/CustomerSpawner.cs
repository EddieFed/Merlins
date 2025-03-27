using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class CustomerSpawner : MonoBehaviour
{
    public int customerLimit;
    public int currCustomerCount;
    public float maxDelayTime;
    public float currDelayTime;
    public GameObject customerPrefab;

    public GameObject shelveGroup;
    public static List<Transform> shelveLocations;

    public Transform spawnPoint;

    private void Start()
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

    public static Transform GetShelveLocation()
    {
        return shelveLocations[Random.Range(0, shelveLocations.Count)];
    }
}