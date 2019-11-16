using UnityEngine;
namespace CompleteProject
{
    public class HealOnTrigger : MonoBehaviour
    {
        public float restoreAmount = 0.3f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                PlayerHealth hp = other.gameObject.GetComponent<PlayerHealth>();
                if (hp.currentHealth != 100)
                {
                    hp.Heal(restoreAmount);
                    Destroy(this.gameObject);
                }
            }
        }
    }
}