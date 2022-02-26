using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndAltar : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            AudioSystem.Instance.PlayTP();
            Player.Instance.ChangeLvl(1);
            UiManager.Instance.Restart();
        }
    }
}
