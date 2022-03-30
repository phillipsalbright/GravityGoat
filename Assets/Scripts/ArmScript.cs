using System;
using UnityEngine;
using UnityEngine.InputSystem;

/**
 * Makes the arm move with the mouse on the player prefab. Has become very complex for this specific scenario, especially
 * with regards to our rendering system for sprites.
 */
public class ArmScript : MonoBehaviour
{
    public Vector3 mouse_pos;


    /** Base of the wand (where the hand touches it) */
    public Transform target;

    /** Should have pivot where the shoulder joint is, or sprite itself */
    public Transform objectToRotate;

    public Vector3 object_pos;

    public float angle;

    public float yRotation;

    bool turnedLeft;

    /** Set to the whole player in editor */
    [SerializeField] private Transform player;

    [SerializeField] private SpriteRenderer body;
    [SerializeField] private SpriteRenderer arm;
    [SerializeField] private Transform arm2;

    private void Start()
    {
        turnedLeft = false;
    }

    void Update()
    {
        //yRotation = player.rotation.eulerAngles.y;
        mouse_pos = new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, 0);
        mouse_pos.z = 15.4f; //The distance between the camera and object
        object_pos = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().WorldToScreenPoint(target.position);
        mouse_pos.x -= object_pos.x;
        mouse_pos.y -= object_pos.y;
        Vector3 object_pos2 = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().WorldToScreenPoint(objectToRotate.position);
        Vector3 mouse_pos2 = Mouse.current.position.ReadValue();
        mouse_pos2.x -= object_pos2.x;
        mouse_pos2.y -= object_pos2.y;
        if ((Math.Abs(mouse_pos2.x) + Math.Abs(mouse_pos2.y)) >= 50)
        {
            float rotAngle;
            if (turnedLeft)
            {
                rotAngle = 146;
            } else
            {
                rotAngle = 35;
            }
            angle = (Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg) + rotAngle;
            if (!turnedLeft)
            {
                objectToRotate.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            }
            else
            {
                objectToRotate.localRotation = Quaternion.Euler(new Vector3(0, 180, angle));
            }
            float zRotation = objectToRotate.localRotation.eulerAngles.z;
            Debug.Log(zRotation);
            if ((!turnedLeft && zRotation < 300 && zRotation > 195) || (turnedLeft && zRotation < 165 && zRotation > 60))
            {
                Vector3 rot = player.rotation.eulerAngles;
                Vector3 negRot = rot;
                if (!turnedLeft)
                {
                    rot.y += 180;
                    turnedLeft = true;
                } else
                {
                    rot.y -= 180;
                    turnedLeft = false;
                }
                player.GetComponent<Rigidbody>().freezeRotation = false;
                player.rotation = Quaternion.Euler(rot);
                player.GetComponent<Rigidbody>().freezeRotation = true;
                if (turnedLeft)
                {
                    body.flipX = true;
                    arm.flipX = true;
                    arm2.localRotation = Quaternion.Euler(rot);
                    body.gameObject.transform.rotation = Quaternion.Euler(negRot);
                } else
                {
                    body.flipX = false;
                    arm.flipX = false;

                    body.gameObject.transform.rotation = Quaternion.Euler(rot);

                    arm2.localRotation = Quaternion.Euler(rot);
                    Debug.Log(arm.gameObject.transform.localRotation.eulerAngles);
                }
                yRotation = rot.y;
            }
        }
    }
}