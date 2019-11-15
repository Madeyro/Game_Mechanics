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
    
    
    // Start is called before the first frame update
    protected virtual void Awake()
    {
        shieldCollider = GetComponent<SphereCollider>();
        shieldRenderer = GetComponent<MeshRenderer>();
        ShieldDeactivation();
    }

    //private void Update()
    //{
    //    if (health <= 0)
    //    {
    //        StopCoroutine("RunTimer");
    //    }

    //    if (!isActive && Input.GetKeyDown(KeyCode.Q))
    //    {
    //        ShieldActivation();
    //    }
    //}

    protected virtual void ShieldActivation()
    {
        // Activate the shield
        health = maxHealth; // New shield => new life
        isActive = true;
        shieldCollider.enabled = true;
        shieldRenderer.enabled = true;
        StartCoroutine("RunTimer");
    }

    protected virtual IEnumerator RunTimer()
    {
        // Start shield duration => after that time deactive the shield
        Debug.Log("Start TIMER");
        yield return new WaitForSeconds(shieldDuration);
        Debug.Log("End TIMER");
        ShieldDeactivation();
    }

    protected virtual void ShieldDeactivation()
    {
        // Deactivate the shield
        shieldCollider.enabled = false;
        shieldRenderer.enabled = false;
        isActive = false;
    }

    public void TakeDamage(int amount)
    {
        Debug.Log("Shield damaged");
        health -= amount;
        if (health <= 0)
        {
            // Deactivate shield if it is broken
            health = 0;
            Debug.Log("Shield destroyed");
            StopCoroutine("RunTimer");
            ShieldDeactivation();
        }
    }
}
