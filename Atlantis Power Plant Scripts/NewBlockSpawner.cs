using UnityEngine;
using System.Collections;

public class NewBlockSpawner : MonoBehaviour {

    public Transform[] ColorBlock;
    public int counter = 0;
    public Vector3 center = new Vector3(-0.22f, 7, -2.38f);
    public Vector3 offCenter = new Vector3(.28f, 7, -2.38f);
    public float timer = 0.0f;
    public float probability;

    public int WhiteChance = 20;
    public int RedChance = 40;
    public int BlueChance = 60;
    public int GreenChance = 80;
    public int BlackChance = 100;

    //------------------------------------
    // Delegation
    public delegate void OnSpawn();
    public static event OnSpawn SpawnEvent;
    //------------------------------------

    void Start()
    {
        SpawnBlock(center);
        SpawnBlock(offCenter);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 5.0f || counter == 2)
        {
            timer = 0;
            counter = 0;
            if (SpawnEvent != null)
            {
                SpawnEvent();
            }

            SpawnBlock(center);
            SpawnBlock(offCenter);
        }
    }

    public void SpawnBlock(Vector3 newPos)
    {
        Instantiate(ChooseBlock(), newPos, Quaternion.identity);
    }

    Transform ChooseBlock()
    {
        probability = Random.Range(0, 100);
        if (probability < WhiteChance)
            return ColorBlock[4];
        else if (probability < RedChance)
            return ColorBlock[1];
        else if (probability < BlueChance)
            return ColorBlock[2];
        else if (probability < GreenChance)
            return ColorBlock[0];
        else
            return ColorBlock[3];
    }
}
