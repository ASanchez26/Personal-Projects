using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour {

    GameObject panel;
    TutorialManager tutorial;
    // Use this for initialization
    void Start ()
    {
        panel = GameObject.Find("MessagePanel");
        tutorial = GameObject.FindObjectOfType<TutorialManager>();
        Time.timeScale = 0;
    }
    void OnEnable()
    {
        Time.timeScale = 0;
    }

    void OnDisable()
    {
        Time.timeScale = 1;
    }

    public void OnClick()
    {
        //if (tutorial.GetMessages() > 1)
        //    UpdatePlayerMessage();
        //else
        //panel.SetActive(false);
    }

    public void UpdatePlayerMessage()
    {
        //Text newMessage = panel.transform.GetChild(0).GetComponent<Text>();
        //newMessage.text = tutorial.GetNextPlayerMessage();
        //tutorial.SetMessages(1);
    }
}
