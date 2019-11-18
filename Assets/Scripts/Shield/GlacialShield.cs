using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

public class GlacialShield : Shield
{
    public float maxFreezeRadius;
    public float minFreezeRadius;
    public float freezeDuration;
    public ParticleSystem freezeParticles;

    private float freezeRadius;
    private int freezeObjectMask;
    private float dividingRatio = 10f;


    protected override void Awake()
    {
        base.Awake();
        CalculateFreezeRadius();
        freezeObjectMask = LayerMask.GetMask("Enemy");
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q) && !isActive)
        {
            ShieldActivation();
        }
    }

    protected override IEnumerator RunTimer()
    {
        // Start shield duration => after that time deactivate the shield
        float duration = shieldDuration;
        shieldUI.SetTimerText(duration.ToString());
        shieldUI.ActivateTimer();

        // Wait duration time and redraw UI timer
        while (duration > 0)
        {
            duration -= 1f;
            // Shield duration can be floating point number ... so we must consider that situation (duration 0.85f - 1f = -0.15f)
            if (duration < 0)
            {   
                yield return new WaitForSeconds(Mathf.Abs(duration));
                duration = 0;
            }
            else
            {
                yield return new WaitForSeconds(1f);
            }
            shieldUI.SetTimerText(duration.ToString());
        }
        
        ShieldDeactivation();
        FreezeEnemies();
    }

    private void FreezeEnemies()
    {
        // Freeze all enemies in particular area
        CalculateFreezeRadius();

        // Set particles lifeTime
        float particlesLifeTime = SetParticles();
        freezeParticles.Play();

        // Get all enemies in freeze area
        Collider[] objectsInFreezeArea = Physics.OverlapSphere(transform.position, freezeRadius, freezeObjectMask);
        // Sort enemies by distance
        List<Collider> sortedEnemiesInFreezeArea = SortEnemiesByDistanceFromShield(objectsInFreezeArea.ToList());

        StartCoroutine(Freeze(sortedEnemiesInFreezeArea, particlesLifeTime));
    }

    private IEnumerator Freeze(List<Collider> sortedEnemiesInFreezeArea, float particlesLifeTime)
    {
        // Freeze simulation time
        float time = 0;
        int numOfEnemies = sortedEnemiesInFreezeArea.Count;
        for (int i = 0; i < numOfEnemies; i++)
        {
            // Try to sort enemies again for more precise result
            sortedEnemiesInFreezeArea = SortEnemiesByDistanceFromShield(sortedEnemiesInFreezeArea);

           // Player to enemy distance
            float distance = Vector3.Distance(transform.position, sortedEnemiesInFreezeArea.First().transform.position);
            // Compute time when the enemy should be freezed
            float currentTime = (distance * particlesLifeTime) / freezeRadius;
            // Calculate the time that left to freeze the enemy
            float freezeTime = currentTime - time;
            // FreezeTime has to be positive value
            freezeTime = freezeTime >= 0 ? freezeTime : 0;
            Debug.Log("FreezeTIME: "+ freezeTime);
            // Change freeze simulation time
            time = currentTime;

            EnemyFreezable freezableEnemy = sortedEnemiesInFreezeArea.First().GetComponent<EnemyFreezable>();
            if (freezableEnemy != null)
            {
                // Freeze the enemy
                yield return new WaitForSeconds(freezeTime);
                freezableEnemy.StartCoroutine("Freeze", freezeDuration);
            }
            sortedEnemiesInFreezeArea.Remove(sortedEnemiesInFreezeArea.First());
        }
    }

    private float SetParticles()
    {
        // Set lifeTime and duration of the particles and returns lifeTime

        ParticleSystem.MainModule newParticleMain = freezeParticles.main;
        newParticleMain.duration = maxFreezeRadius/dividingRatio;

        ParticleSystem.MinMaxCurve x = freezeParticles.main.startLifetime;
        x.constant = freezeRadius/dividingRatio;
        newParticleMain.startLifetime = x;

        return x.constant;
    }

    private List<Collider> SortEnemiesByDistanceFromShield(List<Collider> objects)
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

        return sortedEnemies;
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
