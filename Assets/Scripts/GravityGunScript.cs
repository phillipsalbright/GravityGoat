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
    private float projectileForce = 24f;
    private float fireRate = 1.5f;
    private float nextTimeToFire = 0f;
    /** Debug option for different methods of retrieving orbs */
    private int mode = 0;
    /** Set to audioSource for firing sound */
    [SerializeField] private AudioSource firingSound;

    private void Start()
    {
        ui.UpdateOrbCount(orbCount);
        Physics.IgnoreLayerCollision(7, 8);
        Physics.IgnoreLayerCollision(7, 7);
        Physics.IgnoreLayerCollision(8, 8);
    }

    public void OnAltShoot(InputAction.CallbackContext context)
    {
        if (context.action.triggered && orbCount > 0 && Time.time >= nextTimeToFire)
        {
            Shoot(1);
        }
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.action.triggered && orbCount > 0 && Time.time >= nextTimeToFire)
        {
            Shoot(0);
        }
    }

    private void Shoot(int orbType)
    {
        orbCount--;
        ui.UpdateOrbCount(orbCount);
        nextTimeToFire = Time.time + 1f / fireRate;
        GameObject launchedOrb;
        if (orbType == 0)
        {

            launchedOrb = Instantiate(inwardOrb, launchOrigin.TransformPoint(0, 0, 0) + new Vector3(0, 0, -.5f), launchOrigin.rotation);
        } else
        {

            launchedOrb = Instantiate(outwardOrb, launchOrigin.TransformPoint(0, 0, 0) + new Vector3(0, 0, -.5f), launchOrigin.rotation);
        }
        firingSound.Play();
        Physics.IgnoreLayerCollision(7, 8);
        launchedOrb.GetComponent<Rigidbody>().AddForce(launchOrigin.forward * projectileForce, ForceMode.Impulse);
    }

    public void OnOrbRetrieve(InputAction.CallbackContext context)
    {
        if (context.action.triggered)
        {
            if (mode == 0)
            {
                bool orbObtained = false;
                Ray ray = Camera.main.ScreenPointToRay(new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, 0));
                Vector3 position = new Vector3(arm.mouse_pos.x, arm.mouse_pos.y, GameObject.FindGameObjectWithTag("MainCamera").transform.position.z);
                RaycastHit hit;
                if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, layers))
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
                            orbObtained = true;
                        }
                    }
                }
                if (!orbObtained)
                {
                    bool insideOrb = false;
                    PlayerMovement pm = this.gameObject.GetComponentInParent<PlayerMovement>();
                    if (pm != null)
                    {
                        foreach (GravityField f in pm.currentFieldCollisions)
                        {
                            if (f.GetActive())
                            {
                                insideOrb = true;
                                pm.currentFieldCollisions.Remove(f);
                                f.Implode();
                                orbCount++;
                                ui.UpdateOrbCount(orbCount);
                                break;
                            }
                        }
                    }
                    if (!insideOrb)
                    {
                        RaycastHit hit2;
                        if (Physics.Raycast(launchOrigin.position, launchOrigin.forward, out hit2, Mathf.Infinity, layers))
                        {
                            GameObject objectHit = hit2.transform.gameObject;
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
                }


            } else if (mode == 1)
            {
                Ray ray = Camera.main.ScreenPointToRay(new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, 0));
                Vector3 position = new Vector3(arm.mouse_pos.x, arm.mouse_pos.y, GameObject.FindGameObjectWithTag("MainCamera").transform.position.z);
                RaycastHit hit;
                if (Physics.Raycast(ray.origin, ray.direction, out hit,  Mathf.Infinity, layers))
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
            } else if (mode == 2)
            {
                bool insideOrb = false;
                PlayerMovement pm = this.gameObject.GetComponentInParent<PlayerMovement>();
                if (pm != null)
                {
                    foreach (GravityField f in pm.currentFieldCollisions)
                    {
                        if (f.GetActive())
                        {
                            insideOrb = true;
                            pm.currentFieldCollisions.Remove(f);
                            f.Implode();
                            orbCount++;
                            ui.UpdateOrbCount(orbCount);
                            break;
                        }
                    }
                }
                if (!insideOrb)
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
            }
            
        }
    }
}
