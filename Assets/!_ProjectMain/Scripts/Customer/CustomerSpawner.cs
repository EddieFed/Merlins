using System.Collections.Generic;
using __ProjectMain.Scripts.Game;
using UnityEngine;
using Random = UnityEngine.Random;

namespace __ProjectMain.Scripts.Customer
{
    public class CustomerSpawner : MonoBehaviour
    {
        public int customerLimit;
        public static int currCustomerCount;
        public float maxDelayTime;
        public float currDelayTime;
        public GameObject customerPrefab;
        public GameObject dragonPrefab;
        public List<GameObject> customers = new List<GameObject>();
        
        public GameObject messPrefab;
        public float messTargetFilter;
        
        public GameObject entrance;
        public GameObject exit;
        public Transform spawnPoint;
        private static List<GameObject> _shelfLocations;
        private static List<GameObject> _entranceLocations;
        private static List<GameObject> _exitLocations;
        private static List<GameObject> _registerLocations;

        public float messDelayMax;
        public float messDelayCurrent;

        private void Start()
        {
            _shelfLocations = new List<GameObject>();
            GameObject.FindGameObjectsWithTag("Shelf", _shelfLocations);
        
            _entranceLocations = new List<GameObject> { entrance };
            _exitLocations = new List<GameObject> { exit };

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
                    customers.Add(customer);
                }
            }

            // Create mess on random customer
            if (messDelayCurrent <= 0)
            {
                int tries = 7;
                while (tries > 0)
                {
                    GameObject messTarget = null;
                    if (customers.Count > 0)
                    {
                        messTarget = customers[Random.Range(0, customers.Count)];
                    }
                    if (!messTarget)
                    {
                        tries--;
                        continue;
                    }

                    if (messTarget.GetComponent<CustomerMovement>().timeAlive < messTargetFilter)
                    {
                        tries--;
                        continue;
                    }
                    Transform messLocation = messTarget.transform;
                    messLocation.transform.position = new Vector3(messTarget.transform.position.x, 0f, messTarget.transform.position.z);

                    Instantiate(messPrefab, messLocation.position, messLocation.rotation);
                    messDelayCurrent = messDelayMax;
                    break;

                }
                
            }
            messDelayCurrent -= Time.deltaTime;
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
        
        public static GameObject GetExitLocation()
        {
            return _exitLocations[Random.Range(0, _exitLocations.Count)];
        }
        
        public static GameObject GetRegisterLocation()
        {
            return _registerLocations[Random.Range(0, _registerLocations.Count)];
        }
    }
}