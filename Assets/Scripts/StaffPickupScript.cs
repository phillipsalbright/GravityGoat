using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffPickupScript : MonoBehaviour
{
    [SerializeField] private GameObject playerWithStaff;
    [SerializeField] private GameObject tutorialText;
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private GameObject door;
    [SerializeField] private GameObject levelLoader;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            Vector3 pos = other.transform.position;
            this.GetComponent<BoxCollider>().enabled = false;
            this.GetComponent<SpriteRenderer>().enabled = false;
            Destroy(other.gameObject);
            Instantiate(playerWithStaff, pos, Quaternion.Euler(Vector3.zero));
            tutorialText.SetActive(true);
            door.SetActive(true);
            levelLoader.SetActive(true);
            particles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            Destroy(this.gameObject);
        }
    }
}
