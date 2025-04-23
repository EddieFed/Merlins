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

namespace __ProjectMain.Scripts
{
    public class LobbyManager : NetworkBehaviour
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
            DontDestroyOnLoad(this.gameObject);
            
            joinCodeText.text = string.Empty; // There is a placeholder for UI building, we MUST wipe it!
            menuCanvas.enabled = true;
            readyCanvas.enabled = false;
            howToPlayCanvas.enabled = false;
            creditsCanvas.enabled = false;
            
            hostBtn.onClick.AddListener(StartHostRelay);
            clientBtn.onClick.AddListener(StartClientRelay);
            readyButton.onClick.AddListener(() =>
            {
                if (readyButtonText.text.EndsWith("?"))
                {
                    readyButtonText.text = "Ready!";
                    readyButtonText.color = Color.green;
                    if (IsClient)
                    {
                        PlayerReadyServerRpc();
                    }
                }
                else
                {
                    readyButtonText.text = "Ready?";
                    readyButtonText.color = Color.red;
                    if (IsClient)
                    {
                        PlayerUnReadyServerRpc();
                    }
                }
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

        private async void Start()
        {
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
                string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
                Debug.Log($"[{i}] {sceneName} (Path: {scenePath})");
            }
            
            try
            {
                await UnityServices.InitializeAsync();
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                Debug.Log($"Sign in anonymously succeeded! PlayerID: {AuthenticationService.Instance.PlayerId}");
                
                NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        // Start host relay
        private async void StartHostRelay()
        {
            try
            {
                if (joinCodeText == null || joinCodeText.text.Length > 0)
                {
                    Debug.LogError("You must reference a text field");
                    return;
                }

                Allocation allocation = await RelayService.Instance.CreateAllocationAsync(MaxPlayers);
                string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
                joinCodeText.SetText($"Lobby Code: {joinCode}");
                RelayServerData serverData = allocation.ToRelayServerData("dtls");
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(serverData);
                
                Debug.Log($"JoinCode: {joinCode}");
                GUIUtility.systemCopyBuffer = $"{joinCode}";
                NetworkManager.Singleton.StartHost();
            }
            catch (RelayServiceException e)
            {
                Debug.LogException(e);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private async void StartClientRelay()
        {
            try
            {
                if (joinCodeInputField == null || joinCodeInputField.text.Length < 1)
                {
                    Debug.LogError("You must reference a input field value");
                    return;
                }

                string joinCode = joinCodeInputField.textComponent.text[..6];
                JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
                RelayServerData serverData = allocation.ToRelayServerData("dtls");
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(serverData);
                
                Debug.Log($"JoinCode: {joinCode}");
                NetworkManager.Singleton.StartClient();
            }
            catch (RelayServiceException e)
            {
                Debug.LogException(e);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private void OnClientConnectedCallback(ulong clientId)
        {
            Debug.Log($"Connected to server with ID: {clientId}");
            _currPlayersLobby++;
            menuCanvas.enabled = false;
            readyCanvas.enabled = true;
        }

        [ServerRpc(RequireOwnership = false)]
        private void PlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
        {
            ulong clientId = serverRpcParams.Receive.SenderClientId;
            
            _currentPlayersReady++;
            UpdateReadyPlayersClientRpc(_currentPlayersReady);
            Debug.Log($"{clientId} is ready");

            if (_currentPlayersReady == MaxPlayers || _currentPlayersReady == _currPlayersLobby)
            {
                string targetScene = "Scene01 - Supermarket";
                SceneEventProgressStatus status = NetworkManager.SceneManager.LoadScene(targetScene, LoadSceneMode.Additive);
                if (status != SceneEventProgressStatus.Started)
                {
                    Debug.LogWarning($"Failed to load {targetScene} with a {nameof(SceneEventProgressStatus)}: {status}");
                    Debug.LogWarning($"LOL?? {SceneManager.GetSceneByBuildIndex(1).name}");
                }
            }
        }
        
        [ServerRpc(RequireOwnership = false)]
        private void PlayerUnReadyServerRpc(ServerRpcParams serverRpcParams = default)
        {
            ulong clientId = serverRpcParams.Receive.SenderClientId;
            
            _currentPlayersReady--;
            UpdateReadyPlayersClientRpc(_currentPlayersReady);
            Debug.Log($"{clientId} is NOT ready");
        }

        [ClientRpc]
        private void UpdateReadyPlayersClientRpc(int readyPlayersCount)
        {
            _currentPlayersReady = readyPlayersCount;
            playerCountText.SetText(_currentPlayersReady.ToString());
        }

    }
}