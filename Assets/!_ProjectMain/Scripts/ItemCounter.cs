using System;
using TMPro;
using UnityEngine;

public class ItemCounter : MonoBehaviour
{
    public int itemCount = 10;
    public int maxItems = 10;
    public TextMeshPro stockCountText;
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && itemCount < maxItems)
            itemCount++;
    }

    private void Update()
    {
        stockCountText.text = itemCount + "/" + maxItems;
        
        if (itemCount == 0)
            GetComponent<MeshRenderer>().material.color = Color.red;
        else
            GetComponent<MeshRenderer>().material.color = Color.green;
    }
}
