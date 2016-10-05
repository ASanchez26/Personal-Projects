using UnityEngine;
using System.Collections;

public class PowerTower : MonoBehaviour {

	public int power = 0;
	public int[] powerGoals;
	public GameObject[] myPowerCells;
	public ParticleSystem[] myParticles;

	public GameObject[] myTowers;

	public AudioClip winSoundFX;

	public WinLoseGameState gameManager;

	public bool needPowerToWin = true;
	bool didWin = false;
	float winDelay = 0f;

    //------------------------------
    public delegate void OnPowerUp();
    public static event OnPowerUp PowerUpEvent;
    //------------------------------

    // Use this for initialization
    void Start () 
	{

		myTowers [0] = GameObject.Find ("PowerTower");
		myTowers [1] = GameObject.Find ("PowerTower1");

		//myTowers [0].GetComponent<LineRenderer>().enabled = false;
		//myTowers [1].GetComponent<LineRenderer>().enabled = false;
		//Vector3 up = transform.TransformDirection(Vector3.up);	
		//LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
		//lineRenderer.SetWidth(1f, 1f);
		//AddPower (0);

	}
	
	// Update is called once per frame
	void Update () 
	{
		if (didWin) 
		{
			winDelay += Time.deltaTime;
			if(winDelay >= 3f)
			{
				if(gameManager.didWin == false)
				{
					gameManager.didWin = true;
					GetComponent<AudioSource>().PlayOneShot(winSoundFX);
				}
			}
		}
		//lineRenderer.SetPosition(1, transform.position.);
	}

	public void AddPower(int amount)
	{
		power = power + amount;

		if (PowerUpEvent != null)
			PowerUpEvent();



		if (power >= powerGoals[5]) 
		{
			//Debug.Log("Win!!!");
			if(needPowerToWin)
			{
				//didWin = true;
				gameManager.GetComponent<WinLoseGameState>().WinGame();

			}
            myPowerCells[5].GetComponent<Renderer>().material.color = Color.yellow;
			//myParticles[1].emissionRate = 40;// = true;
			//myTowers[1].GetComponent<LineRenderer>().enabled = true;

		}

		else if (power >= powerGoals[4]) 
		{
			//myParticles[1].emissionRate = 20;
			myPowerCells[4].GetComponent<Renderer>().material.color = Color.yellow;
		}

		else if (power >= powerGoals[3]) 
		{
			//myParticles[1].emissionRate = 10;
			myPowerCells[3].GetComponent<Renderer>().material.color = Color.yellow;
		}

		else if (power >= powerGoals[2]) 
		{
			//myParticles[0].emissionRate = 40;
			//myTowers[0].GetComponent<LineRenderer>().enabled = true;
			myPowerCells[2].GetComponent<Renderer>().material.color = Color.yellow;
		}

		else if (power >= powerGoals[1]) 
		{
			//myParticles[0].emissionRate = 20;
			myPowerCells[1].GetComponent<Renderer>().material.color = Color.yellow;
		}

		else if (power >= powerGoals[0]) 
		{
			//myParticles[0].emissionRate = 10;
			myPowerCells[0].GetComponent<Renderer>().material.color = Color.yellow;
		}

       
    }
}
