using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    private GameManager gameManager;
    public GameObject idle;
    public GameObject fast;

    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyBullet") && !gameManager.isMenu)
        {
            gameManager.playerDeath();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (gameManager.isMenu)
        {
            gameManager.setState("IDLE");
            idle.SetActive(true);
            fast.SetActive(false);
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            gameManager.ShotDone = false;
        }
    }
}
