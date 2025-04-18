using System;
using System.Collections.Generic;
using __ProjectMain.Scripts;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemCounter : MonoBehaviour
{
    private List<Color> stockColors;
    public Color shelfColor;
    public int itemCount;
    public int maxItems = 10;
    public int restockAmount = 5;
    public int minPrice = 5;
    public int maxPrice = 10;
    private AudioSource _audioSource;
    
    [SerializeField] public TextMeshPro stockCountText;
    [SerializeField] private MeshRenderer meshRenderer;
    
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
            _audioSource.Play();
        }
    }
    
    void Start()
    {
        
        stockColors = new List<Color>
        {
            Color.blue,
            Color.magenta,
            Color.yellow
        };
        shelfColor = stockColors[Random.Range(0, stockColors.Count)];
        meshRenderer.material.color = shelfColor;
        itemCount = Random.Range(1, maxItems);
        _audioSource = GetComponent<AudioSource>();
        destinations = new List<Transform>();
        destinations.Add(transform);
    }

    private void Update()
    {
        stockCountText.text = itemCount + "/" + maxItems;
        stockCountText.color = itemCount == 0 ? Color.red : Color.white;
    }
}
