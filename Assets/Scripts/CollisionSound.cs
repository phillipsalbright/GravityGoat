using UnityEngine;

public class CollisionSound : MonoBehaviour
{
    [SerializeField] private AudioClip[] clips;
    [SerializeField] private AudioSource source;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Button")
        {
            return;
        } else if (collision.gameObject.layer == 6)
        {
            if (collision.relativeVelocity.magnitude > 3.5)
            {
                int clipNum = (int)Mathf.Floor(Random.Range(0, 2));
                float audioLevel = collision.relativeVelocity.magnitude / 20;
                if (audioLevel > .61f)
                {
                    audioLevel = .61f;
                }
                source.PlayOneShot(clips[clipNum], audioLevel);
            }

        }
    }

}
