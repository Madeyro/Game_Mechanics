using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShieldUIManager : MonoBehaviour
{

    // SHIELD POWER UI
    public Slider shieldPowerSlider;
    public Text currentShieldPowerText;
    public Text maxShieldPowerText;

    // SHIELD HEALTH UI
    public Slider shieldHealthSlider;
    public Text currentShieldHealthText;
    public Text maxShieldHealthText;

    // TIMER
    public Text timerText;

    public void InitShieldUI(float maxPower, float maxHealth)
    {
        shieldPowerSlider.maxValue = maxPower;
        shieldHealthSlider.maxValue = maxHealth;

        maxShieldPowerText.text = "/" + maxPower.ToString();
        maxShieldHealthText.text = "/" + maxHealth.ToString();

        DeactivateTimer();
    }

    public void RedrawShieldPowerUI(float currentPower)
    {
        shieldPowerSlider.value = currentPower;
        currentShieldPowerText.text = currentPower.ToString();
    }

    public void RedrawShieldHealthUI(float currentHealth)
    {
        shieldHealthSlider.value = currentHealth;
        currentShieldHealthText.text = currentHealth.ToString();
    }

    public void RedrawShieldUI(float currentPower, float currentHealth)
    {
        RedrawShieldPowerUI(currentPower);
        RedrawShieldHealthUI(currentHealth);
    }

    public void ActivateTimer()
    {
        timerText.enabled = true;
    }

    public void DeactivateTimer()
    {
        timerText.enabled = false;
    }
    public void SetTimerText(string text)
    {
        timerText.text = text;
    }
}
