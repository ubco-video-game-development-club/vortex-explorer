using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vortex : MonoBehaviour
{
    private float radius;
    private float innerStrength;
    private float outerStrength;
    private float forceThreshold;
    private float radiusThreshold;
    private LayerMask targetLayers;

    void FixedUpdate() {
        // get all nearby physics objects
        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, radius, targetLayers);

        foreach (Collider2D target in targets) {
            PullTarget pullTarget = target.GetComponent<PullTarget>();

            // get the direction from the target to the vortex
            Vector2 pullDirection = transform.position - pullTarget.transform.position;

            // get the distance from the target to the vortex
            float targetDistance = Vector2.Distance(transform.position, pullTarget.transform.position);

            // calculate the effect of the pull based on the distance
            float distanceScale = targetDistance / radius;
            float pullEffect = Mathf.Lerp(innerStrength, outerStrength, distanceScale);

            // pull the target towards the vortex
            pullTarget.Pull(pullDirection * pullEffect);

            // crush the object if it gets too close and the vortex is big enough
            if (pullEffect > forceThreshold && radius > radiusThreshold) {
                Debug.Log("Oof");

                // if the target is the player
                Player p;
                if (pullTarget.TryGetComponent<Player>(out p)) {
                    // disable the player's movement
                    p.DisableMovement();
                }

                // crush the target
                pullTarget.Crush(transform.position);
            }
        }
    }

    public void Activate(float radius, float innerStrength, float outerStrength, float forceThreshold, float radiusThreshold, LayerMask targetLayers) {
        // set the instance variables of this vortex
        this.radius = radius;
        this.innerStrength = innerStrength;
        this.outerStrength = outerStrength;
        this.forceThreshold = forceThreshold;
        this.radiusThreshold = radiusThreshold;
        this.targetLayers = targetLayers;

        // set the scale of the gameobject based on its radius
        transform.localScale = Vector2.one * radius * 2;
    }
}
