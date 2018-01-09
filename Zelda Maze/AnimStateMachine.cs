using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class AnimStateMachine : MonoBehaviour {

    //Defines the states the guards may be in throughout the scene playing.
	private enum GuardStates
	{
		PATROL,
		IDLE,
		PAUSE,
		ENTERANGRY,
		ANGRY,
		EXITANGRY,
		NUM_STATES
	}

    [SerializeField]
    GuardStates curState;
    [SerializeField]
	GuardStates oldState;

    //Create the handler for the finite state machine
	Dictionary <GuardStates, Action> fsm = new Dictionary<GuardStates, Action>();

	Animator anim;
	AnimDataModel Actor;
	DebugManager Manage;

    [SerializeField]
    private float Timer = 0.0f;
    [SerializeField]
	private bool idling = false;

	// Use this for initialization
	void Start () 
	{
		fsm.Add (GuardStates.PATROL, PatrolState);
		fsm.Add (GuardStates.IDLE, IdleState);
		fsm.Add (GuardStates.PAUSE, PauseState);
		fsm.Add (GuardStates.ENTERANGRY, EnterAngryState);
		fsm.Add (GuardStates.ANGRY, AngryState);
		fsm.Add (GuardStates.EXITANGRY, ExitAngryState);

		anim = GetComponent<Animator> ();
		Actor = GetComponent<AnimDataModel> ();
		Manage = GameObject.Find ("DebugManager").GetComponent<DebugManager> ();
        curState = GuardStates.IDLE;
        oldState = GuardStates.IDLE;
//        if (Actor.isActive)
//			{
              SetState(GuardStates.PATROL);
//				PatrolState ();
//			}
        
	}
	
	// Update is called once per frame
	void Update () 
	{
        //This needs to be corrected so that curState is only called if it's not already running.
        if (curState != oldState)
        {
            fsm[curState].Invoke();
        }

	}

	void SetState(GuardStates nextState)
	{
		if( nextState != curState)
		{
            oldState = curState;
			curState = nextState;
		}
	}

	//This state will set the active guard to walking between the various active path nodes.
	void PatrolState()
	{
		CheckForActive ();
		CheckForAngry ();
		CheckForPause ();

		anim.SetFloat("speed", 1.0f);

		Timer = 0.0f;
		if (idling)
		{
			anim.SetFloat("speed", 0.0f);
			SetState(GuardStates.IDLE);
		}		
		else
		{
			Vector3 nextPathNode = Actor.pathNodes[Actor.pathIndex].transform.position;
			Actor.navAgent.SetDestination (nextPathNode);
		}
	}

	//this function will pause the guard at each given path node for 2 seconds, then continue onward to the next.
	void IdleState()
	{
		CheckForActive ();
		CheckForAngry ();
		CheckForPause ();

		Actor.navAgent.isStopped = true;
		Timer += Time.deltaTime;
		if(Timer > 2.0f || Actor.isAngry == true)
		{
			idling = false;
            Actor.navAgent.isStopped = false;
			SetState (GuardStates.PATROL);
		}
	}

	//this function will pause the active guard in place for the duration of the state
	void PauseState()
	{
		Actor.navAgent.isStopped = true;
		anim.SetFloat("speed", 0.0f);
		CheckForActive ();
		if(!Actor.IsPaused())
		{
			Actor.navAgent.isStopped = false;
			SetState (oldState);
		}
	}

	//this state will enrage the guard and changes its coloration to red
	void EnterAngryState()
	{
		CheckForActive ();
//		float angryScale = Mathf.Lerp (transform.localScale.y, (transform.localScale.y * 1.3f), 1.0f);
		Manage.activeGuard[Manage.curGuard].GetComponentInChildren<SkinnedMeshRenderer>().material.color  = Color.Lerp (Color.yellow, Color.red, 1.0f);
        anim.SetFloat("speed", 2.0f);
		SetState (GuardStates.ANGRY);
	}

	//while enraged, the guard will seek the nearby alarm to trigger.
	void AngryState()
	{
		CheckForActive ();
		CheckForPause ();

		Vector3 Alarm = Actor.AlarmNode[0].transform.position;
		Actor.navAgent.SetDestination (Alarm);
		if(!Actor.IsAngry())
		{
			SetState (GuardStates.EXITANGRY);
		}

	}

	//after triggering the alarm, the guard will transition into this state then back to the state it was in before becoming angry
	void ExitAngryState()
	{
//		CheckForActive ();
//		float angryScale = Mathf.Lerp (transform.localScale.y, (transform.localScale.y / 1.3f), 1.0f);
        if (Actor.isActive)
        {
            Manage.activeGuard[Manage.curGuard].GetComponentInChildren<SkinnedMeshRenderer>().material.color = Color.Lerp(Color.red, Color.blue, 1.0f);
        }
        else
        {
            Manage.activeGuard[Manage.curGuard].GetComponentInChildren<SkinnedMeshRenderer>().material.color = Color.Lerp(Color.red, Color.yellow, 1.0f);
        }
		Actor.isAngry = false;
        anim.SetFloat("speed", 1.0f);
		SetState (GuardStates.PATROL);
	}

	//ensures the guard only stops when walking into the appropriate nodes in a list
	void OnTriggerEnter(Collider other)
	{		
		if ((other.gameObject.transform == Actor.pathNodes[Actor.pathIndex]) && (Actor.isAngry == false))
		{
			idling = true;
            anim.SetFloat("speed", 0.0f);
		}
		else if (other.gameObject.name == "AlarmNode")
		{
			SetState (GuardStates.EXITANGRY);
		}
	}

	void CheckForActive ()
	{
		if (Actor.IsActive())
		{
			SetState(curState);
		}
	}

	void CheckForPause()
	{
		if (Actor.isPaused)
		{
			SetState(GuardStates.PAUSE);
		}
	}

	void CheckForAngry()
	{
		if(Actor.isAngry)
		{
			SetState (GuardStates.ENTERANGRY);
		}
	}
}
