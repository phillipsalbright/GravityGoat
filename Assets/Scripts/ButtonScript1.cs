using UnityEngine;
using System.Collections.Generic;

public class ButtonScript1 : MonoBehaviour
{
    [SerializeField] private Animator crystalAnimator;
    [SerializeField] private Animator buttonAnimator;
    private bool pressed = false;
    public SlidingScript[] objectsToMove;
    private List<GameObject> collidingObjects = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        int layer = other.gameObject.layer;
        if (layer != 7 && layer != 8)
        {
            collidingObjects.Add(other.gameObject);
            if (!pressed)
            {
                pressed = true;
                buttonAnimator.Play("ButtonPress");
                //crystalAnimator.Play("CrystalRaise");
                for (int i = 0; i < objectsToMove.Length; i++)
                {
                    objectsToMove[i].StartSlide();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        collidingObjects.Remove(other.gameObject);
        if (collidingObjects.Count == 0)
        {
            pressed = false;
            buttonAnimator.Play("ButtonUnpress");
            //crystalAnimator.Play("CrystalUnraise");
            for (int i = 0; i < objectsToMove.Length; i++)
            {
                objectsToMove[i].EndSlide();
            }
        }

    }
}
