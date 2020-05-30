using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public double moveSpeed = 0.001;
    public double deceleration = 0.0001;
    public double hitPoints = 100;
    public double maxSpeed = 0.001;
    public double horizontalSpeed = 0;
    public double verticalSpeed = 0;
    public int frameCounter = 0;
 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
            Vector3 directionOfMovement = new Vector3(horizontalMovement, verticalMovement, 0);

            // apply movement to player's transform
            gameObject.transform.Translate(directionOfMovement);
        }
    }
}
