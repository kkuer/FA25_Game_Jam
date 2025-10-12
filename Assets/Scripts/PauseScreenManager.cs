using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseScreenManager : MonoBehaviour
{
    public GameObject bg;
    public GameObject mainContainer;
    public GameObject settingsContainer;

    [SerializeField] private Volume volume;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.instance.gamePaused == false)
            {
                Time.timeScale = 0f;
                bg.SetActive(true);
                if (volume.profile.TryGet<DepthOfField>(out DepthOfField dof)) { dof.active = true; }
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
                    Time.timeScale = GameManager.instance.gameSpeed;
                    bg.SetActive(false);
                    if (volume.profile.TryGet<DepthOfField>(out DepthOfField dof)) { dof.active = false; }
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
        Time.timeScale = GameManager.instance.gameSpeed;
        bg.SetActive(false);
        if (volume.profile.TryGet<DepthOfField>(out DepthOfField dof)) { dof.active = false; }
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
