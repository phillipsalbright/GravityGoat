using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager musicManager;
    [SerializeField] AudioSource[] songs;
    private int currentSongPlaying;

    private void Awake()
    {
        if (musicManager != null && musicManager != this)
        {
            Destroy(this.gameObject);
            return;
        }
        musicManager = this;
        DontDestroyOnLoad(this);
        currentSongPlaying = -1;
    }

    public void PlayMusic(int songNumber)
    {
        if (songNumber < 0)
        {
            foreach(AudioSource a in songs)
            {
                a.Stop();
            }
            currentSongPlaying = songNumber;
            return;
        } else if (songNumber != currentSongPlaying)
        {
            foreach (AudioSource a in songs)
            {
                a.Stop();
            }
            songs[songNumber].Play();
            currentSongPlaying = songNumber;
        }
    }
}
