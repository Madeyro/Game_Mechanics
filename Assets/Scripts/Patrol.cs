using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public enum AIState
{
    Patrolling,
    Following,
    Attacking,
    Dead
}


[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AICharacterControl))]
public class Patrol : MonoBehaviour {

    public Transform playerTarget;
    public Transform[] patrolPoints;
    
    private AICharacterControl aiControl;

    public Transform currentTarget;
    public AIState currentState;

    // Use this for initialization
    void Start() {
        aiControl = GetComponent<AICharacterControl>();

        if (patrolPoints.Length > 0 && aiControl != null)
        {
            aiControl.SetTarget(currentTarget);
            SelectRandomWaypoint();
        }
	}

    void Update()
    {
        switch(currentState)
        {
            case AIState.Patrolling:
                if(Vector3.Distance(playerTarget.position, transform.position) < 5)
                {
                    currentTarget = playerTarget;
                    aiControl.SetTarget(currentTarget);
                    currentState = AIState.Following;
                }
                break;
            case AIState.Following:
                if (Vector3.Distance(playerTarget.position, transform.position) < 2)
                {
                    aiControl.agent.isStopped = true;
                    currentState = AIState.Attacking;
                }
                if (Vector3.Distance(playerTarget.position, transform.position) > 6)
                {

                    currentState = AIState.Patrolling;
                }
                break;
            case AIState.Attacking:
                if (Vector3.Distance(playerTarget.position, transform.position) > 4)
                {
                    currentState = AIState.Following;
                    aiControl.agent.isStopped = false;
                }
                break;
            case AIState.Dead:
                break;
        }
    }
	
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Waypoint" && other.transform == currentTarget)
        {
            SelectRandomWaypoint();
        }
    }

    public void SelectRandomWaypoint()
    {
        Transform nextTarget;
        do
        {
            nextTarget = patrolPoints[Random.Range(0, patrolPoints.Length)];
        }
        while (currentTarget == nextTarget);

        currentTarget = nextTarget;
        aiControl.SetTarget(currentTarget);
    }
}