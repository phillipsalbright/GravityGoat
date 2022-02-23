using UnityEngine;

public class LaunchedOrbScript : MonoBehaviour
{
    [SerializeField] private GameObject field;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            Instantiate(field, this.transform.position, this.transform.rotation);
            Destroy(this.gameObject);
        }
    }
}
