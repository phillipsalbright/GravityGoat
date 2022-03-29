using System;
using UnityEngine;
using UnityEngine.InputSystem;

/**
 * Makes the arm move with the mouse on the player prefab.
 */
public class ArmScript : MonoBehaviour
{
    public Vector3 mouse_pos;

    /** The arm location (shoulder joint). Used to see if mouse too close to center of player. */
    public Transform target;

    /** Base of the wand (where the hand touches it) */
    public Transform target2;

    /** Base of the wand (where hand touches it) */
    //public Transform target2;
    public Transform objectToRotate;

    public Vector3 object_pos;

    public float angle;

    public float yRotation;

    void FixedUpdate()
    {
        /**
         * only necessary if using Update()
        if (Time.timeScale == 0)
        {
            return
        }
        */
        //yRotation = GetComponentInParent<Transform>().rotation.eulerAngles.y;
        yRotation = objectToRotate.rotation.eulerAngles.y;
        mouse_pos = new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, 0);
        mouse_pos.z = 15.4f; //The distance between the camera and object
        object_pos = GameObject.Find("MainCamera").GetComponent<Camera>().WorldToScreenPoint(target2.position);
        mouse_pos.x -= object_pos.x;
        mouse_pos.y -= object_pos.y;
        Vector3 object_pos2 = GameObject.Find("MainCamera").GetComponent<Camera>().WorldToScreenPoint(target.position);
        Vector3 mouse_pos2 = Mouse.current.position.ReadValue();
        mouse_pos2.x -= object_pos2.x;
        mouse_pos2.y -= object_pos2.y;
        if ((Math.Abs(mouse_pos2.x) + Math.Abs(mouse_pos2.y)) >= 50)
        {
            angle = (Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg) + 30;
            //angle -= 90;
            if (yRotation == 0)
            {
                objectToRotate.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            }
            else
            {
                objectToRotate.rotation = Quaternion.Euler(new Vector3(0, 180, -angle));
            }
        }
    }
}