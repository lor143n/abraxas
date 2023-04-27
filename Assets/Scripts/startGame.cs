using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class startGame : MonoBehaviour
{


    private Animator winTransition;
    private Image spriteTransition;
    public Color winColor;

    private void Awake()
    {
        winTransition = GameObject.Find("TransitionManager").GetComponent<Animator>();
        spriteTransition = winTransition.gameObject.GetComponent<Image>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            spriteTransition.color = winColor;
            winTransition.SetTrigger("goTransition");
            StartCoroutine(Delay());
        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Game");
    }
}
