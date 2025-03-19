using System;
using TMPro;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Multiplayer;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace __ProjectMain.Scripts
{
    public class SessionManager : MonoBehaviour
    {
        [SerializeField] private Button hostBtn;
        [SerializeField] private Button clientBtn;
        [SerializeField] private TMP_Text joinCodeText;
        public string joinCode;
        public TMP_Text gameTitle;

        [SerializeField] private TMP_InputField joinCodeInputField;

        private const string _sceneName = "Scene99 - Eddie Test";

        private void Awake()
        {
            hostBtn.onClick.AddListener(StartHostRelay);
            clientBtn.onClick.AddListener(StartClientRelay);
        
        }
        
        async void Start()
        {
            try
            {
                await UnityServices.InitializeAsync();
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                Debug.Log($"Sign in anonymously succeeded! PlayerID: {AuthenticationService.Instance.PlayerId}");
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    
        // Start host relay
        public async void StartHostRelay()
        {
            SessionOptions options = new SessionOptions
            {
                MaxPlayers = 2
            }.WithRelayNetwork(); // or WithDistributedAuthorityNetwork() to use Distributed Authority instead of Relay
            IHostSession session = await MultiplayerService.Instance.CreateSessionAsync(options);
            Debug.Log($"Session {session.Id} created! Join code: {session.Code}");
            
            Debug.Log($"Loading {_sceneName}");
            SceneManager.LoadScene(_sceneName, LoadSceneMode.Single);
        }

        // start client relay
        public async void StartClientRelay()
        {
            if (joinCodeInputField.text.Length > 0)
            {
                await MultiplayerService.Instance.JoinSessionByCodeAsync(joinCodeInputField.text);
            }
        }
    }
}