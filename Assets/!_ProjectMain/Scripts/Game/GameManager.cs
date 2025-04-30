using System;
using __ProjectMain.Scripts.Customer;
using __ProjectMain.Scripts.Slime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        
        // Menu references
        [SerializeField] private GameObject endMenu;
        [SerializeField] private Button playAgainBtn;
        [SerializeField] private Button mainMenuBtn;
    
        public STATE state;
        public GameObject NPCManager;
        public int gameTime;
        public float currTime;
        private int currTimeStage;
        private int currHour;
        private int currMin;
        public static int bankValue;
        public TextMeshProUGUI clockText;
        public TextMeshProUGUI bankText;
        public TextMeshProUGUI customerText;
        public TextMeshProUGUI satisfactionText;

        public static float ConfirmedSatisfaction = 0.0f;
        public static int ConfirmedCustomers = 0;

        void Start()
        {
            state = STATE.SETUP;
            NPCManager.GetComponent<CustomerSpawner>().enabled = false;
            NPCManager.GetComponent<SlimeSpawner>().enabled = false;
            endMenu.SetActive(false);
            playAgainBtn.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("Scene01 - Supermarket");
            });
            mainMenuBtn.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("Scene00 - Main Menu");
            });

            bankValue = 0;
            currTime = 0;
        
            // TODO: Make this part of the ready button
            state = STATE.OPEN;
        }

        void Update()
        {
            customerText.text = $"Customers: {CustomerSpawner.currCustomerCount} ({CustomerSpawner.currCustomerCount + ConfirmedCustomers} total)";
            
            float maxSatisfaction = 100.0f * (CustomerSpawner.currCustomerCount + ConfirmedCustomers);
            float totalSatisfaction = ConfirmedSatisfaction;
            foreach (GameObject customer in GameObject.FindGameObjectsWithTag("Customer"))
            {
                CustomerMovement customerMovement = customer.GetComponent<CustomerMovement>();
                totalSatisfaction += customerMovement.satisfaction;
                bankValue += customerMovement.currentSpent;
            }
            bankText.text = $"Bank: ${bankValue}";
            satisfactionText.text = $"Satisfaction: {(int) ((totalSatisfaction / maxSatisfaction) * 100)}%";
            
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
                    currTimeStage = Mathf.FloorToInt(currTime / ((float)gameTime / 480));
                    currHour = (currTimeStage / 60) + 9;
                    currMin = (currTimeStage % 60);
                    clockText.text = (currHour > 12 ? currHour - 12 : currHour) + ":" + (currMin < 10 ? "0" + currMin : currMin) + (currHour >= 12 ? " PM" : " AM");
                    break;
                case STATE.CLOSED:
                    clockText.text = "CLOSED";
                    NPCManager.GetComponent<CustomerSpawner>().enabled = false;
                    NPCManager.GetComponent<SlimeSpawner>().enabled = false;
                    endMenu.SetActive(true);
                    break;
                case STATE.COMPLETED:
                    // Round over logic here
                    break;
            }
        }
    }
}