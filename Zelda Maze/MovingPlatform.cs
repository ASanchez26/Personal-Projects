//This implements a moving platform that moves about in room 2, moving between each of the ramps and ledges.
//Upon reaching the exit ramp for the room, the platform will stop moving.

using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour 
{

	Vector3 movement = Vector3.zero;
	bool moving = true;

	// Use this for initialization
	void Start () 
    {
	}
	
	// Update is called once per frame
	void Update () 
    {
		movement.x = Time.deltaTime * 1.0f;
		if(moving)
		{
			gameObject.transform.Translate (movement);
		}
	}

	void OnCollisionEnter (Collision other)
	{
        //If it hits the final ramp, stop moving and reposition to center on the main ramp.
		if (other.collider.name == "ExitRamp")
		{
			moving = false;
			this.gameObject.transform.localPosition = new Vector3 (1.38f, 2.05f, -6.62f);
		}
        //Otherwise, turn 90 degrees and keep moving.
		else
		{
			this.gameObject.transform.Rotate(0,-90,0);
		}
	}
}
