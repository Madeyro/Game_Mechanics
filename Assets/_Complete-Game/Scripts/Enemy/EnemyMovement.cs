using UnityEngine;
using System.Collections;

namespace CompleteProject
{
    public class EnemyMovement : MonoBehaviour
    {
        Transform player;               // Reference to the player's position.
        public Transform sentry;               // Reference to the sentry's position.
        PlayerHealth playerHealth;      // Reference to the player's health.
        EnemyHealth enemyHealth;        // Reference to this enemy's health.
        UnityEngine.AI.NavMeshAgent nav;               // Reference to the nav mesh agent.

        Animator anim;

        [SerializeField]
        public bool TargetSentry { get { return targetSentry; } set { targetSentry = (value && sentry); } }
        private bool targetSentry = false;


        void Awake ()
        {
            // Set up the references.
            player = GameObject.FindGameObjectWithTag ("Player").transform;
            sentry = GameObject.FindGameObjectWithTag("Sentry")?.transform;
            playerHealth = player.GetComponent <PlayerHealth> ();
            enemyHealth = GetComponent <EnemyHealth> ();
            nav = GetComponent <UnityEngine.AI.NavMeshAgent> ();
            anim = GetComponent<Animator>();
        }


        void Update ()
        {
            // If the enemy and the player have health left...
            if(enemyHealth.currentHealth > 0 && playerHealth.currentHealth > 0)
            {
                // ... set the destination of the nav mesh agent to the player.
                nav.SetDestination (TargetSentry ? sentry.position : player.position);
            }
            // Otherwise...
            else
            {
                // ... disable the nav mesh agent.
                nav.enabled = false;
            }
        }

        public void StopMovement()
        {
            if (nav.enabled)
            {
                nav.isStopped = true;
                anim.enabled = false;
            }

        }

        public void ResumeMovement()
        {
            if (nav.enabled)
            {
                nav.isStopped = false;
                anim.enabled = true;
            }
        }
    }
}