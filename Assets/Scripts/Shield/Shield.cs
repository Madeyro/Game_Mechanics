using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{

    protected SphereCollider shieldCollider;
    protected MeshRenderer shieldRenderer;

    public float shieldDuration; // How long (seconds) can shield be active 
    public int maxHealth;
    public int health;
    public bool isActive = false;
    public int maxPower;// power that is needed to activate shield
    public int currentPower;

    public ShieldUIManager shieldUI;




    // Start is called before the first frame update
    protected virtual void Awake()
    {
        shieldCollider = GetComponent<SphereCollider>();
        shieldRenderer = GetComponent<MeshRenderer>();

        currentPower = maxPower;
        health = maxHealth;

        ShieldDeactivation();
        shieldUI.InitShieldUI(currentPower, health);
        shieldUI.RedrawShieldUI(currentPower, health);
    }

    protected virtual void ShieldActivation()
    {
        if (currentPower == maxPower)
        {
            // Activate the shield
            health = maxHealth; // New shield => new life
            shieldUI.RedrawShieldHealthUI(health);

            isActive = true;
            shieldCollider.enabled = true;
            shieldRenderer.enabled = true;

            currentPower = 0; // New shield => zero power
            shieldUI.RedrawShieldPowerUI(currentPower);
            StartCoroutine("RunTimer");
        }
    }

    protected virtual IEnumerator RunTimer()
    {
        shieldUI.SetTimerText(shieldDuration.ToString());
        shieldUI.ActivateTimer();
        // Start shield duration => after that time deactive the shield
        yield return new WaitForSeconds(shieldDuration);
        ShieldDeactivation();
    }

    protected virtual void ShieldDeactivation()
    {
        // Deactivate the shield
        shieldCollider.enabled = false;
        shieldRenderer.enabled = false;
        isActive = false;

        shieldUI.DeactivateTimer();
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            // Deactivate shield if it is broken
            health = 0;
            StopCoroutine("RunTimer");
            ShieldDeactivation();
        }
        shieldUI.RedrawShieldHealthUI(health);
    }

    public void AddPower(int amount)
    {
        // Add some amount to shield power and redraw its UI
        currentPower += amount;
        if (currentPower > maxPower)
        {
            currentPower = maxPower;
        }
        else if (currentPower < 0)
        {
            currentPower = 0;
        }
        shieldUI.RedrawShieldPowerUI(currentPower);
    }

    
}
