using UnityEngine;

public class TrackingCameraScript : MonoBehaviour
{
    public float maxYPos = 10;

    public float minYPos = -10;

    public float maxXPos = 20;

    public float minXPos = -10;

    public float zval = -17.1f;

    public GameObject player = null;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            float yval = player.transform.position.y;
            float xval = player.transform.position.x;
            if (yval >= maxYPos)
            {
                yval = maxYPos;
            } else if (yval <= minYPos)
            {
                yval = minYPos;
            }
            if (xval >= maxXPos)
            {
                xval = maxXPos;
            } else if (xval <= minXPos)
            {
                xval = minXPos;
            }
            this.gameObject.transform.position = new Vector3(xval, yval, zval);
        } else
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }
}
