using System;
using UnityEngine;

public class ItemCounter : MonoBehaviour
{
    private int itemCount = 0;
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
            itemCount++;
    }

    private void Update()
    {
        if (itemCount == 0)
            GetComponent<MeshRenderer>().material.color = Color.red;
        else
            GetComponent<MeshRenderer>().material.color = Color.green;
    }
}
