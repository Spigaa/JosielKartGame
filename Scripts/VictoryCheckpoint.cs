using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryCheckpoint : MonoBehaviourPunCallbacks{
    PhotonView photonview;
    public CarSelect _carSelect;
    public Button btnStart;
    public Text startText;
    public bool raceStarts = false;
    public AudioSource pim, pam;

    private void Start(){
        photonview = PhotonView.Get(this);
        _carSelect = FindInActiveObjectByName("NetworkController").GetComponent<CarSelect>();
        PhotonNetwork.AutomaticallySyncScene = true;

        raceStarts = false;

        _carSelect.enterGame();
        StartGame();
        
        if(!PhotonNetwork.IsMasterClient)
            Destroy(btnStart.gameObject);

    }

    void Update(){
        if(PhotonNetwork.IsMasterClient)
            if (PhotonNetwork.CurrentRoom.PlayerCount >= 2)
                btnStart.interactable = true;
    }

    public void startRaceClick(){
        photonview.RPC("RPCStartRace", RpcTarget.All);
        Destroy(btnStart.gameObject);
    }

    [PunRPC]
    void RPCStartRace(){
        StartCoroutine(StartRace());
    }

    IEnumerator StartRace(){
        yield return new WaitForSeconds(1);
        startText.text = "5";
        pim.Play();
        yield return new WaitForSeconds(1);
        startText.text = "4";
        pim.Play();
        yield return new WaitForSeconds(1);
        startText.text = "3";
        pim.Play();
        yield return new WaitForSeconds(1);
        startText.text = "2";
        pim.Play();
        yield return new WaitForSeconds(1);
        startText.text = "1";
        pim.Play();
        yield return new WaitForSeconds(1);
        startText.text = "GO!";
        pam.Play();
        raceStarts = true;
        yield return new WaitForSeconds(1.5f);
        startText.text = "";
    }


    GameObject FindInActiveObjectByName(string name){
        Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].hideFlags == HideFlags.None)
            {
                if (objs[i].name == name)
                {
                    return objs[i].gameObject;
                }
            }
        }
        return null;
    }

    [SerializeField]
    public Transform[] spawnPoints;

    void StartGame()
    {
        Photon.Realtime.Player[] players = PhotonNetwork.PlayerList;
        for (int i = 0; i < players.Length; i++){
            photonview.RPC("RPCStartGame", players[i], spawnPoints[i].position, spawnPoints[i].rotation);
        }
    }
    [PunRPC]
    void RPCStartGame(Vector3 spawnPos, Quaternion spawnRot){
        _carSelect.posCarsInGame(spawnPos, spawnRot);
    }


    [SerializeField]
    public Transform[] victoryTPoints;

    [SerializeField]
    public Transform _player;

    int index = 0;

    public void FinishGame(Transform car){
        _player = car;
        photonview.RPC("RPCEndRace", RpcTarget.All, victoryTPoints[index].position, victoryTPoints[index].rotation);
    }

    [PunRPC]
    void RPCEndRace(Vector3 spawnPos, Quaternion spawnRot){
        index++;
        _player.transform.position = spawnPos;
        _player.transform.rotation = spawnRot;
        _player = null;
    }
}
