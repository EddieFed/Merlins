using UnityEngine;

public class FlockManager : MonoBehaviour
{
    public static FlockManager FM;
    public GameObject batPrefab;
    public int numBats = 20;
    public GameObject[] allBats;
    public Vector3 flyLimits = new Vector3(5, 5, 5); // L W H container 
    public Vector3 goalPosition = Vector3.zero;
    
    [Header ("Bat Settings")]
    [Range(0.0f, 5.0f)]
    public float minSpeed = 1.0f;
    [Range(0.0f, 5.0f)]
    public float maxSpeed = 2.0f;
    [Range(1.0f, 10.0f)]
    public float neighbourDistance = 2.0f;
    [Range(1.0f, 5.0f)]
    public float avoidDistance = 1.0f;
    [Range(1.0f, 5.0f)]
    public float rotationSpeed = 4.0f;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        allBats = new GameObject[numBats];
        for (int i = 0; i < numBats; i++)
        {
            Vector3 pos = this.transform.position + new Vector3(Random.Range(-flyLimits.x, flyLimits.x),
                Random.Range(-flyLimits.y, flyLimits.y), 
                Random.Range(-flyLimits.z, flyLimits.z));
            allBats[i] = Instantiate(batPrefab, pos, Quaternion.identity);
            allBats[i].transform.SetParent(transform); // Parent to this script’s GameObject
        }

        FM = this;
    }

    // Update is called once per frame
    void Update()
    {
        //Change set the goal direction
        if (Random.Range(0, 100) < 10)
        {
            goalPosition = this.transform.position + new Vector3(Random.Range(-flyLimits.x, flyLimits.x),
                Random.Range(-flyLimits.y, flyLimits.y), 
                Random.Range(-flyLimits.z, flyLimits.z));
            
        }
    }
}