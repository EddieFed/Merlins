using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public enum EnemyState {IDLE, SEARCHING, CHASING, RETREATING}

        public enum Goal
        {
            PLAYER,
            CUSTOMER,
            ITEM,
            NONE,
            DIE
        }
        public EnemyState state;
        public Goal goal = Goal.NONE;
        
        public float moveTime;
        public float maxMoveTime;
        public float waitTime;
        public float maxWaitTime;
        
        // public NavMeshAgent agent;
    
        public Transform currentDestination;
        public float detectionRadius = 2f;



        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected void Start()
        {
            state = EnemyState.SEARCHING;
            goal = Goal.NONE;
            currentDestination = null;
            // agent = GetComponent<NavMeshAgent>();
        }

        // Update is called once per frame
        protected void Update()
        {
            float detectionRadius = 2f;
            float sqrRadius = detectionRadius * detectionRadius;
            if ((transform.position - currentDestination.position).sqrMagnitude <= sqrRadius)
            {
                DestinationReached();
            }
        
            // Limit moving time
            // if (state == State.MOVING && moveTime > maxMoveTime)
            // {
            //     waitTime = 0;
            //     state = State.IDLE;
            // }
            
            // Limit the waiting time
            // if (state == State.IDLE && waitTime > maxWaitTime)
            // {
            //     state = State.MOVING;
            //     moveTime = 0;
            // }
            
            switch (state)
            {
                case EnemyState.SEARCHING:
                    // Move towards target
                    // agent.SetDestination(currentDestination.position);
                    moveTime += 1 * Time.deltaTime;
                    break;
                case EnemyState.IDLE:
                    // Wait at current location
                    // agent.SetDestination(transform.position);
                    waitTime += 1 * Time.deltaTime;
                    break;
            }
        }

        protected void DestinationReached()
        {
            // play animation
            // stop movement for animation time
            if (EnemyState.CHASING == state)
            {
                
            }
            
            waitTime = 0;
            // switch (goal)
            // {
            //     // Choose next goal (may reroll another of same goal type
            //     case Goal.CUSTOMER:
            //         goal = Goal.;
            //         currentDestination = CustomerSpawner.GetRegisterLocation();
            //         break;
            //     case Goal.PURCHASE:
            //         goal = Goal.EXIT;
            //         currentDestination = CustomerSpawner.GetEntranceLocation();
            //         break;
            //     case Goal.DIE:
            //         Destroy(gameObject);
            //         break;
            // }
        }
}
