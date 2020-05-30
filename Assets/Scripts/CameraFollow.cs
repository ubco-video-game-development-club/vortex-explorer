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

    private Player player;
    private Vector2 currentVelocity;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void FixedUpdate()
    {
        // get the player's x position value
        float playerX = player.transform.position.x;

        // store the current target to move towards
        Vector2 target = transform.position;

        // determine whether to follow the player based on bounds
        float distToPlayerX = playerX - transform.position.x;
        if (distToPlayerX < leftBound) {
            target = (playerX - leftBound) * Vector2.right;
        } else if (distToPlayerX > rightBound) {
            target = (playerX - rightBound) * Vector2.right;
        }

        // move the camera towards the current target
        Vector3 movement = Vector2.SmoothDamp(transform.position, target, ref currentVelocity, followTime);

        // maintain the camera's current z offset
        Vector3 zOffset = transform.position.z * Vector3.forward;

        // adjust the camera's current position with the movement from this frame
        transform.position = movement + zOffset;
    }
}
