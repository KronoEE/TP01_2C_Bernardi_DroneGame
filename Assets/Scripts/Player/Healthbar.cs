using UnityEngine;
using UnityEngine.UI;
public class Healthbar : MonoBehaviour
{
    [SerializeField] private Image fillHealthBar;
    public void UpdateHealthBar(float maxHealth, float health)
    {
        fillHealthBar.fillAmount = health / maxHealth;
    }
}