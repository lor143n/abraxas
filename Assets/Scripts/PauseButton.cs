using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    public Button myButton;
    public GameManager gameManager;

    void Start()
    {
        myButton = gameObject.GetComponent<Button>();
        gameManager = GameObject.Find("GameController").GetComponent<GameManager>();
        myButton.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        if(Time.timeScale > 0)
        {
            gameManager.setIsPaused(true);
            Time.timeScale = 0;
        } else
        {
            gameManager.setIsPaused(false);
            Time.timeScale = 1;
        }
    }
}
