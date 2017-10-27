using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransition : MonoBehaviour {

    public void ChangeScene(string sceneName)
    {
        print("Loading Next Level");
        Application.LoadLevel(sceneName);
    }
}
