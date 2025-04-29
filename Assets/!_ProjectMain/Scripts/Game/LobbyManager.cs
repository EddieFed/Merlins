using System;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace __ProjectMain.Scripts.Game
{
    public class LobbyManager : MonoBehaviour
    {
        private const int MaxPlayers = 4;
        private int _currPlayersLobby = 0;
        private int _currentPlayersReady = 0;

        // Menu references
        [SerializeField] private Canvas menuCanvas;
        [SerializeField] private Button hostBtn;
        [SerializeField] private Button clientBtn;
        [SerializeField] private TMP_Text joinCodeText;
        [SerializeField] private TMP_InputField joinCodeInputField;
        [SerializeField] private Canvas howToPlayCanvas;
        [SerializeField] private Button howToPlayBtn;
        [SerializeField] private Button htpBackBtn;
        [SerializeField] private Canvas creditsCanvas;
        [SerializeField] private Button creditsBtn;
        [SerializeField] private Button cBackBtn;

        // Lobby references
        [SerializeField] private Canvas readyCanvas;
        [SerializeField] private Button readyButton;
        [SerializeField] private TMP_Text readyButtonText;
        [SerializeField] private TMP_Text playerCountText;

        private void Awake()
        {
            joinCodeText.text = string.Empty; // There is a placeholder for UI building, we MUST wipe it!
            menuCanvas.enabled = true;
            readyCanvas.enabled = false;
            howToPlayCanvas.enabled = false;
            creditsCanvas.enabled = false;

            hostBtn.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("Scene01 - Supermarket");
            });

            howToPlayBtn.onClick.AddListener(() =>
            {
                menuCanvas.enabled = false;
                howToPlayCanvas.enabled = true;
            });

            htpBackBtn.onClick.AddListener(() =>
            {
                menuCanvas.enabled = true;
                howToPlayCanvas.enabled = false;
            });

            creditsBtn.onClick.AddListener(() =>
            {
                menuCanvas.enabled = false;
                creditsCanvas.enabled = true;
            });

            cBackBtn.onClick.AddListener(() =>
            {
                menuCanvas.enabled = true;
                creditsCanvas.enabled = false;
            });
        }
    }
}
