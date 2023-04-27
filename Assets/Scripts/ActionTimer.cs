using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ActionTimer : MonoBehaviour
{

    [Range(0f, 5f)]
    public float duration;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GetComponent<GameManager>();
    }


    public void startTimer()
    {
        StartCoroutine("timer");
    }

    public void stopTimer()
    {
        StopCoroutine("timer");
    }


    IEnumerator timer()
    {
        yield return new WaitForSeconds(duration);
        if(gameManager.getState() == "SLOW" || gameManager.getState() == "ATTACK")
            gameManager.timeIsUp();
    }
}
