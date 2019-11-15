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
        enemyAttack = GetComponent<EnemyAttack>();
        enemyMovement = GetComponent<EnemyMovement>();
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
}
