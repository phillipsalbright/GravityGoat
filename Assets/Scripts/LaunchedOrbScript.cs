using UnityEngine;

public class LaunchedOrbScript : MonoBehaviour
{
    [SerializeField] private GameObject field;
    private bool used = false;

    private void OnCollisionEnter(Collision other)
    {
        this.gameObject.GetComponent<Rigidbody>().velocity.Set(0, 0, 0);
        this.gameObject.GetComponent<GravityScript>().enabled = false;
        if (other.gameObject.layer == 6 && !used)
        {
            used = true;
            Instantiate(field, this.transform.position, this.transform.rotation);
            Destroy(this.gameObject);
        } else if (other.gameObject.layer == 14 && !used)
        {
            used = true;
            Instantiate(field, this.transform.position, this.transform.rotation, other.gameObject.transform);
            Destroy(this.gameObject);
        }
    }
}
