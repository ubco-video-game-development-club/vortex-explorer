using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float rotationSpeed = 1f;

    private float moveSpeed;
    private Rigidbody2D rb2D;

    void Awake() {
        rb2D = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        // rotate over time
        rb2D.rotation += rotationSpeed;

        // maintain the same magnitude of velocity
        rb2D.velocity = rb2D.velocity.normalized * moveSpeed;
    }

    public void Push(Vector2 force) {
        rb2D.AddForce(force, ForceMode2D.Impulse);

        // save the starting velocity magnitude
        moveSpeed = force.magnitude;
    }
}
