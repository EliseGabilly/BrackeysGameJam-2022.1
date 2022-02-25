using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleWood : MonoBehaviour {

    #region Variable
    [SerializeField]
    private GameObject particules;
    private int pv = 3;
    #endregion

    public void TakeDamage() {
        pv--;
        StartCoroutine(ShowDamage());
        if(pv == 0) {
            DestroyObstacle();
        }
    }

    private IEnumerator ShowDamage() {
        particules.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        particules.SetActive(false);
    }

    private void DestroyObstacle() {
        WallSpawner.Instance.RemoveObstacleAt(new Vector2Int (Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y)));
        Destroy(gameObject);
    }

}
