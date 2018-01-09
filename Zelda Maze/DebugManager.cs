/* This system was originally created to test various functionality dealing with the AnimDataModel and the AnimStateMachine by implementing
 * what essentially became a debugger.
 * With testing now complete, all Debug.Log entries have been removed.
 */

using UnityEngine;
using System.Collections;

public class DebugManager : MonoBehaviour 
{

	public UnityEngine.AI.NavMeshAgent [] activeGuard;
	public int curGuard = 0;
    public AnimDataModel guard;

	// Use this for initialization
	void Start () 
	{
		ChangeColor (Color.yellow);
        SetGuard();
	}

    void SetGuard()
    {
        guard = activeGuard[curGuard].GetComponent<AnimDataModel>();
        guard.isActive = true;
        ChangeColor(Color.blue);
    }
	
	// Update is called once per frame
	void Update () 
	{
		//This block passes inputs and settings to the active guard.
        //Due to keyboard anti-ghosting, each keypress must be read independent of others and so must be if/else checks
		if( Input.GetKeyDown(KeyCode.LeftArrow) && curGuard > 0)
		{
            ChangeGuard("left");
		}

		else if( Input.GetKeyDown(KeyCode.RightArrow) && curGuard < activeGuard.Length -1)
		{
            ChangeGuard("right");
		}

		//Alternates between paused or unpaused states.
		else if( Input.GetKeyDown(KeyCode.Alpha1))
		{
            guard.isPaused = !guard.isPaused;
		}

		//Reverse direction of waypoint tracking.
		else if( Input.GetKeyDown(KeyCode.Alpha2))
		{
			System.Array.Reverse(guard.pathNodes);
		}

		//Sets the active guard to Angry state
		else if( Input.GetKeyDown(KeyCode.Alpha3))
		{
            guard.isAngry = !guard.isAngry;
		}

		//Alternate between PingPong or Circular array reading.
		else if( Input.GetKeyDown(KeyCode.Alpha4))
		{
			guard.pingPong = !guard.pingPong;
		}

	}
	
	void ChangeColor(Color x)
	{
		activeGuard[curGuard].GetComponentInChildren<SkinnedMeshRenderer>().material.color = x;
	}

    void ChangeGuard(string x)
	{
		ChangeColor (Color.yellow);
		guard.isActive = false;
		if(x == "left") { curGuard--; } 
        else if(x == "right"){ curGuard++; }
		SetGuard();
		ChangeColor(Color.blue);
	}
}
