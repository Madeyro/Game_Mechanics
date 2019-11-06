using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PushNavigationButton : MonoBehaviour {

    public Barrier[] AssociatedBarriers;
    public NavMeshSurface navMeshSurface;

    private bool playerIn = false;
    private bool barriersOn = false;

    private void Update()
    {
        if (!playerIn)
            return;

        if (Input.GetMouseButtonUp(0))
            ToogleBarriers();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (playerIn)
            return;

        playerIn = other.tag == "Player";
    }

    private void OnTriggerExit(Collider other)
    {
        if (!playerIn)
            return;

        playerIn = !(other.tag == "Player");
    }

    private void ToogleBarriers()
    {
        barriersOn = !barriersOn;

        if (barriersOn)
            foreach (Barrier b in AssociatedBarriers)
                b.SwitchOn();
        else
            foreach (Barrier b in AssociatedBarriers)
                b.SwitchOff();

        //TASK: Rebuild navmesh when barrier is switched on/off
        navMeshSurface.BuildNavMesh();
    }
}
