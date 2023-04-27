using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deathTrigger : MonoBehaviour
{


    public bool isShell;
    public GameObject idleSprite;
    public GameObject deathSprite;
    public GameObject light;

    private GameManager gameManager;
    private Rigidbody2D rb;
    private AudioSource audio;

    private bool isDeath;
    private bool isSlow;

    public AudioClip death;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        audio = gameObject.GetComponent<AudioSource>();
        isDeath = false;
        deathSprite.SetActive(false);
        idleSprite.SetActive(true);
    }

    public bool getDeath()
    {
        return this.isDeath;
    }

    public bool getSlow()
    {
        return this.isSlow;
    }

    public void setSlow(bool mode)
    {
        this.isSlow = mode;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("PlayerBullet") && !isDeath)
        {
            //DISATTIVA IL NEMICO (RENDI SPRITE NERO, DISATTIVA IA)
            Destroy(collision.gameObject);
            deathSprite.SetActive(true);
            idleSprite.SetActive(false);

            if (light)
                light.SetActive(false);

            rb.velocity = Vector3.zero;
            rb.angularVelocity = 0f;
            isDeath = true;

            //COMUNICA AL GAME MANAGER CHE UN NEMICO È STATO COLPITO E CHE BISOGNA RALLENTARE IL TEMPO E RILANCIARE IL PLAYER
            gameManager.enemyHitted(true);

            audio.PlayOneShot(death);
        }

        if (collision.CompareTag("Player") && !isDeath && !isShell)
        {

            if (gameManager.getState() != "FAST")
            {
                gameManager.playerDeath();
                rb.velocity = Vector3.zero;
                isDeath = true;
                return;
            }
            //DISATTIVA IL NEMICO (RENDI SPRITE NERO, DISATTIVA IA)
            deathSprite.SetActive(true);
            idleSprite.SetActive(false);

            if (light)
                light.SetActive(false);

            rb.angularVelocity = 0f;
            rb.velocity = Vector3.zero;
            isDeath = true;

            //COMUNICA AL GAME MANAGER CHE UN NEMICO È STATO COLPITO E CHE BISOGNA RALLENTARE IL TEMPO E RILANCIARE IL PLAYER
            gameManager.enemyHitted(false);

            audio.PlayOneShot(death);
        }
    }
}
