
public class SaveLoader : Singleton<SaveLoader> {

    private void Start() {
        SaveSystem.LoadData();
    }
}
