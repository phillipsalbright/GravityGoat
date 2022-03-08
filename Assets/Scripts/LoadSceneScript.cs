using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneScript : MonoBehaviour
{
    private int sceneToLoad = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
