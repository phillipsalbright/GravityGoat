using UnityEngine;

public class LaunchedOrbScript : MonoBehaviour
{
    [SerializeField] private GameObject field;

    private void OnTriggerEnter(Collider other)
    {
        this.gameObject.GetComponent<Rigidbody>().velocity.Set(0, 0, 0);
        this.gameObject.GetComponent<Rigidbody>().useGravity = false;
        if (other.gameObject.layer == 6)
        {
            Instantiate(field, this.transform.position, this.transform.rotation);
            Destroy(this.gameObject);
        }
    }
}
