using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
public class UiHealthSystem : MonoBehaviour
{
    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private Image healthBarBackground;
    [SerializeField] private Image healthBarFill;
    private void Awake()
    {
        healthSystem.onLifeChanged += HealthSystem_onLifeChanged;
        healthSystem.onDie += HealthSystem_onDie;
    }
    private void OnDestroy()
    {
        healthSystem.onLifeChanged -= HealthSystem_onLifeChanged;
        healthSystem.onDie -= HealthSystem_onDie;
    }
    private void HealthSystem_onLifeChanged(float currentHealth, float maxHealth)
    {
        float barFillAmount = currentHealth / maxHealth;
        healthBarFill.fillAmount = barFillAmount;
    }
    private void HealthSystem_onDie()
    {
        healthBarFill.fillAmount = 0f;
        healthBarBackground.color = Color.black;
    }

}
