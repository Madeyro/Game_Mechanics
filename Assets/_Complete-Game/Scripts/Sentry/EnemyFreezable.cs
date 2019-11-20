using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyFreezable : MonoBehaviour
{
    public Material freezeMaterial;
    public bool isFreezed;

    EnemyAttack enemyAttack;
    EnemyMovement enemyMovement;
    Material originMaterial;
    Renderer enemyRenderer;
    

    // Start is called before the first frame update
    void Awake()
    {
        enemyAttack = GetComponent<EnemyAttack>();
        enemyMovement = GetComponent<EnemyMovement>();
        enemyRenderer = GetComponentInChildren<Renderer>();
        originMaterial = enemyRenderer.material;
        isFreezed = false;
    }

    public IEnumerator Freeze(float duration, float waitForFreeze)
    {
        yield return new WaitForSeconds(waitForFreeze);

        // Freeze this enemy
        isFreezed = true;
        enemyAttack.enabled = false;
        enemyRenderer.material = freezeMaterial;
        enemyMovement.StopMovement();

        yield return new WaitForSeconds(duration);

        // Unfreeze this enemy
        isFreezed = false;
        enemyAttack.enabled = true;
        enemyRenderer.material = originMaterial;
        enemyMovement.ResumeMovement();
    }
}
