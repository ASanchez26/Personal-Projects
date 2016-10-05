using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MatchCheck : MonoBehaviour {

    BlockMove parentBlock;
    RunwayManager Color;
    public GameObject neighbor = null;
    public GameObject otherBlock = null;
    int activeColor = 0;

//Assign list value to blocks based on color tags
	void Start ()
    {
        parentBlock = transform.parent.GetComponent<BlockMove>();
        Color = GameObject.FindObjectOfType<RunwayManager>();
        switch (this.transform.parent.tag)
        {   
            case "GreenBlock":
                activeColor = 0;
                break;
            case "RedBlock":
                activeColor = 1;
                break;
            case "BlueBlock":
                activeColor = 2;
                break;
            case "BlackBlock":
                activeColor = 3;
                break;
            default:
                break;
        }
	}


    //Adds new blocks to a list used to check for Match-3s
    //In case of a match, increment list values for that side and color
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == this.transform.parent.tag)
        {
            neighbor = other.gameObject;
            //            parentBlock.Blocks.Add(other);
        }

        if (other.tag.Contains("Block"))
            otherBlock = other.gameObject;
    }
    
//Removed a block from lists if it is destroyed.
    void OnTriggerExit(Collider other)
    {
        if (neighbor == other.gameObject)
            neighbor = null;
    }

    void DestroyOtherBlocks()
    {
        if (otherBlock != null)
        Destroy(otherBlock, .3f);
    }

}
