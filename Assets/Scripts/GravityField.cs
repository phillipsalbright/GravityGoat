using UnityEngine;

public class GravityField : MonoBehaviour
{
    /** Set in prefab editor. 1 for outward force orb, -1 for inward */
    [SerializeField] private int outwardForce;

    public Vector3 GetPosition()
    {
        return this.transform.position;
    }

    public int GetOutwardForce()
    {
        return outwardForce;
    }
}
