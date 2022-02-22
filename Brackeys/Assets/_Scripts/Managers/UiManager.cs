using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiManager : Singleton<UiManager> {

    #region Variables
    [SerializeField]
    private Text lvlTerrain;

    [Header("Direction box")]
    [SerializeField]
    private Image upImg;
    [SerializeField]
    private Image downImg;
    [SerializeField]
    private Image leftImg;
    [SerializeField]
    private Image rightImg;

    [Header("ArrowsImg")]
    [SerializeField]
    private Sprite arrow_up;
    [SerializeField]
    private Sprite arrow_down;
    [SerializeField]
    private Sprite arrow_left;
    [SerializeField]
    private Sprite arrow_right;
    #endregion

    protected override void Awake() {
        base.Awake();
    }

    private void Start() {
        lvlTerrain.text = "Level : " + Player.Instance.level;
    }

    public void Home() {
        SceneManager.LoadScene("Menu");
    }

    public void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void AddDirectionGuide(PlayerControlManager.Dir inputDir, PlayerControlManager.Dir corespondingDir ) {
        Image inputImg = upImg;
        switch (inputDir) {
            case PlayerControlManager.Dir.up:
                inputImg = upImg;
                break;
            case PlayerControlManager.Dir.down:
                inputImg = downImg;
                break;
            case PlayerControlManager.Dir.left:
                inputImg = leftImg;
                break;
            case PlayerControlManager.Dir.right:
                inputImg = rightImg;
                break;
            case PlayerControlManager.Dir.stop:
                return;
        }
        if (inputImg.enabled) {
            return;
        }
        Sprite arrowImg = arrow_up;
        switch (corespondingDir) {
            case PlayerControlManager.Dir.up:
                arrowImg = arrow_up;
                break;
            case PlayerControlManager.Dir.down:
                arrowImg = arrow_down;
                break;
            case PlayerControlManager.Dir.left:
                arrowImg = arrow_left;
                break;
            case PlayerControlManager.Dir.right:
                arrowImg = arrow_right;
                break;
        }
        inputImg.sprite = arrowImg;
        inputImg.enabled = true;
    }

}
