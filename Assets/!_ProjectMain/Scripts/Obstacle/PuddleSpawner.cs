using UnityEngine;
using Random = UnityEngine.Random;

namespace __ProjectMain.Scripts.Obstacle
{
    public class PuddleSpawner : MonoBehaviour
    {
        [SerializeField] private int puddleLimit = 5;
        private int currPuddleCount = 0;
        [SerializeField] private float maxDelayTime = 5.0f;
        [SerializeField] private float currDelayTime = 0.0f;
        
        [SerializeField] private GameObject puddlePrefab;
        
        private void Update()
        {
            if (currDelayTime <= 0)
            {
                // create new customer
                if (currPuddleCount < puddleLimit)
                {
                    Vector3 location = new Vector3(Random.Range(-30, 30), 0, Random.Range(-30, 10));
                    GameObject puddle = Instantiate(puddlePrefab, location, Quaternion.identity);
                    puddle.tag = "Puddle";
                    currPuddleCount++;
                    currDelayTime = maxDelayTime;
                }
            }

            currDelayTime -= 1 * Time.deltaTime;
        }
    }
}