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
        List<Collider> enemiesInFreezeArea = GetEnemies(objectsInFreezeArea.ToList());

        Freeze(enemiesInFreezeArea, particlesLifeTime);
    }

    private void Freeze(List<Collider> enemiesInFreezeArea, float particlesLifeTime)
    {
        Vector3 originFreezePosition = transform.position;
        // Freeze simulation time
        float time = 0;

        foreach(Collider enemyInFreezeArea in enemiesInFreezeArea)
        {
            // Player to enemy distance
            float distance = Vector3.Distance(originFreezePosition, enemyInFreezeArea.transform.position);
            
            // Compute time when the enemy should be freezed
            time = (distance * particlesLifeTime) / freezeRadius;

            EnemyFreezable freezableEnemy = enemyInFreezeArea.GetComponent<EnemyFreezable>();
            if (freezableEnemy != null)
            {
                // Freeze the enemy
                freezableEnemy.StartCoroutine(freezableEnemy.Freeze(freezeDuration, time));
            }
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

    private List<Collider> GetEnemies(List<Collider> objects)
    {
        List<Collider> enemies = new List<Collider>();
        foreach (Collider obj in objects)
        {
            // Check for enemy ... we want to freeze just an enemy
            if (obj.CompareTag("Enemy"))
            {
                enemies.Add(obj);
            }
        }

        return enemies;
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
