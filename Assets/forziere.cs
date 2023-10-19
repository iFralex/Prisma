using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleSheetsForUnity;
using Newtonsoft.Json;

public class forziere : MonoBehaviour
{
    [System.Serializable]
    public class datiForziere
    {
        [System.Serializable]
        public class oggetto
        {
            public string nome;
            public int prezzo;
        }
        public string nomeForziere;
        public string oggetti;
        public List<oggetto> listaOggetti;

        public void salvaOggetti() => listaOggetti = oggetti == "" ? listaOggetti = new List<oggetto>() { } : listaOggetti = JsonConvert.DeserializeObject<List<oggetto>>(oggetti);
        public void serializzaListaOggetti() => oggetti = listaOggetti.Count == 0 ? "" : oggetti = JsonConvert.SerializeObject(listaOggetti);
    }

    class forziereSemplice
    {
        public string nomeForziere;
        public string oggetti;
    }

    public Text caricamentoT;
    public GameObject oggetto;

    int _id;
    public int id
    {
        get { return _id; }
        set
        {
            if (gameObject.activeInHierarchy && _id == value)
                return;
            _id = value;
            gameObject.SetActive(true);
            caricamentoT.gameObject.SetActive(true);
            caricamentoT.text = "Downloading...";
            transform.GetChild(1).gameObject.SetActive(false);
            Drive.GetObjectsByField("chestshop objects", "nomeForziere", "Tile " + value.ToString());
        }
    }
    public static bool inVendita;
    public int prezzo;

    private void Start()
    {
        movimento.statistiche.monete = 50;
        //print(JsonConvert.DeserializeObject<List<datiForziere>>("[{ \"nomeForziere\":\"Tile 5\",\"oggetti\":\"[{\"nome\":\"pane\",\"prezzo\":4},{\"nome\":\"osso\",\"prezzo\":11}]\"}]"));
        //print("originale " + JsonConvert.SerializeObject(new List<datiForziere>() { new datiForziere() { nomeForziere = "Tile 5", oggetti = new List<datiForziere.oggetto>() { new datiForziere.oggetto() { nome = "d", prezzo = 4 } } } }));
    }
    private void OnEnable() => Drive.responseCallback += Caricato;
    private void OnDisable() => Drive.responseCallback -= Caricato;

    public void Caricato(Drive.DataContainer dataContainer)
    {
        if (dataContainer.objType != "chestshop objects")
            return;

        if (dataContainer.QueryType == Drive.QueryType.getObjectsByField || dataContainer.QueryType == Drive.QueryType.updateObjects)
        {
            datiForziere risultato;
            if (dataContainer.QueryType == Drive.QueryType.getObjectsByField)
                risultato = JsonConvert.DeserializeObject<List<datiForziere>>(dataContainer.payload)[0];
            else
                risultato = JsonConvert.DeserializeObject<datiForziere>(dataContainer.value);
            risultato.salvaOggetti();
            if (risultato.listaOggetti.Count == 0)
            {
                caricamentoT.gameObject.SetActive(false);
                transform.GetChild(2).gameObject.SetActive(true);
                transform.GetChild(2).GetChild(0).gameObject.SetActive(true);
                transform.GetChild(2).GetChild(1).gameObject.SetActive(false);
                transform.GetChild(2).GetChild(2).gameObject.SetActive(false);
            }
            else
            {
                for (int i = oggetto.transform.parent.childCount - 1; i > 0; i--)
                    DestroyImmediate(oggetto.transform.parent.GetChild(i).gameObject);
                oggetto.transform.parent.GetComponent<RectTransform>().sizeDelta = new Vector2(oggetto.transform.parent.GetComponent<RectTransform>().sizeDelta.x, risultato.listaOggetti.Count * oggetto.GetComponent<RectTransform>().sizeDelta.y + 10 * risultato.listaOggetti.Count);
                transform.GetChild(1).gameObject.SetActive(true);
                oggetto.transform.GetChild(0).GetComponent<Text>().text = risultato.listaOggetti[0].nome;
                oggetto.transform.GetChild(1).GetComponent<Text>().text = risultato.listaOggetti[0].prezzo.ToString();
                oggetto.transform.GetChild(2).GetComponent<Button>().onClick.RemoveAllListeners();
                oggetto.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => AcquistaOggetto(risultato, 0));
                for (int i = 1; i < risultato.listaOggetti.Count; i++)
                {
                    RectTransform ogg = Instantiate(oggetto, oggetto.transform.parent).GetComponent<RectTransform>();
                    ogg.GetChild(0).GetComponent<Text>().text = risultato.listaOggetti[i].nome;
                    ogg.GetChild(1).GetComponent<Text>().text = risultato.listaOggetti[i].prezzo.ToString();
                    int x = i;
                    ogg.GetChild(2).GetComponent<Button>().onClick.RemoveAllListeners();
                    ogg.GetChild(2).GetComponent<Button>().onClick.AddListener(() => AcquistaOggetto(risultato, x));
                }
                caricamentoT.gameObject.SetActive(false);
            }
        }
    }

    public void AcquistaOggetto(datiForziere risultato, int index)
    {
        if (movimento.statistiche.monete - risultato.listaOggetti[index].prezzo < 0)
            return;
        movimento.statistiche.monete -= risultato.listaOggetti[index].prezzo;
        if (risultato.listaOggetti[index].nome == "Bread")
            cambiaTessera.player.pane++;
        risultato.listaOggetti.RemoveAt(index);
        risultato.serializzaListaOggetti();
        Drive.UpdateObjects("chestshop objects", "nomeForziere", "Tile " + id.ToString(), JsonConvert.SerializeObject(new forziereSemplice() { nomeForziere = risultato.nomeForziere, oggetti = risultato.oggetti }), false);
        caricamentoT.gameObject.SetActive(true);
        caricamentoT.text = "Updating...";
        transform.GetChild(1).gameObject.SetActive(false);
    }

    public void SelezionaOggetto()
    {
        inVendita = true;
        transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
        transform.GetChild(2).GetChild(1).gameObject.SetActive(true);
    }

    public void ImpostaPrezzo(string nome)
    {
        transform.GetChild(2).GetChild(2).GetChild(2).GetComponent<Button>().interactable = false;
        transform.GetChild(2).GetChild(2).gameObject.SetActive(true);
        transform.GetChild(2).GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).GetChild(2).GetChild(0).GetComponent<Text>().text = nome;
        transform.GetChild(2).GetChild(2).GetChild(2).GetComponent<Button>().onClick.RemoveAllListeners();
        transform.GetChild(2).GetChild(2).GetChild(2).GetComponent<Button>().onClick.AddListener(() => Vendi(nome));
    }

    public void Vendi(string nome)
    {
        transform.GetChild(2).gameObject.SetActive(false);
        transform.GetChild(2).GetChild(2).gameObject.SetActive(false);
        caricamentoT.text = "Updating...";
        caricamentoT.gameObject.SetActive(true);
        Drive.UpdateObjects("chestshop objects", "nomeForziere", "Tile " + id.ToString(), JsonConvert.SerializeObject(new forziereSemplice() { nomeForziere = "Tile " + id.ToString(), oggetti = JsonConvert.SerializeObject(new List<datiForziere.oggetto>() { new datiForziere.oggetto() { nome = nome, prezzo = prezzo } }) }), true);
        inVendita = false;
        if (nome == "Bread")
            cambiaTessera.player.pane--;
        movimento.statistiche.monete += prezzo;
    }

    public void RiempiCampoTestoPrezzo(string s)
    {
        transform.GetChild(2).GetChild(2).GetChild(2).GetComponent<Button>().interactable = s != "";
        prezzo = System.Convert.ToInt32(s);
    }

    public void Chiudi()
    {
        inVendita = false;
        gameObject.SetActive(false);
    }
}