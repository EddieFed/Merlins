using __ProjectMain.Scripts.Player;
using Unity.VisualScripting;
using UnityEngine;

namespace __ProjectMain.Scripts.Obstacle
{
    public class Puddle : MonoBehaviour
    {
        
        private void OnTriggerStay(Collider other)
        {
            // Debug.Log("Collision Enter");
            if (other.gameObject.CompareTag("Player"))
            {
                PlayerController player = other.gameObject.GetComponent<PlayerController>();
                if (player.isHoldingBroom && Input.GetKeyDown(Interactable.InteractionController.InteractKey))
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
    }
}