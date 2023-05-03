using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    public Button myButton;

    void Start()
    {
        myButton = gameObject.GetComponent<Button>();
        myButton.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        if(Time.timeScale > 0)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }
}
