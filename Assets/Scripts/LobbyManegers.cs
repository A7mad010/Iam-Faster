using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;
using Core.Attributes;
using UnityEngine.SceneManagement;
using System;

namespace MyNet
{
    public class LobbyManegers : MonoBehaviour
    {
        public event Action<LobbyEnter_t> onLobbyEnter;
        public event Action<LobbyCreated_t> onLobbyCreat;


        public static LobbyManegers instance;

        //CallBack
        protected Callback<LobbyCreated_t> LobbyCreat;
        protected Callback<LobbyEnter_t> LobbyEnter;

        public ulong LobbyId;
        private const string HostAdrres = "HostAdress";
        private NetworkManager _networkManager;
        public void Awake()
        {
            if (!instance)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
        }
        private void Start()
        {
            if (!SteamManager.Initialized)
            {
                SceneManager.LoadScene(0);
            }
            _networkManager = GetComponent<NetworkManager>();
            LobbyCreat = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
            LobbyEnter = Callback<LobbyEnter_t>.Create(OnLobbyEnter);
        }

        private void OnLobbyCreated(LobbyCreated_t callback)
        {
            if (callback.m_eResult != EResult.k_EResultOK)
            {
                print("Check Your Wif");
                return;
            }

            var lobbyId = callback.m_ulSteamIDLobby;
            print("CreatLobby : " + lobbyId);

            _networkManager.StartHost();

            SteamMatchmaking.SetLobbyData(new CSteamID(lobbyId), HostAdrres, SteamUser.GetSteamID().ToString());
            SteamMatchmaking.SetLobbyData(new CSteamID(lobbyId), "Name", SteamFriends.GetPersonaName() + "`s Lobby");

            onLobbyCreat?.Invoke(callback);
        }

        private void OnJoinLobby(ulong lobby)
        {
            SteamMatchmaking.JoinLobby(new CSteamID(lobby));
        }
        private void OnLobbyEnter(LobbyEnter_t callBack)
        {
            LobbyId = callBack.m_ulSteamIDLobby;

            if (NetworkServer.active) return; // if actice is true your a Host

            string hostAdress = SteamMatchmaking.GetLobbyData(new CSteamID(LobbyId), HostAdrres);
            _networkManager.networkAddress = hostAdress;
            _networkManager.StartClient();

            onLobbyEnter?.Invoke(callBack);
        }
        [Button("Creat Room")]
        public void CreatLobby()
        {
            SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, 2);


        }

    }
}
