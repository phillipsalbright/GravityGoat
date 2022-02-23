using UnityEngine;
using UnityEngine.InputSystem;

public class GravityGunScript : MonoBehaviour
{
    public int orbCount = 5;
    [SerializeField] private GameObject outwardOrb;
    [SerializeField] private GameObject inwardOrb;
    [SerializeField] private ArmScript arm;
    [SerializeField] private PlayerUIScript ui;
    /** Set to an empty game object child of the arm, the place where gravity orbs will be launched from */
    [SerializeField] private Transform launchOrigin;
    private float projectileForce = 22f;
    private float fireRate = 1f;
    private float nextTimeToFire = 0f;

    private void Start()
    {
        ui.UpdateOrbCount(orbCount);
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (orbCount > 0 && Time.time >= nextTimeToFire)
        {
            orbCount--;
            ui.UpdateOrbCount(orbCount);
            nextTimeToFire = Time.time + 1f / fireRate;
            GameObject launchedOrb = Instantiate(outwardOrb, launchOrigin.TransformPoint(0, 0, 0), launchOrigin.rotation);
            launchedOrb.GetComponent<Rigidbody>().AddForce(launchOrigin.forward * projectileForce, ForceMode.Impulse);
        }
    }

    public void OnAltShoot(InputAction.CallbackContext context)
    {
        if (orbCount > 0 && Time.time >= nextTimeToFire)
        {
            orbCount--;
            ui.UpdateOrbCount(orbCount);
            nextTimeToFire = Time.time + 1f / fireRate;
            GameObject launchedOrb = Instantiate(inwardOrb, launchOrigin.TransformPoint(0, 0, 0), launchOrigin.rotation);
            launchedOrb.GetComponent<Rigidbody>().AddForce(launchOrigin.forward * projectileForce, ForceMode.Impulse);
        }
    }
}
