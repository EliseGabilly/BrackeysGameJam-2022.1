using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlManager : MonoBehaviour {

    #region Variables
    private float lerpDuration = 0.4f;
    private bool isMoving = false;

    private Animator anim;
    #endregion

    private void Awake() {
        anim = GetComponent<Animator>();
    }

    private void Update() {
        if (!isMoving) {
            if (Input.GetKeyDown(KeyCode.UpArrow)) {
                anim.SetTrigger("back");
                MoveToward(new Vector2(0, 1));
            } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
                anim.SetTrigger("face");
                MoveToward(new Vector2(0, -1));
            } else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                anim.SetTrigger("left");
                MoveToward(new Vector2(-1, 0));
            } else if (Input.GetKeyDown(KeyCode.RightArrow)) {
                anim.SetTrigger("right");
                MoveToward(new Vector2(1, 0));
            }
        }
    }

    private void MoveToward(Vector2 direction) {
        RaycastHit2D hit = Physics2D.Raycast(this.gameObject.transform.position, direction);
        if (hit.collider != null) {
            isMoving = true;
            Vector2 destination = hit.point - direction/2;
            StartCoroutine(GoToDestination(destination));
        }
    }

    private IEnumerator GoToDestination(Vector2 destination) {
        float timeElapsed = 0;
        while (timeElapsed < lerpDuration) {
            transform.position = Vector2.Lerp(transform.position, destination, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = destination;
        isMoving = false;
    }
}
