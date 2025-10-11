using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeTransition : MonoBehaviour
{
    public static FadeTransition instance { get; private set; }

    [SerializeField] private AnimationCurve easeInCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private AnimationCurve easeOutCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);
    [SerializeField] private float duration = 1f;
    private Image img;
    public bool isPlaying;

    [SerializeField] private bool OkGarminEnableTestingMode;

    private RectTransform rectTransform;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        isPlaying = false;
        img = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void fadeIn()
    {
        if (!isPlaying)
        {
            StartCoroutine(ScaleRoutine((Vector3.one * 11f), (Vector3.one * 0.001f), -45f, 0f, "in"));
        }
    }

    public void fadeOut()
    {
        if (!isPlaying)
        {
            StartCoroutine(ScaleRoutine((Vector3.one * 0.001f), (Vector3.one * 11f), 0f, -45f, "out"));
        }
    }

    private IEnumerator ScaleRoutine(Vector3 startScale, Vector3 targetScale, float startRot, float targetRot, string direction)
    {
        //bool direction
        //1 -> fade in (end on black)
        //2 -> fade out (start on black)

        if (rectTransform == null) yield break;

        isPlaying = true;

        float time = 0f;
        rectTransform.localScale = startScale;
        rectTransform.localEulerAngles = new Vector3(0, 0, startRot);
        img.enabled = true;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            float curveValue;

            if (direction == "in")
            {
                curveValue = easeInCurve.Evaluate(t);
            }
            else
            {
                curveValue = easeOutCurve.Evaluate(t);
            }

            rectTransform.localScale = Vector3.Lerp(startScale, targetScale, curveValue);

            rectTransform.localEulerAngles = new Vector3(0, 0, Mathf.Lerp(startRot, targetRot, curveValue));

            yield return null;
        }

        rectTransform.localScale = targetScale;
        rectTransform.localEulerAngles = new Vector3(0, 0, targetRot);

        if (direction == "out")
        {
            img.enabled = false;
        }

        isPlaying = false;
    }

    private void Update()
    {
        if (OkGarminEnableTestingMode)
        {
            if (Input.GetKeyDown(KeyCode.N))
            {
                fadeIn();
            }

            if (Input.GetKeyDown(KeyCode.M))
            {
                fadeOut();
            }
        }
    }
}
