using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class EnemyAttack : MonoBehaviour
    {
        public float timeBetweenAttacks = 0.5f;     // The time in seconds between each attack.
        public int attackDamage = 10;               // The amount of health taken away per attack.


        Animator anim;                              // Reference to the animator component.
        GameObject player;                          // Reference to the player GameObject.
        GameObject sentry;                          // Reference to the sentry GameObject.
        PlayerHealth playerHealth;                  // Reference to the player's health.
        SentryHealth sentryHealth;                  // Reference to the sentry's health.
        Sentry sentryComp;                          // Reference to the sentry's main component.
        EnemyHealth enemyHealth;                    // Reference to this enemy's health.
        bool playerInRange;                         // Whether player is within the trigger collider and can be attacked.
        bool sentryInRange;                         // Whether player is within the trigger collider and can be attacked.
        float timer;                                // Timer for counting up to the next attack.


        void Awake ()
        {
            // Setting up the references.
            player = GameObject.FindGameObjectWithTag ("Player");
            sentry = GameObject.FindGameObjectWithTag ("Sentry");
            playerHealth = player.GetComponent <PlayerHealth> ();
            sentryHealth = sentry.GetComponent<SentryHealth>();
            sentryComp = sentry.GetComponent<Sentry>();
            enemyHealth = GetComponent<EnemyHealth>();
            anim = GetComponent <Animator> ();
        }


        void OnTriggerEnter (Collider other)
        {
            // If the entering collider is the player...
            if(other.gameObject == player)
            {
                // ... the player is in range.
                playerInRange = true;
            }
            // If the entering collider is the sentry...
            else if (other.gameObject == sentry && !other.isTrigger)
            {
                // ... the sentry is in range.
                sentryInRange = true;
            }
        }


        void OnTriggerExit (Collider other)
        {
            // If the exiting collider is the player...
            if(other.gameObject == player)
            {
                // ... the player is no longer in range.
                playerInRange = false;
            }
            // If the exiting collider is the sentry...
            else if (other.gameObject == sentry)
            {
                // ... the sentry is no longer in range.
                sentryInRange = false;
            }
        }


        void Update ()
        {
            if (!sentryComp.IsDeployed)
            {
                sentryInRange = false;
            }

            // Add the time since Update was last called to the timer.
            timer += Time.deltaTime;

            // If the timer exceeds the time between attacks, the player is in range and this enemy is alive...
            if(timer >= timeBetweenAttacks && enemyHealth.currentHealth > 0)
            {
                // ... attack.
                if (playerInRange) AttackPlayer();
                else if (sentryInRange && sentryComp.IsDeployed) AttackSentry();
            }

            // If the player has zero or less health...
            if(playerHealth.currentHealth <= 0)
            {
                // ... tell the animator the player is dead.
                anim.SetTrigger ("PlayerDead");
            }
        }


        void AttackPlayer ()
        {
            // Reset the timer.
            timer = 0f;

            // If the player has health to lose...
            if(playerHealth.currentHealth > 0)
            {
                // ... damage the player.
                playerHealth.TakeDamage (attackDamage);
            }
        }

        void AttackSentry()
        {
            // Reset the timer.
            timer = 0f;

            // If the player has health to lose...
            if (sentryHealth.currentHealth > 0)
            {
                // ... damage the player.
                sentryHealth.TakeDamage(attackDamage);
            }
        }
    }
}