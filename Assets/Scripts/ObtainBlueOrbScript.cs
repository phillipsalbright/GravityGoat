using UnityEngine;

public class ObtainBlueOrbScript : MonoBehaviour
{
    [SerializeField] private GameObject blueOrb;
    [SerializeField] private GameObject playerWithBlue;
    [SerializeField] private GameObject playerWithoutBlue;
    [SerializeField] private GameObject text1;
    [SerializeField] private GameObject text2;

    private bool started = false;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (blueOrb == null || blueOrb.GetComponent<GravityField>() == null || !blueOrb.GetComponent<GravityField>().GetActive())
        {
            if (!started)
            {
                started = true;
                EventStart();
            }
        }
    }

    private void EventStart()
    {
        Vector3 pos = playerWithoutBlue.transform.position;
        text1.SetActive(false);
        text2.SetActive(true);
        Destroy(playerWithoutBlue);
        Instantiate(playerWithBlue, pos, Quaternion.Euler(Vector3.zero));
    }
}
