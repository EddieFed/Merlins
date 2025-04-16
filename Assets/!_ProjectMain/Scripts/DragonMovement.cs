using Microsoft.ML.Probabilistic.Models;
using Unity.Mathematics.Geometry;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace __ProjectMain.Scripts
{
    public class DragonMovement : CustomerMovement
    {
        private const float FAR_DIST_DELTA = 5.0f;
        private const float MED_DIST_DELTA = 2.5f;
        
        
        private DragonAI dragonAI = new DragonAI();
        private Animator animator;
        
        float timePassed = 0f;

        private void Start()
        {
            state = State.MOVING;
            goal = Goal.SHOP;
            currentDestination = this.transform;
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            timePassed = 50f;
        }
        
        private void Update()
        {
            timePassed += Time.deltaTime;
            if (timePassed > 50f)
            {
                DragonAI.Distance distance = DragonAI.Distance.Far; 
                GameObject[] customers = GameObject.FindGameObjectsWithTag("Customer");
                
                int i = Random.Range(0, customers.Length);
                float delta_dist = Vector3.Distance(customers[i].transform.position, this.transform.position);
                if (delta_dist > FAR_DIST_DELTA)
                {
                    distance = DragonAI.Distance.Far;
                }
                else if (delta_dist > MED_DIST_DELTA)
                {
                    distance = DragonAI.Distance.Medium;
                }
                else
                {
                    distance = DragonAI.Distance.Near;
                }
                
                currentDestination = customers[i].transform;
                Debug.Log($"Checking dragon AI status : {distance}");
                dragonAI.GetDragonAction(distance);
                
                timePassed = 0f;
            }
            
            float radius = 2f;
            float sqrRadius = radius * radius;
            if ((transform.position - currentDestination.position).sqrMagnitude <= sqrRadius)
            {
                DestinationReached();
            }
        
            if (state == State.MOVING && moveTime > maxMoveTime)
            {
                waitTime = 0;
                state = State.IDLE;
            }
            if (state == State.IDLE && waitTime > maxWaitTime)
            {
                state = State.MOVING;
                moveTime = 0;
            }
            switch (state)
            {
                case State.MOVING:
                    animator.Play("axe|walk");
                    agent.SetDestination(currentDestination.position);
                    moveTime += 1 * Time.deltaTime;
                    break;
                case State.IDLE:
                    animator.Play("axe|idle");
                    agent.SetDestination(transform.position);
                    waitTime += 1 * Time.deltaTime;
                    break;
            }
        }
    }
}