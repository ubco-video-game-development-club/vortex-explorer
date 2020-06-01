using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Tooltip("The max distance from the camera position the player can be to the left.")]
    public float leftBound = -6f;
    [Tooltip("The max distance from the camera position the player can be to the right.")]
    public float rightBound = 1f;
    [Tooltip("Time it takes to move to the player's current position.")]
    public float followTime = 0.1f;

    private Transform target;
    private Vector2 currentVelocity;

    public void Follow(Transform target) {
        this.target = target;
    }

    void FixedUpdate() {
        if (target == null) {
            return;
        }

        // get the player's x position value
        float targetX = target.position.x;

        // store the current target to move towards
        Vector2 targetPos = transform.position;

        // determine whether to follow the target based on bounds
        float distToPlayerX = targetX - transform.position.x;
        if (distToPlayerX < leftBound) {
            targetPos = (targetX - leftBound) * Vector2.right;
        } else if (distToPlayerX > rightBound) {
            targetPos = (targetX - rightBound) * Vector2.right;
        }

        // move the camera towards the current target
        Vector3 movement = Vector2.SmoothDamp(transform.position, targetPos, ref currentVelocity, followTime);

        // maintain the camera's current z offset
        Vector3 zOffset = transform.position.z * Vector3.forward;

        // adjust the camera's current position with the movement from this frame
        transform.position = movement + zOffset;
    }
}
