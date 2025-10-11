using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PauseScreenManager : MonoBehaviour
{
    public GameObject bg;
    public GameObject mainContainer;
    public GameObject settingsContainer;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.instance.gamePaused == false)
            {
                Time.timeScale = 0f;
                bg.SetActive(true);
                GameManager.instance.gamePaused = true;
            }
            else
            {
                if (settingsContainer.activeSelf == true)
                {
                    exitSettings();
                }
                else
                {
                    Time.timeScale = 1f;
                    bg.SetActive(false);
                    GameManager.instance.gamePaused = false;
                }
            }
        }
    }

    public void goToSettings()
    {
        mainContainer.SetActive(false);
        settingsContainer?.SetActive(true);
    }

    public void exitSettings()
    {
        settingsContainer?.SetActive(false);
        mainContainer.SetActive(true);
    }

    public void goToResume()
    {
        Time.timeScale = 1f;
        bg.SetActive(false);
        GameManager.instance.gamePaused = false;
    }

    public void goToMenu()
    {
        Time.timeScale = 1f;
        FadeTransition.instance.fadeIn("StartScene");
        if (FadeTransition.instance.isPlaying == false)
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}
