using UnityEngine;

namespace CompleteProject
{
    public class MedkitManager : MonoBehaviour
    {

        public PlayerHealth playerHealth;       // Reference to the player's heatlh.
        public GameObject medkit;               // The enemy prefab to be spawned.
        public float spawnTime = 30f;           // How long between each spawn.
        public Transform[] spawnPoints;         // An array of the spawn points med-kit can spawn from.


        void Start()
        {
            // Call the Spawn function after a delay of the spawnTime and then continue to call after the same amount of time.
            InvokeRepeating("Spawn", spawnTime, spawnTime);
        }


        void Spawn()
        {
            // If the player has no health left...
            if (playerHealth.currentHealth <= 0f)
            {
                // ... exit the function.
                return;
            }

            if (!GameObject.Find("Medkit(Clone)"))
            {
                Instantiate(medkit, spawnPoints[Random.Range(0, spawnPoints.Length)]);
            }
        }
    }
}
