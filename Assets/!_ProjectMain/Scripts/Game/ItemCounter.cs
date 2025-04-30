using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace __ProjectMain.Scripts.Game
{
    public class ItemCounter : MonoBehaviour
    {
        private List<Color> stockColors;
        public Color shelfColor;
        public int itemCount;
        public int maxItems = 10;
        public int restockAmount = 10;
        public int minPrice = 5;
        public int maxPrice = 10;
        private AudioSource _audioSource;
    
        [SerializeField] public TextMeshPro stockCountText;
        [SerializeField] private MeshRenderer meshRenderer;
        
        public GameObject targetGroup;
        public List<Transform> destinations;

        public void AcceptRestock(Player.PlayerController player)
        {
        
            if (itemCount < maxItems 
                && Mathf.Abs(player.heldRestock.r - shelfColor.r) < 0.1f 
                && Mathf.Abs(player.heldRestock.g - shelfColor.g) < 0.1f 
                && Mathf.Abs(player.heldRestock.b - shelfColor.b) < 0.1f)
            {
                player.heldRestock = Color.clear;
                itemCount = itemCount + restockAmount >= maxItems ? maxItems : itemCount + restockAmount;
                _audioSource.Play();
            }
        }
    
        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("Player") && Input.GetKey(Interactable.InteractionController.InteractKey))
            {
                AcceptRestock(other.gameObject.GetComponent<Player.PlayerController>());
            }
        }
    
        void Start()
        {
            // Since we are not oriented to the player, flip the text on init
            if (transform.localRotation.eulerAngles.y != 0)
            {
                stockCountText.transform.localRotation = Quaternion.Euler(
                    stockCountText.transform.localRotation.eulerAngles.x, 
                    180 + stockCountText.transform.localRotation.eulerAngles.y,
                    stockCountText.transform.localRotation.eulerAngles.z
                );
            }

            stockColors = Restock.AllColors;
            shelfColor = stockColors[Random.Range(0, stockColors.Count)];
            meshRenderer.material.SetColor("_Base_Color", shelfColor);
            itemCount = Random.Range(1, maxItems);
            _audioSource = GetComponent<AudioSource>();
            destinations = new List<Transform>();
            destinations.Add(transform);
        }

        private void Update()
        {
            stockCountText.text = itemCount + "/" + maxItems;
            stockCountText.color = itemCount == 0 ? new Color32(0xC0, 0x16, 0x16, 0xFF) : new Color32(221, 221, 221, 255);
        }

        public void FlipTextPerspective()
        {
            stockCountText.transform.localRotation = Quaternion.Euler(
                stockCountText.transform.localRotation.eulerAngles.x,
                (180 + stockCountText.transform.localRotation.eulerAngles.y) % 360,
                stockCountText.transform.localRotation.eulerAngles.z
            );
        }
    }
}
