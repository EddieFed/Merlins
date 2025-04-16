using __ProjectMain.Scripts;
using UnityEngine;

public class Restock : MonoBehaviour
{
    public Color shelfColor;
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
            other.gameObject.GetComponent<PlayerController>().heldRestock = shelfColor;
    }
    
    void Start()
    {
        GetComponent<MeshRenderer>().material.color = shelfColor;
    }
}
