using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using UnityEngine.SceneManagement;

public class SteamLeader : MonoBehaviour
{
    private void Start()
    {
        if(SteamAPI.Init())
        {
            SceneManager.LoadScene(1);

        }
        else
        {
            print("Please Loade Steam First");
        }
    }

}
