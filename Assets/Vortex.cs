using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vortex : MonoBehaviour
{
    public float radius = 1f;
    public float strength = 1f;

    private Player player;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void FixedUpdate() {
        // get all nearby physics objects
        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (Collider2D target in targets) {
            // get the direction from the target to the vortex
            Vector2 pullDirection = transform.position - target.transform.position;

            // pull the target towards the vortex
            target.GetComponent<Rigidbody2D>().AddForce(pullDirection * strength, ForceMode2D.Force);
        }
    }
}
