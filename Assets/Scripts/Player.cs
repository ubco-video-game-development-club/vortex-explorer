using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;

    private bool canMove;
    private Rigidbody2D rb2D;

    void Awake() {
        rb2D = GetComponent<Rigidbody2D>();
    }

    void Start() {
        canMove = true;

        // give the player some initial velocity to the right
        rb2D.AddForce(Vector2.right * speed, ForceMode2D.Impulse);
    }

    void FixedUpdate() {
        if (!canMove) {
            return;
        }

        // get the current direction of movement
        Vector2 dir = rb2D.velocity.normalized;

        // rotate the player to face its current direction
        transform.rotation = Quaternion.FromToRotation(Vector2.up, dir);

        // lock the player's velocity to the set speed
        rb2D.velocity = dir * speed;
    }

    public void DisableMovement() {
        canMove = false;
    }
}
