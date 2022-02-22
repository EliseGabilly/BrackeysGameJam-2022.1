using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUiManager : Singleton<MenuUiManager> {

    #region Variables
    [Header("Panels")]
    [SerializeField]
    private Canvas main;
    [SerializeField]
    private Canvas option;
    [SerializeField]
    private Canvas rules;
    [SerializeField]
    private Canvas credit;

    #endregion

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

    public void Quit() {
        Application.Quit();
    }

}
