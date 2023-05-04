using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ShootingEnemy : MonoBehaviour
{
    public GameObject bullet;
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

        bullet.transform.position = gameObject.transform.position;
        GameObject bull1 = Instantiate(bullet);
        bull1.GetComponent<Rigidbody2D>().velocity = (((Vector2)shooterPointTop.position) - (Vector2)gameObject.transform.position).normalized * bull1.GetComponent<speedModifier>().standardSpeed;

        GameObject bull2 = Instantiate(bullet);
        bull2.GetComponent<Rigidbody2D>().velocity = (((Vector2)shooterPointDown.position) - (Vector2)gameObject.transform.position).normalized * bull2.GetComponent<speedModifier>().standardSpeed;

        audio.PlayOneShot(shot,0.6f);
    }

    public void startShooting()
    {
        StartCoroutine(shootingRoutine());
    }
}
