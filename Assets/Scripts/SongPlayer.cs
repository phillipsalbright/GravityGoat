using UnityEngine;

/**
 * Cues the musicManager to play songs at the beginning of scenes
 */
public class SongPlayer : MonoBehaviour
{
    /** Set in editor to the song number for this scene */
    public int songNumber;
    [SerializeField] private MusicManager musicManager;

    void Start()
    {
        if (MusicManager.musicManager != null)
        {
            MusicManager.musicManager.PlayMusic(songNumber);
        } else
        {
            Instantiate(musicManager);
            MusicManager.musicManager.PlayMusic(songNumber);
        }
    }
}
