using System;
using System.Collections.Generic;
using __ProjectMain.Scripts;
using TMPro;
using UnityEngine;

public class ItemCounter : MonoBehaviour
{
    public Color shelfColor;
    public int itemCount = 10;
    public int maxItems = 10;
    public int restockAmount = 5;
    public int minPrice = 5;
    public int maxPrice = 10;
    public TextMeshPro stockCountText;
    public GameObject targetGroup;
    public List<Transform> destinations;
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") 
            && itemCount < maxItems 
            && Mathf.Abs(other.gameObject.GetComponent<PlayerController>().heldRestock.r - shelfColor.r) < 0.1f 
            && Mathf.Abs(other.gameObject.GetComponent<PlayerController>().heldRestock.g - shelfColor.g) < 0.1f 
            && Mathf.Abs(other.gameObject.GetComponent<PlayerController>().heldRestock.b - shelfColor.b) < 0.1f)
        {
            other.gameObject.GetComponent<PlayerController>().heldRestock = Color.clear;
            itemCount = itemCount + restockAmount >= maxItems ? maxItems : itemCount + restockAmount;
        }
    }
    
    void Start()
    {
        GetComponent<MeshRenderer>().material.color = shelfColor;
        destinations = new List<Transform>();
        foreach (Transform child in targetGroup.transform)
        {
            destinations.Add(child);
        }
    }

    private void Update()
    {
        stockCountText.text = itemCount + "/" + maxItems;
        stockCountText.color = itemCount == 0 ? Color.red : Color.white;
    }
}
