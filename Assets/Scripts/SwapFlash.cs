using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SwapFlash : MonoBehaviour
{
    public static SwapFlash instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        flashImage = GetComponent<Image>();
        flashImage.enabled = false;
        text.SetActive(false);
    }

    private Image flashImage;
    public GameObject text;

    public void flash()
    {
        StartCoroutine(flashCoroutine());
    }

    public IEnumerator flashCoroutine()
    {
        flashImage.enabled = true;
        text.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        flashImage.enabled = false;
        yield return new WaitForSeconds(0.7f);
        text.SetActive(false);
        yield return new WaitForSeconds(0.05f);
        text.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        text.SetActive(false);
    }
}
