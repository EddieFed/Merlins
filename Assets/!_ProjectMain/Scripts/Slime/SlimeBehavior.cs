using __ProjectMain.Scripts.Game;
using UnityEngine;
using UnityEngine.AI;

namespace __ProjectMain.Scripts.Slime
{
    public class SlimeBehavior : MonoBehaviour
    {
        public enum STATE
        {
            IDLE,
            MOVING,
            EATING,
            DEAD
        }

        // public enum GOAL
        // {
        //     EAT_STOCK
        // }
    
        private STATE state;
        // private GOAL goal;
    
        private Animator anim;
        private NavMeshAgent agent;
        private AudioSource audio;
    
        public Transform currentDestination;
        private Transform currentShelf;
    
        [SerializeField] private float destockCooldown = 5;
        [SerializeField] private float destockTimer = 0;
        [SerializeField] private float deathRadius = 2f;
        [SerializeField] private float radius = 2f;
    
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            anim = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();
            audio = GetComponent<AudioSource>();
            currentShelf = SlimeSpawner.GetShelf();
            currentDestination = currentShelf;
            state = STATE.MOVING;
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
                    if (currentShelf.gameObject.GetComponentInChildren<ItemCounter>().itemCount <= 0)
                        break;
                    if (destockTimer <= 0)
                    {
                        destockTimer = destockCooldown;
                        currentShelf.gameObject.GetComponentInChildren<ItemCounter>().itemCount--;
                        audio.Play();
                    }
                    else
                        destockTimer -= Time.deltaTime;
                    break;
                case STATE.DEAD:
                    anim.SetTrigger("Dead");
                    audio.Play();
                    break;
            }
        }
    }
}
