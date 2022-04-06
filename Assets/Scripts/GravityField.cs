using UnityEngine;
using System.Collections;

public class GravityField : MonoBehaviour
{
    /** Set in prefab editor. 1 for outward force orb, -1 for inward */
    [SerializeField] private int outwardForce;
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem impactParticle;
    [SerializeField] private ParticleSystem destroyParticle;

    public void Start()
    {
        impactParticle.Play();
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
        StartCoroutine("ImplodingWithAnimation");
    }

    IEnumerator ImplodingWithAnimation()
    {
        animator.Play("FieldAbsorb");
        destroyParticle.Play();
        yield return new WaitForSeconds(.5f);
        Destroy(this.gameObject);
    }
}
