using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFreezable : MonoBehaviour
{
    public Material freezeMaterial;

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
    }

    public IEnumerator Freeze(float duration)
    {
        Debug.Log("EnenmyFreezed");
        // FreezeThisEnemy
        enemyAttack.enabled = false;
        enemyRenderer.material = freezeMaterial;
        enemyMovement.StopMovement();
        yield return new WaitForSeconds(duration);
        enemyAttack.enabled = true;
        enemyRenderer.material = originMaterial;
        enemyMovement.ResumeMovement();
    }
}
