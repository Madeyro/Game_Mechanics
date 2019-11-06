using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PushLevelButton : MonoBehaviour {
    public string levelToLoad;

    private bool playerIn = false;

    private void Update()
    {
        if (!playerIn)
            return;

        if (Input.GetMouseButtonUp(0))
            LoadNextLevel();

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

    private void LoadNextLevel()
    {
        //TASK: Load level defined in the "levelToLoad" attribute.

    }
}
