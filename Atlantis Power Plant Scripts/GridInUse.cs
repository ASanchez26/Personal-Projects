using UnityEngine;
using System.Collections;

public class GridInUse : MonoBehaviour {

	[SerializeField] Transform[] Turret;
    PowerCellStats ChargeLevel;
    public bool canBuild;
	public bool InUse = false;
	public int turretSelect;
	private Color defaultColor;
    public Collider nested;
    public Transform ghostWall, ghostBomb;

    public delegate void OnSpawnTurret();
    public static event OnSpawnTurret SpawnTurretEvent;

    void Start ()
    {
		canBuild = false;
		defaultColor = this.GetComponent<Renderer>().material.color;
        ChargeLevel = GameObject.Find("PowerCellTotalPower").GetComponent<PowerCellStats>();
    }


    void FixedUpdate()
    {
        if (nested == null)
            InUse = false;
    }


    void OnTriggerStay(Collider other)
    {
        if (other != null)
        {
            InUse = true;
            nested = other;
        }
        else
            InUse = false;
	}

    void OnMouseEnter()
	{
		if(!InUse)
		{
			this.GetComponent<Renderer>().material.color = new Color(0,0,0,.25f);
		}
        if(canBuild)
        {
            if(turretSelect == 1)
            {
                ghostWall.transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            }
            else if(turretSelect == 3)
            {
                ghostBomb.transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            }
        }
	}

	void OnMouseExit()
	{
		this.GetComponent<Renderer>().material.color = defaultColor;
        ghostWall.transform.position = new Vector3(transform.position.x, transform.position.y -5, transform.position.z);
        ghostBomb.transform.position = new Vector3(transform.position.x, transform.position.y -5, transform.position.z);
    }

    //OnMouseClick, spawn selected defense on the clicked space
    //Auto-orient to parent transform.
	void OnMouseUpAsButton()
	{
		if (!InUse && canBuild)
		{
			Transform spawnTurret = Instantiate(Turret[turretSelect - 1], new Vector3(transform.position.x, transform.position.y +1f, transform.position.z), transform.rotation) as Transform;
            
           spawnTurret.transform.parent = gameObject.transform;
            ChargeLevel.energy -= (turretSelect * 10);
            if(turretSelect == 3)
            {
                ChargeLevel.energy -= 20;
            }
            else if (SpawnTurretEvent != null)
            {
                SpawnTurretEvent();
            }

            GridInUse [] runwayBlocks = FindObjectsOfType(typeof(GridInUse)) as GridInUse[];
			foreach(GridInUse block in runwayBlocks)
			{
				block.canBuild = false;
			}
        }
	}

	public void ChooseTurret (int num)
	{
		turretSelect = num;
	}
}
