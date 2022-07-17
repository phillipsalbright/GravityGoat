using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Vector3[] positions;
    [SerializeField] private float speed;
    private GameObject[] gravityFields;
    private Rigidbody rb;
    private int positionNum;
    private int numPositions;
    private bool moving;
    private Rigidbody playerrb;

    private void Start()
    {
        positionNum = 0;
        numPositions = positions.Length;
        this.transform.position = positions[0];
        moving = false;
        rb = this.GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        if (!moving)
        {
            StartCoroutine(Vector3LerpCoroutine());
            moving = true;
        }

    }

    IEnumerator Vector3LerpCoroutine()
    {
        Vector3 startPos = this.transform.position;
        Vector3 endPos = positions[(positionNum + 1) % numPositions];
        float time = 0;
        while (rb.position != endPos)
        {
            rb.MovePosition(Vector3.Lerp(startPos, endPos, (time / Vector3.Distance(startPos, endPos)) * speed));
            time += Time.deltaTime;
            yield return null;
        }
        positionNum = (positionNum + 1) % numPositions;
        moving = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 7 || collision.gameObject.layer == 8)
        {
            collision.gameObject.transform.parent = this.transform;
        }
    }
}
