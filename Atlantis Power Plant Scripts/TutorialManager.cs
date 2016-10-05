using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class TutorialManager : MonoBehaviour {
    //-----------------------------------------------------------
    //List of GameObjects to be activated
    [SerializeField] GameObject[] turrets;
    [SerializeField] GameObject spawner;
    [SerializeField] GameObject enemySpawner;
    [SerializeField] GameObject controlsPanel;
    [SerializeField] NewBlockSpawner probability;
    public List<GameObject> Blocks = new List<GameObject>();

    enum Events
    {
        EVENT_ONE,
        EVENT_TWO,
        EVENT_THREE,
        EVENT_FOUR,
        NUM_EVENTS
    }
    private Events curEvent = Events.EVENT_ONE;
    private Dictionary<Events, Action> fsm = new Dictionary<Events, Action>();

    //-----------------------------------------------------------
    //Event Checks

    bool runEventOne = true;
    bool goal1 = false;
    bool goal2 = false;
    bool goal3 = false;
    bool goal4 = false;
    bool goal5a = false;
    bool goal5b = false;
    bool goal5c = false;
    bool goal6 = false;
    bool goal7 = false;
    bool goal8 = false;

    public int matches = 0;
    public int walls = 0;
    int curText = 0;
    int messagesInEvent = 0;

    //-----------------------------------------------------------
    WinLoseGameState checkWin;
    PowerTower tower;
    PowerCellStats powercheck;
    GridInUse[] runway;
    ChargeLevels greenValue, redValue, blueValue;
    Text TextA, TextB, TextC, TextC1, TextC2, TextC3;

    //-----------------------------------------------------------
    void Awake()
    {
        fsm.Add(Events.EVENT_ONE, new Action(EventOne));
        fsm.Add(Events.EVENT_TWO, new Action(EventTwo));
        fsm.Add(Events.EVENT_THREE, new Action(EventThree));
        fsm.Add(Events.EVENT_FOUR, new Action(EventFour));
        SetState(Events.EVENT_ONE);
    }

    void OnEnable()
    {
        GridInUse.SpawnTurretEvent += AddWall;
    }

    void OnDisable ()
    {
        GridInUse.SpawnTurretEvent -= AddWall;
    }
    void Start()
    {
        checkWin = GameObject.FindObjectOfType<WinLoseGameState>();
        tower = GameObject.FindObjectOfType<PowerTower>();
        powercheck = GameObject.FindObjectOfType<PowerCellStats>();

        TextA = GameObject.Find("GoalsTextA").GetComponent<Text>();
        TextB = GameObject.Find("GoalsTextB").GetComponent<Text>();
        TextC = GameObject.Find("GoalsTextC").GetComponent<Text>();
        TextC1 = GameObject.Find("GoalsTextC1").GetComponent<Text>();
        TextC2 = GameObject.Find("GoalsTextC2").GetComponent<Text>();
        TextC3 = GameObject.Find("GoalsTextC3").GetComponent<Text>();

        greenValue = GameObject.Find("TurretActiveTimeGreen").GetComponent<ChargeLevels>();
        redValue = GameObject.Find("TurretActiveTimeRed").GetComponent<ChargeLevels>();
        blueValue = GameObject.Find("TurretActiveTimeBlue").GetComponent<ChargeLevels>();
    }

    // Update is called once per frame
    void Update()
    {
        fsm[curEvent].Invoke();
        CheckWinCondtitions();
    }

    //Event One
    //Block and Power Cell Movement
    //Generate Power
    void EventOne()
    {
        controlsPanel.SetActive(true);
        Time.timeScale = 0;
        if(runEventOne == false)
        {
            Time.timeScale = 1;
            controlsPanel.SetActive(false);
            SetState(Events.EVENT_TWO);
        }

    }

    //Event Two
    void EventTwo()
    {
        UpdateGoals(2);
        spawner.SetActive(true);
        if(goal1 && goal2)
        {
            TextA.color = Color.white;
            TextB.color = Color.white;
            TextC.color = Color.white;
            SetState(Events.EVENT_THREE);
        }

    }

    //Event Three
    void EventThree()
    {
        UpdateGoals(3);
        Blocks.Clear();
        turrets[0].SetActive(true);
        turrets[1].SetActive(true);
        turrets[2].SetActive(true);
        UpdateProbability(0, 30, 60, 90, 100);
        if(goal4 && goal5a && goal5b && goal5c)
        {
            TextA.color = Color.white;
            TextB.color = Color.white;
            TextC.color = Color.white;
            SetState(Events.EVENT_FOUR);
        }
    }

    //Event Four
    void EventFour()
    {
        UpdateGoals(4);
        UpdateProbability(35, 55, 75, 90, 100);
        enemySpawner.SetActive(true);
        if (goal6 && goal7 && goal8)
            checkWin.didWin = true;
    }
    //----------------------------------------------

    //Systemic checks for event activation
    void UpdateProbability(int whiteChance, int redChance, int blueChance, int greenChance, int blackChance)
    {
        probability.WhiteChance = whiteChance;
        probability.RedChance = redChance;
        probability.GreenChance = greenChance;
        probability.BlueChance = blueChance;
        probability.BlackChance = blackChance;
    }

    void SetState(Events newEvent)
    {
        curEvent = newEvent;
    }

    void CheckWinCondtitions()
    {
        if (powercheck.energy >= 100)
            goal1 = true;

        if (Blocks.Count >= 24)
            goal2 = true;

        if (matches > 10)
            goal4 = true;

        if (greenValue.sliderValue > 20)
            goal5a = true;

        if (redValue.sliderValue > 20)
            goal5b = true;

        if (blueValue.sliderValue > 20)
            goal5c = true;
        
        if(goal6 && goal7)
        {
            goal8 = true;
            checkWin.didWin = true;
        }
    }
    void UpdateGoals(int CurEvent)
    {
        switch (CurEvent)
        {
        case 2:
            TextA.text = "- Generate 100 Power: " + Mathf.Clamp(powercheck.energy,0,100) + "/100";
            TextB.text = "- Surround Power Cell with 1 row of blocks";
            TextC.text = "";
                if (powercheck.energy == 100)
                    TextA.color = Color.green;
                if (Blocks.Count == 24)
                    TextB.color = Color.green;

        break;

        case 3:
		    TextA.text = "- Match 3 or more blocks of a single color";
            TextB.text = "- Rotate matched blocks toward the runway: " + Mathf.Clamp(matches, 0, 10) + "/10";
                TextC.text = "- Charge each color to 50%";
                TextC1.text = "	Green Turrets: " + Mathf.Round(Mathf.Clamp(greenValue.sliderValue / 40 * 100, 0, 100)) + "%";
                TextC2.text = "	Red Turrets: " + Mathf.Round(Mathf.Clamp(redValue.sliderValue / 40 * 100, 0, 100)) + "%"; 
                TextC3.text = "	Blue Turrets: " + Mathf.Round(Mathf.Clamp(blueValue.sliderValue / 40 * 100, 0, 100)) + "%";
                if (true)
                {
                    TextA.color = Color.green;
                }

                if (matches >= 10)
                {
                    TextB.color = Color.green;
                }

                if (greenValue.sliderValue >= 20)
                {
                    TextC1.color = Color.green;
                }

                if (redValue.sliderValue >= 20)
                {
                    TextC2.color = Color.green;
                }

                if (blueValue.sliderValue >= 20)
                {
                    TextC3.color = Color.green;
                }

                if (greenValue.sliderValue >= 20 &&
                    redValue.sliderValue >= 20 &&
                    blueValue.sliderValue >= 20)
                    TextC.color = Color.green;
                break;

        case 4:
		    //TextA.text = "Build 2 Walls: " + Mathf.Clamp(walls, 0, 2) + "/2";
            TextA.text = "Defend the Power Cell";
            TextB.text = "Create 6 Power Block Matches: " + Mathf.Clamp(tower.power, 0, 6) + "/6";
            TextC.text = "Build 2 Walls: " + Mathf.Clamp(walls, 0, 2) + "/2";
            TextC1.text = "";
            TextC2.text = "";
            TextC3.text = "";

                if (powercheck.energy == 100)
                    TextA.color = Color.green;

                if (tower.power > 5)
                {
                    TextB.color = Color.green;
                    goal7 = true;
                }
                if (walls >= 2)
                {
                    TextC.color = Color.green;
                    goal6 = true;
                }
                if(goal8)
                    TextA.color = Color.green;
                break;
        }
    }

    void AddWall()
    {
        walls++;
    }

    public void OnClick()
    {
        controlsPanel.SetActive(false);
        runEventOne = false;
    }
}
