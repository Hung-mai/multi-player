using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class MatchManager : MonoBehaviour
{
    public static MatchManager ins;

    private void Awake() 
    {
        ins = this;
    }

    private void Start() {
        if(PhotonNetwork.IsConnected == false)
        {
            SceneManager.LoadScene(0);
        }
    }

    public void btn_BackToMainMenu()
    {

    }
    

}
