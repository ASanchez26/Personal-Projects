/*
     This is used to create and maintain the list of NavPoints for the Actor to move toward during their Patrol.
 */  

using UnityEngine;
using System.Collections;

public class NavAgent : MonoBehaviour {

	UnityEngine.AI.NavMeshAgent myNavAgent;
	NavPoint[]  myNavPoints;
	int	navIndex = 0;

	// Use this for initialization
	void Start () 
	{
		myNavAgent = GetComponent("NavMeshAgent") as UnityEngine.AI.NavMeshAgent;
		myNavPoints = FindObjectsOfType<NavPoint> ();
		navIndex = 0;
		FindDestination ();
	}
	
	// Update is called once per frame
	void Update () 
    {
	}

    //Set the next position for the Actor to move towards
	void FindDestination()
	{
		Vector3 newTravelPosition = myNavPoints [navIndex].transform.position;
		myNavAgent.SetDestination (newTravelPosition);
	}

    //Once an object enters the collider of the NavPoint, the next NavPoint is selected.
	void OnTriggerEnter()
	{
		++navIndex;
		if (navIndex >= myNavPoints.Length)
			navIndex = 0;

		FindDestination ();
    }
}
