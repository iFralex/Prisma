using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class network : Photon.PunBehaviour
{
    public RectTransform menùPan, connessionePan;
    public UnityEngine.UI.Text erroreT;

    public void Connetti()
    {
        menùPan.gameObject.SetActive(false);
        connessionePan.gameObject.SetActive(true);
        PhotonNetwork.ConnectUsingSettings("0");
    }
    
    public override void OnConnectedToMaster()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        PhotonNetwork.JoinOrCreateRoom("Mondo", new RoomOptions() { IsVisible = true, MaxPlayers = 16, IsOpen = true, EmptyRoomTtl = 300, CleanupCacheOnLeave = true}, new TypedLobby() { Type = LobbyType.Default, Name = "Lobby" });
    }

    void Errore(string s)
    {
        erroreT.text = s;
        StartCoroutine(Disattiva());
    }

    IEnumerator Disattiva()
    {
        erroreT.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        erroreT.gameObject.SetActive(false);
    }

    public override void OnPhotonJoinRoomFailed(object[] codeAndMsg)
    {
        Errore("Connection error...");
        menùPan.gameObject.SetActive(true);
        connessionePan.gameObject.SetActive(false);
    }

    public override void OnFailedToConnectToPhoton(DisconnectCause cause)
    {
        Errore("Connection error...\nCause: " + cause);
        menùPan.gameObject.SetActive(true);
        connessionePan.gameObject.SetActive(false);
    }

    public override void OnConnectionFail(DisconnectCause cause)
    {
        Errore("Connection error...\nCause: " + cause);
        menùPan.gameObject.SetActive(true);
        connessionePan.gameObject.SetActive(false);
    }

    public override void OnPhotonCreateRoomFailed(object[] codeAndMsg)
    {
        Errore("Connection error...");
        menùPan.gameObject.SetActive(true);
        connessionePan.gameObject.SetActive(false);
    }

    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        Errore("Connection error...");
        menùPan.gameObject.SetActive(true);
        connessionePan.gameObject.SetActive(false);
    }

    public override void OnDisconnectedFromPhoton()
    {
        Errore("Connection error...");
        menùPan.gameObject.SetActive(true);
        connessionePan.gameObject.SetActive(false);
    }
}