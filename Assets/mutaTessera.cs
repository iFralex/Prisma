using System.Collections;
using System.Collections.Generic;
using GoogleSheetsForUnity;
using UnityEngine;

public class mutaTessera : Photon.MonoBehaviour
{
    bool azioneAttivata;

    private void Start()
    {

    }

    public void AvviaGrano(int tempo, Tessera.Tessere tesFin, cambiaTessera ct)
    {
        StartCoroutine(MutaGrano(tempo, tesFin, ct));
    }

    IEnumerator MutaGrano(float tempo, Tessera.Tessere tesFin, cambiaTessera ct)
    {
        azioneAttivata = true;
        yield return new WaitForSeconds(tempo);
        Tessera t = Tessera.TesseraDaLista(tesFin);
        ct.StartCoroutine(ct.CambiaColore(transform.GetChild(0).GetComponent<SpriteRenderer>(), t.sprite));
        ct.ImpostaTessera(gameObject, t);
        Destroy(this);
        if (PhotonNetwork.isMasterClient)
        {
            Drive.SetCellValue("Tiles", "C", transform.GetSiblingIndex().ToString(), "", true);
            Drive.SetCellValue("Tiles", "D", transform.GetSiblingIndex().ToString(), "", true);
        }
    }

    public void AvviaZombi(gameManager gm)
    {
        StartCoroutine(MutaZombi(60, gm));
    }

    IEnumerator MutaZombi(float tempo, gameManager gm)
    {
        azioneAttivata = true;
        for (; ; )
        {
            yield return new WaitForSeconds(tempo);
            if (PhotonNetwork.isMasterClient)
                if (gm.ai.Count < 3)
                {
                    Vector3 pos = (Vector3)(Random.insideUnitCircle * 5) + transform.position;
                    gm.ai.Add(PhotonNetwork.InstantiateSceneObject(gm.zombiPrefab.name, new Vector3(pos.x, pos.y, -1.1f), Quaternion.identity, 0, null).GetComponent<AIDestinationSetter>());
                    PhotonNetwork.InstantiateSceneObject("moneta", pos, Quaternion.identity, 0, null);
                }
        }
        azioneAttivata = false;
    }
}