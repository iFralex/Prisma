using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GoogleSheetsForUnity;
using UnityEngine;
using UnityEngine.Events;

public class cambiaTessera : Photon.MonoBehaviour
{
    public Tessera tesSel;
    public Transform elencoBottoni;
    public static usaOggetti player;
    public forziere forzierePan;

    void Start()
    {
        SelezionaTessera(0);
    }

    public void SelezionaTessera(int n)
    {
        for (int i = 0; i < elencoBottoni.childCount; i++)
            if (i != n)
                elencoBottoni.GetChild(i).localScale = Vector3.one;
            else
                elencoBottoni.GetChild(i).localScale = Vector3.one * 1.2f;
        tesSel = Tessera.TesseraDaLista((Tessera.Tessere)n);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Camera.main.transform.forward, Mathf.Infinity, layerMask: 1 << 6);
            if (hit.collider != null)
            {
                if (hit.collider.tag == "moneta")
                {
                    movimento.statistiche.monete++;
                    PhotonNetwork.Destroy(hit.collider.gameObject);
                    return;
                }
                else if (hit.transform.parent.tag == "forziere" && gameManager.mode == gameManager.Modes.interattiva)
                {
                    forzierePan.id = hit.transform.parent.GetSiblingIndex();
                }
                else
                    photonView.RPC("ScegliAzione", PhotonTargets.AllViaServer, gameManager.mode, hit.transform.parent.GetSiblingIndex(), tesSel.index, usaOggetti.oggSel.index, player.GetComponentInParent<PhotonView>().viewID, Random.Range(300, 900));
                //Drive.SetCellValue("Tiles", "B", (2 + hit.transform.parent.GetSiblingIndex()).ToString(), hit.transform.tag != "Untaged" ? tesSel.index.ToString() : "null", true);
            }
        }
    }

    [PunRPC]
    public void ScegliAzione(gameManager.Modes mode, int tesIndex, Tessera.Tessere tesSelIndex, Oggetto.Oggetti oggSelIndex, int viewID, int tempo = 0)
    {
        Transform hitTr = gameManager.tessere.GetChild(tesIndex).GetChild(0);
        Tessera _tesSel = Tessera.TesseraDaLista(tesSelIndex);
        Oggetto _oggSel = Oggetto.OggettoDaLista(oggSelIndex);
        usaOggetti _player = PhotonView.Find(viewID).gameObject.GetComponentInChildren<usaOggetti>();
        if (hitTr.GetComponentInParent<mutaTessera>())
            Destroy(hitTr.GetComponentInParent<mutaTessera>());
        if (mode == gameManager.Modes.creativa)
        {
            StartCoroutine(CambiaColore(hitTr.GetComponent<SpriteRenderer>(), _tesSel.sprite));
            ImpostaTessera(hitTr.parent.gameObject, _tesSel);
            if (_tesSel.index == Tessera.Tessere.semi)
            {
                hitTr.parent.gameObject.AddComponent<mutaTessera>().AvviaGrano(tempo, Tessera.Tessere.grano, this);
                if (PhotonNetwork.isMasterClient)
                {
                    Drive.SetCellValue("Tiles", "C", (2 + tesIndex).ToString(), tempo.ToString(), true);
                    Drive.SetCellValue("Tiles", "D", (2 + tesIndex).ToString(), System.DateTime.UtcNow.ToString(), true);
                }
                print(tempo + "  " + tesIndex);
            }
            else if (_tesSel.index == Tessera.Tessere.zombi)
                hitTr.parent.gameObject.AddComponent<mutaTessera>().AvviaZombi(GetComponent<gameManager>());
            else if (_tesSel.index == Tessera.Tessere.forziere)
                Drive.CreateObject(Newtonsoft.Json.JsonConvert.SerializeObject(new forziere.datiForziere() { nomeForziere = "Tile " + tesIndex, oggetti = "" }), "chestshop objects");
            if (PhotonNetwork.isMasterClient)
                Drive.SetCellValue("Tiles", "B", (2 + tesIndex).ToString(), hitTr.tag != "Untaged" ? _tesSel.index.ToString() : "null", true);
        }
        else if (interaggisciConTessere.tessereInter.Contains(hitTr.parent.GetComponent<Collider2D>()) || player.GetComponentInParent<PhotonView>().viewID != viewID)
        {
            if (hitTr.parent.tag == Tessera.TesseraDaLista(Tessera.Tessere.acqua).tag && _oggSel.index == Oggetto.Oggetti.secchioVuoto)
                _player.Azioni(Oggetto.Oggetti.secchioVuoto);
            else if (hitTr.parent.tag == Tessera.TesseraDaLista(Tessera.Tessere.terra).tag && _oggSel.index == Oggetto.Oggetti.secchioPieno)
            {
                _player.Azioni(Oggetto.Oggetti.secchioPieno);
                CambiaTessera(hitTr, Tessera.Tessere.terraBagnata);
            }
            else if (hitTr.parent.tag == Tessera.TesseraDaLista(Tessera.Tessere.terraBagnata).tag && _oggSel.index == Oggetto.Oggetti.semi)
            {
                hitTr.parent.gameObject.AddComponent<mutaTessera>().AvviaGrano(tempo, Tessera.Tessere.grano, this);
                CambiaTessera(hitTr, Tessera.Tessere.semi);
                if (PhotonNetwork.isMasterClient)
                {
                    Drive.SetCellValue("Tiles", "C", (2 + tesIndex).ToString(), tempo.ToString(), true);
                    Drive.SetCellValue("Tiles", "D", (2 + tesIndex).ToString(), System.DateTime.UtcNow.ToString(), true);
                    print(tempo + "  " + tesIndex);
                }
            }
            else if (hitTr.parent.tag == Tessera.TesseraDaLista(Tessera.Tessere.grano).tag && _oggSel.index == Oggetto.Oggetti.falce)
            {
                if (viewID == player.GetComponentInParent<PhotonView>().viewID)
                    _player.pane++;
                print(GetComponentInParent<PhotonView>().viewID + "  " + viewID + "    " + _player.pane);
                CambiaTessera(hitTr, Tessera.Tessere.terra);
            }
        }
    }

    public IEnumerator CambiaColore(SpriteRenderer tes, Sprite sprite)
    {
        tes.enabled = true;
        for (float i = 0; i < 1; i += .1f)
        {
            tes.color = new Color(1, 1, 1, Mathf.Lerp(0, 1, i));
            yield return new WaitForSeconds(.02f);
        }

        tes.transform.parent.GetComponent<SpriteRenderer>().sprite = sprite;
        for (float i = 0; i < 1; i += .1f)
        {
            tes.color = new Color(1, 1, 1, Mathf.Lerp(1, 0, i));
            yield return new WaitForSeconds(.02f);
        }
        tes.enabled = false;
    }

    public void ImpostaTessera(GameObject ob, Tessera t)
    {
        ob.GetComponent<BoxCollider2D>().enabled = t.collisione;
        ob.GetComponent<BoxCollider2D>().isTrigger = t.trigger;
        ob.tag = t.tag;
        if (t.index == Tessera.Tessere.muro)
            ob.layer = LayerMask.NameToLayer("Muro");
        else
            ob.layer = LayerMask.NameToLayer("Default");
    }

    public void CambiaTessera(Transform tr, Tessera.Tessere t, bool sincronizza = true)
    {
        if (PhotonNetwork.isMasterClient && sincronizza)
            Drive.SetCellValue("Tiles", "B", (2 + tr.parent.GetSiblingIndex()).ToString(), t.ToString(), true);
        SelezionaTessera((int)t);
        StartCoroutine(CambiaColore(tr.GetComponent<SpriteRenderer>(), tesSel.sprite));
        ImpostaTessera(tr.parent.gameObject, tesSel);
    }
}

public class Tessera
{
    public Sprite sprite;
    public bool collisione;
    public bool trigger;
    public string tag;
    public enum Tessere { erba, muro, pavimento, sentiero, terra, terraBagnata, acqua, grano, semi, zombi, forziere }
    public Tessere index;

    public static List<Tessera> listaTessere = new List<Tessera>()
    {
        new Tessera() {tag = "erba", collisione = true, trigger = true, sprite = Resources.LoadAll<Sprite>("tessere").Single(s => s.name == "erba tessera"), index = Tessere.erba },
        new Tessera() {tag = "muro", collisione = true, trigger = false, sprite = Resources.LoadAll<Sprite>("tessere").Single(s => s.name == "muro tessera"), index = Tessere.muro },
        new Tessera() {tag = "pavimento", collisione = true, trigger = true, sprite = Resources.LoadAll<Sprite>("tessere").Single(s => s.name == "pavimento tessera"), index = Tessere.pavimento },
        new Tessera() {tag = "sentiero", collisione = true, trigger = true, sprite = Resources.LoadAll<Sprite>("tessere").Single(s => s.name == "sentiero tessera"), index = Tessere.sentiero },
        new Tessera() {tag = "terra", collisione = true, trigger = true, sprite = Resources.LoadAll<Sprite>("tessere").Single(s => s.name == "terra tessera"), index = Tessere.terra },
        new Tessera() {tag = "terra bagnata", collisione = true, trigger = true, sprite = Resources.LoadAll<Sprite>("tessere").Single(s => s.name == "terra bagnata tessera"), index = Tessere.terraBagnata },
        new Tessera() {tag = "acqua", collisione = true, trigger = true, sprite = Resources.LoadAll<Sprite>("tessere").Single(s => s.name == "acqua tessera"), index = Tessere.acqua },
        new Tessera() {tag = "grano", collisione = true, trigger = true, sprite = Resources.LoadAll<Sprite>("tessere").Single(s => s.name == "grano tessera"), index = Tessere.grano },
        new Tessera() {tag = "semi", collisione = true, trigger = true, sprite = Resources.LoadAll<Sprite>("oggetti").Single(s => s.name == "semi oggetto"), index = Tessere.semi },
        new Tessera() {tag = "zombi", collisione = true, trigger = true, sprite = Resources.LoadAll<Sprite>("tessere").Single(s => s.name == "zombi tessera"), index = Tessere.zombi },
        new Tessera() {tag = "forziere", collisione = true, trigger = true, sprite = Resources.LoadAll<Sprite>("tessere").Single(s => s.name == "forziere tessera"), index = Tessere.forziere }
    };

    public static Tessera TesseraDaLista(Tessere t)
    {
        foreach (Tessera a in listaTessere)
            if (a.index == t)
                return a;
        return null;
    }
}