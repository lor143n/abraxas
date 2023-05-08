using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    [Range(0f, 20f)]
    public float playerSpeed;
    [Range(0f, 2f)]
    public float slowSpeed;
    [Range(0f, 20f)]
    public float bulletSpeed;
    public GameObject bullet;
    public GameObject idle;
    public GameObject fast;
    public List<GameObject> enemies;
    public Color deathColor;
    public Color winColor;
    public AudioClip laser;
    public AudioClip deathSound;
    public AudioClip slow;
    public AudioClip heartSound;
    public bool isMenu;
    public TextMeshProUGUI levelText;
    public Texture2D cursorTex;

    private ActionTimer timer;
    private Transform player;
    private Rigidbody2D plRb;
    private GameObject endButton;
    private float speed;
    private LevelCreator creator;
    private string playerState; //IDLE, SLOW, FAST, ATTACK, DEATH, WIN
    [HideInInspector] public bool ShotDone;
    private AudioSource audio;
    private Animator winTransition;
    private Image spriteTransition;
    private int levelCounter;
    private bool isNoMouseRange;
    private bool isPaused;


    //INIZIALIZZAZIONE DELLE VARIABILI
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        plRb = player.gameObject.GetComponent<Rigidbody2D>();
        endButton = GameObject.Find("EndButton");
        creator = GetComponent<LevelCreator>();
        timer = GetComponent<ActionTimer>();
        winTransition = GameObject.Find("TransitionManager").GetComponent<Animator>();
        spriteTransition = winTransition.gameObject.GetComponent<Image>();
        audio = GetComponent<AudioSource>();
        isPaused = false;

        if (!isMenu)
        {
            levelText.text = levelCounter.ToString();
        }
    }

    
    //INIZIALIZZAZIONE DEL LIVELLO E DEI PARAMETRI
    private void Start()
    {
        if (isMenu)
        {
            Initialize("MENU");
            return;
        }

        this.levelCounter = 0;

        Cursor.SetCursor(cursorTex, new Vector2(256,256), CursorMode.Auto);

        Initialize("START");
    }


    //INPUT SYSTEM
    //---------------------------

    void Update()
    {
        InputManager();
    }

    private void InputManager()
    {

        if(isNoMouseRange || isPaused)
        {
            return;
        }

        //Move and attack
        if (Input.GetMouseButtonDown(0) && (playerState == "SLOW" || playerState == "IDLE"))
        {
            speed = playerSpeed;
            Vector2 mousePos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
            plRb.velocity = (mousePos - (Vector2)player.position).normalized * speed;

            if (playerState != "IDLE")
                timer.stopTimer();

            idle.SetActive(false);
            fast.SetActive(true);
            playerState = "FAST";

            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i].GetComponent<EnemyAI>().recoverSpeed();
                enemies[i].GetComponent<deathTrigger>().setSlow(false);
            }

            GameObject[] bullets = GameObject.FindGameObjectsWithTag("EnemyBullet");
            for (int i = 0; i < bullets.Length; i++)
            {
                bullets[i].GetComponent<speedModifier>().recoverSpeed();
            }

        }

        //Entry in shooting mode
        if (Input.GetKeyDown(KeyCode.Space) && playerState == "FAST" && !ShotDone)
        {
            ShotDone = true;

            speed = slowSpeed;
            plRb.velocity = plRb.velocity.normalized * speed;
            playerState = "ATTACK";

            //CONTACT THE ENEMIES AND BULLETS
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i].GetComponent<EnemyAI>().slowDown(slowSpeed / 2);
                enemies[i].GetComponent<deathTrigger>().setSlow(true);
            }

            GameObject[] bullets = GameObject.FindGameObjectsWithTag("EnemyBullet");
            for (int i = 0; i < bullets.Length; i++)
            {
                bullets[i].GetComponent<speedModifier>().slowDownSpeed(slowSpeed);
            }

            audio.PlayOneShot(slow);

            timer.startTimer();
        }


        //Shootings
        if (Input.GetMouseButtonDown(0) && playerState == "ATTACK")
        {
            timer.stopTimer();

            speed = playerSpeed;
            plRb.velocity = plRb.velocity.normalized * speed;
            playerState = "FAST";

            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i].GetComponent<EnemyAI>().recoverSpeed();
                enemies[i].GetComponent<deathTrigger>().setSlow(false);
            }

            GameObject[] bullets = GameObject.FindGameObjectsWithTag("EnemyBullet");
            for (int i = 0; i < bullets.Length; i++)
            {
                bullets[i].GetComponent<speedModifier>().recoverSpeed();
            }

            Vector2 mousePos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GameObject new_bullet = Instantiate(bullet);
            new_bullet.transform.position = player.position;
            new_bullet.GetComponent<Rigidbody2D>().velocity = (mousePos - (Vector2)player.position).normalized * bulletSpeed;

            audio.PlayOneShot(laser);
        }


        //Restart level
        if (Input.GetKeyDown(KeyCode.R) && this.playerState != "DEATH")
        {
            if(this.isMenu)
                Initialize("MENU");
            else
                Initialize("RESTART");
        }

    }

    //---------------------------
    //INPUT SYSTEM END


    //timer event
    public void timeIsUp()
    {
        if (playerState == "FAST")
            return;

        speed = playerSpeed;
        plRb.velocity = plRb.velocity.normalized * speed;
        playerState = "FAST";
    }

    //enemy hit event
    public void enemyHitted(bool isBullet)
    { 
        verifyEnemiesStatus();

        if (isBullet)
            return;

        //if isn't from bullet set the player in slow mode and start timer

        ShotDone = false;

        speed = slowSpeed;
        plRb.velocity = plRb.velocity.normalized * speed;
        playerState = "SLOW";


        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].GetComponent<EnemyAI>().slowDown(slowSpeed / 2);
            enemies[i].GetComponent<deathTrigger>().setSlow(true);
        }

        GameObject[] bullets = GameObject.FindGameObjectsWithTag("EnemyBullet");
        for (int i = 0; i < bullets.Length; i++)
        {
            bullets[i].GetComponent<speedModifier>().slowDownSpeed(slowSpeed);
        }

        audio.PlayOneShot(slow);

        timer.startTimer();

    }

    //lives hit event
    public void heartHitted()
    {

        ShotDone = false;

        speed = slowSpeed;
        plRb.velocity = plRb.velocity.normalized * speed;
        playerState = "SLOW";

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].GetComponent<EnemyAI>().slowDown(slowSpeed / 2);
            enemies[i].GetComponent<deathTrigger>().setSlow(true);
        }

        GameObject[] bullets = GameObject.FindGameObjectsWithTag("EnemyBullet");
        for (int i = 0; i < bullets.Length; i++)
        {
            bullets[i].GetComponent<speedModifier>().slowDownSpeed(slowSpeed);
        }

        audio.PlayOneShot(heartSound);

        timer.startTimer();
    }


    //auxiliary function used to verify enemy count
    public void verifyEnemiesStatus()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemies.Length; i++)
        {
            if (!enemies[i].GetComponent<deathTrigger>().getDeath())
            {
                return;
            }
        }

        Debug.Log("Nemici finiti!");
        endButton.SetActive(true);
    }


    //level won event
    public void Win()
    {
        //player setting
        this.playerState = "WIN";
        plRb.velocity = Vector2.zero;
        
        //transition
        winTransition.SetTrigger("goTransition");
        spriteTransition.color = winColor;
        
        //game parameters update
        levelCounter++;
        levelText.text = levelCounter.ToString();

        //delay for transition
        StartCoroutine(Delay("WIN"));
    }

    //player death event
    public void playerDeath()
    {
        audio.PlayOneShot(deathSound, 0.4f);

        //player setting
        plRb.velocity = Vector2.zero;
        plRb.freezeRotation = true;
        idle.SetActive(false);
        fast.SetActive(false);
        playerState = "DEATH";

        //transition
        winTransition.SetTrigger("goTransition");
        spriteTransition.color = deathColor;

        //delay for transition
        StartCoroutine(Delay("DEATH"));
    }



    //initilization of every level in every mode
    public void Initialize(string mode)
    {

        ShotDone = false;

        //player settings
        plRb.velocity = Vector2.zero;
        plRb.freezeRotation = true;
        speed = playerSpeed;
        playerState = "IDLE";
        idle.SetActive(true);
        fast.SetActive(false);

        //if menu mode stop
        if (mode == "MENU")
        {
            player.position = new Vector2(0, -4f);
            return;
        }

        endButton.SetActive(false);
        player.position = Vector2.zero;

        //Destroy all enemies, bullets and hearts
        if (mode != "START")
        {
            GameObject[] enem = GameObject.FindGameObjectsWithTag("Enemy");
            for (int i = 0; i < enem.Length; i++)
            {
                Destroy(enem[i]);
            }

            GameObject[] bullets = GameObject.FindGameObjectsWithTag("EnemyBullet");
            for (int i = 0; i < bullets.Length; i++)
            {
                Destroy(bullets[i]);
            }

            GameObject[] liv = GameObject.FindGameObjectsWithTag("heart");
            for (int i = 0; i < liv.Length; i++)
            {
                Destroy(liv[i]);
            }
        }


        //manage different modalities
        if (mode == "WIN") {

            creator.addDifficulty();
            winTransition.SetTrigger("goReturn");

        } 
        else if(mode == "RESTART") {

            timer.stopTimer();
            
        }
        else if(mode == "DEATH") {

            winTransition.SetTrigger("goReturn");
        }

        //if lives are finished return to menu

        if(mode == "START")
        {
            creator.newLevel("START");
        } else
        {
            creator.newLevel("GAME");
        }
    }
    

    //auxiliary functions to set and get the player state
    public string getState()
    {
        return this.playerState;
    }

    public void setState(string mode)
    {
        this.playerState = mode;
    }

    public void setIsNoMouseRange(bool value)
    {
        this.isNoMouseRange = value;
    }

    public void setIsPaused(bool value)
    {
        this.isPaused = value;
    }


    //dealy for the initilization
    IEnumerator Delay(string type)
    {
        yield return new WaitForSeconds(0.9f);
        Initialize(type);
    }

}
