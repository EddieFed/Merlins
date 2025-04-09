using UnityEngine;

public class Flock : MonoBehaviour
{
    private float speed;
    bool turning = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        speed = Random.Range(FlockManager.FM.minSpeed, FlockManager.FM.maxSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        Bounds bounds = new Bounds(FlockManager.FM.transform.position, FlockManager.FM.flyLimits * 2);
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
            Vector3 direction = FlockManager.FM.transform.position - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), FlockManager.FM.rotationSpeed * Time.deltaTime);
        }
        else
        {
            if (Random.Range(0, 100) < 80)
            {
                speed = Random.Range(FlockManager.FM.minSpeed, FlockManager.FM.maxSpeed);
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
        var bats = FlockManager.FM.allBats;
        
        Vector3 vCenter = Vector3.zero;
        Vector3 vAvoid = Vector3.zero;

        float groupSpeed = 0.01f;
        float neighbourDistance;
        int groupSize = 0;

        foreach (var bat in bats)
        {
            if (bat == this.gameObject) continue;
            neighbourDistance = Vector3.Distance(bat.transform.position, this.transform.position);
            
            if (neighbourDistance <= FlockManager.FM.neighbourDistance)
            {
                vCenter += bat.transform.position;
                groupSize++;

                if (neighbourDistance <= FlockManager.FM.avoidDistance)
                {
                    vAvoid += this.transform.position - bat.transform.position;
                }
                    
                Flock anotherFlock = bat.GetComponent<Flock>();
                groupSpeed += anotherFlock.speed;
            }
        }

        if (groupSize > 0)
        {
            vCenter = vCenter / groupSize + (FlockManager.FM.goalPosition - this.transform.position);
            speed = groupSpeed / groupSize; // 
            if (speed > FlockManager.FM.maxSpeed)
            {
                speed = FlockManager.FM.maxSpeed;
            }
            
            Vector3 vDirection = (vCenter + vAvoid) - this.transform.position;
            if (vDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(vDirection),FlockManager.FM.rotationSpeed * Time.deltaTime);
            }
        }
    }
}
