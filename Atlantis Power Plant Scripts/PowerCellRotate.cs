using UnityEngine;
using System.Collections;

public class PowerCellRotate : MonoBehaviour {

	GameObject powerCore;
	//Color defaultColor;
	Color disruptColor;

	//Color[] defaultColors;

	public GameObject disruptParticles;

    PowerCellStats healthLevel;
    WinLoseGameState LoseCheck;

    public bool disrupted;
    public float health;
    public float rotateSpeed = 8;
    public float dropTimer;
    float degree = 0;
    float timer = 0;


	[SerializeField] Renderer[] disruptableComponents;

    //------------------------------------
    // Delegation
    public delegate void OnRotate();
    public static event OnRotate RotateEvent;
    //------------------------------------

    //TEST CASE: REMOVE AFTER TESTING
    //bool lerping = false;
    //TEST CASE: REMOVE AFTER TESTING


    void OnEnable()
    {
        dropTimer = GameManager.manager.dropTimer;
        NewBlockSpawner.SpawnEvent += ResetTimer;
    }

    void OnDisable()
    {
        NewBlockSpawner.SpawnEvent -= ResetTimer;
    }
    void Start () {
		healthLevel = GameObject.Find("PowerCellTotalPower").GetComponent<PowerCellStats>();
        health = 100;
        disrupted = false;
		powerCore = GameObject.Find("PowerCore");
		disruptableComponents = GetComponentsInChildren<Renderer> ();

		//defaultColor = powerCore.GetComponent<GameObject>().GetComponent<Renderer>().material.color;
		disruptColor = Color.black;

		disruptParticles = GameObject.Find("DisruptParticles");
		disruptParticles.SetActive(false);
		LoseCheck = GameObject.Find ("WinLoseGameCanvas").GetComponent<WinLoseGameState> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
        timer += Time.deltaTime;
        //TEST CASE: REMOVE AFTER TESTING
        //if (Input.GetKeyDown(KeyCode.T))
        //lerping = !lerping;
        //TEST CASE: REMOVE AFTER TESTING
        //if (lerping)
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, degree, 0), Time.deltaTime * rotateSpeed);

        UpdateHealth();
		if (health <= 0)
		{
			// Application.LoadLevel(0);
		}
		if(!disrupted)
		{
            //powerCore.GetComponent<Renderer>().material.color = defaultColor;
			foreach (Renderer components in disruptableComponents) 
			{
				if(components.gameObject.name.Contains("disruptable"))
				{
	
						components.GetComponent<Renderer>().GetComponent<Renderer>().material.color = 
						components.GetComponent<StoreColor>().defaultColor;

					if(components.gameObject.name.Contains("Capsule"))
					{
						components.GetComponent<Renderer>().GetComponent<Renderer>().material = 
							components.GetComponent<StoreColor>().defaultMaterial;
						components.GetComponent<Renderer>().GetComponent<Renderer>().material.color = Color.white;
						//	components.GetComponent<StoreColor>().defaultColor;
					}
				}
			}

			//disruptParticles.SetActive(false);
			CheckInputs();
		}
		else
			//powerCore.GetComponent<Renderer>().material.color = disruptColor;
			foreach (Renderer components in disruptableComponents) 
		{
			if(components.gameObject.name.Contains("disruptable"))
			{
				if(components.gameObject.name.Contains("Capsule"))
				{
					components.GetComponent<Renderer>().GetComponent<Renderer>().material = 
						components.GetComponent<StoreColor>().disruptMaterial;
					//components.GetComponent<Renderer>().material.color = 
					//	components.GetComponent<StoreColor>().disruptColor;
				}

				else
				{
					components.GetComponent<Renderer>().material.color = 
					components.GetComponent<StoreColor>().disruptColor;
				}
			}
		}


	}

	IEnumerator OnTriggerEnter(Collider other)
	{
        if (other.tag == "DisrupterProjectile")
        {
            yield return StartCoroutine("Disruption");
        }

        else if (other.gameObject.name.Contains("Simple"))
        {
            health -= 2;

			if(health <= 0){

				LoseCheck.LostGame();
			}
        }

        else if (other.tag != "DisrupterProjectile" &&
            other.name != "PowerBlock_3D(Clone)")
        {
            health -= 5;

			if(health <= 0){
				
				LoseCheck.LostGame();
			}
        }
	}

    void ResetTimer()
    {
        timer = 0;
    }

    void UpdateHealth()
    {
        healthLevel.health = health;
    }

	void CheckInputs()
	{
        if(Input.GetKeyDown(KeyCode.Space))
        {
            timer += 3;
        }
        if (timer < dropTimer)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                RotateLeft();
                if (RotateEvent != null)
                    RotateEvent();
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                RotateRight();
                if (RotateEvent != null)
                    RotateEvent();
            }
        }
	}

	//Allows for button controls on screen.
	public void RotateLeft()
    {
        degree += 90;
        //if (lerping)
        //else
		//transform.Rotate(0,90,0);
	
	}

	//Allows for button controls on screen.
	public void RotateRight()
    {
        degree -= 90;
        //if (lerping)
        //else
        //transform.Rotate(0,-90,0);

    }
	
	IEnumerator Disruption()
	{
        disrupted = true;
		disruptParticles.SetActive(true);
        if (!Application.loadedLevelName.Contains("Tutorial"))
        {
            yield return new WaitForSeconds(2.0f);
            disrupted = false;
			disruptParticles.SetActive(false);
        }
	}

	public bool GetDisrupted()
	{
		return disrupted;
	}
}
