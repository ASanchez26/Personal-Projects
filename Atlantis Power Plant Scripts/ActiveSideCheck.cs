using UnityEngine;
using System.Collections;

public class ActiveSideCheck : MonoBehaviour {

    public int thisSide;
    public bool isRightSide;
    public float dropTimer;
    int activeSide = 1;
    bool disrupted;
    float timer = 0;

    PowerCellRotate powerCell;

    // Use this for initialization
    void OnEnable()
    {
        NewBlockSpawner.SpawnEvent += ResetTimer;
    }

    void OnDisable()
    {
        NewBlockSpawner.SpawnEvent -= ResetTimer;
    }

    void Start () {
        dropTimer = GameManager.manager.dropTimer;
        powerCell = GameObject.Find("PowerCell").GetComponent<PowerCellRotate>();

    }
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        disrupted = powerCell.GetDisrupted();
        if (!disrupted)
        {
            CheckInput();
        }

        if (activeSide > 3) { activeSide = 0; }
        if (activeSide < 0) { activeSide = 3; }

        if (thisSide == activeSide)
            isRightSide = true;
        else
            isRightSide = false;
    }

    void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            timer += 3;

        if (timer < dropTimer)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                activeSide++;
            }

            else if (Input.GetKeyDown(KeyCode.E))
            {
                activeSide--;
            }
        }
    }

   public bool IsRightSide()
    {
        return thisSide == activeSide;
    }

    void ResetTimer()
    {
        timer = 0;
    }
}
