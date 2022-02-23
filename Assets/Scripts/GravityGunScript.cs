using UnityEngine;
using UnityEngine.InputSystem;

public class GravityGunScript : MonoBehaviour
{
    public int ammo = 5;
    [SerializeField] private GameObject outwardOrb;
    [SerializeField] private GameObject inwardOrb;
    [SerializeField] private ArmScript arm;
    /** Set to an empty game object child of the arm, the place where gravity orbs will be launched from */
    [SerializeField] private Transform launchOrigin;
    private float projectileSpeed = 6f;

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (ammo > 0)
        {
            ammo--;
            GameObject launchedOrb = Instantiate(outwardOrb, launchOrigin.TransformPoint(0, 0, 0), launchOrigin.rotation);
            launchedOrb.GetComponent<Rigidbody>().AddForce(launchOrigin.forward * projectileSpeed, ForceMode.Impulse);
            
        }
    }

    public void OnAltShoot(InputAction.CallbackContext context)
    {

    }
}
