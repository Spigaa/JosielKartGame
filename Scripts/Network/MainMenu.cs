//using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using Photon.Pun.UtilityScripts;

public class MainMenu : MonoBehaviourPunCallbacks{
    public InputField privateRoomName;
    public InputField selectRoomName;
    public Button btnConnect, btnCreate;
    public GameObject statusConnecting;
    public Text status, txtSala;
    private Dictionary<string, RoomInfo> cachedRoomList;
    private Dictionary<string, GameObject> roomListEntries;
    public InputField PlayerNameInput;

    public GameObject multiplayerCanvas,mainCanvas;

    private void Awake()
    {
        cachedRoomList = new Dictionary<string, RoomInfo>();
        roomListEntries = new Dictionary<string, GameObject>();

        privateRoomName.interactable = false;
        selectRoomName.interactable = false;
        btnConnect.interactable = false;
        btnCreate.interactable = false;
        txtSala.text = "";
    }
    public void buttonPrivateRoomClicked()
    {
        if (PhotonNetwork.IsConnected)
        {
            if(privateRoomName.text.Length > 1)
            {
                txtSala.text = "";
                PhotonNetwork.CreateRoom(privateRoomName.text);
                Debug.Log("Room Name: " + PhotonNetwork.CurrentRoom);
            }
            else
            {
                txtSala.text = "O nome da sala deve conter ao menos 2 letras";
            }     
        }
    }
    public void OnLoginButtonClicked()
    {
        if (!PhotonNetwork.IsConnected)
        {
            string playerName = PlayerNameInput.text;
            if (playerName.Length > 1)
            {
                if (!playerName.Equals(""))
                {
                    StartCoroutine(Connecting(3));
                    PhotonNetwork.LocalPlayer.NickName = playerName;
                    PhotonNetwork.ConnectUsingSettings();
                    mainCanvas.SetActive(false);
                    statusConnecting.gameObject.SetActive(true);
                }
                else
                {
                    Debug.LogError("Player Name is invalid.");
                }    
            }
        }   
    }

    public void buttonSelectRoomClicked()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (selectRoomName.text.Length > 1)
            {
                txtSala.text = "";
                PhotonNetwork.JoinRoom(selectRoomName.text);
            }
            else
            {
                txtSala.text = "O nome da sala deve conter ao menos 2 letras";
            }    
        }
    }

    public void buttonDisconnectClicked()
    {
        if (PhotonNetwork.IsConnected)
        {
            status.text = "Status: Desconectado";
            status.color = Color.red;
            PhotonNetwork.Disconnect();
            btnConnect.interactable = false;
            btnCreate.interactable = false;
            privateRoomName.interactable = false;
            selectRoomName.interactable = false;
        }
    }

    IEnumerator Connecting(int time)
    {
        yield return new WaitForSeconds(time);
        statusConnecting.gameObject.SetActive(false);
        multiplayerCanvas.SetActive(true);
        status.text = "Status: Conectado";
        status.color = Color.green;
        btnConnect.interactable = true;
        btnCreate.interactable = true;
        privateRoomName.interactable = true;
        selectRoomName.interactable = true;
    }
}