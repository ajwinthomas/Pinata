using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    public static event Action OnPlayerDeath;
    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();   
    }
    private void OnEnable()
    {
        OnPlayerDeath += kill;
    }

    private void OnDisable()
    {
        OnPlayerDeath -= kill;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Trap"))
        {
            Die();
        }
    }

    public void Die()
    {
        OnPlayerDeath?.Invoke();
    }



    void kill()
    {
        rb.AddForce(Vector2.up * 250);
        gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
        //gameObject.SetActive(false);
    }
}
