using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffPickupScript : MonoBehaviour
{
    [SerializeField] private GameObject playerWithStaff;
    [SerializeField] private GameObject tutorialText;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            Vector3 pos = other.transform.position;
            this.GetComponent<BoxCollider>().enabled = false;
            tutorialText.SetActive(true);
            Destroy(other.gameObject);
            Instantiate(playerWithStaff, pos, Quaternion.Euler(Vector3.zero));
            Destroy(this.gameObject);
        }
    }
}
