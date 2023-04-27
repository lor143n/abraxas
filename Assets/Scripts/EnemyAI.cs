using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    [Range(0f, 2f)]
    public float enemySpeed;

    private Transform player;
    private Rigidbody2D rb;
    private deathTrigger death;
    private float realtimeSpeed;
    private bool isStarted = false;

    public bool isShooter;

    void Start()
    {
        isStarted = false;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        death = GetComponent<deathTrigger>();
        rb = GetComponent<Rigidbody2D>();
        recoverSpeed();

        StartCoroutine(enemiesStartDelay());
    }

    void Update()
    {

        if (!isStarted)
            return;

        if (death.getDeath())
            return;

        rb.velocity = (player.position - transform.position).normalized * realtimeSpeed;

        if (isShooter)
            return;

        Vector2 direction = rb.velocity.normalized;

        float zAngle = Mathf.Rad2Deg * Mathf.Acos(direction.x) * Mathf.Sign(direction.y);

        gameObject.transform.rotation = Quaternion.Euler(0, 0, zAngle);
    }

    public void startEnemy()
    {
        this.isStarted = true;

        if(GetComponent<ShootingEnemy>() != null)
            GetComponent<ShootingEnemy>().startShooting();
    }


    public void slowDown(float speed)
    {
        this.realtimeSpeed = speed;
    }

    public void recoverSpeed()
    {
        this.realtimeSpeed = enemySpeed;
    }

    IEnumerator enemiesStartDelay()
    {
        yield return new WaitForSeconds(0.8f);
        startEnemy();

    }
}
