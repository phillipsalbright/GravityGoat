using System.Collections;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private float despawnTime = 15;
    [SerializeField] private float forceFactor = 20;
    private Rigidbody rb;

    void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
        rb.AddForce(this.transform.forward * forceFactor, ForceMode.Impulse);
    }

    private void Update()
    {
        if (rb != null && rb.velocity != Vector3.zero)
        {
            this.transform.rotation = Quaternion.LookRotation(rb.velocity);
        }
        this.GetComponent<MeshRenderer>().enabled = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 3)
        {
            GameObject.FindObjectOfType<PlayerMovement>().HitByArrow();
        }
        this.gameObject.GetComponent<Collider>().enabled = false;
        this.gameObject.GetComponent<GravityScript>().enabled = false;
        Destroy(rb);
        this.gameObject.transform.parent = collision.gameObject.transform;
    }

    IEnumerator DespawnArrow()
    {
        yield return new WaitForSeconds(despawnTime);
        Destroy(this.gameObject);
    }
}
