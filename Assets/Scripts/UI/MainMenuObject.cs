using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * This class is connected to a GameObject in the mainmenu scene and contains methods to be
 * called by attached buttons on the menu to load different scenes.
 * Set the menuScreens and use the LoadScreen and LoadScene methods for buttons on the menus by
 * setting them within the Unity Editor.
 */
public class MainMenuObject : MonoBehaviour
{
    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    private void Start()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("Volume", 1f);
    }
}
