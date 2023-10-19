using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class statistiche : MonoBehaviour
{
    float _fame;
    public float fame
    {
        get { return _fame; }
        set
        {
            if (_fame == value)
                return;
            value = Mathf.Min(value, fameMassima);
            
            _fame = value;
            fameT.text = "Hunger " + ((int)value + 1).ToString() + "/" + fameMassima.ToString();
            fameBarra.localScale = new Vector3((float)value / fameMassima, 1, 1);
        }
    }
    public int fameMassima = 10;
    public Text fameT;
    public RectTransform fameBarra;

    float _potenza;
    public float potenza
    {
        get { return _potenza; }
        set
        {
            if (_potenza == value)
                return;
            value = Mathf.Min(value, 100);

            _potenza = value;
            potenzaBarra.localScale = new Vector3((float)value / 100, 1, 1);
            potenzaBarra.GetComponent<Image>().color = Color.Lerp(new Color(0, 1, 1, 1), Color.blue, value / 100);
        }
    }
    public RectTransform potenzaBarra;

    int _hp;
    public int hp
    {
        get { return _hp; }
        set
        {
            if (_hp == value)
                return;
            value = Mathf.Min(value, 1000);

            if (value < 0)
                value = 0;
            _hp = value;
            hpBarra.localScale = new Vector3((float)value / 1000, 1, 1);
            hpBarra.GetComponent<Image>().color = Color.Lerp(Color.red, Color.green, value / 1000f);
            hpT.text = "HP " + value.ToString() + "/" + 1000;
        }
    }
    public RectTransform hpBarra;
    public Text hpT;

    int _monete;
    public int monete
    {
        get { return _monete; }
        set
        {
            if (_monete == value)
                return;

            _monete = value;
            moneteT.text = value.ToString();
        }
    }
    public Text moneteT;

    public int teschi;
    Coroutine potCor;

    void Start()
    {
        fame = fameMassima;
        StartCoroutine(DiminuisciFame());
        potenza = 0;
        IniziaPotenza();
        hp = 1000;
        StartCoroutine(AumentaHP());
    }

    IEnumerator DiminuisciFame()
    {
        for (; ; )
        {
            yield return new WaitForSeconds(1);
            if (fame > 0)
                fame -= .0167f;
        }
        //fameT.text = "Hunder " + 0.ToString() + "/" + fameMassima.ToString();
    }

    public IEnumerator GestisciPotenza()
    {
        for (; ; )
        {
            if (potenza == 0)
            {
                for (float i = 0; i < 2; i += .05f)
                {
                    potenza = 50 * i * i / 2;
                    yield return new WaitForSeconds(.05f);
                }

                for (float i = 0; i < .2f; i += .05f)
                {
                    potenza = 100 - 100 * i;
                    yield return new WaitForSeconds(.05f);
                }
                potenza = 80;
            }
            yield return new WaitForSeconds(.05f);
        }
    }

    public void IniziaPotenza()
    {
        potCor = StartCoroutine(GestisciPotenza());
    }

    public void InterrompiPotenza()
    {
        if (potCor != null)
            StopCoroutine(potCor);
    }

    IEnumerator AumentaHP()
    {
        for (; ; )
        {
            yield return new WaitForSeconds(1);
            if ((int)fame >= 5)
                hp += 10;
            hp += 10;
        }
    }
}