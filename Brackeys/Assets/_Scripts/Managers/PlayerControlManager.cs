using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerControlManager : Singleton<PlayerControlManager> {

    #region Variables
    private float lerpDuration = 0.4f;
    private bool isMoving = false;

    private Animator anim;
    private SpriteRenderer sr;

    [SerializeField]
    private Dictionary<Dir, Dir> controlSubstitute;
    public enum Dir { up, down, left, right, stop};

    #endregion

    protected override void Awake() {
        base.Awake();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start() {
        controlSubstitute = new Dictionary<Dir, Dir>();
        BaseControl();
        StartCoroutine(LateStart());
    }

    private IEnumerator LateStart() {
        yield return new WaitForSeconds(0.01f);
        RandomizeControl();
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

    private void BaseControl() {
        controlSubstitute.Add(Dir.up, Dir.up);
        controlSubstitute.Add(Dir.down, Dir.down);
        controlSubstitute.Add(Dir.left, Dir.left);
        controlSubstitute.Add(Dir.right, Dir.right);
    }

    private void RandomizeControl() {        
        int lvl = Player.Instance.level;
        if (lvl <= 5) {
            return;
        } else if (lvl <= 10) {
            EasyRandomizeControl();
        } else if (lvl <= 15) {
            MediumRandomizeControl();
        } else {
            HardRandomizeControl();
        } 
    }

    private void EasyRandomizeControl() {
        //invert either up/down or left/right
        int ran = Random.Range(0, 2);

        int index1 = ran==0 ? 0 : 2;
        int index2 = ran == 0 ? 1 : 3;
        Dir key1 = controlSubstitute.ElementAt(index1).Key;
        Dir key2 = controlSubstitute.ElementAt(index2).Key;
        Dir val1 = controlSubstitute.ElementAt(index1).Value;
        Dir val2 = controlSubstitute.ElementAt(index2).Value;
        controlSubstitute[key1] = val2;
        controlSubstitute[key2] = val1;
    }

    private void MediumRandomizeControl() {
        //invert two control
        int index1 = Random.Range(0, 4);
        int index2 = Random.Range(0, 4);
        while (index2 == index1) {
            index2 = Random.Range(0, 4);
        }
        Dir key1 = controlSubstitute.ElementAt(index1).Key;
        Dir key2 = controlSubstitute.ElementAt(index2).Key;
        Dir val1 = controlSubstitute.ElementAt(index1).Value;
        Dir val2 = controlSubstitute.ElementAt(index2).Value;
        controlSubstitute[key1] = val2;
        controlSubstitute[key2] = val1;

    }

    private void HardRandomizeControl() {
        //mix all control
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
        //TODO AudioSystem.Instance.PlayHit();
        if (hit.collider != null) {
            ObstacleWood obstacleWood = hit.collider.gameObject.GetComponent<ObstacleWood>();
            if(obstacleWood != null) {
                obstacleWood.TakeDamage();
                //TODO move if far + particules not on the good place
            } else {
                isMoving = true;
                Vector2 destination = hit.collider.gameObject.CompareTag("Finish") ? hit.point + direction / 2 : hit.point - direction / 2;
                StartCoroutine(GoToDestination(destination));
            }
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
        WallSpawner.Instance.MovePlayerTo(destination);
    }

    public Dir GetSubstituteDir(Dir initDir) {
        return controlSubstitute[initDir];
    }

    /// <summary>
    /// use to freeze the movement until the dialogue are over
    /// </summary>
    /// <param name="isMoving"></param>
    public void FreezeForDialogue(bool isFreeze) {
        this.isMoving = isFreeze;
    }
}
