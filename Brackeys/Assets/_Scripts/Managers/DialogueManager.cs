using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : Singleton<DialogueManager> {

    #region Variables
    [SerializeField]
    private TextAsset dataset;

    private List<LvlDialogue> lvlDialogues;
    private enum Character { Nolwenn, Magalie, Dark}
    #endregion

    protected override void Awake() {
        base.Awake();


        lvlDialogues = new List<LvlDialogue>();

        string[] splitData = dataset.text.Split(new char[] { '\n' });
        string[] splitRow;
        LvlDialogue lvlDialogue;
        foreach (string row in splitData) {
            splitRow = row.Split(new char[] { ',' });
            if (splitRow.Length <= 1) break;
            lvlDialogue = GetLvlDialogueOfLvl(int.Parse(splitRow[0].Trim()));
            lvlDialogue.AddLine(new DialogueLigne(splitRow[1], splitRow[2]));            
        }
    }

    private LvlDialogue GetLvlDialogueOfLvl(int lvl) {
        foreach (LvlDialogue lvlDialogue in lvlDialogues) {
            if (lvlDialogue.GetLvl() == lvl) return lvlDialogue;
        }
        LvlDialogue lvlDiag = new LvlDialogue(lvl);
        lvlDialogues.Add(lvlDiag);
        return lvlDiag;
    }

    public LvlDialogue ShowLvlDialogue(int lvl) {
        LvlDialogue lvlDiag = null;
        foreach (LvlDialogue lvlDialogue in lvlDialogues) {
            if (lvlDialogue.GetLvl() == lvl) {
                lvlDiag = lvlDialogue;
                break;
            }
        }
        return lvlDiag;
    }

    public bool HaveLvlDialogue(int lvl) {
        foreach (LvlDialogue lvlDialogue in lvlDialogues) {
            if (lvlDialogue.GetLvl() == lvl) {
                return true;
            }
        }
        return false;
    }

    public class LvlDialogue {
        int lvl;
        List<DialogueLigne> lines;

        public LvlDialogue(int lvl) {
            this.lvl = lvl;
            lines = new List<DialogueLigne>();
        }

        public void AddLine(DialogueLigne line) {
            lines.Add(line);
        }

        public int GetLvl() {
            return lvl;
        }

        public List<DialogueLigne> GetLines() {
            return lines;
        }
    }

    public class DialogueLigne {
        string character;
        string line;

        public DialogueLigne(string character, string line) {
            this.character = character;
            this.line = line;
        }

        public string GetCharacter() {
            return character;
        }

        public string GetLine() {
            return line;
        }
    }
}
