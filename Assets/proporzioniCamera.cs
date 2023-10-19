using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class proporzioniCamera : MonoBehaviour
{
    public enum Modes { statica, segui }
    public static Modes mode;

    void Awake()
    {
        Camera cam = GetComponent<Camera>();
        if (cam.aspect < 1.77777778f)
        {
            float n = cam.aspect / 1.777777778f;
            cam.rect = new Rect(0, (1 - n) / 2, 1, n);
        }
        else
        {
            float n = ((float)Screen.height / Screen.width) / (9f / 16f);
            cam.rect = new Rect((1 - n) / 2, 0, n, 1);
        }
    }

    public void Sposta(string s)
    {
        int x = System.Convert.ToInt32(s[0] + "");
        if (x == 2)
            x = -1;
        int y = System.Convert.ToInt32(s[1] + "");
        if (y == 2)
            y = -1;
        Vector2 dir = new Vector2(x, y);
        transform.Translate(dir * 5);
    }
    float tempo;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            transform.position = new Vector3(cambiaTessera.player.transform.parent.position.x, cambiaTessera.player.transform.parent.position.y, -10);
            tempo = Time.time;
        }

        if (Input.GetKeyUp(KeyCode.P))
            if (tempo + 1 < Time.time)
                mode = mode == Modes.segui ? mode = Modes.statica : mode = Modes.segui;
    }
}