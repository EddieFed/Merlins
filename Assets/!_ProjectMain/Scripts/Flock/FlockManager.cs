using UnityEngine;

namespace __ProjectMain.Scripts.Flock
{
    public class FlockManager : MonoBehaviour
    {
        public float initialSpawnDelay = 0;
        public GameObject batPrefab;
        public int numBats = 20;
        public GameObject[] allBats;
        public int currentCount;
        public float deathLimit = .5f;
        public Vector3 flyLimits = new Vector3(5, 5, 5); // L W H container 
        public Vector3 baseFlyLimits = new Vector3(5, 5, 5); // L W H container 
        public float patrolLimitMultiplier = 1.5f;
        public float chaseLimitMultiplier = 3f;
        public Vector3 goalPosition = Vector3.zero;
        public Transform goalTransform;

        public GameObject targetCustomer = null;

        public float respawnTimer = 120f;
        public float maxRespawnTime = 120f;
    
        public AudioSource audioSource;
        private bool firstSpawn = false;

        [Header("Bat Settings")] [Range(0.0f, 5.0f)]
        public float minSpeed = 1.0f;

        [Range(0.0f, 5.0f)] public float maxSpeed = 2.0f;
        [Range(1.0f, 10.0f)] public float neighbourDistance = 2.0f;
        [Range(1.0f, 5.0f)] public float avoidDistance = 1.0f;
        [Range(1.0f, 5.0f)] public float rotationSpeed = 4.0f;
    
        public bool isDead = false;

        public void SetTargetCustomer(GameObject target)
        {
            if (targetCustomer) return;
            Bounds chaseBounds = new Bounds(transform.position, baseFlyLimits * chaseLimitMultiplier);
            if (chaseBounds.Contains(target.transform.position))
            {
                print("new customer hit");
                targetCustomer = target;
                flyLimits = new Vector3(
                    baseFlyLimits.x * chaseLimitMultiplier,
                    baseFlyLimits.y,
                    baseFlyLimits.z * chaseLimitMultiplier
                );
            }
        }

        public void DeathReport(GameObject bat)
        {
            currentCount -= 1;
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            SpawnBats();
        }

        private void SpawnBats()
        {
            allBats = new GameObject[numBats];
            for (int i = 0; i < numBats; i++)
            {
                Vector3 pos = transform.position + new Vector3(
                    Random.Range(-baseFlyLimits.x, baseFlyLimits.x),
                    Random.Range(-baseFlyLimits.y, baseFlyLimits.y),
                    Random.Range(-baseFlyLimits.z, baseFlyLimits.z)
                );
                allBats[i] = Instantiate(batPrefab, pos, Quaternion.identity);
                allBats[i].GetComponent<Flock>().FM = this;
                allBats[i].transform.SetParent(transform); // Parent to this script’s GameObject
            }

            currentCount = numBats;
        }

        // Update is called once per frame
        void Update()
        {
            if (firstSpawn && initialSpawnDelay <=0)
            {
                firstSpawn = false;
                SpawnBats();
            }

            if (initialSpawnDelay > -1)
            {
                initialSpawnDelay -= Time.deltaTime;
            }
            if (isDead)
            {
                respawnTimer -= Time.deltaTime;
                if (respawnTimer <= 0)
                {
                    isDead = false;
                    SpawnBats();
                    audioSource.Play();
                }

                return;
            }
            if (currentCount < numBats * deathLimit)
            {
                isDead = true;
                respawnTimer = maxRespawnTime;
                audioSource.Stop();
                foreach (var bat in allBats)
                {
                    Destroy(bat);
                }
            }
            Bounds chaseBounds = new Bounds(transform.position, flyLimits = new Vector3(
                baseFlyLimits.x * chaseLimitMultiplier,
                baseFlyLimits.y,
                baseFlyLimits.z * chaseLimitMultiplier
            ));


            //Change set the goal direction
            if (targetCustomer)
            {
                if (!chaseBounds.Contains(targetCustomer.transform.position))
                {
                    targetCustomer = null;
                }
                else
                {
                    goalPosition = targetCustomer.transform.position;
                }
            }
            else if (Random.Range(0, 100) < 5)
            {
                flyLimits = new Vector3(
                    baseFlyLimits.x * patrolLimitMultiplier,
                    baseFlyLimits.y,
                    baseFlyLimits.z * patrolLimitMultiplier
                );
                goalPosition = transform.position + new Vector3(
                    Random.Range(-flyLimits.x, flyLimits.x),
                    Random.Range(-flyLimits.y, flyLimits.y),
                    Random.Range(-flyLimits.z, flyLimits.z));
            }

            goalTransform.localPosition = goalPosition;
        }
    }
}