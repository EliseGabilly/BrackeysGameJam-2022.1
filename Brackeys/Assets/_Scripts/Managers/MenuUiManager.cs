using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUiManager : Singleton<MenuUiManager> {

    #region Variables
    [Header("Toggle")]
    [SerializeField]
    private Toggle musicToggle;
    [SerializeField]
    private Toggle soundToggle;
    [Header("Panels")]
    [SerializeField]
    private Canvas main;
    [SerializeField]
    private Canvas option;
    [SerializeField]
    private Canvas rules;
    [SerializeField]
    private Canvas credit;
    [Header("Level")]
    [SerializeField]
    private Text lvlTxt;

    #endregion

    private void Start() {
        musicToggle.isOn = Player.Instance.isMusicOn;
        soundToggle.isOn = Player.Instance.isSoundOn;
        StartCoroutine(LvlUpdate());
    }

    private IEnumerator LvlUpdate() {
        yield return new WaitForSeconds(0.2f);
        lvlTxt.text = "Level : "+Player.Instance.level;
    }

    public void Play() {
        SceneManager.LoadScene("Game");
    }

    public void OpenMain() {
        main.enabled = true;
        option.enabled = false;
        rules.enabled = false;
        credit.enabled = false;
    }

    public void OpenOption() {
        main.enabled = false;
        option.enabled = true;
    }

    public void OpenRules() {
        main.enabled = false;
        rules.enabled = true;
    }

    public void OpenCredit() {
        main.enabled = false;
        credit.enabled = true;
    }

    public void ToggleSound() {
        Player.Instance.ChangeSound();
        AudioSystem.Instance.TurnSoundOn(soundToggle.isOn);
    }

    public void ToggleMusic() {
        Player.Instance.ChangeMusic();
        AudioSystem.Instance.TurnMusicOn(musicToggle.isOn);
    }

    public void Quit() {
        Application.Quit();
    }

    public void BtnChangeLvl(int change) {
        Player.Instance.ChangeLvl(change);
        lvlTxt.text = "Level : " + Player.Instance.level;
        StartCoroutine(LvlUpdate());
    }

}
