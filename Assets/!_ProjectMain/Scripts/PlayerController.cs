using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace __ProjectMain.Scripts
{
    public class PlayerController : NetworkBehaviour
    {
        private Rigidbody rb;
        [SerializeField] private Transform playerTransform;
        
        // Clear is used as a placeholder, indicating that the player is not holding any item
        public Color heldRestock = Color.clear;
        [SerializeField] public GameObject heldItemGameObject;
        public TextMeshProUGUI heldRestockText;

        private bool isInvertedPerspective = false;

        private void Start()
        {
            heldItemGameObject.GetComponent<Renderer>().enabled = false;
            if (playerTransform == null)
            {
                Debug.LogError("Player transform is null, assign it to Wizard dude prefab!!!!");
            }
            rb = GetComponent<Rigidbody>();
        }
        
        private void Update()
        {
            // Keep stock held updated
            if (heldRestock != Color.clear)
            {
                heldItemGameObject.GetComponent<Renderer>().enabled = true;
                heldItemGameObject.GetComponent<Renderer>().material.color = heldRestock;
            }
            else
            {
                heldItemGameObject.GetComponent<Renderer>().enabled = false;
            }

            // Flip camera
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                isInvertedPerspective = !isInvertedPerspective;

                if (!Camera.main)
                {
                    Debug.LogWarning("Camera not found");
                    return;
                }

                Camera.main.transform.localPosition = new Vector3(
                    -1 * Camera.main.transform.localPosition.x,
                    Camera.main.transform.localPosition.y,
                    -1 * Camera.main.transform.localPosition.z
                );
                Camera.main.transform.localRotation = Quaternion.Euler(
                    Camera.main.transform.localRotation.eulerAngles.x,
                    ((isInvertedPerspective) ? -1 : 1) * 180 + Camera.main.transform.localRotation.eulerAngles.y,
                    Camera.main.transform.localRotation.eulerAngles.z
                );
                
                // Shelf text adjustments
                foreach (GameObject shelf in GameObject.FindGameObjectsWithTag("Shelf"))
                {
                    shelf.GetComponent<ItemCounter>().FlipTextPerspective();
                }
            }
            
            Vector3 movement = new Vector3(0f, 0f, 0f);
            if (Input.GetKey(KeyCode.W))
            {
                movement += new Vector3(10f, 0f, 10f);
            }
            if (Input.GetKey(KeyCode.S))
            {
                movement += new Vector3(-10f, 0f, -10f);
            }
            if (Input.GetKey(KeyCode.A))
            {
                movement += new Vector3(-10f, 0f, 10f);
            }
            if (Input.GetKey(KeyCode.D))
            {
                movement += new Vector3(10f, 0f, -10f);
            }

            // Adjust movement
            if (isInvertedPerspective)
            {
                movement *= -1f;
            }
            
            rb.linearVelocity = movement;
            playerTransform.LookAt(transform.position + movement);
            
            if (heldRestock == Color.clear)
                heldRestockText.text = null;
            else
            {
                heldRestockText.text = "Held Item";
                heldRestockText.color = heldRestock;
            }
        }
    }
}
