using System;
using UnityEngine;
using UnityEngine.AI;

namespace __ProjectMain.Scripts
{
    public class CustomerMovement : MonoBehaviour
    {
        public enum State {Idle, Moving}

        public enum Goal
        {
            Shop,
            Purchase,
            Exit
        }
        
        public State state;
        public Goal goal = Goal.Purchase;
        public float moveTime;
        public float maxMoveTime;
        public float waitTime;
        public float maxWaitTime;
        public float speed;
        protected NavMeshAgent agent;
    
        protected Transform currentDestination;
        protected Transform shelveLocation;
        protected Transform exitLocation;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected void Start()
        {
            state = State.Moving;
            goal = Goal.Shop;
            currentDestination = CustomerSpawner.GetShelveLocation().transform;
            agent = GetComponent<NavMeshAgent>();
        }

        // Update is called once per frame
        protected void Update()
        {
            const float radius = 2f;
            const float sqrRadius = radius * radius;
            if ((transform.position - currentDestination.position).sqrMagnitude <= sqrRadius)
            {
                DestinationReached();
            }
        
            if (state == State.Moving && moveTime > maxMoveTime)
            {
                waitTime = 0;
                state = State.Idle;
            }
            if (state == State.Idle && waitTime > maxWaitTime)
            {
                state = State.Moving;
                moveTime = 0;
            }
            switch (state)
            {
                case State.Moving:
                    agent.SetDestination(currentDestination.position);
                    moveTime += 1 * Time.deltaTime;
                    break;
                case State.Idle:
                    agent.SetDestination(transform.position);
                    waitTime += 1 * Time.deltaTime;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected void DestinationReached()
        {
            // play animation
            // stop movement for animation time
            state = State.Idle;
            waitTime = 0;
            switch (goal)
            {
                // Choose next goal (may reroll another of same goal type
                case Goal.Shop:
                    goal = Goal.Purchase;
                    currentDestination = CustomerSpawner.GetRegisterLocation().transform;
                    break;
                case Goal.Purchase:
                    goal = Goal.Exit;
                    currentDestination = CustomerSpawner.GetEntranceLocation().transform;
                    break;
                case Goal.Exit:
                    Destroy(gameObject);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
