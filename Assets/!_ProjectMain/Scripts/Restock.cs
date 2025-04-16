using System.Collections.Generic;
using __ProjectMain.Scripts;
using UnityEngine;

public class Restock : MonoBehaviour
{
    public Color shelfColor;
    private void OnCollisionEnter(Collision other)
    {
        // TODO: Make colors globally selectable
        List<Color> stockColors = new List<Color>
        {
            Color.blue,
            Color.magenta,
            Color.yellow
        };
        shelfColor = stockColors[Random.Range(0, stockColors.Count)];
        
        if (other.gameObject.CompareTag("Player"))
            other.gameObject.GetComponent<PlayerController>().heldRestock = shelfColor;
    }
    
    void Start()
    {
        GetComponent<MeshRenderer>().material.color = shelfColor;
    }
}
