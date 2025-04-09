using UnityEngine;

public class FlockManager : MonoBehaviour
{
    public static FlockManager FM;
    public GameObject batPrefab;
    public int numBats = 20;
    public GameObject[] allBats;
    public Vector3 flyLimits = new Vector3(5, 5, 5); // L W H container 
    
    [Header ("Bat Settings")]
    [Range(0.0f, 5.0f)]
    public float minSpeed = 1.0f;
    [Range(0.0f, 5.0f)]
    public float maxSpeed = 2.0f;
    [Range(1.0f, 10.0f)]
    public float neighbourDistance = 2.0f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        allBats = new GameObject[numBats];
        foreach (var bat in allBats)
        {
            Vector3 pos = this.transform.position + new Vector3(Random.Range(-flyLimits.x, flyLimits.x),
                                                                Random.Range(-flyLimits.y, flyLimits.y), 
                                                                Random.Range(-flyLimits.z, flyLimits.z));
            Instantiate(batPrefab, pos, Quaternion.identity);
        }

        FM = this;
    }

    // Update is called once per frame
    void Update()
    {
    }
}