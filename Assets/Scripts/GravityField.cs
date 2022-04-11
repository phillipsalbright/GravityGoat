using UnityEngine;
using System.Collections;

public class GravityField : MonoBehaviour
{
    /** Set in prefab editor. 1 for outward force orb, -1 for inward */
    [SerializeField] private int outwardForce;
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem impactParticle;
    [SerializeField] private ParticleSystem destroyParticle;
    [SerializeField] private SphereCollider field;
    [SerializeField] private AudioSource implodingSound;
    private bool active;

    public void Start()
    {
        active = true;
        if (destroyParticle.gameObject.activeSelf)
        {
            impactParticle.Play();
        }
    }

    public bool GetActive()
    {
        return active;
    }

    public Vector3 GetPosition()
    {
        return this.transform.position;
    }

    public int GetOutwardForce()
    {
        return outwardForce;
    }

    public void Implode()
    {
        active = false;
        this.gameObject.layer = 0;
        StartCoroutine("ImplodingWithAnimation");
    }

    IEnumerator ImplodingWithAnimation()
    {
        animator.Play("FieldAbsorb");
        destroyParticle.Play();
        this.GetComponent<Collider>().enabled = false;
        field.enabled = false;
        this.GetComponent<MeshRenderer>().enabled = false;
        implodingSound.Play();
        yield return new WaitForSeconds(.5f);
        this.GetComponentInChildren<Light>().enabled = false;
        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);
    }
}
