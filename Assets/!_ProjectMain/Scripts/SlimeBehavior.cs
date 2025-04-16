using System;
using __ProjectMain.Scripts;
using UnityEngine;
using UnityEngine.AI;

public class SlimeBehavior : MonoBehaviour
{
    public enum STATE
    {
        IDLE,
        MOVING,
        EATING,
        DEAD
    }

    public enum GOAL
    {
        EAT_STOCK
    }
    
    public STATE state;
    public GOAL goal;
    
    private Animator anim;
    private NavMeshAgent agent;
    public Transform currentDestination;
    private Transform currentShelf;
    public float destockCooldown = 5;
    private float destockTimer = 0;
    
    public float deathRadius = 2f;
    public float radius = 2f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        currentShelf = SlimeSpawner.GetShelf();
        currentDestination = SlimeSpawner.GetShelfLocation(currentShelf);
        state = STATE.MOVING;
        goal = GOAL.EAT_STOCK;
    }

    // Update is called once per frame
    void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Dead"))
        {
            SlimeSpawner.currSlimeCount--;
            Destroy(gameObject);
        }

        if (state == STATE.DEAD)
            return;
        
        Collider[] nearbyObjects = Physics.OverlapSphere(transform.position, deathRadius);
        foreach (var obj in nearbyObjects)
        {
            if (obj.CompareTag("Player"))
            {
                state = STATE.DEAD;
                break;
            }
        }
        
        if (state == STATE.MOVING && (transform.position - currentDestination.position).sqrMagnitude <= radius * radius)
            state = STATE.EATING;
        
        switch (state)
        {
            case STATE.IDLE:
                anim.SetBool("Moving", false);
                break;
            case STATE.MOVING:
                anim.SetBool("Moving", true);
                agent.SetDestination(currentDestination.position);
                break;
            case STATE.EATING:
                anim.SetBool("Moving", false);
                anim.SetBool("Eating", true);
                if (currentShelf.gameObject.GetComponent<ItemCounter>().itemCount <= 0)
                    break;
                if (destockTimer <= 0)
                {
                    destockTimer = destockCooldown;
                    currentShelf.gameObject.GetComponent<ItemCounter>().itemCount--;
                }
                else
                    destockTimer -= Time.deltaTime;
                break;
            case STATE.DEAD:
                anim.SetTrigger("Dead");
                break;
        }
    }
}
