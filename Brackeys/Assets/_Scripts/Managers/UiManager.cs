using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiManager : Singleton<UiManager> {

    #region Variables
    [SerializeField]
    private Text lvlTerrain;
    [SerializeField]
    private GameObject upBox;
    [SerializeField]
    private GameObject downBox;
    [SerializeField]
    private GameObject leftBox;
    [SerializeField]
    private GameObject rightBox;
    #endregion

    private void Start() {
        lvlTerrain.text = "Level : " + Player.Instance.level;
    }

    public void Home() {
        SceneManager.LoadScene("Menu");
    }

    public void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
