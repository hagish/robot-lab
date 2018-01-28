using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour {
    public static MySceneManager Instance;

	public AudioClip mainMenuMusic;
	private AudioSource audioSource;

    private void Awake() {
        if (Instance != null && Instance != this) {
            GameObject.Destroy(gameObject);
        } else {
            Instance = this;
        }

        GameObject.DontDestroyOnLoad(gameObject);

		audioSource = GetComponent<AudioSource> ();
    }

	void Start()
	{
		if (mainMenuMusic != null) {
			audioSource.clip = mainMenuMusic;
			audioSource.Play ();
		}
	}

    public void GotoMenu() {
        SceneManager.LoadScene("_menu", LoadSceneMode.Single);
    }

    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            GotoMenu();
        } else if (Input.GetKeyDown(KeyCode.F1)) {
            SceneManager.LoadScene("_level0", LoadSceneMode.Single);
        } else if (Input.GetKeyDown(KeyCode.F2)) {
            SceneManager.LoadScene("_level1", LoadSceneMode.Single);
        } else if (Input.GetKeyDown(KeyCode.F3)) {
            SceneManager.LoadScene("_level2", LoadSceneMode.Single);
	    } else if (Input.GetKeyDown(KeyCode.C)) {
		    SceneManager.LoadScene("_controls", LoadSceneMode.Single);
	    }
    }
}
