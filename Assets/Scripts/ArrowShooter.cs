using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowShooter : MonoBehaviour
{
    [SerializeField] private float timeBetweenShots;
    [SerializeField] private Transform firingLocation;
    [SerializeField] private GameObject arrow;
    private float timer;

    private void Start()
    {
        timer = 0;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > timeBetweenShots)
        {
            timer = 0;
            GameObject arrowInstance = Instantiate(arrow, firingLocation.position, firingLocation.rotation);
            Physics.IgnoreCollision(arrowInstance.GetComponent<Collider>(), this.gameObject.GetComponent<Collider>());
        }
    }
}
