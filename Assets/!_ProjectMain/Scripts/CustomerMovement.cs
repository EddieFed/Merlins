using UnityEngine;
using UnityEngine.AI;

public class CustomerMovement : MonoBehaviour
{
    public enum State {IDLE, MOVING}
    public State state;
    public float moveTime;
    public float maxMoveTime;
    public float waitTime;
    public float maxWaitTime;
    public float speed;
    public NavMeshAgent agent;
    
    public Transform shelveLocation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        state = State.MOVING;
        shelveLocation = CustomerSpawner.GetShelveLocation();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
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
        if (state == State.MOVING)
        {
            agent.SetDestination(shelveLocation.position);
            // transform.Translate(0, 0, speed * Time.deltaTime);
            moveTime += 1 * Time.deltaTime;
        }
        if (state == State.IDLE)
        {
            agent.SetDestination(transform.position);
            waitTime += 1 * Time.deltaTime;
        }
    }
}
