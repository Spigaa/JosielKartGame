using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayersListingsMenu : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Transform _content;
    [SerializeField]
    private PlayerListing _playerListining;
    [SerializeField]
    private RawImage _readyUp;
    [SerializeField]
    private CarSelect _carSelect;

    private List<PlayerListing> _listings = new List<PlayerListing>();
    private RoomsCanvases _roomsCanvases;
    public bool _ready = false;

    public Button _readyBtn, _playBtn;

    private void Awake()
    {
        GetCurrentRoomPlayers();
        if (PhotonNetwork.IsMasterClient){
            _readyBtn.gameObject.SetActive(false);
        }
        else{
            _playBtn.gameObject.SetActive(false);
        }
    }

    public override void OnEnable()
    {
        base.OnEnable();
        SetReadyUp(false);
    }

    public void FirstInitialize(RoomsCanvases canvases)
    {
        _roomsCanvases = canvases;
    }

    private void SetReadyUp(bool state)
    {
        _ready = state;
        if (_ready)
        {
            _readyUp.color = Color.green;
        }

        else
        {
            _readyUp.color = Color.red;
        }
            
    }

    public override void OnLeftRoom()
    {
        _content.DestroyChildren();
    }

    private void GetCurrentRoomPlayers()
    {
        if (!PhotonNetwork.IsConnected)
            return;
        if (PhotonNetwork.CurrentRoom == null || PhotonNetwork.CurrentRoom.Players == null)
            return;

        foreach(KeyValuePair<int, Player> playerInfo in PhotonNetwork.CurrentRoom.Players)
        {
            AddPlayerListing(playerInfo.Value);
        }     
    }

    PlayerListing listingColor;

    private void AddPlayerListing(Player player)
    {
        PlayerListing listing = Instantiate(_playerListining, _content);
        if (listing != null){
            listing.SetPlayerInfo(player);
            _listings.Add(listing);
        }

        listingColor = listing;
    }



    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        _roomsCanvases.CurrentRoomCanvas.LeaveRoomMenu.OnClick_LeaveRoom();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddPlayerListing(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        int index = _listings.FindIndex(x => x.Player == otherPlayer);
        if (index != -1)
        {
            Destroy(_listings[index].gameObject);
            _listings.RemoveAt(index);
        }
    }

    public void OnClick_StartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < _listings.Count; i++)
            {
                if(_listings[i].Player != PhotonNetwork.LocalPlayer)
                {
                    if (!_listings[i].Ready)
                        return;
                }
            }
            _playBtn.interactable = false;
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.LoadLevel(1);
        }     
    }

    public void OnClick_ReadyUp()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            SetReadyUp(!_ready);
            base.photonView.RPC("RPC_ChangeReadyState", RpcTarget.MasterClient,PhotonNetwork.LocalPlayer, _ready);
            base.photonView.RPC("RPC_changeColor", RpcTarget.All, _ready);
        }
    }

    [PunRPC]
    private void RPC_ChangeReadyState(Player player, bool ready)
    {
        int index = _listings.FindIndex(x => x.Player == player);
        if (index != -1)
            _listings[index].Ready = ready;  
    }

   [PunRPC]
    private void RPC_changeColor(bool ready)
    {
        if (ready)
            listingColor.gameObject.GetComponent<RawImage>().color = Color.green;
        else
            listingColor.gameObject.GetComponent<RawImage>().color = Color.red;
    }
}