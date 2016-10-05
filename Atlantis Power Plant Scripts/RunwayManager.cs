using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RunwayManager : MonoBehaviour
{

    public int[] ColorsRight = { 0, 0, 0, 0 }; //Array of values for Green, Red, Blue, and Black blocks, respectively, on the right side of the PowerCell.
    public int[] ColorsLeft = { 0, 0, 0, 0 };  //Array of values for Green, Red, Blue, and Black blocks, respectively, on the left side of the PowerCell.
    public int[][] Sides = new int[4][];
    public int[] LeftCommands;
    public int[] RightCommands;
    public int activeSide = 0;
    public int LeftSide = 3;
    public int RightSide = 1;
    public float dropTimer;
    float timer = 0;
    bool disrupted;

    PowerCellRotate powerCell;
    ChargeLevels greenValue;
    ChargeLevels redValue;
    ChargeLevels blueValue;
    //----------------------------------------------------
    void OnEnable()
    {
        NewBlockSpawner.SpawnEvent += ResetTimer;
    }

    void OnDisable()
    {
        NewBlockSpawner.SpawnEvent -= ResetTimer;
    }

    void Start()
    {
        dropTimer = GameManager.manager.dropTimer;
        for (int i = 0; i < Sides.Length; i++)
        {
            Sides[i] = new int[4];
        }

        powerCell = GameObject.Find("PowerCell").GetComponent<PowerCellRotate>();
        greenValue = GameObject.Find("TurretActiveTimeGreen").GetComponent<ChargeLevels>();
        redValue = GameObject.Find("TurretActiveTimeRed").GetComponent<ChargeLevels>();
        blueValue = GameObject.Find("TurretActiveTimeBlue").GetComponent<ChargeLevels>();

        LeftCommands = Sides[LeftSide];
        RightCommands = Sides[RightSide];
    }

//Checks for PowerCell disrupted state. If not disrupted, checks for player inputs.
    void Update()
    {
        disrupted = powerCell.GetDisrupted();
        if (!disrupted)
        {
            CheckInput();
        }
        Activator();
    }

//Disable all turrets on rotate, then recheck each side for matches.
//Activate turrets based on number of Match-3s on a given side.
    void Activator()
       {
        //Find each block in the runway and effect changes on that block's children (turrets)
        GridInUse[] runway = FindObjectsOfType(typeof(GridInUse)) as GridInUse[];
           foreach (GridInUse child in runway)
           {
            TurretController childTwo = child.GetComponentInChildren<TurretController>();
            if (childTwo != null)
            {
                childTwo.isActivated = false;
                if (child.transform.localPosition.x > 0 && child.transform.localPosition.x <= (Mathf.CeilToInt(greenValue.sliderValue/5)) * 2)
                {
                    childTwo.SetTurretActive("GreenTurret");
                }

                if (child.transform.localPosition.x > 0 && child.transform.localPosition.x <= (Mathf.CeilToInt(redValue.sliderValue / 5)) * 2)
                {
                    childTwo.SetTurretActive("RedTurret");
                }

                if (child.transform.localPosition.x > 0 && child.transform.localPosition.x <= (Mathf.CeilToInt(blueValue.sliderValue / 5)) * 2)
                {
                    childTwo.SetTurretActive("BlueTurret");
                }
            }
        }
    }
    
    void ResetTimer()
    {
        timer = 0;
    } 

//Checks for player input for Q or E during Update.
    void CheckInput()
    {
        timer += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space))
            timer += 3;

        if (timer < dropTimer)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                IncrementSides();
                UpdateTurrets();
            }

            else if (Input.GetKeyDown(KeyCode.E))
            {
                DecrementSides();
                UpdateTurrets();
            }
        }
    }

//If player input == Q, increment all values, reseting arrays to 0 if array element exceeds 3.
    void IncrementSides()
    {
        LeftSide++;
        RightSide++;
        activeSide++;
        if (LeftSide > 3) { LeftSide = 0; }
        if (LeftSide < 0) { LeftSide = 3; }
        if (RightSide > 3) { RightSide = 0; }
        if (RightSide < 0) { RightSide = 3; }
        if (activeSide > 3) { activeSide = 0; }
        if (activeSide < 0) { activeSide = 3; }
        LeftCommands = Sides[LeftSide];
        RightCommands = Sides[RightSide];
    }

//If player input == E, decrement all values, changing -1 values to 3 for each array element.
    void DecrementSides()
    {
        LeftSide--;
        RightSide--;
        activeSide--;
        if (LeftSide > 3) { LeftSide = 0; }
        if (LeftSide < 0) { LeftSide = 3; }
        if (RightSide > 3) { RightSide = 0; }
        if (RightSide < 0) { RightSide = 3; }
        if (activeSide > 3) { activeSide = 0; }
        if (activeSide < 0) { activeSide = 3; }
        LeftCommands = Sides[LeftSide];
        RightCommands = Sides[RightSide];
    }

//After values are changed due to player input, new values are updated and turrets are activated based on matches found.
    void UpdateTurrets()
    {
        ColorsRight = RightCommands;
        ColorsLeft = LeftCommands;
    //    Activator();
    }
}
