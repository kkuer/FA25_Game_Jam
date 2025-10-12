using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CinematicManager : MonoBehaviour
{
    public List<GameObject> panels = new List<GameObject>();

    void Start()
    {
        FadeTransition.instance.fadeOut();
        StartCoroutine(playCinematicCoroutine(5f));
    }

    public IEnumerator playCinematicCoroutine(float panelDuration)
    {
        float time = 0f;

        for (int i = 0; i < panels.Count; i++)
        {
            panels[i].SetActive(true);
            if (i > 0)
            {
                panels[i - 1].SetActive(false);
            }
            time = 0f;
            while (time < panelDuration)
            {
                time += Time.deltaTime;
                yield return null;
            }
        }
        FadeTransition.instance.fadeIn("Main");
    }
}
