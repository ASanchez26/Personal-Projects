using UnityEngine;
using System.Collections;

public class BlockFall : MonoBehaviour {

	[SerializeField] Vector2 curLoc;
	BlockSpawn lists;
	private int counter = 0;
	private int thisList;
    private bool canQuickDrop;

	// Use this for initialization
	void Start () {
        canQuickDrop = true;
		lists = GameObject.FindObjectOfType(typeof(BlockSpawn)) as BlockSpawn;
		curLoc = this.transform.position;
		StartCoroutine("ResumeFall");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        QuickDrop();
		switch (counter)
		{
		case 0:
			this.GetComponent<Renderer>().material.color = Color.green;
			break;

		case 1:
			this.GetComponent<Renderer>().material.color = Color.yellow;
			break;

		case 2:
			this.GetComponent<Renderer>().material.color = Color.red;
			break;

		case 3:
                this.gameObject.layer = LayerMask.NameToLayer("BlockDrop");
			this.GetComponent<MeshRenderer>().enabled = false;
			lists.Lists[thisList].Add(new Vector2(curLoc.x, curLoc.y));
			Destroy (this.gameObject, 0.5f);
			break;

		default:
			break;
		}
	}

	void OnCollisionEnter(Collision other)
	{
        if (other.collider.gameObject.tag == "Enemy" ||
            other.collider.gameObject.tag == "RangedEnemy")
        {
            counter++;
            other.gameObject.GetComponent<Collider>().enabled = false;
        }
        else
        {
            GetComponent<LineRenderer>().enabled = false;
        }
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.name == "PowerCell_3DBase")
		{
			thisList = lists.GetComponent<BlockSpawn>().activeList;
		}
	}

	IEnumerator ResumeFall()
	{
		yield return new WaitForSeconds (2.5f);
		this.GetComponent<Rigidbody>().isKinematic = false;
	}

    void QuickDrop()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (canQuickDrop)
            {
                canQuickDrop = false;
                rb.isKinematic = false;
                rb.AddForce(Vector3.down * 6, ForceMode.VelocityChange);
            }
        }
    }
}
