using UnityEngine;

/**
 * Cues the musicManager to play songs at the beginning of scenes
 */
public class SongPlayer : MonoBehaviour
{
    /** Set in editor to the song number for this scene */
    public int songNumber;

    void Start()
    {
        if (MusicManager.musicManager != null)
        {
            MusicManager.musicManager.PlayMusic(songNumber);
        }
    }
}
