using UnityEngine;

public class GravityScript : MonoBehaviour
{
    private float gravityMultiplier = 2.6f;
    private float fieldGravityMultiplier = .6f;
    private Vector3 fieldLocation;
    private Rigidbody rb;

    /** True or false depending on whether the physics object is in our out of each respective field */
    private bool outField = false;
    private bool inField = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            fieldLocation = other.gameObject.transform.position;
            outField = true;
        }
        else if (other.gameObject.layer == 8)
        {
            fieldLocation = other.gameObject.transform.position;
            inField = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            outField = false;
        }
        else if (other.gameObject.layer == 8)
        {
            inField = false;
        }
    }

    void FixedUpdate()
    {
        CalculateGravity();
    }

    void CalculateGravity()
    {
        if (inField)
        {
            rb.AddForce((this.transform.position - fieldLocation) * Physics.gravity.magnitude * fieldGravityMultiplier, ForceMode.Acceleration);
        }
        else if (outField)
        {
            rb.AddForce(-(this.transform.position - fieldLocation) * Physics.gravity.magnitude * fieldGravityMultiplier, ForceMode.Acceleration);
        }
        else
        {
            rb.AddForce(Physics.gravity * gravityMultiplier, ForceMode.Acceleration);
        }
    }
}
