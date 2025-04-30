using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace __ProjectMain.Scripts.Game
{
    public class EndManager : MonoBehaviour
    {
        // Menu references
        [SerializeField] private Canvas endCanvas;
        [SerializeField] private Button playAgainBtn;
        [SerializeField] private Button mainMenuBtn;

        private void Awake()
        {
            endCanvas.enabled = true;

            playAgainBtn.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("Scene01 - Supermarket");
            });

            mainMenuBtn.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("Scene00 - Main Menu");
            });
        }
    }
}
