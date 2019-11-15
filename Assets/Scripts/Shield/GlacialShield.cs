using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

public class GlacialShield : Shield
{
    public float maxFreezeRadius;
    public float minFreezeRadius;
    public float freezeDuration;

    [SerializeField]
    private float freezeRadius;
    private int freezeObjectMask;


    protected override void Awake()
    {
        base.Awake();
        CalculateFreezeRadius();
        freezeObjectMask = LayerMask.GetMask("Shootable");
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            ShieldActivation();
        }
    }

    protected override IEnumerator RunTimer()
    {
        // Start shield duration => after that time deactive the shield
        Debug.Log("Start TIMER");
        yield return new WaitForSeconds(shieldDuration);
        Debug.Log("End TIMER");
        ShieldDeactivation();
        FreezeEnemies();
    }

    private void FreezeEnemies()
    {
        // Freeze all enemies in particular
        CalculateFreezeRadius();
        Collider[] objectsInFreezeArea = Physics.OverlapSphere(transform.position, freezeRadius, freezeObjectMask);
        Collider[] sortedEnemiesInFreezeArea = SortEnemiesByDistanceFromShield(objectsInFreezeArea);
        Debug.Log("FreezeEnemies Number: " + sortedEnemiesInFreezeArea.Length);
        StartCoroutine("Freeze", sortedEnemiesInFreezeArea);
    }

    private IEnumerator Freeze(Collider[] sortedEnemiesInFreezeArea)
    {
        foreach (Collider enemyInFreezeArea in sortedEnemiesInFreezeArea)
        {
            EnemyFreezable freezableEnemy = enemyInFreezeArea.GetComponent<EnemyFreezable>();
            if (freezableEnemy != null)
            {
                freezableEnemy.StartCoroutine("Freeze", freezeDuration);
                yield return new WaitForSeconds(0.01f);
            }
        }
        
        
    }

    private Collider[] SortEnemiesByDistanceFromShield(Collider[] objects)
    {
        List<Collider> sortedEnemies = new List<Collider>();
        foreach (Collider obj in objects)
        {
            // Check for enemy ... we want to freeze just an enemy
            if (obj.CompareTag("Enemy"))
            {
                sortedEnemies.Add(obj);
            }
        }

        sortedEnemies = sortedEnemies.OrderBy(x => Vector2.Distance(transform.position, x.transform.position)).ToList();

        return sortedEnemies.ToArray();
    }

    private void CalculateFreezeRadius()
    {
        // Freeze radius calculation based on shield health

        freezeRadius = (health / maxHealth) * maxFreezeRadius;
        if(freezeRadius < minFreezeRadius)
        {
            freezeRadius = minFreezeRadius;
        }
    }
    
}
