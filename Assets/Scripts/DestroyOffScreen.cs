using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOffScreen : MonoBehaviour
{
    void Update() {
        float boundX = Camera.main.ScreenToWorldPoint(Vector3.zero).x;
        if (transform.position.x + transform.localScale.x < boundX) {
            Destroy(gameObject);
        }
    }
}
