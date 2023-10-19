using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class raccogliOggetti : Photon.MonoBehaviour
{ 
    void Start()
    {
        if (!photonView.isMine)
            enabled = false;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "moneta")
        {
            PhotonNetwork.Destroy(col.gameObject);
            movimento.statistiche.monete++;
        }
    }
}
