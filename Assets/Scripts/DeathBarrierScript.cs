using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathBarrierScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        } else if (other.gameObject.layer == 7 || other.gameObject.layer == 8)
        {
            Destroy(other.gameObject);
            GravityGunScript gg = GameObject.FindObjectOfType<GravityGunScript>();
            if (gg != null)
            {
                gg.orbCount++;
            }
            PlayerUIScript ui = GameObject.FindObjectOfType<PlayerUIScript>();
            if (ui != null)
            {
                ui.UpdateOrbCount(gg.orbCount);
            }
        }
    }
}
