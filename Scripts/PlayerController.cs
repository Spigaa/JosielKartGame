using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviour
{
    private PhotonView photonview;
    public Camera playerCam;
    public GameObject EscMenu;

    private void Awake()

    {
        DontDestroyOnLoad(this.gameObject);

    }
    // Start is called before the first frame update
    void Start()
    {
        this.transform.tag = "Player";
        this.photonview = PhotonView.Get(this);
        this.playerCam.enabled = true;
        if (photonview.IsMine)
        {
            this.transform.GetComponent<PlayerScript>().enabled = true;
            this.transform.GetComponent<Laps>().enabled = true;
        }
        else
        {
            transform.GetComponent<PlayerScript>().enabled = false;
            transform.GetComponent<Laps>().enabled = false;
            Destroy(transform.GetChild(1).gameObject);
            Destroy(transform.GetChild(0).GetChild(2).gameObject);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!photonview.IsMine)
        {
            //playerCam.enabled = false;
            return;
        }
        else
        {
            this.playerCam.enabled = true;
            if (Input.GetKeyDown(KeyCode.Escape))
                EscMenu.SetActive(true);
        }
    }
}
