using UnityEngine;
using System.Collections;

public class Showstopper : MonoBehaviour {

    int counter = 0;
	
    void Update()
    {
        if (counter > 0)
        {
            SendMessageUpwards("ChangeFull", true, SendMessageOptions.DontRequireReceiver);
        }
        else
        {
            SendMessageUpwards("ChangeFull", false, SendMessageOptions.DontRequireReceiver);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name.Contains("Puzzle"))
        {
            counter++;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.Contains("Puzzle"))
        {
            counter--;
        }
    }
}
