using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float initialSpeed = 3f;
    public float minSpeed = 2f;
    public float maxSpeed = 4f;

    private bool canMove;
    private Rigidbody2D rb2D;

    void Awake() {
        rb2D = GetComponent<Rigidbody2D>();
    }

    void Start() {
        canMove = true;

        // give the player some initial velocity to the right
        rb2D.AddForce(Vector2.right * initialSpeed, ForceMode2D.Impulse);
    }

    void Update() {
        // die if the player goes too far back to the left
        float boundX = Camera.main.ScreenToWorldPoint(Vector3.zero).x;
        if (transform.position.x + transform.localScale.x < boundX) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
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
        float speed = Mathf.Clamp(rb2D.velocity.magnitude, minSpeed, maxSpeed);
        rb2D.velocity = dir * speed;
    }

    public void DisableMovement() {
        canMove = false;
    }
}
