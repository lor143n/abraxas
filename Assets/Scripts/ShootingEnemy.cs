using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ShootingEnemy : MonoBehaviour
{
    public GameObject bullet;
    public float bulletSpeed;
    public float burstTime;
    public Transform shooterPointTop;
    public Transform shooterPointDown;
    public AudioClip shot;

    private deathTrigger death;
    private AudioSource audio;

    // Update is called once per frame
    private void Start()
    {
        death = GetComponent<deathTrigger>();
        audio = GetComponent<AudioSource>();

        gameObject.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
        gameObject.GetComponent<Rigidbody2D>().angularVelocity = 100;
    }

    IEnumerator shootingRoutine()
    {
        yield return new WaitForSeconds(burstTime);
        Shooting();
        StartCoroutine(shootingRoutine());
    }

    private void Shooting()
    {

        if (death.getDeath())
            return;

        if (death.getSlow())
            return;

        GameObject bullTop = new GameObject();
        GameObject bullDown = new GameObject();

        bullTop = Instantiate(bullet);
        bullDown = Instantiate(bullet);
        bullTop.transform.position = gameObject.transform.position;
        bullTop.GetComponent<speedModifier>().setStandardSpeeed(bulletSpeed);       
        bullTop.GetComponent<Rigidbody2D>().velocity = (((Vector2) shooterPointTop.position) - (Vector2)gameObject.transform.position).normalized * bulletSpeed;
        bullDown.transform.position = gameObject.transform.position;
        bullDown.GetComponent<speedModifier>().setStandardSpeeed(bulletSpeed);
        bullDown.GetComponent<Rigidbody2D>().velocity = (((Vector2)shooterPointDown.position) - (Vector2)gameObject.transform.position).normalized * bulletSpeed;

        audio.PlayOneShot(shot,0.6f);
    }

    public void startShooting()
    {
        StartCoroutine(shootingRoutine());
    }
}
