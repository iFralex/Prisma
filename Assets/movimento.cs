using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movimento : Photon.MonoBehaviour
{
    public float velocità;
    bool suSentiero;
    public static statistiche statistiche;
    public int hp = 1000;

    void Start()
    {
        if (!AIDestinationSetter.targets.Contains(transform))
            AIDestinationSetter.targets.Add(transform);
        if (photonView.isMine)
            cambiaTessera.player = GetComponentInChildren<usaOggetti>();
    }

    void Update()
    {
        if (photonView.isMine)
        {
            if (proporzioniCamera.mode == proporzioniCamera.Modes.segui)
                Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
            transform.LookAt(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward);
            transform.eulerAngles = new Vector3(0, 0, -transform.eulerAngles.z);
        }
    }

    void FixedUpdate()
    {
        if (photonView.isMine)
        {
            if (Input.GetKey(KeyCode.W))
                GetComponent<Rigidbody2D>().AddForce(Vector2.up * velocità, ForceMode2D.Impulse);
            if (Input.GetKey(KeyCode.S))
                GetComponent<Rigidbody2D>().AddForce(Vector2.down * velocità, ForceMode2D.Impulse);
            if (Input.GetKey(KeyCode.D))
                GetComponent<Rigidbody2D>().AddForce(Vector2.right * velocità, ForceMode2D.Impulse);
            if (Input.GetKey(KeyCode.A))
                GetComponent<Rigidbody2D>().AddForce(Vector2.left * velocità, ForceMode2D.Impulse);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (photonView.isMine)
            if (col.tag == Tessera.TesseraDaLista(Tessera.Tessere.sentiero).tag)
                if (!suSentiero)
                {
                    suSentiero = true;
                    velocità /= .6f;
                }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (photonView.isMine)
            if (col.tag == Tessera.TesseraDaLista(Tessera.Tessere.sentiero).tag)
                if (suSentiero)
                {
                    suSentiero = false;
                    velocità *= .6f;
                }
    }

    public void Danno(int d)
    {
        hp -= d;
        statistiche.hp = hp;
    }

    [PunRPC]
    public void DannoPugnale(int d)
    {
        if (photonView.isMine)
            Danno(d);
    }


    [PunRPC]
    public void SelezionaOggettoPlayer(Oggetto.Oggetti o)
    {
        transform.Find("oggetto").GetComponent<SpriteRenderer>().sprite = Oggetto.OggettoDaLista(o).sprite;
    }
}