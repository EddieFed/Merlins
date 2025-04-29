using __ProjectMain.Scripts.Customer;
using __ProjectMain.Scripts.Slime;
using TMPro;
using UnityEngine;

namespace __ProjectMain.Scripts.Game
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
        public TextMeshProUGUI satisfactionText;

        public static int totalCustomers;
        public static int totalSatisfaction;
        public static int bankValue;

        void Start()
        {
            state = STATE.SETUP;
            NPCManager.GetComponent<CustomerSpawner>().enabled = false;
            NPCManager.GetComponent<SlimeSpawner>().enabled = false;

            totalCustomers = 0;
            totalSatisfaction = 0;
            bankValue = 0;
            currTime = 0;
        
            // TODO: Make this part of the ready button
            state = STATE.OPEN;
        }

        void Update()
        {
            bankText.text = "Bank: $" + bankValue;
            customerText.text = "Customers: " + NPCManager.GetComponent<CustomerSpawner>().currCustomerCount + " (" + totalCustomers + " total)";
            // satisfactionText.text = "Satisfaction: " + (totalCustomers == 0 ? 100 : totalSatisfaction / (totalCustomers * 100)) + "%";
            satisfactionText.text = "";
        
            if (currTime >= gameTime)
            {
                state = STATE.CLOSED;
            }

            switch (state)
            {
                case STATE.OPEN:
                    NPCManager.GetComponent<CustomerSpawner>().enabled = true;
                    NPCManager.GetComponent<SlimeSpawner>().enabled = true;
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