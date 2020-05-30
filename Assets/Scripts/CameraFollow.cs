using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Tooltip("Time it takes to move to the player's current position.")]
    public float followTime;

    private Player player;
    private Vector2 currentVelocity;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void FixedUpdate()
    {
        // we only want to get the player's horizontal movement
        Vector2 playerHorizontal = player.transform.position * Vector2.right;

        // move the camera towards the player in the horizontal direction
        Vector3 movement = Vector2.SmoothDamp(transform.position, playerHorizontal, ref currentVelocity, followTime);

        // maintain the camera's current z offset
        Vector3 zOffset = transform.position.z * Vector3.forward;

        // adjust the camera's current position with the movement from this frame
        transform.position = movement + zOffset;
    }
}
