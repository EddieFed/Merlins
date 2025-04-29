using System.Collections.Generic;
using System.Runtime.CompilerServices;
using __ProjectMain.Scripts.Player;
using UnityEngine;

namespace __ProjectMain.Scripts.Game
{
    public class Restock : MonoBehaviour
    {
    
        public static Color Color1
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Color32(0xC0, 0x16, 0x16, 0xFF);
        }
    
        public static Color Color2
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Color32(0x00, 0xBC, 0x5E, 0xFF);
        }
    
        public static Color Color3
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Color32(0x00, 0x67, 0xD9, 0xFF);
        }
    
        public static Color Color4
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Color32(0xDD, 0x86, 0x00, 0xFF);
        }
    
        public static Color Color5
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Color32(0xFF, 0x52, 0xB2, 0xFF);
        }
    
        public Color shelfColor;
        [SerializeField] public int shelfNum = 0;

        public static readonly List<Color> AllColors = new() {Color1, Color2, Color3, Color4, Color5};

        public void RestockPlayer(GameObject controller)
        {
            controller.GetComponent<PlayerController>().heldRestock = shelfColor;
        }

        // private void OnCollisionEnter(Collision other)
        // {
        //     if (other.gameObject.CompareTag("Player"))
        //         other.gameObject.GetComponent<PlayerController>().heldRestock = shelfColor;
        // }
    
        void Start()
        {
            shelfColor = AllColors[shelfNum];
            GetComponent<MeshRenderer>().material.color = shelfColor;
        }
    }
}
