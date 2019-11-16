using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CompleteProject
{

    [RequireComponent(typeof(MeshRenderer))]
    public class Sentry : MonoBehaviour
    {
        public bool IsDeployed { private set; get; } = false;
        private MeshRenderer mr;
        private CapsuleCollider col;
        private SphereCollider scol;
        public Transform barrelEnd;

        const float range = 10.0f;
        const float rotationSpeed = 90.0f;
        private EnemyHealth target;

        public List<EnemyHealth> enemiesInRange = new List<EnemyHealth>();

        public int damagePerShot = 40;
        public float timeBetweenBullets = 0.15f;
        public float bulletRange = 100f;

        float timer;
        Ray shootRay = new Ray();
        RaycastHit shootHit;
        int shootableMask;
        public ParticleSystem gunParticles;
        public LineRenderer gunLine;
        public AudioSource gunAudio;
        public Light gunLight;
        float effectsDisplayTime = 0.2f;

        void Start()
        {
            shootableMask = LayerMask.GetMask("Shootable");
            mr = GetComponent<MeshRenderer>();
            col = GetComponent<CapsuleCollider>();
            scol = GetComponent<SphereCollider>();
            scol.radius = range;
        }

        public void PickUp()
        {
            foreach (EnemyMovement enm in EnemyHealth.enemies)
            {
                enm.TargetSentry = false;
            }

            DisableEffects();

            IsDeployed = false;
            mr.enabled = false;
            col.enabled = false;
            target = null;
        }

        public bool Deploy(Vector3 spawnPosition, Quaternion spawnRotation)
        {
            transform.parent.position = new Vector3(spawnPosition.x, 0, spawnPosition.z);
            transform.parent.rotation = spawnRotation;

            for (float angle = 0.0f; angle < 361.0f; angle += 60.0f)
            {
                Vector3 dir = Quaternion.AngleAxis(angle, Vector3.up) * transform.forward;
                if (Physics.Raycast(transform.position + dir, -dir, 1.0f, shootableMask)) return false;
            }

            GetComponent<SentryHealth>().RestoreHealth();
            IsDeployed = true;
            mr.enabled = true;
            col.enabled = true;
            return true;
        }

        public void OnTriggerEnter(Collider c)
        {
            if (c.gameObject.layer == 9)
            {
                EnemyHealth comp = c.GetComponent<EnemyHealth>();
                if (comp != null)
                {
                    enemiesInRange.Add(comp);
                    comp.Died += new EnemyHealth.EnemyDeathEventHandler(this.OnEnemyDeath);
                }
            }
        }
        public void OnTriggerExit(Collider c)
        {
            if (c.gameObject.layer == 9)
            {
                EnemyHealth comp = c.GetComponent<EnemyHealth>();
                if (comp != null)
                {
                    enemiesInRange.Remove(comp);
                    comp.Died -= new EnemyHealth.EnemyDeathEventHandler(this.OnEnemyDeath);
                }
            }
        }

        protected virtual void OnEnemyDeath(GameObject source)
        {
            EnemyHealth comp = source.GetComponent<EnemyHealth>();
            if (comp != null)
            {
                enemiesInRange.Remove(comp);
                comp.Died -= new EnemyHealth.EnemyDeathEventHandler(OnEnemyDeath);
            }
        }


        void Update()
        {
            if (!IsDeployed)
            {
                return;
            }

            timer += Time.deltaTime;

            if (timer >= timeBetweenBullets * effectsDisplayTime)
            {
                DisableEffects();
            }

            if (!target || target.IsDead)
            {
                SortedList<float, EnemyHealth> validTargets = new SortedList<float, EnemyHealth>();
                enemiesInRange.RemoveAll(o => o == null);
                foreach (EnemyHealth comp in enemiesInRange)
                {
                    Transform trans = comp.transform;
                    if (Vector3.Magnitude(transform.position - trans.position) > range * range || !CheckVisibility(trans))
                    {
                        continue;
                    }
                    float tangle = Mathf.Abs(Vector3.SignedAngle(transform.forward, trans.position - transform.position, Vector3.up));
                    if (!validTargets.ContainsKey(tangle))
                    {
                        validTargets.Add(tangle, comp);
                    }
                }
                if (validTargets.Count > 0)
                {
                    target = validTargets.Values[0];
                }
                else
                {
                    target = null;
                    return;
                }
            }
            if (Vector3.Magnitude(transform.position - target.transform.position) > range * range)
            {
                target = null;
                return;
            }

            Vector3 dirVector = target.transform.position - transform.position;
            dirVector.y = 0;

            Quaternion newRotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(dirVector, Vector3.up), Time.deltaTime * rotationSpeed);
            transform.rotation = newRotation;

            float angle = Mathf.Abs(Vector3.SignedAngle(transform.forward, dirVector, Vector3.up));

            if (angle < 10.0f && timer >= timeBetweenBullets && Time.timeScale != 0)
            {
                Shoot();
            }
        }
        public void DisableEffects()
        {
            gunLine.enabled = false;
            gunLight.enabled = false;
        }

        private bool CheckVisibility(Transform obj)
        {
            shootRay.origin = transform.position;
            Vector3 vec = range * Vector3.Normalize(obj.position - transform.position);
            vec.y = 0;
            shootRay.direction = vec;

            if (Physics.Raycast(shootRay, out shootHit, range, shootableMask))
            {
                return shootHit.collider.transform == obj;
            }
            return false;
        }

        private void Shoot()
        {
            timer = 0f;

            gunAudio.Play();

            gunLight.enabled = true;

            gunParticles.Stop();
            gunParticles.Play();

            gunLine.enabled = true;
            gunLine.SetPosition(0, barrelEnd.position);

            shootRay.origin = barrelEnd.position;
            shootRay.direction = barrelEnd.forward;

            if (Physics.Raycast(shootRay, out shootHit, range, shootableMask))
            {
                EnemyHealth enemyHealth = shootHit.collider.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damagePerShot, shootHit.point, gameObject);
                }
                gunLine.SetPosition(1, shootHit.point);
            }
            else
            {
                gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
            }
        }
    }
}