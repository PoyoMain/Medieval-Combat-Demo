using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    private void Awake()
    {
        Screen.SetResolution(1920, 1080, FullScreenMode.ExclusiveFullScreen);
    }
    public void GoToScene(int sceneNum)
    {
        SceneManager.LoadScene(sceneNum);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
