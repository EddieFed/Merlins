using TMPro;
using UnityEngine;

namespace __ProjectMain.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public enum STATE
        {
            SETUP,
            OPEN,
            CLOSED, // No more customers will spawn, existing customers will finish their orders
            COMPLETED // All customers have left
        }

        public STATE state;
        public GameObject NPCManager;
        public int gameTime;
        public float currTime;
        public TextMeshProUGUI clockText;
        public TextMeshProUGUI bankText;
        public TextMeshProUGUI customerText;

        public static int bankValue;

        void Start()
        {
            state = STATE.SETUP;
            NPCManager.GetComponent<CustomerSpawner>().enabled = false;
            NPCManager.GetComponent<SlimeSpawner>().enabled = false;
            NPCManager.GetComponent<FlockManager>().enabled = false;

            bankValue = 0;
            currTime = 0;
        }

        void Update()
        {
            bankText.text = "Bank: $" + bankValue;
            customerText.text = "Customers: " + NPCManager.GetComponent<CustomerSpawner>().currCustomerCount;

            if (currTime >= gameTime)
            {
                state = STATE.CLOSED;
            }

            switch (state)
            {
                case STATE.OPEN:
                    NPCManager.GetComponent<CustomerSpawner>().enabled = true;
                    NPCManager.GetComponent<SlimeSpawner>().enabled = true;
                    NPCManager.GetComponent<FlockManager>().enabled = true;
                    currTime += Time.deltaTime;
                    clockText.text = Mathf.RoundToInt(currTime) + "/" + gameTime;
                    break;
                case STATE.CLOSED:
                    clockText.text = "CLOSED";
                    NPCManager.GetComponent<CustomerSpawner>().enabled = false;
                    NPCManager.GetComponent<SlimeSpawner>().enabled = false;
                    break;
                case STATE.COMPLETED:
                    // Round over logic here
                    break;
            }
        }
    }
}
