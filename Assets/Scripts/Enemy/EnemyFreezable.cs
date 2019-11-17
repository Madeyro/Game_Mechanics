using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFreezable : MonoBehaviour
{

    EnemyAttack enemyAttack;
    EnemyMovement enemyMovement;
    // Start is called before the first frame update
    void Awake()
    {
       // enemyAttack = GetComponent<EnemyAttack>();
       // enemyMovement = GetComponent<EnemyMovement>();
    }

    public IEnumerator Freeze(float duration)
    {
        // FreezeThisEnemy
        enemyAttack.enabled = false;
        enemyMovement.StopMovement();
        yield return new WaitForSeconds(duration);
        enemyAttack.enabled = true;
        enemyMovement.ResumeMovement();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("i do not know: " + collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Freeze"))
        {
            Debug.Log("Triggered with particles");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger i do not know: " + other.tag);
        if (other.CompareTag("Freeze"))
        {
            Debug.Log("BUUUM");
        }
    }
}
