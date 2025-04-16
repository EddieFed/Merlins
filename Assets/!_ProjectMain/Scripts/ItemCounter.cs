using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemCounter : MonoBehaviour
{
    private const int maxItems = 10;
    public int itemCount;
    private AudioSource _audioSource;
    [SerializeField] private TextMeshPro stockCountText;
    [SerializeField] private MeshRenderer meshRenderer;

    private void Start()
    {
        itemCount = Random.Range(1, maxItems);
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && itemCount < maxItems)
        {
            itemCount++;
            _audioSource.Play();
        }
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
