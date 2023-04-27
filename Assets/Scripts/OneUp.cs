using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneUp : MonoBehaviour
{

    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            gameManager.heartHitted();


            Destroy(gameObject);

            gameManager.enemyHitted(false);
        }
    }
}
