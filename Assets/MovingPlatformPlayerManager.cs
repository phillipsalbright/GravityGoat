using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformPlayerManager : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 3)
        {
            collision.gameObject.transform.parent = this.transform;
        }
    }

    private void OnTriggerEnter(Collision collision)
    {
        if (collision.gameObject.layer == 3)
        {
            collision.gameObject.transform.parent = null;
        }
    }
}
