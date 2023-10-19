using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pathfinding;
using GoogleSheetsForUnity;
using Newtonsoft.Json;

public class gameManager : Photon.PunBehaviour
{
    public enum Modes { creativa, interattiva}
    public static Modes mode = Modes.interattiva;
    public Button[] bottoni;
    public GameObject listaTessere, listaOggetti;
    public List<AIDestinationSetter> ai;
    public GameObject zombiPrefab, playerPrefab, caricamentoPan;
    public static Transform tessere;

    public void SelezionaMode(int m)
    {
        if (m != (int)mode)
            mode = (Modes)m;
        else
            return;

        listaTessere.SetActive(m == 0);
        listaOggetti.SetActive(m != 0);

        StartCoroutine(CambaiColore(bottoni[m].GetComponent<Image>(), new Color(.7f, .7f, .7f, 1)));
        if (m == 0)
            m = 1;
        else
            m = 0;
        bottoni[m].GetComponent<Image>().color = Color.white;
        foreach (AIDestinationSetter a in ai)
            a.enabled = m == 0;
        if (m == 0)
        {
            GetComponent<AstarPath>().Scan();
        }
    }

    IEnumerator CambaiColore(Image img, Color colFin)
    {
        Color colIni = img.color;
        for (float i = 0; i < 1; i += .1f)
        {
            img.color = Color.Lerp(colIni, colFin, i);
            yield return new WaitForSeconds(.02f);
        }    
    }

    public void Start()
    {
        if (!PhotonNetwork.connected)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            return;
        }
        Drive.GetTable("Tiles", true);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.Instantiate(playerPrefab.name, playerPrefab.transform.position, Quaternion.identity, 0);
    }

    public override void OnDisconnectedFromPhoton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void Disconnetti()
    {
        PhotonNetwork.Disconnect();
        if (PhotonNetwork.room.PlayerCount == 1)
            print(2);//SalvaTessere();
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        if (PhotonNetwork.isMasterClient)
        {
            PhotonView[] p = FindObjectsOfType<PhotonView>();
            foreach (PhotonView a in p)
                if (a.owner == otherPlayer)
                    PhotonNetwork.Destroy(a);
        }
    }

    void ScaricaTessere()
    {
        for (int i = 0; i < tessere.childCount; i++)
            Drive.GetTable("Tiles", true);

    }

    void OnEnable()
    {
        Drive.responseCallback += Callback;
    }

    void OnDisable()
    {
        Drive.responseCallback -= Callback;
    }

    struct tab
    {
        public string Name, Type, Time, StartedTime;
    }

    void Callback(Drive.DataContainer dataContainer)
    {
        if (dataContainer.QueryType == Drive.QueryType.getTable)
        {
            string rawJSon = dataContainer.payload;
            if (string.Compare(dataContainer.objType, "Tiles") == 0)
            {
                tab[] _tessera = JsonConvert.DeserializeObject<tab[]>(rawJSon);
                for (int i = 0; i < _tessera.Length; i++)
                {
                    if (_tessera[i].Type != "null")
                    {
                        Tessera.Tessere index = (Tessera.Tessere)System.Enum.Parse(typeof(Tessera.Tessere), _tessera[i].Type);
                        GetComponent<cambiaTessera>().CambiaTessera(tessere.GetChild(i).GetChild(0), index, false);
                        if (index == Tessera.Tessere.semi)
                            tessere.GetChild(i).gameObject.AddComponent<mutaTessera>().AvviaGrano((System.DateTime.Parse(_tessera[i].StartedTime).AddSeconds(System.Convert.ToDouble(_tessera[i].Time)) - System.DateTime.UtcNow).Seconds, Tessera.Tessere.grano, GetComponent<cambiaTessera>());
                        else if (index == Tessera.Tessere.zombi)
                            tessere.GetChild(i).gameObject.AddComponent<mutaTessera>().AvviaZombi(this);
                    }
                }
                Destroy(caricamentoPan);
            }
        }
    }
}