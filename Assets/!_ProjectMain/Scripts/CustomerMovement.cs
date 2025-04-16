using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace __ProjectMain.Scripts
{
    public class CustomerMovement : MonoBehaviour
    {
        public enum State {IDLE, MOVING}

        public enum Goal
        {
            SHOP,
            PURCHASE,
            EXIT
        }
        
        public State state;
        public Goal goal = Goal.PURCHASE;
        public float moveTime;
        public float maxMoveTime;
        public float waitTime;
        public float maxWaitTime;
        public float speed;
        public NavMeshAgent agent;

        public int itemValue;
        public Transform currentDestination;
        public Transform currentShelf;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected void Start()
        {
            state = State.MOVING;
            goal = Goal.SHOP;
            currentShelf = CustomerSpawner.GetShelf();
            currentDestination = currentShelf;
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
                    agent.SetDestination(currentDestination.position);
                    moveTime += 1 * Time.deltaTime;
                    break;
                case State.IDLE:
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
            state = State.IDLE;
            waitTime = 0;
            switch (goal)
            {
                // Choose next goal (may reroll another of same goal type
                case Goal.SHOP:
                    if (currentShelf.gameObject.GetComponent<ItemCounter>().itemCount > 0)
                    {
                        itemValue = Random.Range(currentShelf.gameObject.GetComponent<ItemCounter>().minPrice,
                            currentShelf.gameObject.GetComponent<ItemCounter>().maxPrice);
                        currentShelf.gameObject.GetComponent<ItemCounter>().itemCount--;
                        goal = Goal.PURCHASE;
                        currentDestination = CustomerSpawner.GetRegisterLocation().transform;
                    }
                    break;
                case Goal.PURCHASE:
                    goal = Goal.EXIT;
                    currentDestination = CustomerSpawner.GetEntranceLocation().transform;
                    break;
                case Goal.EXIT:
                    Destroy(gameObject);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
