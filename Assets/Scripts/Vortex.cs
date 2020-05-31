using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vortex : MonoBehaviour
{
    public float destroyDistance = 0.3f;
    public float baseRadius = 0.5f;
    public float baseInnerStrength = 2f;
    public float baseOuterStrength = 1f;

    private Player player;
    private float radius;
    private float innerStrength;
    private float outerStrength;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void FixedUpdate() {
        // get all nearby physics objects
        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (Collider2D target in targets) {
            // get the direction from the target to the vortex
            Vector2 pullDirection = transform.position - target.transform.position;

            // get the distance from the target to the vortex
            float targetDistance = Vector2.Distance(transform.position, target.transform.position);

            // calculate the effect of the pull based on the distance
            float distanceScale = (targetDistance - destroyDistance) / (radius - destroyDistance);
            float pullEffect = Mathf.Lerp(innerStrength, outerStrength, distanceScale);

            // pull the target towards the vortex
            target.GetComponent<Rigidbody2D>().AddForce(pullDirection * pullEffect, ForceMode2D.Force);

            // crush the object if it gets too close
            if (targetDistance < destroyDistance) {
                Player p;
                if (target.TryGetComponent<Player>(out p)) {
                    p.DisableMovement();
                }
                target.GetComponent<PullTarget>().Crush(transform.position);
            }
        }
        if (player.transform.position.x - this.transform.position.x > 20) {
            Destroy(this.gameObject);
        }


    }

    public void Activate(float powerLevel) {
        // scale the base radius and strength values by the power level
        radius = baseRadius * powerLevel;
        innerStrength = baseInnerStrength * powerLevel;
        outerStrength = baseOuterStrength * powerLevel;

        // set the scale of the gameobject based on its radius
        transform.localScale = Vector2.one * radius * 2;
    }
}
