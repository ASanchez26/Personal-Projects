using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class BlockMove : MonoBehaviour {

	//VEGA : Uses orbspawner object to modify bool value and colors of orbs / particles.
	//**See Increment power timer function**
	//SEE INCREMENT POWER TIMER FOR AUDIO
	public OrbSpawner myOrbSpawner;
	public AudioClip match3SoundFX;
	public AudioClip blockLandSoundFX;
    public GameObject[] Blocks = new GameObject[4];
    public Vector3 curPos;

    public bool canQuickDrop;
    public float dropTimer;
    public int counter = 0;

	//used for black blocks particle systems
	public Component[] particleComponents;
	//used to disable all mesh renderers when destroyed
	public Component[] meshRendComponents;

    private float xMin;
    private float xMax;
    private int activeColor = 0;
    private string colorName = null;
    private bool gaveScore = false;
    private bool isBossLevel = false;

    RaycastHit hit = new RaycastHit();
    RunwayManager Color;
    Rigidbody rigidbody;

    [SerializeField] MatchCheck[] Feelers = new MatchCheck[4];

    //-------------------------------------------------------------------------------------
    /*
     *  FUNCTIONS START
     */
    //-------------------------------------------------------------------------------------

    void OnEnable()
    {
        dropTimer = GameManager.manager.dropTimer;
        if(gameObject.tag == "BlackBlock" || gameObject.tag == "PowerBlock")
            NewBlockSpawner.SpawnEvent += CheckBlackWhite;

        PowerCellRotate.RotateEvent += CheckMatches;
    }

    void OnDisable()
    {
        if (gameObject.tag == "BlackBlock" || gameObject.tag == "PowerBlock")
            NewBlockSpawner.SpawnEvent -= CheckBlackWhite;

        PowerCellRotate.RotateEvent -= CheckMatches;
    }

    void Start()
    {
        if (Application.loadedLevelName.Contains("Boss"))
            isBossLevel = true;

        rigidbody = GetComponent<Rigidbody>();
        canQuickDrop = true;
        curPos = transform.position;
        xMin = curPos.x - 1;
        xMax = curPos.x + 1;
        StartCoroutine("ResumeFall");

        Color = GameObject.FindObjectOfType<RunwayManager>();
        switch (this.transform.tag)
        {
            case "GreenBlock":
                activeColor = 0;
                colorName = "Green";
                break;
            case "RedBlock":
                activeColor = 1;
                colorName = "Red";
                break;
            case "BlueBlock":
                activeColor = 2;
                colorName = "Blue";
                break;
            case "BlackBlock":
                activeColor = 3;
                colorName = "Black";
                break;
            default:
                break;


        }

		meshRendComponents = GetComponentsInChildren<MeshRenderer> ();

		if(gameObject.name.Contains("Black"))
		{
			//Debug.Log("Get particles and set into array.");
			particleComponents = GetComponentsInChildren<ParticleSystem>();
		}
    }

//While blocks are not parented, allow movement and quickdrop
    void Update()
    {
        UpdateNeighbors();
        if (transform.parent == null)
        {
            if (rigidbody.isKinematic == true)
            {
                QuickDrop();
                CheckInput();
            }

            Vector3 temp = transform.position;
            temp.x = Mathf.Clamp(temp.x, xMin, xMax);
            transform.position = temp;
        }

        if(transform.parent != null)
        {
            ActiveSideCheck side = transform.parent.GetComponent<ActiveSideCheck>();
            if (side.isRightSide)
                CheckMatches();
        }
    }

//Determine Block condition based on health counter
//On taking more than 3 damage, destroy the block
    void FixedUpdate()
    {
        Vector3 curVelocity = rigidbody.velocity;
        if (curVelocity.y <= 0f)
            return;
        curVelocity.y = 0f;
        rigidbody.velocity = curVelocity; 
    }

//On collision with an enemy, increment counter by 1, then destroy enemy.
//On collision with anything else, disable line renderer
    void OnCollisionEnter(Collision other)
    {
        if (other.collider.gameObject.tag == "Enemy" ||
            other.collider.gameObject.tag == "RangedEnemy" ||
            other.collider.gameObject.tag == "EnemyProjectile")
        {
			//only disable other objects collider if it is not a boss
			if(other.collider.gameObject.name.Contains("Enemy"))
			{
				other.gameObject.GetComponent<Collider>().enabled = false;
			}
            this.gameObject.layer = LayerMask.NameToLayer("BlockDrop");
            //this.GetComponent<MeshRenderer>().enabled = false;
			foreach(MeshRenderer meshes in meshRendComponents)
			{
				meshes.GetComponent<MeshRenderer>().enabled = false;
			}

			//turns off particles from center of block.
			GetComponentInChildren<ParticleSystem>().maxParticles = 0;// = 0;//startSize = 0;// = 0;

			Destroy(this.gameObject, 0.5f);
        }

        else
        {
			GetComponent<AudioSource>().PlayOneShot(blockLandSoundFX);
            GetComponent<LineRenderer>().enabled = false;
            TutorialList();
            if(transform.parent != null)
                CullBlocks();
        }
    }

//Use this coroutine to wait 2.5 seconds before making blocks non-kinematic
    IEnumerator ResumeFall()
    {
        yield return new WaitForSeconds(dropTimer);
        rigidbody.isKinematic = false;
    }

//Move Blocks based on keyboard input
    void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.Translate(Vector3.left/2);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            transform.Translate(Vector3.right/2);
        }
    }

//When pressing Space, any kinematic blocks will become affected by gravity
//Slight downward force applied to simulate -quick- drop
    void QuickDrop()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (canQuickDrop)
            {
                canQuickDrop = false;
                rb.isKinematic = false;
                rb.AddForce(Vector3.down * 6, ForceMode.VelocityChange);
            }
        }
    }

    void CullBlocks()
    {
        Transform[] children;
        if (transform.position.y > 3.6f && transform.position.y < 4.0f)
        {

            children = transform.parent.GetComponentsInChildren<Transform>();
            foreach (Transform block in children)
            {
                if (isBossLevel)
                {
                    if (block.localPosition.y == .75 && block.name.Contains("Block"))
                        Destroy(block.gameObject);
                }
                else if (block.position.y == .75f && block.name.Contains("Block"))
                { 
                    Destroy(block.gameObject);
                }
            }
        }
    }

    void CheckMatches()
    {
        if (transform.parent != null)
        {
            int[] updateColor = Color.Sides[Color.activeSide];

            if (Blocks[0] != null && Blocks[1] != null)
            {
                if (Blocks[0].tag == Blocks[1].tag)
                {
                   //isMatched = true;
                    updateColor[activeColor]++;
                    DestroyBlocks(0, 1);
                }
            }
            if (Blocks[2] != null && Blocks[3] != null)
            {
                if (Blocks[2].tag == Blocks[3].tag)
                {
                    //isMatched = true;
                    updateColor[activeColor]++;
                    DestroyBlocks(2, 3);
                }
            }
        }
    }

    public void CheckBlackWhite()
    {
        if (Blocks[0] != null && Blocks[1] != null)
        {
            if (Blocks[0].tag == Blocks[1].tag)
            {
                DestroyBlackWhite(3, 2, 1, 0);
            }
        }
        if (Blocks[2] != null && Blocks[3] != null)
        {
            if (Blocks[2].tag == Blocks[3].tag)
            {
                DestroyBlackWhite(0, 1, 2, 3);
            }
        }
    }

    //called to destroy vertical matches
    void DestroyBlackWhite(int x, int y, int z, int w)
    {
        PowerTower powerTower = GameObject.Find("PowerTowers").GetComponent<PowerTower>();
        if (Blocks[z].tag == Blocks[w].tag)
        {
            if (tag == "BlackBlock")
            {
			
                foreach (GameObject block in Blocks)
                {
                    if (block != null)
                        block.BroadcastMessage("DestroyOtherBlocks", SendMessageOptions.DontRequireReceiver);
                }
				foreach(ParticleSystem particles in particleComponents)
				{
					Debug.Log("Activate Particles");
					particles.enableEmission = true;
					particles.Emit(15);
				}
                Destroy(Feelers[x].otherBlock, .3f);
                Destroy(Feelers[y].otherBlock, .3f);
            }

            if (tag == "PowerBlock")
            {
                powerTower.AddPower(1);
                GetComponent<AudioSource>().PlayOneShot(match3SoundFX);
            }

            Destroy(Blocks[z], .3f);
            Destroy(Blocks[w], .3f);
            Destroy(gameObject, .3f);
        }
    }

    void IncrementPowerTimer()
    {
		if (myOrbSpawner != null) 
		{
			myOrbSpawner.GetComponent<OrbSpawner> ().willSpawn = true;
			ChargeLevels powers = GameObject.Find("TurretActiveTime" + colorName).GetComponent<ChargeLevels>();
            if (colorName != null)
            {
				    //PLAY AUDIO EFFECT UPON MATCH
			    GetComponent<AudioSource>().PlayOneShot(match3SoundFX);
                float sliderValue = powers.sliderValue;
                powers.sliderValue += 5.0f;
            }
		}
    }

    void UpdateNeighbors()
    {
        Blocks[0] = Feelers[0].GetComponent<MatchCheck>().neighbor;
        Blocks[1] = Feelers[1].GetComponent<MatchCheck>().neighbor;
        Blocks[2] = Feelers[2].GetComponent<MatchCheck>().neighbor;
        Blocks[3] = Feelers[3].GetComponent<MatchCheck>().neighbor;
    }

    void DestroyBlocks(int x, int y)
    {
        ActiveSideCheck side = transform.parent.GetComponent<ActiveSideCheck>();
        if (side.IsRightSide())
        {
            Destroy(Blocks[x], .3f);
            Destroy(Blocks[y], .3f);
            Destroy(gameObject, .3f);
            AddScore();
        }
    }

    void AddScore()
    {
        if (!gaveScore)
        {
            IncrementPowerTimer();
            ScoreKeeper.UpdateScore(50);
            TutorialMatch();
            gaveScore = true;
        }
    }

    void TutorialList()
    {
        if (Application.loadedLevelName.Contains("Tutorial"))
        {
            if (this.transform.localPosition.y < .77f)
            {
                TutorialManager tutorial = FindObjectOfType<TutorialManager>();
                if(!tutorial.Blocks.Contains(gameObject))
                tutorial.Blocks.Add(gameObject);
            }
        }
    }

    void TutorialMatch()
    {
        if (Application.loadedLevelName.Contains("Tutorial"))
        {
            TutorialManager tutorial = FindObjectOfType<TutorialManager>();
            tutorial.matches++;
        }
    }
}
