using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleSheetsForUnity;

public class salvaTessereOnline : MonoBehaviour
{
    private void Start()
    {
        //StartCoroutine(Salva());
    }

    IEnumerator Salva()
    {
        yield return new WaitForSeconds(1);
        Drive.SetCellValue("Tiles", "B", "2", "terra", true);
    }
}
