using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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

    public IEnumerator Freeze(float duration, float waitForFreeze)
    {
        yield return new WaitForSeconds(waitForFreeze);
        Debug.Log("EnenmyFreezed");
        // FreezeThisEnemy
        enemyAttack.enabled = false;
        enemyRenderer.material = freezeMaterial;
        enemyMovement.StopMovement();
        Debug.Log("Before Freeze");
        yield return new WaitForSeconds(duration);
        Debug.Log("After Freeze");
        enemyAttack.enabled = true;
        enemyRenderer.material = originMaterial;
        enemyMovement.ResumeMovement();
    }

    public async Task TaskAsyncFreeze(float duration, float waitForFreeze)
    {
        await Task.Delay(TimeSpan.FromSeconds(waitForFreeze));
        Debug.Log("EnenmyFreezed");

        // FreezeThisEnemy
        enemyAttack.enabled = false;
        Debug.Log("enemyAttack.enabled");
        enemyRenderer.material = freezeMaterial;
        Debug.Log("enemyRenderer.material");
        enemyMovement.StopMovement();
        Debug.Log("Before Freeze");
        await Task.Delay(TimeSpan.FromSeconds(duration));
        Debug.Log("After Freeze");
        enemyAttack.enabled = true;
        enemyRenderer.material = originMaterial;
        enemyMovement.ResumeMovement();
    }

    public void ThreadFreeze(float duration, float waitForFreeze)
    {
        Thread.Sleep(TimeSpan.FromSeconds(waitForFreeze));
        Debug.Log("EnenmyFreezed");
        // FreezeThisEnemy
        enemyAttack.enabled = false;
        enemyRenderer.material = freezeMaterial;
        enemyMovement.StopMovement();
        Debug.Log("Before Freeze");
        Thread.Sleep(TimeSpan.FromSeconds(duration));
        Debug.Log("After Freeze");
        enemyAttack.enabled = true;
        enemyRenderer.material = originMaterial;
        enemyMovement.ResumeMovement();
    }

    public void InvokeFreeze1(float duration, float waitForFreeze)
    {
        Debug.Log("EnenmyFreezed");
        // FreezeThisEnemy
        enemyAttack.enabled = false;
        enemyRenderer.material = freezeMaterial;
        enemyMovement.StopMovement();
        Debug.Log("Before Freeze");
        Invoke("InvokeFreeze2",waitForFreeze);
       
    }

    public void InvokeFreeze2()
    {

        Debug.Log("After Freeze");
        enemyAttack.enabled = true;
        enemyRenderer.material = originMaterial;
        enemyMovement.ResumeMovement();
    }

    public void Freeze()
    {
        enemyAttack.enabled = false;
        enemyRenderer.material = freezeMaterial;
        enemyMovement.StopMovement();
    }

    public void UnFreeze()
    {
        enemyAttack.enabled = true;
        enemyRenderer.material = originMaterial;
        enemyMovement.ResumeMovement();
    }
}
