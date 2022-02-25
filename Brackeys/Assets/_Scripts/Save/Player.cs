using UnityEngine;

/// <summary>
/// Player class containing the information that are "translated" in playerdata then saved
/// </summary>
public class Player : Singleton<Player> {

    #region Variables
    public static int Level { get; private set; } = 1;
    public static bool isSoundOn { get; private set; } = true;
    public static bool isMusicOn { get; private set; } = true;
    #endregion

    protected override void Awake() {
        base.Awake();
    }

    public Player ChangeData(PlayerData data) {
        Level = data.level;
        isSoundOn = data.isSoundOn;
        isMusicOn = data.isMusicOn;
        return this;
    }

    public void ChangeLvl(int change) {
        Level += change;
        Level = Mathf.Max(1, Level);
        SaveSystem.SavePlayer();
    }
    public void ChangeSound() {
        isSoundOn = !isSoundOn;
        SaveSystem.SavePlayer();
    }
    public void ChangeMusic() {
        isMusicOn = !isMusicOn;
        SaveSystem.SavePlayer();
    }

}
