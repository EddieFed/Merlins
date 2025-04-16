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

        public GameObject shelveGroup;
        public GameObject entranceGroup;
        public GameObject registerGroup;
        public static List<Transform> shelveLocations;
        public static List<Transform> entranceLocations;
        public static List<Transform> registerLocations;
        public Transform spawnPoint;

        private void Start()
        {
            shelveLocations = new List<Transform>();
            foreach (Transform child in shelveGroup.transform)
            {
                shelveLocations.Add(child);
            }
        
            entranceLocations = new List<Transform>();
            foreach (Transform child in entranceGroup.transform)
            {
                entranceLocations.Add(child);
            }
        
            registerLocations = new List<Transform>();
            foreach (Transform child in registerGroup.transform)
            {
                registerLocations.Add(child);
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
            return shelveLocations[Random.Range(0, shelveLocations.Count)];
        }
        
        public static Transform GetShelfLocation(Transform shelf)
        {
            return shelf.GetComponent<ItemCounter>().destinations[Random.Range(0, shelf.GetComponent<ItemCounter>().destinations.Count)];
        }
    
        public static Transform GetEntranceLocation()
        {
            return entranceLocations[Random.Range(0, entranceLocations.Count)];
        }
        public static Transform GetRegisterLocation()
        {
            return registerLocations[Random.Range(0, registerLocations.Count)];
        }
    }
}