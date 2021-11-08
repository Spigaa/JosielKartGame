using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class CarSelect : MonoBehaviourPunCallbacks {
    public GameObject player = null;
    public List<GameObject> cars = new List<GameObject>();
    public Button btnReady, btnStartGame, btnNextCar , btnPrevCar;

    [SerializeField] private List<GameObject> carsShowcase;

    public int index;

    public int carsShowcaseIndex;

    [SerializeField]
    private PlayersListingsMenu playersMenu;


    private void Start()
    {
        foreach (GameObject cars in carsShowcase) {
            cars.gameObject.SetActive(false);
            carsShowcase[carsShowcaseIndex].gameObject.SetActive(true);
        }
    }

    public void selectCar() {
        player = cars[carsShowcaseIndex];
        carsShowcase[carsShowcaseIndex].transform.gameObject.GetComponent<Animation>().Play();
        btnStartGame.interactable = true;
        btnReady.interactable = true;
        btnNextCar.interactable = false;
        btnPrevCar.interactable = false;

    }

    public void cancelSelect()
    {
        if (playersMenu._ready)
            playersMenu.OnClick_ReadyUp();
        player = null;
        carsShowcase[carsShowcaseIndex].transform.gameObject.GetComponent<Animation>().Play();
        btnStartGame.interactable = false;
        btnReady.interactable = false;
        btnNextCar.interactable = true;
        btnPrevCar.interactable = true;
    }

    GameObject _playerCar;

    public void enterGame(){
        Vector3 pos = new Vector3(0, 0, 0);
        _playerCar = PhotonNetwork.Instantiate(player.name, pos, Quaternion.identity, 0);
    }

    public void posCarsInGame(Vector3 pos, Quaternion rot){
        _playerCar.transform.position = pos;
        _playerCar.transform.rotation = rot;
    }

    public void btnNext(){
        if (carsShowcaseIndex == 10){
            carsShowcase[carsShowcaseIndex].gameObject.SetActive(false);
            carsShowcase[0].gameObject.SetActive(true);
            carsShowcaseIndex = 0;
        }
        else{
            carsShowcase[carsShowcaseIndex].gameObject.SetActive(false);
            carsShowcaseIndex++;
            carsShowcase[carsShowcaseIndex].gameObject.SetActive(true);
        }        
    }

    public void btnPrev(){   
        if(carsShowcaseIndex == 0){
            carsShowcase[carsShowcaseIndex].gameObject.SetActive(false);
            carsShowcase[10].gameObject.SetActive(true);
            carsShowcaseIndex = 10;
        }
        else{
            carsShowcase[carsShowcaseIndex].gameObject.SetActive(false);
            carsShowcaseIndex--;
            carsShowcase[carsShowcaseIndex].gameObject.SetActive(true);      
        }    
    }
}