using __ProjectMain.Scripts.Player;
using UnityEngine;

namespace __ProjectMain.Scripts.Obstacle
{
    public class Puddle : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            // Debug.Log("Collision Enter");
            if (other.gameObject.CompareTag("Player"))
            {
                PlayerController player = other.gameObject.GetComponent<PlayerController>();
                if (player.isHoldingBroom)
                {
                    Destroy(this.gameObject);
                }
                
            }
        }
    }
}