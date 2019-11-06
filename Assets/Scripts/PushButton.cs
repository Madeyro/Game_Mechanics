using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushButton : MonoBehaviour {


    public Animator PushAnimation;
    public Rigidbody BallRigidbody;
    private bool PlayerIn = false;

    private void Update()
    {
        if (!PlayerIn)
            return;

        if (Input.GetAxis("Fire1") != 0)
            FireBall();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (PlayerIn)
            return;

        PlayerIn = other.tag == "Player";
    }

    private void OnTriggerExit(Collider other)
    {
        if (!PlayerIn)
            return;

        PlayerIn = !(other.tag == "Player");
    }

    private void FireBall()
    {
        PushAnimation.SetTrigger("Pushed");
        BallRigidbody.isKinematic = false;
    }
}
