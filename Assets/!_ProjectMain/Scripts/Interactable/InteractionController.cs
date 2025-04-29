using System.Collections.Generic;
using UnityEngine;

namespace __ProjectMain.Scripts.Interactable
{
    public class InteractionController : MonoBehaviour
    {
        public KeyCode interactKey = KeyCode.E;
        [SerializeField] public List<IInteractable> interactables = new List<IInteractable>();

        private void InteractCurrent()
        {
            interactables[0].Interact(this.gameObject);
        }
        void Update()
        {
            if (Input.GetKeyDown(interactKey)) // Example: press E to interact
            {
                Debug.Log("Interact");
                if (interactables.Count > 0)
                {
                    Debug.Log("interacting");
                    InteractCurrent();
                }
            }
        }

        public void InteractEnter(Collider other)
        {
            IInteractable interactable = other.GetComponent<IInteractable>();
            if (interactable != null && !interactables.Contains(interactable))
            {
                Debug.Log(interactables.Count);
                interactables.Add(interactable);
            }
        }

        public void InteractExit(Collider other)
        {
            IInteractable interactable = other.GetComponent<IInteractable>();
            if (interactable != null && interactables.Contains(interactable))
            {
                Debug.Log(interactables.Count);
                interactables.Remove(interactable);
            }
        }
    }
}