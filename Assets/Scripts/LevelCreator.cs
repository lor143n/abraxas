using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LevelCreator : MonoBehaviour
{
    public Transform[] endPositions;
    public GameObject[] enemyPositions;

    [Range(0, 100)] public int normalSpawnPercentage;
    [Range(0, 100)] public int shellSpawnPercentage;
    [Range(0, 100)] public int shooterSpawnPercentage;
    [Range(0, 100)] public int heartSpawnPercentage;

    public AudioClip lightSound;

    public GameObject enemy;
    public GameObject shooter;
    public GameObject shell;
    public GameObject heart;

    private GameObject endButton;
    private AudioSource lightsAudio;
    private AudioSource soundtrackSource;
    private float difficulty;

    private void Awake()
    {
        endButton = GameObject.Find("EndButton");
        lightsAudio = GetComponent<AudioSource>();
        soundtrackSource = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>();
        difficulty = 1f;
    }

    private void levelGenerator()
    {
        //Scegliere un numero n di nemici (quanti shooter e quanti shell)

        //Posizionare end button in una delle 4 posizioni
        int endRandPosition = Random.Range(0, 4);
        endButton.transform.position = endPositions[endRandPosition].position;
        endButton.transform.rotation = endPositions[endRandPosition].rotation;

        //Distribuire n punti equidistanti e spawnare i nemici

        sattoloShuffling(enemyPositions);

        int normalCount = (int)Mathf.Round(difficulty * ((float)normalSpawnPercentage / 100));
        int shellCount = (int)Mathf.Round(difficulty * ((float)shellSpawnPercentage / 100));
        int shooterCount = (int)Mathf.Round(difficulty * ((float)shooterSpawnPercentage / 100));
        int heartCount = (int)Mathf.Round(difficulty * ((float)heartSpawnPercentage / 100));

        for (int i = 0; i < normalCount; i++)
        {
            enemy.transform.position = enemyPositions[i].transform.position;
            Instantiate(enemy);
        }

        for (int i = normalCount; i < normalCount + shellCount; i++)
        {
            shell.transform.position = enemyPositions[i].transform.position;
            Instantiate(shell);
        }

        for (int i = normalCount + shellCount; i < normalCount + shellCount + shooterCount; i++)
        {
            Debug.Log(i);
            shooter.transform.position = enemyPositions[i].transform.position;
            Instantiate(shooter);
        }

        if (heartCount > 0)
        {

            float yesHeart = Random.Range(0f, 1f);

            if (yesHeart < 0.4)
            {
                int heartIndex = normalCount + shellCount + shooterCount;
                Random.Range(heartIndex, enemyPositions.Length);
                heart.transform.position = enemyPositions[Random.Range(heartIndex, enemyPositions.Length)].transform.position;
                GameObject aus = Instantiate(heart);
            }
        }
    }

    public void newLevel(string type) //START, GAME
    {
        if (type == "START")
        {
            GameObject[] lights = GameObject.FindGameObjectsWithTag("Light");

            for (int i = 0; i < lights.Length; i++)
            {
                lights[i].GetComponent<Light2D>().intensity = 0;
            }

            StartCoroutine(firstGameDelay());
        } else
        {
            levelGenerator();
        }
    }

    public void addDifficulty()
    {
        if (difficulty > 15)
        {
            return;
        }

        this.difficulty += 0.5f;
    }


    private void sattoloShuffling(GameObject[] source)
    {
        int i = source.Length;

        while(i > 1){

            i = i - 1;
            int j = Random.Range(0, i+1);

            GameObject aus = source[j];
            source[j] = source[i];
            source[i] = aus;
        }
    }

    IEnumerator firstGameDelay()
    {
        yield return new WaitForSeconds(2f);

        GameObject[] lights = GameObject.FindGameObjectsWithTag("Light");

        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].GetComponent<Light2D>().intensity = 1f;
        }

        lightsAudio.PlayOneShot(lightSound);
        soundtrackSource.Play();

        levelGenerator();
    }
}
