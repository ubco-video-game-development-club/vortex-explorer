using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentBackground : MonoBehaviour
{
    public GameObject backgroundObject;

    void Update()
    {
        backgroundObject.transform.position = new Vector2(transform.position.x, transform.position.y);
    }
}
