using UnityEngine;
using System.Collections;

public class DrawLine : MonoBehaviour {

	RaycastHit hit = new RaycastHit();
	public GameObject hitPoint;
	float distanceToBlock;
    LayerMask layerMask;
	// Use this for initialization
	void Start () {
        layerMask = ~(1 << 9 | 1 << 2);
		Vector3 down = transform.TransformDirection(Vector3.down);	
		LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = (Material)Resources.Load("Linecast 2");
        lineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off; 
		lineRenderer.SetWidth(.15f, .15f);
        lineRenderer.material.color = Color.red;
    }
	
	// Update is called once per frame
	void Update () {
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, layerMask))
        {
            if (hitPoint != hit.collider.gameObject)
                hitPoint = hit.collider.gameObject;
        }
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.SetPosition(0, this.transform.position);
        lineRenderer.SetPosition(1, new Vector3(transform.position.x, -1, -2.38f));
        //lineRenderer.SetPosition (1,new Vector3(this.transform.position.x, hitPoint.transform.position.y, hitPoint.transform.position.z));
	}
}
