using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class infoBox : MonoBehaviour
{
    public Text testo;
    public GraphicRaycaster ray;
    public EventSystem eventSystem;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D[] hit = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Camera.main.transform.forward, Mathf.Infinity);
        string t = "";
        foreach (RaycastHit2D h in hit)
        {
            string s = "";
            if (h.collider.tag == "tessera")
            {
                s += "Tile: ";
                switch (h.transform.parent.tag)
                {
                    case "Untagged":
                        s += "void";
                        break;
                    case "erba":
                        s += "grass";
                        break;
                    case "muro":
                        s += "wall";
                        break;
                    case "pavimento":
                        s += "floor";
                        break;
                    case "sentiero":
                        s += "path";
                        break;
                    case "terra":
                        s += "dirt";
                        break;
                    case "terra bagnata":
                        s += "wet dirt";
                        break;
                    case "acqua":
                        s += "water";
                        break;
                    case "grano":
                        s += "grain";
                        break;
                    case "semi":
                        s += "seeds";
                        break;
                    case "zombi":
                        s += "zombi spown";
                        break;
                    case "forziere":
                        s += "chestshop";
                        break;
                }
            }
            else if (h.collider.GetComponent<nemico>())
                s += "Enemy: Zombi";
            else if (h.collider.GetComponent<PhotonView>())
                s += "Player: " + (h.collider.GetComponent<PhotonView>().isMine ? "I" : "Other");
            else if (h.collider.tag == "moneta")
                s += "Coin";
            if (s != "")
                s += "\n";
            t += s;
        }

        List<RaycastResult> risultati = new List<RaycastResult>();
        ray.Raycast(new PointerEventData(eventSystem) { position = Input.mousePosition }, risultati);
        foreach (RaycastResult r in risultati)
        {
            string s = "";
            switch (r.gameObject.name)
            {
                case "erba tessera bt":
                    s += "UI) Object: grass tile";
                    break;
                case "muro tessera bt":
                    s += "UI) Object: wall tile";
                    break;
                case "pavimento tessera bt":
                    s += "UI) Object: floor tile";
                    break;
                case "sentiero tessera bt":
                    s += "UI) Object: path tile";
                    break;
                case "terra tessera bt":
                    s += "UI) Object: dirt tile";
                    break;
                case "terra bagnata tessera bt":
                    s += "UI) Object: wet dirt tile";
                    break;
                case "acqua tessera bt":
                    s += "UI) Object: water tile";
                    break;
                case "grano tessera bt":
                    s += "UI) Object: grain tile";
                    break;
                case "semi tessera bt":
                    s += "UI) Object: seens tile";
                    break;
                case "zombi tessera bt":
                    s += "UI) Object: zombi spawn tile";
                    break;
                case "forziere tessera bt":
                    s += "UI) Object: chestshop tile";
                    break;

                case "pugnale oggetto bt":
                    s += "UI) Object: dagger";
                    break;
                case "falce oggetto bt":
                    s += "UI) Object: sickle";
                    break;
                case "secchio vuoto oggetto bt":
                    s += "UI) Object: empty bucket";
                    break;
                case "secchio pieno oggetto bt":
                    s += "UI) Object: full bucket";
                    break;
                case "semi oggetto bt":
                    s += "UI) Object: seens";
                    break;
                case "pane oggetto bt":
                    s += "UI) Object: bread";
                    break;

                case "barra fame":
                    s += "UI) Vitals: hunger bar";
                    break;
                case "barra potenza":
                    s += "UI) Vitals: power bar";
                    break;
                case "barra hp":
                    s += "UI) Vitals: hp bar";
                    break;

                case "info box":
                    s += "UI) Info box";
                    break;
                case "esci bt":
                    s += "UI) Button: Exit";
                    break;
            }
            if (s != "")
                s += "\n";
            t += s;
        }
        if (t != "")
            testo.text = t.Substring(0, t.Length - 1);
    }
}