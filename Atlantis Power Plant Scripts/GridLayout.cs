using UnityEngine;
using System.Collections;

public class GridLayout : MonoBehaviour {

	[SerializeField]
	GameObject GridTrigger;
	private Transform runwayParent;

	// Use this for initialization
	void Start () {
		runwayParent = new GameObject("Runway").transform;
		for (int x = 0; x < 15; x++) {
			for(int z = 0; z < 3; z++){
				GameObject setGridRight = Instantiate(GridTrigger, new Vector3(x + 3,-0.45f,z - 1), Quaternion.identity) as GameObject;
				GameObject setGridLeft = Instantiate(GridTrigger, new Vector3(x - 17,-0.45f,z - 1), Quaternion.identity) as GameObject;

				setGridLeft.transform.SetParent(runwayParent);
				setGridRight.transform.SetParent(runwayParent);
            }
		}
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
