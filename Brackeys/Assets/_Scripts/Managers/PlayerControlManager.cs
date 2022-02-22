using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerControlManager : MonoBehaviour {

    #region Variables
    private float lerpDuration = 0.4f;
    private bool isMoving = false;

    private Animator anim;
    private SpriteRenderer sr;

    [SerializeField]
    private bool isControlRandom = true;
    private Dictionary<Dir, Dir> controlSubstitute;
    public enum Dir { up, down, left, right, stop};

    #endregion

    private void Awake() {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start() {
        controlSubstitute = new Dictionary<Dir, Dir>();
        if (isControlRandom) {
            RandomizeControl();
        } else {
            BaseControl();
        }
    }

    private void Update() {
        if (!isMoving) {
            Dir pressedDirection = Dir.stop;
            if (Input.GetKeyDown(KeyCode.UpArrow)) {
                pressedDirection = Dir.up;
            } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
                pressedDirection = Dir.down;
            } else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                pressedDirection = Dir.left;
            } else if (Input.GetKeyDown(KeyCode.RightArrow)) {
                pressedDirection = Dir.right;
            }
            if(pressedDirection != Dir.stop) ControlFromDir(pressedDirection);
        }
    }

    private void RandomizeControl() {
        BaseControl();
        for(int i=0; i<4; i++) {
            int index1 = Random.Range(0, 4);
            int index2 = Random.Range(0, 4);
            Dir key1 = controlSubstitute.ElementAt(index1).Key;
            Dir key2 = controlSubstitute.ElementAt(index2).Key;
            Dir val1 = controlSubstitute.ElementAt(index1).Value;
            Dir val2 = controlSubstitute.ElementAt(index2).Value;
            controlSubstitute[key1] = val2;
            controlSubstitute[key2] = val1;
        }
    }

    private void BaseControl() {
        controlSubstitute.Add(Dir.up, Dir.up);
        controlSubstitute.Add(Dir.down, Dir.down);
        controlSubstitute.Add(Dir.left, Dir.left);
        controlSubstitute.Add(Dir.right, Dir.right);
    }

    private void ControlFromDir(Dir pressedDirection) {
        Dir substituteDirection = controlSubstitute[pressedDirection];
        UiManager.Instance.AddDirectionGuide(pressedDirection, substituteDirection);
        switch (substituteDirection) {
            case Dir.up:
                anim.SetTrigger("back");
                MoveToward(new Vector2(0, 1));
                break;
            case Dir.down:
                anim.SetTrigger("face");
                MoveToward(new Vector2(0, -1));
                break;
            case Dir.left:
                anim.SetTrigger("left");
                MoveToward(new Vector2(-1, 0));
                break;
            case Dir.right:
                anim.SetTrigger("right");
                MoveToward(new Vector2(1, 0));
                break;
        }
    }

    private void MoveToward(Vector2 direction) {
        RaycastHit2D hit = Physics2D.Raycast(this.gameObject.transform.position, direction);
        if (hit.collider != null) {
            isMoving = true;
            Vector2 destination = hit.collider.gameObject.CompareTag("Finish") ? hit.point + direction / 2 : hit.point - direction / 2;
            StartCoroutine(GoToDestination(destination));
        }
    }

    private IEnumerator GoToDestination(Vector2 destination) {
        float timeElapsed = 0;
        while (timeElapsed < lerpDuration) {
            transform.position = Vector2.Lerp(transform.position, destination, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            sr.sortingOrder = WallSpawner.Instance.GetArenaSize() - Mathf.FloorToInt(transform.position.y);
            yield return null;
        }
        transform.position = destination;
        isMoving = false;
    }
}
