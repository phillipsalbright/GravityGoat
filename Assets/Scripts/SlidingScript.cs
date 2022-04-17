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

    public void Awake()
    {
        startPos = this.transform.localPosition;
        isSlided = false;
    }

    public void StartSlide()
    {
        if (!isSlided)
        {
            if (SlidingCoroutine != null)
            {
                StopCoroutine(SlidingCoroutine);
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
        while (time < 1)
        {
            transform.localPosition = Vector3.Lerp(startPosLocal, finalPos, time);
            yield return null;
            time += Time.deltaTime;
        }
    }

    private IEnumerator SlidingEnd()
    {
        Vector3 finalPos = startPos;
        Vector3 startPosLocal = transform.localPosition;
        float time = 0;
        isSlided = false;
        while (time < 1)
        {
            transform.localPosition = Vector3.Lerp(startPosLocal, finalPos, time);
            yield return null;
            time += Time.deltaTime;
        }
    }
}
