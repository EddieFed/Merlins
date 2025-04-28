using System;
using __ProjectMain.Scripts;
using UnityEngine;

public class InteractBox : MonoBehaviour
{
    private PlayerController controller;
    private InteractionController interactionController;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Interactable"))
        {
            Debug.Log("Interactable enter");
            Debug.Log(other.gameObject.name);
            interactionController.InteractEnter(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Interactable"))
        {
            Debug.Log("Interactable exit");
            interactionController.InteractExit(other);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        interactionController = controller.GetComponent<InteractionController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
