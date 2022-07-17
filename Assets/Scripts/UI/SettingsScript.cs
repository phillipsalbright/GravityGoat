using UnityEngine;
using UnityEngine.UI;

public class SettingsScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        GameObject.Find("Volume").GetComponent<Slider>().value = PlayerPrefs.GetFloat("Volume", 1f);
    }

    public void SetVolume(System.Single volume)
    {
        PlayerPrefs.SetFloat("Volume", volume);
        AudioListener.volume = volume;
    }
}
