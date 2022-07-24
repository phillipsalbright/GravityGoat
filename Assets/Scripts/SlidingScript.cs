using UnityEngine;
using System.Collections;

public class SlidingScript : MonoBehaviour
{
    /** All variables available for editing in the editor for different sliding doors in different levels */
    public bool isSlided;
    public Vector3 slidingDirection;
    public Vector3 startPos;
    private Coroutine SlidingCoroutine;
    public float slidingDistance;
    public bool playSound;
    private bool soundPlaying;
    public AudioSource source;
    public float timeToSlideUp;
    public float timeToSlideDown;

    public void Awake()
    {
        startPos = this.transform.localPosition;
        isSlided = false;
        soundPlaying = false;
    }

    public void StartSlide()
    {
        if (!isSlided)
        {
            if (SlidingCoroutine != null)
            {
                StopCoroutine(SlidingCoroutine);
            } else
            {
                if (playSound)
                {
                    source.Play();
                }
            }
            SlidingCoroutine = StartCoroutine(SlidingBegin());
        }
    }

    public void EndSlide()
    {
        if (isSlided)
        {
            if (SlidingCoroutine != null)
            {
                StopCoroutine(SlidingCoroutine);
            } else
            {
                if (playSound)
                {
                    source.Play();
                }
            }
            SlidingCoroutine = StartCoroutine(SlidingEnd());
        }
    }

    private IEnumerator SlidingBegin()
    {
        Vector3 finalPos = startPos + (slidingDistance * slidingDirection);
        Vector3 startPosLocal = this.transform.localPosition;
        float time = 0;
        isSlided = true;
        while (time < timeToSlideUp)
        {
            transform.localPosition = Vector3.Lerp(startPosLocal, finalPos, time / timeToSlideUp);
            yield return null;
            time += Time.deltaTime;
        }
        if (playSound)
        {
            source.Stop();
        }
        SlidingCoroutine = null;
    }

    private IEnumerator SlidingEnd()
    {
        Vector3 finalPos = startPos;
        Vector3 startPosLocal = this.transform.localPosition;
        float time = 0;
        isSlided = false;
        while (time < timeToSlideDown)
        {
            transform.localPosition = Vector3.Lerp(startPosLocal, finalPos, time / timeToSlideDown);
            yield return null;
            time += Time.deltaTime;
        }
        if (playSound)
        {
            source.Stop();
        }
        SlidingCoroutine = null;
    }
}
