using UnityEngine;
using System.Collections.Generic;

public class GravityScript : MonoBehaviour
{
    //private float gravityMultiplier = 2.6f;
    private float fieldGravityMultiplier = .7f;
    private Rigidbody rb;

    List<GravityField> currentFieldCollisions = new List<GravityField>();

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7 || other.gameObject.layer == 8)
        {
            currentFieldCollisions.Add(other.gameObject.GetComponentInParent<GravityField>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 7 || other.gameObject.layer == 8)
        {
            currentFieldCollisions.Remove(other.gameObject.GetComponentInParent<GravityField>());
        }
    }

    void FixedUpdate()
    {
        CalculateGravity();
    }

    void CalculateGravity()
    {
        if (currentFieldCollisions.Count > 0)
        {
            Vector3 forceSum = new Vector3(0, 0, 0);
            foreach (GravityField f in currentFieldCollisions)
            {
                if (f != null && f.GetActive())
                {
                    /** accurate  */
                    Vector3 direction = ((this.transform.position - f.GetPosition()) * f.GetOutwardForce()).normalized;
                    float magnitude = ((this.transform.position - f.GetPosition()) * f.GetOutwardForce()).magnitude;
                    magnitude = Mathf.Clamp(magnitude, 0, 3.5f);
                    magnitude = 3.5f - magnitude;
                    magnitude = magnitude / 10.5f;
                    magnitude += 2.1f;
                    forceSum += direction * magnitude;
                   

                    /** how its been */
                    //forceSum += (this.transform.position - f.GetPosition()) * f.GetOutwardForce();
                }
                else
                {
                    currentFieldCollisions.Remove(f);
                    break;
                }
            }
            rb.useGravity = false;
            rb.AddForce(forceSum * Physics.gravity.magnitude * fieldGravityMultiplier, ForceMode.Acceleration);
        } else
        {
            if (this.gameObject.layer != 15)
            {
                rb.useGravity = true;
            }
        }
    }
}
