using UnityEngine;

public class CustomerMovement : MonoBehaviour
{
    public enum State {IDLE, MOVING}
    public State state;
    public float moveTime;
    public float maxMoveTime;
    public float waitTime;
    public float maxWaitTime;
    public float speed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        state = State.MOVING;
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
            transform.Translate(0, 0, speed * Time.deltaTime);
            moveTime += 1 * Time.deltaTime;
        }
        
        if (state == State.IDLE)
        {
            waitTime += 1 * Time.deltaTime;
        }
        
    }
}
