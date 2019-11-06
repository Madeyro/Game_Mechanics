using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour {

    public Barrier[] Barriers;

    private int objectsIn = 0;

    private void OnTriggerEnter(Collider other)
    {
        objectsIn++;
        //TASK: switch all barriers off
        foreach (Barrier b in Barriers)
            b.SwitchOff();
    }

    private void OnTriggerExit(Collider other)
    {
        objectsIn--;
        //TASK: switch all barriers on when necessary
        if(objectsIn == 0)
            foreach (Barrier b in Barriers)
                b.SwitchOn();

    }
}
