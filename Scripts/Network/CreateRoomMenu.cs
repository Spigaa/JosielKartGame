using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;

public class CreateRoomMenu : MonoBehaviourPunCallbacks{
    [SerializeField]
    private Text _roomName;

    private RoomsCanvases _roomsCanvases;
    public void FirstInitialize(RoomsCanvases canvases)
    {
        _roomsCanvases = canvases;
    }


    [SerializeField]
    private Dropdown _MaxPlayerstxt;

    [SerializeField]
    private byte _MaxPlayers;

    [SerializeField]
    private Button btnCreateRoom;

    private void Awake() {
        _MaxPlayers = 2;
    }

    public void OnClick_CreateRoom(){
        if (!PhotonNetwork.IsConnected)
            return;
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = _MaxPlayers;
        PhotonNetwork.JoinOrCreateRoom(_roomName.text, options, TypedLobby.Default);
        btnCreateRoom.interactable = false;
    }

    private void Update(){
        if (_MaxPlayerstxt.value == 0){
            _MaxPlayers = 2;
        }
        if (_MaxPlayerstxt.value == 1){
            _MaxPlayers = 3;
        }
        if (_MaxPlayerstxt.value == 2){
            _MaxPlayers = 4;
        }

        if(_roomName.text.Length <= 1)
        {
            btnCreateRoom.interactable = false;
        }
        else
        {
            btnCreateRoom.interactable = true;
        }

    }

    public override void OnCreatedRoom()
    {
        _roomsCanvases.CurrentRoomCanvas.Show();
        _roomsCanvases.CreateOrJoinRoomCanvas.Hide();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        
    }
}
