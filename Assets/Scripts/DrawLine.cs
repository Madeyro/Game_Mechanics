using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour {

    public Transform AnchorTransform;
    private LineRenderer lr;

    private void Start()
    {
        lr = GetComponent<LineRenderer>();
        if(lr != null)
        {
            lr.SetPosition(0, AnchorTransform.position);
        }
    }

    // Update is called once per frame
    void Update () {
		if(lr != null)
        {
            lr.SetPosition(1, transform.position);
        }
	}
}
