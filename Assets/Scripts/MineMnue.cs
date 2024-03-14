using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using Mirror;
using TMPro;

namespace MyNet
{
    public class MineMnue : MonoBehaviour
    {
        LobbyManegers m_LobbyManegers;
        NetworkManager networkManager;

        [SerializeField] TextMeshProUGUI namePlayer;
        [SerializeField] TextMeshProUGUI nameRoom;

        // Start is called before the first frame update
        void Start()
        {
            m_LobbyManegers = GameObject.FindObjectOfType<LobbyManegers>();
            networkManager = GameObject.FindObjectOfType<NetworkManager>();

            namePlayer.text = SteamFriends.GetPersonaName();
            nameRoom.text = SteamMatchmaking.GetLobbyData(new CSteamID(m_LobbyManegers.LobbyId), "Name");
        }
        public void LeaveRoom()
        {
            print("Leave");
            SteamMatchmaking.LeaveLobby(new CSteamID(m_LobbyManegers.LobbyId));
            networkManager.StopHost();
            
            
            
        }
        // Update is called once per frame
        void Update()
        {

        }
    }
}
