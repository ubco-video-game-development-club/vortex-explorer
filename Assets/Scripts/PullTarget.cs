using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PullTarget : MonoBehaviour
{
    public float crushDuration = 2f;

    private Vector2 currentVelocity;

    public void Crush(Vector2 centerPoint) {
        StartCoroutine(PullToCenter(centerPoint));
    }

    private IEnumerator PullToCenter(Vector2 centerPoint) {
        float crushTimer = 0;
        while (crushTimer < crushDuration) {
            crushTimer += Time.deltaTime;
            transform.position = Vector2.SmoothDamp(transform.position, centerPoint, ref currentVelocity, crushDuration);
            transform.localScale = Vector2.Lerp(transform.localScale, Vector2.zero, crushTimer / crushDuration);
            transform.rotation *= Quaternion.AngleAxis(360f / crushDuration * Time.deltaTime, Vector3.forward);
            yield return null;
        }
    }
}
