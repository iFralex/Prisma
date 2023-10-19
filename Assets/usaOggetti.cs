using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class usaOggetti : MonoBehaviour
{
    public static Oggetto oggSel;
    public static statistiche statistiche;
    public static cambiaTessera cambTes;

    [Header("Pane")]
    int _pane = -1;
    public int pane
    {
        get { return _pane; }
        set
        {
            if (_pane == value)
                return;
            _pane = value;

            paneT.text = value.ToString() + "/" + paneMassimo.ToString();
            paneT.transform.parent.GetComponent<Button>().interactable = value > 0;
        }
    }
    public int paneMassimo;
    public static Text paneT;

    public static Transform oggetti;
    bool stoAttaccando;

    void Start()
    {
        pane = 0;
        oggSel = Oggetto.OggettoDaLista(0);
        if (GetComponentInParent<PhotonView>().isMine)
        {
            for (int i = 0; i < oggetti.childCount - 1; i++)
            {
                int x = i;
                oggetti.GetChild(i).GetComponent<Button>().onClick.AddListener(() => SelezionaOggetto(x));
            }
            oggetti.GetChild(oggetti.childCount - 1).GetComponent<Button>().onClick.AddListener(() => AzionePane());
        }
    }

    public void SelezionaOggetto(int n)
    {
        if (oggSel.index != (Oggetto.Oggetti)n)
            oggSel = Oggetto.OggettoDaLista((Oggetto.Oggetti)n);
        else
            return;

        for (int i = 0; i < oggetti.childCount; i++)
            if (i != (int)Oggetto.Oggetti.pane)
            {
                if (i != n)
                    oggetti.GetChild(i).GetChild(0).gameObject.SetActive(false);
                else
                    oggetti.GetChild(i).GetChild(0).gameObject.SetActive(true);
            }
        GetComponentInParent<PhotonView>().RPC("SelezionaOggettoPlayer", PhotonTargets.AllViaServer, oggSel.index);
    }

    public void Azioni(Oggetto.Oggetti ogg)
    {
        switch (ogg)
        {
            case Oggetto.Oggetti.falce:
                break;
            case Oggetto.Oggetti.secchioVuoto:
                ModificaOggetto(Oggetto.Oggetti.secchioVuoto, Oggetto.Oggetti.secchioPieno);
                break;
            case Oggetto.Oggetti.secchioPieno:
                ModificaOggetto(Oggetto.Oggetti.secchioPieno, Oggetto.Oggetti.secchioVuoto);
                break;
            case Oggetto.Oggetti.semi:
                //ModificaOggetto(Oggetto.Oggetti.se, Oggetto.Oggetti.secchioVuoto);
                break;
        }
    }

    public void AzionePane()
    {
        if (!forziere.inVendita)
        {
            statistiche.fame += 3;
            pane--;
        }
        else
            cambTes.forzierePan.ImpostaPrezzo("Bread");
    }

    void ModificaOggetto(Oggetto.Oggetti oggDisatt, Oggetto.Oggetti oggAtt)
    {
        oggetti.GetChild((int)oggAtt).gameObject.SetActive(true);
        oggetti.GetChild((int)oggDisatt).gameObject.SetActive(false);
        SelezionaOggetto((int)oggAtt);
    }

    private void Update()
    {
        if (gameManager.mode == gameManager.Modes.interattiva)
            if (GetComponentInParent<PhotonView>().isMine)
                if (Input.GetKeyDown(KeyCode.Mouse0))
                    switch (oggSel.index)
                    {
                        case Oggetto.Oggetti.pugnale:
                            if (!stoAttaccando)
                                StartCoroutine(AzionePugnale());
                            break;
                    }
    }

    IEnumerator AzionePugnale()
    {
        statistiche.InterrompiPotenza();
        //statistiche.potenza = 0;
        statistiche.potenzaBarra.parent.gameObject.SetActive(false);
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Collider2D>().enabled = true;
        GetComponent<Animator>().SetTrigger("e");
        stoAttaccando = true;
        yield return new WaitForSeconds(.5f);
        stoAttaccando = false;
        statistiche.potenzaBarra.parent.gameObject.SetActive(true);
        statistiche.potenza = 0;
        statistiche.IniziaPotenza();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (gameManager.mode != gameManager.Modes.interattiva)
            return;

        if (col != GetComponentInParent<Collider2D>() && stoAttaccando)
        {
            if (col.GetComponent<nemico>())
                col.GetComponent<nemico>().Danno((int)statistiche.potenza);
            else if (col.GetComponent<movimento>())
                col.GetComponent<PhotonView>().RPC("DannoPugnale", PhotonTargets.Others, (int)statistiche.potenza);
        }
    }
}

[System.Serializable]
public class Oggetto
{   
    public Sprite sprite;
    public enum Oggetti { pugnale, falce, secchioVuoto, secchioPieno, semi, pane }
    public Oggetti index;

    public static List<Oggetto> listaOggetti = new List<Oggetto>()
    {
        new Oggetto() {sprite = Resources.LoadAll<Sprite>("oggetti").Single(s => s.name == "pugnale oggetto"), index = Oggetti.pugnale },
        new Oggetto() {sprite = Resources.LoadAll<Sprite>("oggetti").Single(s => s.name == "falce oggetto"), index = Oggetti.falce },
        new Oggetto() {sprite = Resources.LoadAll<Sprite>("oggetti").Single(s => s.name == "secchio vuoto oggetto"), index = Oggetti.secchioVuoto },
        new Oggetto() {sprite = Resources.LoadAll<Sprite>("oggetti").Single(s => s.name == "secchio pieno oggetto"), index = Oggetti.secchioPieno },
        new Oggetto() {sprite = Resources.LoadAll<Sprite>("oggetti").Single(s => s.name == "semi oggetto"), index = Oggetti.semi },
        new Oggetto() {sprite = Resources.LoadAll<Sprite>("oggetti").Single(s => s.name == "pane oggetto"), index = Oggetti.pane }
    };

    public static Oggetto OggettoDaLista(Oggetti o)
    {
        foreach (Oggetto a in listaOggetti)
            if (a.index == o)
                return a;
        return null;
    }
}