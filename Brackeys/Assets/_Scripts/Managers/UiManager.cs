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
    private GameObject imgs;
    [SerializeField]
    private Image upImg;
    [SerializeField]
    private Image downImg;
    [SerializeField]
    private Image leftImg;
    [SerializeField]
    private Image rightImg;
    private int guideDisplay = 0;

    [Header("ArrowsImg")]
    [SerializeField]
    private Sprite arrow_up;
    [SerializeField]
    private Sprite arrow_down;
    [SerializeField]
    private Sprite arrow_left;
    [SerializeField]
    private Sprite arrow_right;

    [Header("ArrowsImg")]
    [SerializeField]
    private Animator fadeAnim;

    [Header("Dialogue")]
    [SerializeField]
    private GameObject dialogueBox;
    [SerializeField]
    private Text characterName;
    [SerializeField]
    private Text lineText;
    [SerializeField]
    private Image characterImg;
    [SerializeField]
    private Sprite nolwennImg;
    [SerializeField]
    private Sprite darkImg;
    [SerializeField]
    private Sprite stephImg;

    private int lvl;
    private bool isDialogueOn = false;
    private DialogueManager.LvlDialogue lvlDialogue;
    private int dialogueLineCount = 0;
    private int dialogueLineLeft = 0;
    #endregion

    private void Start() {
        StartCoroutine(LateStart());
    }

    private IEnumerator LateStart() {
        yield return new WaitForSeconds(0.04f);
        lvl = Player.Level;
        lvlTerrain.text = "Level : " + lvl ;
        if (lvl <= 5) {
            ShowAllGuide();
        } else if (lvl >= 20) {
            imgs.SetActive(false);
        }

        lvlDialogue = DialogueManager.Instance.ShowLvlDialogue(lvl);
        if (lvlDialogue != null) {
            PlayerControlManager.Instance.FreezeForDialogue(true);
            isDialogueOn = true;
            dialogueLineLeft = lvlDialogue.GetLines().Count;
            dialogueLineCount = dialogueLineLeft;
            OpenNextLine();
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            Restart();
        }
        if (isDialogueOn) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                CloseLine();
                if (dialogueLineLeft == 0) {
                    PlayerControlManager.Instance.FreezeForDialogue(false);
                    isDialogueOn = false;
                } else {
                    OpenNextLine();
                }

            }
        }
    }

    public void Home() {
        StartCoroutine(LoadHomeAfterFade());
    }

    private IEnumerator LoadHomeAfterFade() {
        fadeAnim.SetTrigger("fadeIn");
        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene("Menu");
    }

    public void Restart() {
        StartCoroutine(RestartAfterFade());
    }

    private IEnumerator RestartAfterFade() {
        fadeAnim.SetTrigger("fadeIn");
        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void PlayFadeInAnim() {
        fadeAnim.SetTrigger("fadeIn");
    }

    public void AddDirectionGuide(PlayerControlManager.Dir inputDir, PlayerControlManager.Dir corespondingDir ) {
        if (lvl >=20) { // no guid for high lvl
            return;
        } else if (guideDisplay == 4) { // all arrows are displayed
            return;
        } else if (guideDisplay == 1 && Player.Level <= 10) {
            ShowAllGuide();
        }
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
        Sprite arrowImg = GetCorespondingSprite(corespondingDir);
        inputImg.sprite = arrowImg;
        inputImg.enabled = true;
        guideDisplay++;
    }

    private Sprite GetCorespondingSprite(PlayerControlManager.Dir corespondingDir) {
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
        return arrowImg;
    }

    private void ShowAllGuide() {
        if (!upImg.enabled) {
            upImg.enabled = true;
            Sprite arrowImg = GetCorespondingSprite(PlayerControlManager.Instance.GetSubstituteDir(PlayerControlManager.Dir.up));
            upImg.sprite = arrowImg;
        }
        if (!downImg.enabled) {
            downImg.enabled = true;
            Sprite arrowImg = GetCorespondingSprite(PlayerControlManager.Instance.GetSubstituteDir(PlayerControlManager.Dir.down));
            downImg.sprite = arrowImg;
        }
        if (!leftImg.enabled) {
            leftImg.enabled = true;
            Sprite arrowImg = GetCorespondingSprite(PlayerControlManager.Instance.GetSubstituteDir(PlayerControlManager.Dir.left));
            leftImg.sprite = arrowImg;
        }
        if (!rightImg.enabled) {
            rightImg.enabled = true;
            Sprite arrowImg = GetCorespondingSprite(PlayerControlManager.Instance.GetSubstituteDir(PlayerControlManager.Dir.right));
            rightImg.sprite = arrowImg;
        }
        guideDisplay = 4;
    }

    private void OpenNextLine() {
        DialogueManager.DialogueLigne dialogueLigne = lvlDialogue.GetLines()[dialogueLineCount- dialogueLineLeft];
        dialogueBox.SetActive(true);
        string name = dialogueLigne.GetCharacter();
        characterName.text = name;
        lineText.text = dialogueLigne.GetLine();
        Sprite img = nolwennImg;
        if (name.ToLower().Equals("dark")) {
            img = darkImg;
        } else if (name.ToLower().Equals("steph")) {
            img = stephImg;
        }
        characterImg.sprite = img;
    }

    private void CloseLine() {
        dialogueLineLeft--;
        dialogueBox.SetActive(false);
    }

}
