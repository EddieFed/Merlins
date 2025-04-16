using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace __ProjectMain.Scripts
{
    public class CustomerSpawner : MonoBehaviour
    {
        public int customerLimit;
        public int currCustomerCount;
        public float maxDelayTime;
        public float currDelayTime;
        public GameObject customerPrefab;
        public GameObject dragonPrefab;
        
        public GameObject entrance;
        public Transform spawnPoint;
        private static List<GameObject> _shelfLocations;
        private static List<GameObject> _entranceLocations;
        private static List<GameObject> _registerLocations;

        private void Start()
        {
            _shelfLocations = new List<GameObject>();
            GameObject.FindGameObjectsWithTag("Shelf", _shelfLocations);
        
            _entranceLocations = new List<GameObject> { entrance };

            _registerLocations = new List<GameObject>();
            GameObject.FindGameObjectsWithTag("Register", _registerLocations);
        }

        // Update is called once per frame
        private void Update()
        {
            if (currDelayTime <= 0)
            {
                // create new customer
                if (currCustomerCount < customerLimit)
                {
                    GameObject customer = Instantiate(customerPrefab, spawnPoint.position, spawnPoint.rotation);
                    customer.tag = "Customer";
                    currCustomerCount++;
                    currDelayTime = maxDelayTime;
                }
            }

            currDelayTime -= 1 * Time.deltaTime;
        }

        public static Transform GetShelf()
        {
            return _shelfLocations[Random.Range(0, _shelfLocations.Count)].transform;
        }
        
        public static Transform GetShelfLocation(Transform shelf)
        {
            return shelf.GetComponent<ItemCounter>().destinations[Random.Range(0, shelf.GetComponent<ItemCounter>().destinations.Count)];
        }
    
        public static GameObject GetEntranceLocation()
        {
            return _entranceLocations[Random.Range(0, _entranceLocations.Count)];
        }
        public static GameObject GetRegisterLocation()
        {
            return _registerLocations[Random.Range(0, _registerLocations.Count)];
        }
    }
}