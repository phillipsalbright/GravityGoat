using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateRespawn : MonoBehaviour
{

    private Vector3 startPos;
    private Quaternion startRot;

    void Start()
    {
        startPos = transform.position;
        startRot = transform.rotation;
        Debug.Log(startPos.ToString());
    }

    void OnTriggerEnter(Collider col)
    {
        Debug.Log(col.gameObject.name);

        if (col.gameObject.name == "CrateReset")
        {
            Debug.Log(transform.position.ToString());

            transform.position = startPos;
            transform.rotation = startRot;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
}
