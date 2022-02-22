using UnityEngine;

/// <summary>
/// Player class containing the information that are "translated" in playerdata then saved
/// </summary>
public class Player : Singleton<Player> {

    #region Variables
    public int level = 1;
    public bool isSoundOn = true;
    public bool isMusicOn = true;
    #endregion

    public Player ChangeData(PlayerData data) {
        level = data.level;
        isSoundOn = data.isSoundOn;
        isMusicOn = data.isMusicOn;
        return this;
    }

    public void ChangeLvl(int change) {
        level += change;
        SaveSystem.SavePlayer(this);
    }
    public void ChangeSound() {
        isSoundOn = !isSoundOn;
        SaveSystem.SavePlayer(this);
    }
    public void ChangeMusic() {
        isMusicOn = !isMusicOn;
        SaveSystem.SavePlayer(this);
    }

}
