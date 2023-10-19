using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interaggisciConTessere : Photon.MonoBehaviour
{
    public static List<Collider2D> tessereInter = new List<Collider2D>();
    public bool attivo;

    void Start()
    {
        if (!GetComponentInParent<PhotonView>().isMine)
            Destroy(gameObject);
    }

    public void AttivaDisattiva(bool a)
    {
        attivo = a;
        for (int i = 0; i < tessereInter.Count; i++)
            tessereInter[i].transform.GetChild(1).gameObject.SetActive(a);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (usaOggetti.oggSel == Oggetto.OggettoDaLista(0))
            return;
        if (col.transform.childCount > 0)
            if (col.transform.GetChild(0).tag != "tessera")
                return;
        if (tessereInter.Contains(col))
            return;
        else
            tessereInter.Add(col);

        if (attivo)// && col.tag != "Default")
            col.transform.GetChild(1).gameObject.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.transform.childCount > 0)
            if (col.transform.GetChild(0).tag != "tessera")
                return;
        if (!tessereInter.Contains(col))
            return;
        else
            tessereInter.Remove(col);

        col.transform.GetChild(1).gameObject.SetActive(false);
    }
}
