using UnityEngine;

public class SceneChanger : MonoBehaviour
{
    public void changeScene(string sceneName)
    {
        FadeTransition.instance.fadeIn(sceneName);
    }

    public void quitGame()
    {
        Application.Quit();
    }
}
