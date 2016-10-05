using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DestroyNeighbors : MonoBehaviour {

    [SerializeField] List<Collider> neighbors = new List<Collider>();

    void OnTriggerEnter (Collider other)
    {
        if (other.transform.name.Contains("Puzzle"))
        {
            if (!neighbors.Contains(other))
            {
                neighbors.Add(other);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.transform.name.Contains("Puzzle"))
        {
            if (neighbors.Contains(other))
            { 
                neighbors.Remove(other);
            }
        }
    }

    public void Boom()
    {
        foreach (Collider block in neighbors)
        {
            BlockMove health = block.gameObject.GetComponent<BlockMove>();
            health.counter ++;
        }
    }

}
