using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleWood : MonoBehaviour {

    #region Variable
    [SerializeField]
    private GameObject particules;
    [SerializeField]
    private GameObject crack1;
    [SerializeField]
    private GameObject crack2;
    [SerializeField]
    private Animator anim;
    private int pv = 3;
    #endregion

    public void TakeDamage() {
        pv--;
        StartCoroutine(ShowDamage());        
    }

    private IEnumerator ShowDamage() {
        anim.SetTrigger("hit");
        particules.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        particules.SetActive(false);
        if (pv == 0) {
            DestroyObstacle();
        } else if (pv == 1) {
            crack2.SetActive(true);
        } else if (pv == 2) {
            crack1.SetActive(true);
        }
    }

    private void DestroyObstacle() {
        WallSpawner.Instance.RemoveObstacleAt(new Vector2Int (Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y)));
        Destroy(gameObject);
    }

}
