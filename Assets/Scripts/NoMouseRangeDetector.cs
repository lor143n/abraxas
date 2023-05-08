using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoMouseRangeDetector : MonoBehaviour
{

    public GameManager gameManager;

    private void Awake()
    {
        gameManager = GameObject.Find("GameController").GetComponent<GameManager>();
    }

    private void OnMouseEnter()
    {
        gameManager.setIsNoMouseRange(true);
    }

    private void OnMouseExit()
    {
        gameManager.setIsNoMouseRange(false);
    }
}
