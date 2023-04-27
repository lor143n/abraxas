using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class speedModifier : MonoBehaviour
{
    
    private Rigidbody2D rb;
    private float standardSpeed;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void slowDownSpeed(float speed)
    {
        rb.velocity = rb.velocity.normalized * speed;
    }

    public void recoverSpeed()
    {
        rb.velocity = rb.velocity.normalized * standardSpeed;
    }

    public void setStandardSpeeed(float speed)
    {
        this.standardSpeed = speed;
    }
}
