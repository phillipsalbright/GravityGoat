using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneWithAnimation : MonoBehaviour
{
    public int sceneToLoad = 2;
    [SerializeField] private Animator image;
    private bool startedAnim = false;

    private void Start()
    {
        startedAnim = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3 && !startedAnim)
        {
            startedAnim = true;
            StartCoroutine(StartLoad());
        }
    }

    IEnumerator StartLoad()
    {
        image.Play("FadeOut");
        yield return new WaitForSeconds(.7f);
        SceneManager.LoadScene(sceneToLoad);
    }
}
