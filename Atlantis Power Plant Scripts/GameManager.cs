using UnityEngine;
using System;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class GameManager : MonoBehaviour {

    public static GameManager manager;
    PlayerSaveData Level = new PlayerSaveData();

    int MAXLEVEL = 12;

    new string name = "";
    string[] highScoringPlayer = new string[12];
    int score = 0;
    int[] highScore = new int[12];
    int loadLevel = 0;
    int maxLevel = 0;

    public float dropTimer = 3f;

    //singleton-esque implementation of this game manager script
	void Awake ()
    {
	    if(manager == null)
        {
            DontDestroyOnLoad(gameObject);
            manager = this;
        }
        else if(manager != this)
        {
            Destroy(gameObject);
        }
	}

    //void OnEnable()
    //{
    //    //for(int i = 0; i < MAXLEVEL; i++)
    //    //{
    //    //    Level.highScore[i] = 0;
    //    //    Level.highScoringPlayer[i]= "";
    //    //    Debug.Log(i);
    //    //}

    //    Debug.Log("Name is: " + name);
    //    Debug.Log("MaxLevel is: " + maxLevel);
    //    Debug.Log("High Score is: " + Level.highScore[Application.loadedLevel]);
    //    Debug.Log("High Scoring Player is: " + Level.highScoringPlayer[Application.loadedLevel]);
    //}

    //Save functionality able to be called when needed
    //Writes to Unity appData folder
    public void Save ()
    {
        BinaryFormatter format = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/saveData"+name+".dat");

        Level.name = name;
        Level.maxLevel = maxLevel;
        Level.highScore[Application.loadedLevel] = highScore[Application.loadedLevel];
        Level.highScoringPlayer[Application.loadedLevel] = highScoringPlayer[Application.loadedLevel];
        
//        data.loadLevel = loadLevel;
//        data.score = score;
//        data.highScore = highScore;

        format.Serialize(file, Level);
        file.Close();
    }

    //Loads data from save file if exists.
    //Will update current values and loadlevel number when called.
    public void Load()
    {
        if(File.Exists(Application.persistentDataPath + "/saveData"+name+".dat"))
        {
            BinaryFormatter format = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/saveData"+name+".dat", FileMode.Open);
            PlayerSaveData data = (PlayerSaveData)format.Deserialize(file);
            file.Close();

            name = data.name;
            maxLevel = data.maxLevel;
            highScoringPlayer = data.highScoringPlayer;
            highScore = data.highScore;
//            loadLevel = data.loadLevel;
//            score = data.score;
//            highScore = data.highScore;
        }
    }

    void OnLevelWasLoaded(int level)
    {
        Time.timeScale = 1;
        if (manager != this)
            return;
        else
        {
            score = 0;
            highScore[Application.loadedLevel] = highScore[Application.loadedLevel];
            highScoringPlayer[Application.loadedLevel] = highScoringPlayer[Application.loadedLevel];
            Debug.Log("The High Score for this level is " + highScore[Application.loadedLevel]);
            Debug.Log("The Highest Scoring Player for this level is "+ highScoringPlayer[Application.loadedLevel]);
        }
    }

    //-------------------------------------
    //Setters/Getters for Save/Load values
    //Add methods as needed for new values
    public void SetLoadLevel(int level)
    {
        loadLevel = level;
    }

    public int GetLoadLevel()
    {
        return loadLevel; 
    }


    public void SetScore(int curScore)
    {
        score = curScore;
        Debug.Log("The current score is: " + score);
        SetHighScore();
    }

    public int GetScore()
    {
        return score;
    }


    public void SetHighScore()
    {
        try
        {
            if (score > highScore[Application.loadedLevel])
            {
                highScore[Application.loadedLevel] = score;
                highScoringPlayer[Application.loadedLevel] = name;
            }
        }
        catch (NullReferenceException e)
        { }
    }

    public int GetHighScore()
    {
        return highScore[Application.loadedLevel];
    }


    public void SetName(string Name)
    {
        name = Name;
    }

    public string GetName()
    {
        return name;
    }


    public void SetMaxLevel()
    {
        if (Application.loadedLevel > maxLevel)
            maxLevel = Application.loadedLevel;
    }

    public int GetMaxLevel()
    {
        return maxLevel + 2;
    }
    //------------------------------------
}

[Serializable] class PlayerSaveData
{

    public string name;
    public string[] highScoringPlayer = new string[12];
    public int maxLevel;
    public int[] highScore = new int [12];

    //    public int loadLevel;
    //    public int score;
}
