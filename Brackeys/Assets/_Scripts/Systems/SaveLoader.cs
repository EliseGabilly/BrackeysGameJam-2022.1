using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoader : Singleton<SaveLoader> {

    private void Start() {
        SaveSystem.LoadData();
    }
}
