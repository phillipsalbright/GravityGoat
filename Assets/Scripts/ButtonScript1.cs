using UnityEngine;

public class ButtonScript1 : MonoBehaviour
{
    [SerializeField] private Animator crystalAnimator;
    [SerializeField] private Animator buttonAnimator;
    private bool pressed = false;
    private int collisions = 0;


    private void OnTriggerEnter(Collider other)
    {
        collisions++;
        if (!pressed)
        {
            pressed = true;
            buttonAnimator.Play("ButtonPress");
            crystalAnimator.Play("CrystalRaise");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        collisions--;
        if (collisions == 0)
        {
            pressed = false;
            buttonAnimator.Play("ButtonUnpress");
            crystalAnimator.Play("CrystalUnraise");
        }

    }
}
