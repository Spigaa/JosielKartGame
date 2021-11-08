using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
public class NetworkManager : MonoBehaviourPunCallbacks{
    void Start(){
        DontDestroyOnLoad(this);
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "0.0.1";

        if(PhotonNetwork.IsConnected)
            PhotonNetwork.Disconnect();
    }

    public override void OnConnectedToMaster(){
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }          
    }

    public static void OnSceneChange()
    {
        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
    }

    public override void OnConnected()
    {
        base.OnConnected();
    }
}