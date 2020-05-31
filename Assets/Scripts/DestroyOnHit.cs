using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DestroyOnHit : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col) {
        Player player;
        if (col.TryGetComponent<Player>(out player)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        } else {
            Destroy(gameObject);
        }
    }
}
