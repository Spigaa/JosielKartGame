using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class EscMenu : MonoBehaviour
{
    public void disconnect()
    {
        PhotonNetwork.LoadLevel(0);
    }

    public void SetQuality(int qualityIndex){
        QualitySettings.SetQualityLevel(qualityIndex);
    }
}
