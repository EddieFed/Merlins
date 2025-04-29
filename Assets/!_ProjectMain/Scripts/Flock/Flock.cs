using UnityEngine;

namespace __ProjectMain.Scripts.Flock
{
    public class Flock : MonoBehaviour
    {
        public float speed;
        public bool turning = false;
        public bool touchCustomer = false;

        public FlockManager FM;
    
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            speed = Random.Range(FM.minSpeed, FM.maxSpeed);
        }
    
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                FM.DeathReport(this.gameObject);
                Destroy(gameObject);
                return;
            }
            if (other.gameObject.CompareTag("Customer"))
            {
                touchCustomer = true;
                FM.SetTargetCustomer(other.gameObject);
            }
        }

        // Update is called once per frame
        void Update()
        {
            // Turn Flock inwards when leaving bounds
            Bounds bounds = new Bounds(FM.transform.position, FM.flyLimits);
            if (!bounds.Contains(transform.position))
            {
                turning = true;
            }
            else
            {
                turning = false;
            }

            if (turning)
            {
                Vector3 direction = FM.transform.position - transform.position;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), FM.rotationSpeed * Time.deltaTime);
            }
            else
            {
                if (Random.Range(0, 100) < 80)
                {
                    speed = Random.Range(FM.minSpeed, FM.maxSpeed);
                }

                if (Random.Range(0, 100) < 80)
                {
                    ApplyFlockRules();
                }
            }
        
            this.transform.Translate(0,0, speed * Time.deltaTime);
        }

        void ApplyFlockRules()
        {
            var bats = FM.allBats;
        
            Vector3 vCenter = Vector3.zero;
            Vector3 vAvoid = Vector3.zero;

            float groupSpeed = 0.01f;
            float neighbourDistance;
            int groupSize = 0;

            foreach (var bat in bats)
            {
                if (!bat) {continue;}
                if (bat == this.gameObject) continue;
                neighbourDistance = Vector3.Distance(bat.transform.position, this.transform.position);
            
                if (neighbourDistance <= FM.neighbourDistance)
                {
                    vCenter += bat.transform.position;
                    groupSize++;

                    if (neighbourDistance <= FM.avoidDistance)
                    {
                        vAvoid += this.transform.position - bat.transform.position;
                    }
                    
                    Flock anotherFlock = bat.GetComponent<Flock>();
                    groupSpeed += anotherFlock.speed;
                }
            }

            if (groupSize > 0)
            {
                vCenter = vCenter / groupSize + (FM.goalPosition - this.transform.position);
                speed = groupSpeed / groupSize; // 
                if (speed > FM.maxSpeed)
                {
                    speed = FM.maxSpeed;
                }
            
                Vector3 vDirection = (vCenter + vAvoid) - this.transform.position;
                if (vDirection != Vector3.zero)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(vDirection),FM.rotationSpeed * Time.deltaTime);
                }
            }
        }
    }
}
