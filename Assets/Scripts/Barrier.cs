using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour {

    public Color OffColor = Color.green;
    public Color OnColor = Color.red;
    public bool StartOff = true;

    private void Start()
    {
        if (StartOff)
            SwitchOff();
        else
            SwitchOn();
    }

    public void SwitchOn()
    {
        Renderer rend = GetComponent<Renderer>();

        rend.material.SetColor("_Color", OnColor);
        //TASK: Make sure you can't go through barrier barrier is switched on

        MeshCollider col = GetComponent<MeshCollider>();
        if (col != null)
            col.enabled = true;
    }

    public void SwitchOff()
    {
        Renderer rend = GetComponent<Renderer>();

        rend.material.SetColor("_Color", OffColor);

        //TASK: Make sure you can go through barrier barrier is switched off
        MeshCollider col = GetComponent<MeshCollider>();
        if (col != null)
            col.enabled = false;
    }
}
