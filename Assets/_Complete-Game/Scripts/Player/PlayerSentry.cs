using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CompleteProject
{
    public class PlayerSentry : MonoBehaviour
    {
        public Sentry sentryGun;
        public Text sentryTipText;
        public float Cooldown { get; set; } = 0.0f;

        // Update is called once per frame
        void Update()
        {
            if (Cooldown > 0.0f)
            {
                Cooldown -= Time.deltaTime;
                sentryTipText.text = "Sentry cooldown: " + Mathf.CeilToInt(Cooldown);
            }
            else
                sentryTipText.text = (sentryGun.IsDeployed) ? "Press T near the sentry to pick it up." : "Press T to place the sentry.";

            if (Input.GetButtonDown("PlaceSentry"))
            {
                if (sentryGun.IsDeployed && Vector3.Magnitude(sentryGun.transform.position - transform.position) < 4.0f)
                {
                    sentryGun.PickUp();
                }
                else if (!sentryGun.IsDeployed)
                {
                    if (Cooldown > 0.0f)
                    {
                        return;
                    }

                    bool status = sentryGun.Deploy(transform.position + transform.forward + (Vector3.up * 0.5f), transform.rotation);
                    if (!status)
                    {
                        // TODO write error message
                    }
                }
            }
        }
    }
}
