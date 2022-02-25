/// <summary>
/// Serializable class savable with the save system
/// </summary>
[System.Serializable]
public class PlayerData {

    #region Variables
    public int level;
    public bool isSoundOn;
    public bool isMusicOn;
    #endregion

    public PlayerData() {
        level = Player.Level;
        isSoundOn = Player.isSoundOn;
        isMusicOn = Player.isMusicOn;
    }

}
