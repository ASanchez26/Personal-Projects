//Rotates the moving platform 90 degrees when it collides with a block.

using UnityEngine;
using System.Collections;

public class MovePlatform : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision other)
	{
		other.gameObject.transform.Rotate (0,-90, 0);
	}
}
