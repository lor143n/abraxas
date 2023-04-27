using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOrientation : MonoBehaviour
{

    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    void Update()
    {

        if (gameManager.getState() == "IDLE")
        {

            Vector2 mousePos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mousePos - (Vector2)transform.position).normalized;

            float zAngle = Mathf.Rad2Deg * Mathf.Acos(direction.x) * Mathf.Sign(direction.y);

            transform.rotation = Quaternion.Euler(0, 0, zAngle);
        } else if (gameManager.getState() == "FAST")
        {

            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            Vector2 direction = (rb.velocity - (Vector2) transform.position).normalized;

            float zAngle = Mathf.Rad2Deg * Mathf.Acos(direction.x) * Mathf.Sign(direction.y);

            transform.rotation = Quaternion.Euler(0, 0, zAngle);
        }

    }
}
