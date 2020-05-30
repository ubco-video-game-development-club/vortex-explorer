using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 0.001f;
    public float deceleration = 0.0001f;
    public float hitPoints = 100f;
    public float maxSpeed = 0.001f;
    public float horizontalSpeed = 0;
    public float verticalSpeed = 0;
 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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
        horizontalSpeed += (0f - horizontalSpeed) * 0.02f;
        verticalSpeed += (0f - verticalSpeed) * 0.02f;

        float horizontalMovement = horizontalSpeed;
        float verticalMovement = verticalSpeed;

        //assign movement to a single vector3
        Vector3 directionOfMovement = new Vector3(horizontalMovement, verticalMovement, 0);

        // apply movement to player's transform
        gameObject.transform.Translate(directionOfMovement);
    }
}
