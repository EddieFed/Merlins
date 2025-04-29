using __ProjectMain.Scripts;
using __ProjectMain.Scripts.Game;
using __ProjectMain.Scripts.Interactable;
using UnityEngine;

public class RestockShelf : MonoBehaviour, IInteractable
{
    private Restock restock;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        restock = GetComponent<Restock>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Interact(GameObject controller)
    {
        Debug.Log("Interact Shelf");
        restock.RestockPlayer(controller);
    }
}