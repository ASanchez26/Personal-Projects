/* Flickers the color of the alarm until it is touched by another object
 * To enforce specific constraints about the object collision, use other.gameObject.transform 
 * Check for the transform type of the other object.
 */

using UnityEngine;
using System.Collections;

public class AlarmColor : MonoBehaviour 
{
    //SerializeField allows Unity to publicly display and edit a private variable within the Unity inspector. 
	[SerializeField]
    Color objColor;

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
    {
        this.GetComponent<Renderer>().material.color = objColor;
        AlarmIdle();
	}

    //When any object enters the collider for the alarm, run the included method(s).
	void OnTriggerEnter(Collider other)
	{
		TriggerAlarm ();
	}

    //Decoupled from OnTriggerEnter in case more functionality is added later, such as sounds.
	void TriggerAlarm()
	{
		objColor = Color.yellow;
	}

    //Run this every frame to flicker the alarm nodes while they are not triggered.
    void AlarmIdle()
    {
        if (objColor != Color.yellow)
        {
            objColor = Color.Lerp(Color.white, Color.red, Mathf.PingPong(Time.time,1));
        }
    }
}
