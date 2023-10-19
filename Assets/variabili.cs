using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class variabili : MonoBehaviour
{
    public statistiche statistiche;
    public cambiaTessera cambTes;
    public Text paneT;
    public Transform oggetti;
    public Transform tessere;

    void Awake()
    {
        AIDestinationSetter.targets.Clear();
        movimento.statistiche = usaOggetti.statistiche = statistiche;
        usaOggetti.cambTes = cambTes;
        usaOggetti.paneT = paneT;
        usaOggetti.oggetti = oggetti;
        gameManager.tessere = tessere;
    }
}
