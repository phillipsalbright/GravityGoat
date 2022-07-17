using UnityEngine;
using TMPro;

public class PlayerUIScript : MonoBehaviour
{
    [SerializeField] private TMP_Text orbCountDisplay;

    public void UpdateOrbCount(int newOrbCount)
    {
        orbCountDisplay.text = "Orbs: " + newOrbCount;
    }
}
