using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathDetection : MonoBehaviour
{

    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && gameManager.getState() != "WIN")
        {
            gameManager.playerDeath();
        }

        if (collision.CompareTag("EnemyBullet") || collision.CompareTag("PlayerBullet"))
        {
            Destroy(collision.gameObject);
        }
    }
}
