using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

namespace CompleteProject
{
    public class SentryHealth : MonoBehaviour
    {
        public int startingHealth = 100;                            // The amount of health the sentry starts the game with.
        public int currentHealth;                                   // The current health the sentry has.
        public Slider healthSlider;                                 // Reference to the UI's health bar.
        public AudioClip deathClip;                                 // The audio clip to play when the sentry dies.
        public float flashSpeed = 5f;                               // The speed the damageImage will fade at.
        public Color flashColour = new Color(1f, 0f, 0f, 0.1f);     // The colour the damageImage is set to, to flash.

        PlayerSentry playerSentry;                                  // Reference to the PlayerSentry component on player.
        ParticleSystem part;                                        // Reference to the Animator component.
        AudioSource sentryAudio;                                    // Reference to the AudioSource component.
        Sentry sentryComp;                                          // Reference to the Sentry script.
        bool isDead;                                                // Whether the sentry is dead.


        void Awake()
        {
            // Setting up the references.
            part = GetComponent<ParticleSystem>();
            sentryAudio = GetComponent<AudioSource>();
            sentryComp = GetComponent<Sentry>();
            playerSentry = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSentry>();

            // Set the initial health of the sentry.
            currentHealth = startingHealth;
        }

        public void RestoreHealth()
        {
            isDead = false;
            currentHealth = startingHealth;
            healthSlider.value = currentHealth;
        }

        public void TakeDamage(int amount)
        {
            // Reduce the current health by the damage amount.
            currentHealth -= amount;

            // Set the health bar's value to the current health.
            healthSlider.value = currentHealth;

            // Play the hurt sound effect.
            sentryAudio.Play();

            // If the player has lost all it's health and the death flag hasn't been set yet...
            if (currentHealth <= 0 && !isDead)
            {
                // ... it should die.
                Death();
            }
        }


        void Death()
        {
            // Set the death flag so this function won't be called again.
            isDead = true;

            // Pick the sentry up.
            sentryComp.PickUp();
            playerSentry.Cooldown = 5.0f;
        }
    }
}