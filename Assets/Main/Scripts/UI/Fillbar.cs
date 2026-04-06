using UnityEngine;
using UnityEngine.UI;
public class Fillbar : MonoBehaviour
{
    [SerializeField] private Image fillBar;
    public void UpdateBars(float maxAmount, float currentAmount)
    {
        fillBar.fillAmount = currentAmount / maxAmount;
    }
}