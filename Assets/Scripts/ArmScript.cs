using System;
using UnityEngine;
using UnityEngine.InputSystem;

/**
 * Makes the arm move with the mouse on the player prefab.
 */
public class ArmScript : MonoBehaviour
{
    public Vector3 mouse_pos;


    /** Base of the wand (where the hand touches it) */
    public Transform target;

    /** Should have pivot where the shoulder joint is */
    public Transform objectToRotate;

    public Vector3 object_pos;

    public float angle;

    public float yRotation;

    bool turnedLeft;

    /** Set to the whole player in editor */
    [SerializeField] private Transform player;

    private void Start()
    {
        turnedLeft = false;
    }

    void FixedUpdate()
    {
        yRotation = player.rotation.eulerAngles.y;
        mouse_pos = new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, 0);
        mouse_pos.z = 15.4f; //The distance between the camera and object
        object_pos = GameObject.Find("MainCamera").GetComponent<Camera>().WorldToScreenPoint(target.position);
        mouse_pos.x -= object_pos.x;
        mouse_pos.y -= object_pos.y;
        Vector3 object_pos2 = GameObject.Find("MainCamera").GetComponent<Camera>().WorldToScreenPoint(objectToRotate.position);
        Vector3 mouse_pos2 = Mouse.current.position.ReadValue();
        mouse_pos2.x -= object_pos2.x;
        mouse_pos2.y -= object_pos2.y;
        if ((Math.Abs(mouse_pos2.x) + Math.Abs(mouse_pos2.y)) >= 50)
        {
            float rotAngle;
            if (yRotation == 180)
            {
                rotAngle = 160;
            } else
            {
                rotAngle = 35;
            }
            angle = (Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg) + rotAngle;
            //angle -= 90;
            if (yRotation == 0)
            {
                objectToRotate.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            }
            else
            {
                objectToRotate.rotation = Quaternion.Euler(new Vector3(0, 180, -angle));
            }
            float zRotation = objectToRotate.rotation.eulerAngles.z;
            Debug.Log(zRotation);
            if (zRotation < 300 && zRotation > 195)
            {
                Vector3 rot = player.rotation.eulerAngles;
                if (rot.y == 0)
                {
                    rot.y += 180;
                } else
                {
                    rot.y -= 180;
                }
                player.rotation = Quaternion.Euler(rot);
            }
        }
    }
}