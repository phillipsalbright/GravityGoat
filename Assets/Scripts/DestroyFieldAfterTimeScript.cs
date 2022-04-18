using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyFieldAfterTimeScript : MonoBehaviour
{

    public float timeBeforeImplosion = 3f;


    void Start()
    {
        StartCoroutine("TimedImplosion");
    }

    IEnumerator TimedImplosion()
    {
        yield return new WaitForSeconds(timeBeforeImplosion);
        this.gameObject.GetComponent<GravityField>().Implode();
    }
}
