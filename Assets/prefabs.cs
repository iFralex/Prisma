using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class prefabs : MonoBehaviour
{
    public Transform p;
    public bool m = true;

    void Update()
    {
        if (m)
        {
            m = false;
            List<Transform> a = new List<Transform>();
            for (int i = 0; i < p.childCount; i++)
                a.Add(p.GetChild(i));

            float x = 0;
            float y = 0;
            for (int i = 0; i < a.Count; i++)
            {
                if (y < 49.5f)
                    a[i].position = new Vector3(x, y, 0);
                else
                {
                    x += .5f;
                    y = 0;
                    continue;
                }
                y += .5f;
            }
        }
    }
}