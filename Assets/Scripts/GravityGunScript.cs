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
    [SerializeField] private LayerMask layers;
    private float projectileForce = 25f;
    private float fireRate = 1f;
    private float nextTimeToFire = 0f;

    private void Start()
    {
        ui.UpdateOrbCount(orbCount);
    }

    public void OnAltShoot(InputAction.CallbackContext context)
    {
        if (context.action.triggered && orbCount > 0 && Time.time >= nextTimeToFire)
        {
            orbCount--;
            ui.UpdateOrbCount(orbCount);
            nextTimeToFire = Time.time + 1f / fireRate;
            GameObject launchedOrb = Instantiate(outwardOrb, launchOrigin.TransformPoint(0, 0, 0) + new Vector3(0, 0, -.5f), launchOrigin.rotation);
            launchedOrb.GetComponent<Rigidbody>().AddForce(launchOrigin.forward * projectileForce, ForceMode.Impulse);
        }
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.action.triggered && orbCount > 0 && Time.time >= nextTimeToFire)
        {
            orbCount--;
            ui.UpdateOrbCount(orbCount);
            nextTimeToFire = Time.time + 1f / fireRate;
            GameObject launchedOrb = Instantiate(inwardOrb, launchOrigin.TransformPoint(0, 0, 0) + new Vector3(0, 0, -.5f), launchOrigin.rotation);
            launchedOrb.GetComponent<Rigidbody>().AddForce(launchOrigin.forward * projectileForce, ForceMode.Impulse);
        }
    }

    public void OnOrbRetrieve(InputAction.CallbackContext context)
    {
        if (context.action.triggered)
        {
            RaycastHit hit;
            if (Physics.Raycast(launchOrigin.position, launchOrigin.forward, out hit, Mathf.Infinity, layers))
            {
                GameObject objectHit = hit.transform.gameObject;
                if (objectHit.layer == 7 || objectHit.layer == 8)
                {
                    GravityField f = objectHit.transform.GetComponentInParent<GravityField>();
                    if (f.GetActive())
                    {
                        f.Implode();
                        //Destroy(objectHit.transform.parent.gameObject);
                        orbCount++;
                        ui.UpdateOrbCount(orbCount);
                    }
                }
            }
        }
        PlayerMovement pm = this.gameObject.GetComponentInParent<PlayerMovement>();
        if (pm != null)
        {
            foreach (GravityField f in pm.currentFieldCollisions)
            {
                if (f.GetActive())
                {
                    pm.currentFieldCollisions.Remove(f);
                    f.Implode();
                    orbCount++;
                    ui.UpdateOrbCount(orbCount);
                    break;
                }
            }
        }
    }
}
