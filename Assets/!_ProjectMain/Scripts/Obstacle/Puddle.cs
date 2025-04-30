using System;
using __ProjectMain.Scripts.Player;
using Unity.VisualScripting;
using UnityEngine;

namespace __ProjectMain.Scripts.Obstacle
{
    public class Puddle : MonoBehaviour
    {
        [SerializeField] private float slipDelayMax = 1.5f;
        [SerializeField] public float slipDelay = 1.5f;
        private void OnTriggerStay(Collider other)
        {
            // Debug.Log("Collision Enter");
            if (other.gameObject.CompareTag("Player"))
            {
                PlayerController player = other.gameObject.GetComponent<PlayerController>();
                slipDelay -= Time.deltaTime;
                if (slipDelay <= 0)
                {
                    player.Slip();
                }
                if (player.isHoldingBroom && Input.GetKey(Interactable.InteractionController.InteractKey))
                {
                    GameObject indicator = GetComponent<Indicator>().indicatorInstance;
                    if (indicator != null)
                    {
                        Destroy(indicator);
                    }
                    Destroy(this.gameObject);
                }
                
            }
        }

        private void OnTriggerExit(Collider other)
        {
            slipDelay = slipDelayMax;
        }
    }
}