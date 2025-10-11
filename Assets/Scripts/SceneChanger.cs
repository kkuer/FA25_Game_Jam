using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void GoToSampleScene()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void GoToStartScene()
    {
        SceneManager.LoadScene("StartScene");
    }

    private void Start()
    {
    }

    private System.Collections.IEnumerator AutoJump()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("SampleScene");
    }
}
