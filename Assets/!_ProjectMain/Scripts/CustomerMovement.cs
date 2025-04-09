using UnityEngine;
using UnityEngine.AI;

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
    
    public Transform currentDestination;
    public Transform shelveLocation;
    public Transform exitLocation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        state = State.MOVING;
        goal = Goal.SHOP;
        currentDestination = CustomerSpawner.GetShelveLocation();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
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
                agent.SetDestination(currentDestination.position);
                moveTime += 1 * Time.deltaTime;
                break;
            case State.IDLE:
                agent.SetDestination(transform.position);
                waitTime += 1 * Time.deltaTime;
                break;
        }
    }

    void DestinationReached()
    {
        // play animation
        // stop movement for animation time
        state = State.IDLE;
        waitTime = 0;
        switch (goal)
        {
            // Choose next goal (may reroll another of same goal type
            case Goal.SHOP:
                goal = Goal.PURCHASE;
                currentDestination = CustomerSpawner.GetRegisterLocation();
                break;
            case Goal.PURCHASE:
                goal = Goal.EXIT;
                currentDestination = CustomerSpawner.GetEntranceLocation();
                break;
            case Goal.EXIT:
                Destroy(gameObject);
                break;
        }
    }
}
