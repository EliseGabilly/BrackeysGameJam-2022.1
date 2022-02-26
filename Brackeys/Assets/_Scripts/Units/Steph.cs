using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steph : MonoBehaviour {

    public Vector3 EndPos { get; set; }
    private float lerpDuration = 1.5f;

    private void Start() {
        StartCoroutine(LateStart());
    }

    private IEnumerator LateStart() {
        yield return new WaitForSeconds(0.5f);
        Vector2 dest = new Vector2(EndPos.x, EndPos.y);
        StartCoroutine(GoToDestination(dest));
    }

    private IEnumerator GoToDestination(Vector2 destination) {
        float timeElapsed = 0;
        while (timeElapsed < lerpDuration) {
            transform.position = Vector2.Lerp(transform.position, destination, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = destination;
        Destroy(gameObject);
    }
}
