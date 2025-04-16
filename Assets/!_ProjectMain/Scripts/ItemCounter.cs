using System;
using TMPro;
using UnityEngine;

public class ItemCounter : MonoBehaviour
{
    public int itemCount = 10;
    public int maxItems = 10;
    [SerializeField] private TextMeshPro stockCountText;
    [SerializeField] private MeshRenderer meshRenderer;
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && itemCount < maxItems)
            itemCount++;
    }

    private void Update()
    {
        stockCountText.text = itemCount + "/" + maxItems;
        
        if (itemCount == 0)
            meshRenderer.material.color = Color.red;
        else
            meshRenderer.material.color = Color.green;
    }
}
