using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullTarget : MonoBehaviour
{
    public float crushDuration = 2f;

    private bool isBeingCrushed;
    private Vector2 currentVelocity;
    private Rigidbody2D rb2D;

    void Awake() {
        rb2D = GetComponent<Rigidbody2D>();
    }

    public void Pull(Vector2 force) {
        if (!isBeingCrushed) {
            rb2D.AddForce(force, ForceMode2D.Force);
        }
    }

    public void Crush(Vector2 centerPoint) {
        if (!isBeingCrushed) {
            isBeingCrushed = true;
            // stop all physics movement
            rb2D.constraints = RigidbodyConstraints2D.FreezePosition;
            // run a timed function that will pull the object towards the target point over a certain amount of time
            StartCoroutine(PullToCenter(centerPoint));
            // add points if this is an asteroid
            Asteroid asteroid;
            if (TryGetComponent<Asteroid>(out asteroid)) {
                GameController.instance.AddVortexCrushPoints();
            }
        }
    }

    private IEnumerator PullToCenter(Vector2 centerPoint) {
        // store the initial scale of the object for interpolation
        Vector2 startScale = transform.localScale;

        // every frame for crushDuration seconds, spin/shrink/pull the object towards the center point
        float crushTimer = 0;
        while (crushTimer < crushDuration) {
            crushTimer += Time.deltaTime;

            // pull  the object towards the center point
            transform.position = Vector2.SmoothDamp(transform.position, centerPoint, ref currentVelocity, crushDuration);
            // shrink the object the closer it gets
            transform.localScale = Vector2.Lerp(startScale, Vector2.zero, crushTimer / crushDuration);
            // spin the object over time
            transform.rotation *= Quaternion.AngleAxis(360f / crushDuration * Time.deltaTime, Vector3.forward);

            // wait one frame
            yield return null;
        }

        // destroy this gameobject or end the game if this is the player
        Player player;
        if (TryGetComponent<Player>(out player)) {
            GameController.instance.LoseGame();
        } else {
            Destroy(gameObject);
        }
    }
}
