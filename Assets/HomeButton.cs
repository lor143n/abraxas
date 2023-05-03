using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HomeButton : MonoBehaviour
{
    public Button myButton;

    void Start()
    {
        myButton = gameObject.GetComponent<Button>();
        myButton.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        SceneManager.LoadScene("menu");
    }
}
