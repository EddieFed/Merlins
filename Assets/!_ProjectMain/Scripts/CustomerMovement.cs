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
            EXIT,
            FLEE
        }

        public float timeAlive = 0;
        
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
        public Transform shelveLocation;
        public Transform exitLocation;

        public float maxIFrame = 10f;
        public float currIFrame = 0;
        public int satisfaction = 100;
        
        public bool isDead = false;

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Puddle"))
            {
                GameManager.totalSatisfaction += satisfaction;
                if (currIFrame <= 0)
                {
                    currIFrame = maxIFrame;
                    satisfaction -= 10;
                }

                if (satisfaction <= 0 && !isDead)
                {
                    isDead = true;
                    satisfaction = 0;
                    speed = speed * 4;
                }
                
            }
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected void Start()
        {
            timeAlive = 0f;
            state = State.MOVING;
            goal = Goal.SHOP;
            currentShelf = CustomerSpawner.GetShelf();
            currentDestination = currentShelf;
            agent = GetComponent<NavMeshAgent>();
        }

        // Update is called once per frame
        protected void Update()
        {
            timeAlive += Time.deltaTime;
            currIFrame -= 1;
            if (satisfaction <= 0 && goal != Goal.FLEE)
            {
                goal = Goal.FLEE;
                state = State.MOVING;
                maxMoveTime = 999;
                moveTime = 0;
                currentDestination = CustomerSpawner.GetEntranceLocation().transform;
            }
            const float radius = 5.0f;
            const float sqrRadius = radius * radius;
            if ((transform.position - currentDestination.position).sqrMagnitude <= sqrRadius)
            {
                Debug.Log($"Customer {gameObject.name} reached his destination!");
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
                case Goal.FLEE:
                    Destroy(gameObject);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
