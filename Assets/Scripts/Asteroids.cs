using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroids : MonoBehaviour
{
    private Rigidbody2D rb2D;
    //private float speed = -0.2f;
    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        rb2D.AddForce(Vector2.left * 0.2f, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void FixedUpdate()
    {
        rb2D.rotation += 1.0f;
    }
}
