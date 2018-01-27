using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour {
    public static MySceneManager Instance;

    private void Awake() {
        if (Instance != null && Instance != this) {
            GameObject.Destroy(gameObject);
        } else {
            Instance = this;
        }

        GameObject.DontDestroyOnLoad(gameObject);
    }

    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SceneManager.LoadScene("Scenes/_menu", LoadSceneMode.Single);
        } else if (Input.GetKeyDown(KeyCode.F1)) {
            SceneManager.LoadScene("Scenes/_level0", LoadSceneMode.Single);
        } else if (Input.GetKeyDown(KeyCode.F2)) {
            SceneManager.LoadScene("_Scenes/level1", LoadSceneMode.Single);
        } else if (Input.GetKeyDown(KeyCode.F3)) {
            SceneManager.LoadScene("_Scenes/level2", LoadSceneMode.Single);
        }
    }
}
