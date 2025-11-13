using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private GameManager gameManager;
    private AudioManager audioManager;
    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        audioManager = FindAnyObjectByType<AudioManager>();

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            audioManager.PlayCoinSound();
            Destroy(collision.gameObject);
            gameManager.AddScore(1);
        }
        else if (collision.CompareTag("Trap"))
        {
            gameManager.GameOver();
        }
        else if (collision.CompareTag("Enemy"))
        {
            Rigidbody2D playerRb = GetComponent<Rigidbody2D>();

            if (playerRb.velocity.y < -0.1f)  
            {
                Rigidbody2D enemyRb = collision.GetComponent<Rigidbody2D>();
                if (enemyRb != null)
                {
                    enemyRb.velocity = new Vector2(0, -3f);  
                }

                gameManager.AddScore(3);
                Destroy(collision.gameObject, 0.5f); 
            }
            else
            {
                gameManager.GameOver();
            }
        }
        else if (collision.CompareTag("Key"))
        {
            Destroy(collision.gameObject);
            gameManager.GameWin();
        }
        else if (collision.CompareTag("River"))
        {
            gameManager.GameOver();
        }
    }
}
