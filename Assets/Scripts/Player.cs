using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public double deceleration = 0.0001;
    public float hitPoints = 100f;
    public double maxSpeed = 0.001;
    public double horizontalSpeed = 0;
    public double verticalSpeed = 0;
    public int frameCounter = 0;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Player::Start cant find RigidBody2D </sadface>");
        }
    }

    // Update is called once per frame
    void Update()
    {   /*
        frameCounter++;
        if (frameCounter % 3 == 0)
        {
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {

                // convert user input into world movement
                horizontalSpeed += Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime;
                verticalSpeed += Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime;


            }
            if (horizontalSpeed > maxSpeed)
            {
                horizontalSpeed = maxSpeed;
            }
            else if (horizontalSpeed < 0 - maxSpeed)
            {
                horizontalSpeed = 0 - maxSpeed;
            }
            if (verticalSpeed > maxSpeed)
            {
                verticalSpeed = maxSpeed;
            }
            else if (verticalSpeed < 0 - maxSpeed)
            {
                verticalSpeed = 0 - maxSpeed;
            }
            horizontalSpeed += (0 - horizontalSpeed) * 0.02;
            verticalSpeed += (0 - verticalSpeed) * 0.02;

            float horizontalMovement = (float)horizontalSpeed;
            float verticalMovement = (float)verticalSpeed;

            //assign movement to a single vector3
            Vector2 directionOfMovement = new Vector2(horizontalMovement, verticalMovement);

            // apply movement to player's transform
            gameObject.transform.Translate(directionOfMovement);
        }
        */
    }

    // this is called at a fixed interval for use with physics objects like the RigidBody2D
    void FixedUpdate()
    {
        // check if user has pressed some input keys
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {

            // convert user input into world movement
            float horizontalMovement = Input.GetAxisRaw("Horizontal") * moveSpeed;
            float verticalMovement = Input.GetAxisRaw("Vertical") * moveSpeed;

            //assign world movements to a Veoctor2
            Vector2 directionOfMovement = new Vector2(horizontalMovement, verticalMovement);

            // apply movement to player's transform
            rb.AddForce(directionOfMovement);
            Debug.Log(horizontalMovement);
        }
    }
}
