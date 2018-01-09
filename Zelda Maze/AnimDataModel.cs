/* This script works with the AnimStateMachine to handle the behavior of the guards which have the script applied to them
 * The AnimDataModel does not handle any behaviors on its own, it provides the AnimStateMachine with information about the elements of AnimDataModel that can be referenced and editted. 
 * The ease of adding properties to support new states makes data models perfect for this finite state machine.
 * These values were all set to public for testing purposes, but would be switched to [SerializeField] when being published.
 * [SerializeField] allows a private variable to be viewed and edited while in the Unity inspector, so elements can be dragged and dropped into the appropriate property.
 */

using UnityEngine;
using System.Collections;

public class AnimDataModel : MonoBehaviour {

	public UnityEngine.AI.NavMeshAgent navAgent;
	public Transform [] pathNodes = new Transform[3];
	public Transform [] AlarmNode = new Transform[1];
	public int pathIndex = 0;

	public bool isActive = false;
	public bool isPaused = false;
	public bool isAngry = false;
	public bool pingPong = false;

	// Use this for initialization
	void Start () 
	{
		navAgent = GetComponent ("NavMeshAgent") as UnityEngine.AI.NavMeshAgent;
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

    //Implement pathing behavior on node contact depending on whether or nor PingPong is active
	void OnTriggerEnter(Collider other)
	{
		if (other.transform.position == pathNodes[pathIndex].transform.position)
		{
			if (pathIndex == pathNodes.Length -1)
			{
				if (pingPong)
				{
					System.Array.Reverse(pathNodes);
				}
				pathIndex = 0;
				if (pingPong)
				{
					pathIndex++;
				}
			}
			else
			pathIndex++;
		}
	}

    //Get methods for model states.
	public bool IsActive()
	{
		return isActive;
	}

	public bool IsPaused()
	{
		return isPaused;
	}

	public bool IsAngry()
	{
		return isAngry;
	}
}
