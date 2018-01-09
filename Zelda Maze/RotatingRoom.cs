//This script rotates the center block in Room 3 throughout the scene.
using UnityEngine;
using System.Collections;

public class RotatingRoom : MonoBehaviour {

	private float rotateSpeed = 1.0f;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
		gameObject.transform.Rotate (0,1,0);
	}
}
