using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector2 spawnPosition;
    public float speed;

    private Rigidbody2D rb2D;

    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        // spawn the player at the spawn position
        transform.position = spawnPosition;

        // give the player some initial velocity to the right
        rb2D.AddForce(Vector2.right * speed, ForceMode2D.Impulse);
    }
}
